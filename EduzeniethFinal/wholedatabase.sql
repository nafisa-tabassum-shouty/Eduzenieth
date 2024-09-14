


CREATE TABLE Questions  (
    QuesID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    CourseID INT NOT NULL,
    Question NVARCHAR(MAX) NOT NULL,
    Ans_A NVARCHAR(MAX) NOT NULL,
    Ans_B NVARCHAR(MAX) NOT NULL,
    Ans_C NVARCHAR(MAX) NOT NULL,
    Ans_D NVARCHAR(MAX) NOT NULL,
    Correct_Ans NVARCHAR(MAX) NOT NULL,
    Quiz_id INT NOT NULL,
   
);

CREATE TABLE Answer  (
    AnsID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Ans NVARCHAR(MAX) NOT NULL,
    statuss INT NOT NULL,
    Quiz_id INT NOT NULL,
    CourseID INT NOT NULL,
    CourseName NVARCHAR(MAX) NOT NULL,
    studentID INT NOT NULL,
    studentName NVARCHAR(40) NOT NULL,
    QuesID INT NOT NULL,
    Question NVARCHAR(MAX) NOT NULL,
);

CREATE TABLE [dbo].[Comments] (
    [CommentID]      INT            IDENTITY (1, 1) NOT NULL,
    [PostID]         INT            NOT NULL,
    [UserID]         INT            NOT NULL,
    [UserStatus]     INT            NOT NULL,
    [CommentContent] NVARCHAR (MAX) NOT NULL,
    [CommentDate]    DATETIME       DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([CommentID] ASC)
);

CREATE TABLE [dbo].[Course] (
    [Course_Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Course_Code] VARCHAR (15)  NOT NULL,
    [Course_Name] VARCHAR (250) NOT NULL,
    [teacherID]   INT           DEFAULT ((0)) NULL,
    [Course_desc] VARCHAR (250) NOT NULL,
    [Created_at]  DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Course_Id] ASC)
);

CREATE TABLE [dbo].[Enroll] (
    [id]     INT IDENTITY (1, 1) NOT NULL,
    [sid]    INT NOT NULL,
    [cid]    INT NOT NULL,
    [status] INT DEFAULT ((0)) NULL,
    [role]   INT DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);
CREATE TABLE [dbo].[Grades] (
    [GradeID]      INT            IDENTITY (1, 1) NOT NULL,
    [Grade]        CHAR (20)      NULL,
    [GradeDate]    DATE           NULL,
    [Course_Id]    INT            NULL,
    [Course_Name]  VARCHAR (250)  NULL,
    [TeacherID]    INT            NULL,
    [Teacher_Name] NVARCHAR (100) NULL,
    [StudentID]    INT            NULL,
    [StudentName]  NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([GradeID] ASC)
);
CREATE TABLE [dbo].[Posts] (
    [PostID]      INT            IDENTITY (1, 1) NOT NULL,
    [CourseID]    INT            NOT NULL,
    [PostContent] NVARCHAR (MAX) NOT NULL,
    [PostDate]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [UserID]      INT            NOT NULL,
    [UserStatus]  INT            NOT NULL,
    [PostType]    VARCHAR (50)   NOT NULL,
    PRIMARY KEY CLUSTERED ([PostID] ASC)
);
CREATE TABLE [dbo].[Student] (
    [StudentID]    INT            IDENTITY (1, 1) NOT NULL,
    [Password]     NVARCHAR (50)  NOT NULL,
    [Nationality]  NVARCHAR (50)  NOT NULL,
    [MobileNumber] NVARCHAR (20)  NULL,
    [FirstName]    NVARCHAR (50)  NOT NULL,
    [Email]        NVARCHAR (100) NOT NULL,
    [LastName]     NVARCHAR (50)  NOT NULL,
    [FatherName]   NVARCHAR (50)  NULL,
    [Religion]     NVARCHAR (50)  NULL,
    [Address]      NVARCHAR (200) NULL,
    [DateOfBirth]  DATE           NULL,
    [BloodGroup]   NVARCHAR (10)  NULL,
    [Username]     NVARCHAR (50)  NOT NULL,
    [Status]       INT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([StudentID] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC)
);

CREATE TABLE [dbo].[Teacher] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [Username]         VARCHAR (250) NOT NULL,
    [Password]         VARCHAR (250) NOT NULL,
    [first_name]       VARCHAR (250) NOT NULL,
    [last_name]        VARCHAR (250) NOT NULL,
    [father_name]      VARCHAR (250) NOT NULL,
    [Email]            VARCHAR (250) NOT NULL,
    [Designation]      VARCHAR (10)  NOT NULL,
    [MaritalStatus]    VARCHAR (10)  NOT NULL,
    [MobileNumber]     VARCHAR (14)  NOT NULL,
    [Nationality]      VARCHAR (50)  NOT NULL,
    [Religion]         VARCHAR (10)  NOT NULL,
    [Address]          VARCHAR (250) NOT NULL,
    [Gender]           VARCHAR (10)  NOT NULL,
    [EducationalField] VARCHAR (50)  NOT NULL,
    [NID_PassportNo]   VARCHAR (20)  NOT NULL,
    [Status]           INT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
