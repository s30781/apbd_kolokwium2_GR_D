-- foreign keys
ALTER TABLE Booking_Attraction DROP CONSTRAINT Booking_Attraction_Attraction;

ALTER TABLE Booking_Attraction DROP CONSTRAINT Booking_Attraction_Booking;

ALTER TABLE Booking DROP CONSTRAINT Booking_Employee;

ALTER TABLE Booking DROP CONSTRAINT Booking_Guest;

-- tables
DROP TABLE Attraction;

DROP TABLE Booking;

DROP TABLE Booking_Attraction;

DROP TABLE Employee;

DROP TABLE Guest;

-- End of file.

