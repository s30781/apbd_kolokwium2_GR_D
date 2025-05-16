-- tables
-- Table: Attraction
CREATE TABLE Attraction (
    attraction_id int  NOT NULL,
    name nvarchar(100)  NOT NULL,
    price decimal(10,2)  NOT NULL,
    CONSTRAINT Attraction_pk PRIMARY KEY  (attraction_id)
);

-- Table: Booking
CREATE TABLE Booking (
    booking_id int  NOT NULL,
    guest_id int  NOT NULL,
    employee_id int  NOT NULL,
    date datetime  NOT NULL,
    CONSTRAINT Booking_pk PRIMARY KEY  (booking_id)
);

-- Table: Booking_Attraction
CREATE TABLE Booking_Attraction (
    booking_id int  NOT NULL,
    attraction_id int  NOT NULL,
    amount int  NOT NULL,
    CONSTRAINT Booking_Attraction_pk PRIMARY KEY  (booking_id,attraction_id)
);

-- Table: Employee
CREATE TABLE Employee (
    employee_id int  NOT NULL,
    first_name nvarchar(100)  NOT NULL,
    last_name nvarchar(100)  NOT NULL,
    employee_number nvarchar(22)  NOT NULL,
    CONSTRAINT Employee_pk PRIMARY KEY  (employee_id)
);

-- Table: Guest
CREATE TABLE Guest (
    guest_id int  NOT NULL,
    first_name nvarchar(100)  NOT NULL,
    last_name nvarchar(100)  NOT NULL,
    date_of_birth datetime  NOT NULL,
    CONSTRAINT Guest_pk PRIMARY KEY  (guest_id)
);

-- foreign keys
-- Reference: Booking_Attraction_Attraction (table: Booking_Attraction)
ALTER TABLE Booking_Attraction ADD CONSTRAINT Booking_Attraction_Attraction
    FOREIGN KEY (attraction_id)
    REFERENCES Attraction (attraction_id);

-- Reference: Booking_Attraction_Booking (table: Booking_Attraction)
ALTER TABLE Booking_Attraction ADD CONSTRAINT Booking_Attraction_Booking
    FOREIGN KEY (booking_id)
    REFERENCES Booking (booking_id);

-- Reference: Booking_Employee (table: Booking)
ALTER TABLE Booking ADD CONSTRAINT Booking_Employee
    FOREIGN KEY (employee_id)
    REFERENCES Employee (employee_id);

-- Reference: Booking_Guest (table: Booking)
ALTER TABLE Booking ADD CONSTRAINT Booking_Guest
    FOREIGN KEY (guest_id)
    REFERENCES Guest (guest_id);

-- End of file.

-- Inserting data into Attraction
INSERT INTO Attraction (attraction_id, name, price) VALUES
(1, 'Roller Coaster', 15.00),
(2, 'Ferris Wheel', 10.00),
(3, 'Haunted House', 12.50),
(4, 'Water Slide', 8.00);

-- Inserting data into Employee
INSERT INTO Employee (employee_id, first_name, last_name, employee_number) VALUES
(1, 'Alice', 'Johnson', 'EMP001'),
(2, 'Bob', 'Smith', 'EMP002'),
(3, 'Carol', 'Taylor', 'EMP003');

-- Inserting data into Guest
INSERT INTO Guest (guest_id, first_name, last_name, date_of_birth) VALUES
(1, 'John', 'Doe', '1990-05-15'),
(2, 'Emma', 'Wilson', '1985-08-20'),
(3, 'Liam', 'Brown', '2000-12-01');

-- Inserting data into Booking
INSERT INTO Booking (booking_id, guest_id, employee_id, date) VALUES
(1, 1, 1, '2024-07-01 10:00:00'),
(2, 2, 2, '2024-07-01 11:00:00'),
(3, 3, 1, '2024-07-02 09:30:00');

-- Inserting data into Booking_Attraction
INSERT INTO Booking_Attraction (booking_id, attraction_id, amount) VALUES
(1, 1, 2),  -- John booked 2 Roller Coaster rides
(1, 2, 1),  -- John booked 1 Ferris Wheel
(2, 3, 3),  -- Emma booked 3 Haunted House entries
(3, 4, 2);  -- Liam booked 2 Water Slide entries

