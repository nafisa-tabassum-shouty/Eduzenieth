ALTER TABLE Course
DROP COLUMN File_Path, Posts;


CREATE TABLE Posts (
    PostID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    CourseID INT NOT NULL,
    PostContent NVARCHAR(MAX) NOT NULL,
    PostDate DATETIME NOT NULL DEFAULT GETDATE(),
    UserID INT NOT NULL,
    UserStatus INT NOT NULL,
    PostType VARCHAR(50) NOT NULL, -- You can adjust the size of VARCHAR as needed
    FOREIGN KEY (CourseID) REFERENCES Course(Course_Id)
);


CREATE TABLE Comments (
    CommentID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    PostID INT NOT NULL,
    UserID INT NOT NULL,
    UserStatus INT NOT NULL,
    CommentContent NVARCHAR(MAX) NOT NULL,
    CommentDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (PostID) REFERENCES Posts(PostID)
);
Select * From Student;
Select * From Teacher;
Select * From Course;
Select * From Enroll;
Select * From Comments;
Select * From Posts;

INSERT INTO Enroll (sid, cid, status)
VALUES (101, 201, 1);

INSERT INTO Enroll (sid, cid, status)
VALUES (102, 202, 0);

INSERT INTO Enroll (sid, cid, status)
VALUES (103, 203, 1);

INSERT INTO Enroll (sid, cid, status)
VALUES (104, 204, 0);

INSERT INTO Enroll (sid, cid, status)
VALUES (105, 205, 1);

INSERT INTO Posts (CourseID, PostContent, UserID, UserStatus, PostType)
VALUES (1, 'This is the first post content.', 1, 1, 'Announcement');

INSERT INTO Posts (CourseID, PostContent, UserID, UserStatus, PostType)
VALUES (2, 'Here is another post about the upcoming assignment.', 2, 1, 'Assignment');

INSERT INTO Posts (CourseID, PostContent, UserID, UserStatus, PostType)
VALUES (3, 'Reminder: There will be a quiz next week.', 3, 1, 'Reminder');

INSERT INTO Posts (CourseID, PostContent, UserID, UserStatus, PostType)
VALUES (4, 'Discussion topic: What are your thoughts on the latest lecture?', 4, 1, 'Discussion');

INSERT INTO Posts (CourseID, PostContent, UserID, UserStatus, PostType)
VALUES (5, 'Important announcement: Changes in the course schedule.', 5, 1, 'Announcement');

INSERT INTO Comments (PostID, UserID, UserStatus, CommentContent)
VALUES (1, 101, 1, 'Great announcement! Looking forward to it.');

INSERT INTO Comments (PostID, UserID, UserStatus, CommentContent)
VALUES (1, 102, 1, 'Excited about this.');

INSERT INTO Comments (PostID, UserID, UserStatus, CommentContent)
VALUES (2, 103, 1, 'I have a question regarding the assignment.');

INSERT INTO Comments (PostID, UserID, UserStatus, CommentContent)
VALUES (16, 104, 1, 'Thanks for the reminder.');

INSERT INTO Comments (PostID, UserID, UserStatus, CommentContent)
VALUES (16, 105, 1, 'Interesting topic! Can''t wait to discuss.');

INSERT INTO Comments (PostID, UserID, UserStatus, CommentContent)
VALUES (20, 106, 1, 'I agree with your point.');

INSERT INTO Comments (PostID, UserID, UserStatus, CommentContent)
VALUES (19, 107, 1, 'This is very helpful.');

INSERT INTO Comments (PostID, UserID, UserStatus, CommentContent)
VALUES (18, 108, 1, 'I have a different perspective on this.');

INSERT INTO Comments (PostID, UserID, UserStatus, CommentContent)
VALUES (17, 109, 1, 'Looking forward to the updates.');

INSERT INTO Comments (PostID, UserID, UserStatus, CommentContent)
VALUES (16, 110, 1, 'I have some suggestions for improvement.');
