-- Dropping the foreign key constraint on Comments table
ALTER TABLE Comments DROP CONSTRAINT FK_Comments_PostID; 

-- Dropping the foreign key constraint on Posts table
ALTER TABLE Posts DROP CONSTRAINT FK_Posts_CourseID;

SELECT 
    tc.TABLE_NAME, 
    tc.CONSTRAINT_NAME, 
    kcu.COLUMN_NAME, 
    ccu.TABLE_NAME AS REFERENCED_TABLE_NAME, 
    ccu.COLUMN_NAME AS REFERENCED_COLUMN_NAME
FROM 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS tc 
    JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS kcu
      ON tc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME
    JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS ccu
      ON ccu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
WHERE 
    tc.CONSTRAINT_TYPE = 'FOREIGN KEY'
    AND (tc.TABLE_NAME = 'Comments' OR tc.TABLE_NAME = 'Posts');




    -- Dropping the foreign key constraints
ALTER TABLE Comments DROP CONSTRAINT FK__Comments__PostID__2DE6D218;
ALTER TABLE Posts DROP CONSTRAINT FK__Posts__CourseID__2A164134;


CREATE TABLE Grades (
    GradeID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Grade CHAR(20),
    GradeDate DATE,
    Course_Id INT,
    Course_Name VARCHAR(250),
    TeacherID INT,
    Teacher_Name  NVARCHAR(100),
    StudentID INT,
    StudentName NVARCHAR(100)
);


Select * From Student;
Select * From Teacher;
Select * From Course;
Select * From Enroll;
Select * From Comments;
Select * From Posts;