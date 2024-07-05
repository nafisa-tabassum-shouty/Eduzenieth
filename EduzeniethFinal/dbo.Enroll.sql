CREATE TABLE [dbo].[Enroll] (
    [id]     INT IDENTITY (1, 1) NOT NULL,
    [sid]    INT NOT NULL,
    [cid]    INT NOT NULL,
    [status] INT DEFAULT ((0)) NULL,
    [role]   INT DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);