
CREATE TABLE Student (
    StudentID INT PRIMARY KEY,
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
-- Create tblTeacher table
CREATE TABLE Teacher (
  Id INT PRIMARY KEY IDENTITY(1,1),
  Username VARCHAR(250) NOT NULL,
  [Password] VARCHAR(250) NOT NULL, -- Password is a reserved keyword, so it's enclosed in square brackets
  first_name VARCHAR(250) NOT NULL,
  last_name VARCHAR(250) NOT NULL,
  father_name VARCHAR(250) NOT NULL,
  Email VARCHAR(250) NOT NULL,
  Designation VARCHAR(10) NOT NULL,
  MaritalStatus VARCHAR(10) NOT NULL,
  MobileNumber VARCHAR(14) NOT NULL,
  Nationality VARCHAR(50) NOT NULL,
  Religion VARCHAR(10) NOT NULL,
  [Address] VARCHAR(250) NOT NULL, -- Address is a reserved keyword, so it's enclosed in square brackets
  Gender VARCHAR(10) NOT NULL,
  EducationalField VARCHAR(50) NOT NULL,
  NID_PassportNo VARCHAR(20) NOT NULL -- Assuming NID_PassportNo is stored as a string
);

-- Create Course table
CREATE TABLE Course (
  id INT PRIMARY KEY IDENTITY(1,1),
  Course_Id VARCHAR(15) NOT NULL,
  Course_Name VARCHAR(250) NOT NULL,
  teacherID INT DEFAULT 0, -- Assuming teacherID is an integer
  Course_desc VARCHAR(250) NOT NULL,
  Created_at DATETIME NOT NULL DEFAULT GETDATE()
);

-- Create StudentCourse table
CREATE TABLE Enroll (
  id INT PRIMARY KEY IDENTITY(1,1),
  sid INT NOT NULL,
  cid INT NOT NULL,
  status INT DEFAULT 0 -- Assuming status is an integer
);

-- Insert data into tblTeacher
INSERT INTO Teacher (Username, [Password], first_name, last_name, father_name, Email, Designation, MaritalStatus, MobileNumber, Nationality, Religion, [Address], Gender, EducationalField, NID_PassportNo)
VALUES ('tech', 'world234', 'Rizoan', 'Hossain', 'shakhawat', 'rishad@gmail.com', 'Professor', 'Unmarried', '01971984503', 'Bangladeshi', 'Muslim', 'bashabo 2456', 'Male', 'CSE', 'NID123'),
       ('techh', 'world234', 'Demo', 'Mia', '', 'teacher@gmail.com', 'Professor', 'Unmarried', '01971984503', 'Bangladeshi', 'Muslim', 'Jamsal', 'Male', 'CSE', 'NID123'),
       ('admin', 'admin', 'Main', 'Admin', '', 'admin@gmail.com', 'Professor', 'Unmarried', '01971984503', 'Bangladeshi', 'Muslim', 'Admin Address', 'Male', 'CSE', 'Noneed123');-- Insert data into Course

INSERT INTO Course (Course_Id, Course_Name, teacherID, Course_desc)
VALUES ('HUM1101', 'Mathematics', 0, 'Introduction to Mathematics'),
       ('HUM1102', 'Science', 0, 'Introduction to Science'),
       ('HUM1103', 'History', 0, 'Introduction to History'),
       ('HUM1103', 'History', 1, 'Introduction to History');

Select * From Student;
Select * From Enroll;
