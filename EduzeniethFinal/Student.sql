DROP TABLE Student;
CREATE TABLE Student (
    StudentID INT PRIMARY KEY IDENTITY(1,1),
    Password NVARCHAR(50) NOT NULL,
    Nationality NVARCHAR(50) NOT NULL,
    MobileNumber NVARCHAR(20),
    FirstName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    FatherName NVARCHAR(50),
    Religion NVARCHAR(50),
    Address NVARCHAR(200),
    DateOfBirth DATE,
    BloodGroup NVARCHAR(10),
    Username NVARCHAR(50) NOT NULL UNIQUE
);
INSERT INTO Student (Password, Nationality, MobileNumber, FirstName, Email, LastName, FatherName, Religion, Address, DateOfBirth, BloodGroup, Username)
VALUES
('123456', 'USA', '1234567890', 'John', 'john@example.com', 'Doe', 'Michael Doe', 'Christian', '123 Street Name, City', '1995-05-10', 'A+', 'john_doe'),
('123456', 'Canada', '9876543210', 'Jane', 'jane@example.com', 'Smith', 'David Smith', 'Atheist', '456 Avenue Name, Town', '1998-08-20', 'B-', 'jane_smith'),
('123456', 'UK', '1122334455', 'Alice', 'alice@example.com', 'Brown', 'George Brown', 'Muslim', '789 Road Name, Village', '1997-02-15', 'O+', 'alice_brown'),
('123456', 'Australia', '9988776655', 'Michael', 'michael@example.com', 'Johnson', 'Daniel Johnson', 'Jewish', '1010 Lane Name, Suburb', '1996-10-30', 'AB-', 'michael_johnson'),
('123456', 'Germany', '5544332211', 'Sophia', 'sophia@example.com', 'Martinez', 'James Martinez', 'Hindu', '1212 Drive Name, City', '1999-07-25', 'A-', 'sophia_martinez'),
('123456', 'France', '3344556677', 'William', 'william@example.com', 'Garcia', 'Robert Garcia', 'Sikh', '1414 Boulevard Name, Town', '1994-12-05', 'B+', 'william_garcia'),
('123456', 'Italy', '7766554433', 'Olivia', 'olivia@example.com', 'Lopez', 'Richard Lopez', 'Buddhist', '1616 Street Name, Village', '1993-04-18', 'O-', 'olivia_lopez'),
('123456', 'Spain', '8899776655', 'Daniel', 'daniel@example.com', 'Hernandez', 'Charles Hernandez', 'Other', '1818 Avenue Name, Suburb', '1992-09-08', 'AB+', 'daniel_hernandez');
