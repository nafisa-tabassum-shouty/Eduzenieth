Select * From Student;
Select * From Teacher;
Select * From Course;
Select * From Enroll;
Select * From Comments;
Select * From Posts;
/*permission error*/
DROP TABLE Post;
DROP TABLE Comment;
CREATE TABLE Posts (
    PostID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    CourseID INT NOT NULL,
    PostContent NVARCHAR(MAX) NOT NULL,
    PostDate DATETIME NOT NULL DEFAULT GETDATE(),
    UserID INT NOT NULL,
    UserStatus INT NOT NULL,
    PostType VARCHAR(50) NOT NULL
);



CREATE TABLE Comments (
    CommentID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    PostID INT NOT NULL,
    UserID INT NOT NULL,
    UserStatus INT NOT NULL,
    CommentContent NVARCHAR(MAX) NOT NULL,
    CommentDate DATETIME NOT NULL DEFAULT GETDATE()
);