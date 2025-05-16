using kolos2s30781grD.DTO;
using Microsoft.Data.SqlClient;

namespace kolos2s30781grD.Services;

public class DatabaseService : IDatabaseService
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<BookingResponse?> GetBookingAsync(int bookingId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string bookingQuery = @"
            SELECT b.date, 
                   g.guest_id, g.first_name, g.last_name, g.date_of_birth,
                   e.employee_id, e.first_name, e.last_name, e.employee_number
            FROM Booking b
            JOIN Guest g ON b.guest_id = g.guest_id
            JOIN Employee e ON b.employee_id = e.employee_id
            WHERE b.booking_id = @id;";

        await using var cmd = new SqlCommand(bookingQuery, connection);
        cmd.Parameters.AddWithValue("@id", bookingId);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        var response = new BookingResponse
        {
            Date = reader.GetDateTime(0),
            Guest = new GuestDTO
            {
                GuestId = reader.GetInt32(1),
                FirstName = reader.GetString(2),
                LastName = reader.GetString(3),
                DateOfBirth = reader.GetDateTime(4)
            },
            Employee = new EmployeeDTO
            {
                EmployeeId = reader.GetInt32(5),
                FirstName = reader.GetString(6),
                LastName = reader.GetString(7),
                EmployeeNumber = reader.GetString(8)
            },
            Attractions = new List<AttractionDTO>()
        };

        await reader.CloseAsync();

        const string attractionsQuery = @"
            SELECT a.attraction_id, a.name, a.price, ba.amount
            FROM Booking_Attraction ba
            JOIN Attraction a ON ba.attraction_id = a.attraction_id
            WHERE ba.booking_id = @id;";

        await using var cmd2 = new SqlCommand(attractionsQuery, connection);
        cmd2.Parameters.AddWithValue("@id", bookingId);
        await using var reader2 = await cmd2.ExecuteReaderAsync();

        while (await reader2.ReadAsync())
        {
            response.Attractions.Add(new AttractionDTO
            {
                AttractionId = reader2.GetInt32(0),
                Name = reader2.GetString(1),
                Price = reader2.GetDecimal(2),
                Amount = reader2.GetInt32(3)
            });
        }

        return response;
    }

    public async Task<(bool Success, string? Message, BookingResponse? Booking)> CreateBookingAsync(BookingRequest request)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            // Check if Booking ID already exists
            var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Booking WHERE booking_id = @id", connection, (SqlTransaction)transaction);
            checkCmd.Parameters.AddWithValue("@id", request.BookingId);
            var count = (int)await checkCmd.ExecuteScalarAsync();
            if (count > 0)
                return (false, "Booking with this ID already exists", null);

            // Check Guest
            var guestCmd = new SqlCommand("SELECT COUNT(*) FROM Guest WHERE guest_id = @id", connection, (SqlTransaction)transaction);
            guestCmd.Parameters.AddWithValue("@id", request.GuestId);
            if ((int)await guestCmd.ExecuteScalarAsync() == 0)
                return (false, "Guest not found", null);

            // Get Employee ID
            int? employeeId = null;
            var empCmd = new SqlCommand("SELECT employee_id FROM Employee WHERE employee_number = @num", connection, (SqlTransaction)transaction);
            empCmd.Parameters.AddWithValue("@num", request.EmployeeNumber);
            await using var empReader = await empCmd.ExecuteReaderAsync();
            if (await empReader.ReadAsync())
                employeeId = empReader.GetInt32(0);
            await empReader.CloseAsync();

            if (employeeId == null)
                return (false, "Employee not found", null);

            // Insert Booking
            var insertBooking = new SqlCommand("INSERT INTO Booking (booking_id, guest_id, employee_id, date) VALUES (@id, @guest, @emp, @date)", connection, (SqlTransaction)transaction);
            insertBooking.Parameters.AddWithValue("@id", request.BookingId);
            insertBooking.Parameters.AddWithValue("@guest", request.GuestId);
            insertBooking.Parameters.AddWithValue("@emp", employeeId);
            insertBooking.Parameters.AddWithValue("@date", DateTime.Now);
            await insertBooking.ExecuteNonQueryAsync();

            foreach (var attr in request.Attractions)
            {
                // Get Attraction ID
                var attrIdCmd = new SqlCommand("SELECT attraction_id, price FROM Attraction WHERE name = @name", connection, (SqlTransaction)transaction);
                attrIdCmd.Parameters.AddWithValue("@name", attr.Name);
                int? attrId = null;
                await using var attrReader = await attrIdCmd.ExecuteReaderAsync();
                if (await attrReader.ReadAsync())
                {
                    attrId = attrReader.GetInt32(0);
                }
                await attrReader.CloseAsync();

                if (attrId == null)
                    return (false, $"Attraction '{attr.Name}' not found", null);

                var insertBA = new SqlCommand("INSERT INTO Booking_Attraction (booking_id, attraction_id, amount) VALUES (@bid, @aid, @amt)", connection, (SqlTransaction)transaction);
                insertBA.Parameters.AddWithValue("@bid", request.BookingId);
                insertBA.Parameters.AddWithValue("@aid", attrId);
                insertBA.Parameters.AddWithValue("@amt", attr.Amount);
                await insertBA.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();

            // Return inserted booking
            var result = await GetBookingAsync(request.BookingId);
            return (true, null, result);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (false, ex.Message, null);
        }
    }
}

