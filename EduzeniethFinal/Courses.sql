﻿
Select * From Course;
DROP TABLE Course;

CREATE TABLE Course (
  Course_Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
  Course_Code VARCHAR(15) NOT NULL,
  Course_Name VARCHAR(250) NOT NULL,
  teacherID INT DEFAULT 0, 
  Course_desc VARCHAR(250) NOT NULL,
  Created_at DATETIME NOT NULL DEFAULT GETDATE()
  File_Path NVARCHAR(500) NULL,
  Posts NVARCHAR(MAX) NULL;
);

INSERT INTO Course (Course_Code, Course_Name, teacherID, Course_desc)
VALUES ('CS101', 'Introduction to Computer Science', 1, 'Basic concepts of computer science');

INSERT INTO Course (Course_Code, Course_Name, teacherID, Course_desc)
VALUES ('MATH101', 'Calculus I', 2, 'Introduction to differential and integral calculus');

INSERT INTO Course (Course_Code, Course_Name, teacherID, Course_desc)
VALUES ('PHY101', 'Physics I', 3, 'Fundamentals of mechanics and thermodynamics');

INSERT INTO Course (Course_Code, Course_Name, teacherID, Course_desc)
VALUES ('HIST101', 'World History', 4, 'Overview of major historical events and periods');

INSERT INTO Course (Course_Code, Course_Name, teacherID, Course_desc)
VALUES ('ENG101', 'English Literature', 5, 'Study of significant literary works and authors');

ALTER TABLE Course
ADD 
