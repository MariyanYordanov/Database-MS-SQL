-- 8 Create Table Users
CREATE TABLE Users
(
	Id BIGINT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	ProfilePicture VARCHAR(MAX),
	LastLoginTime DATETIME,
	IsDeleted BIT
);

INSERT INTO Users (Username, [Password], ProfilePicture, LastLoginTime, IsDeleted)
VALUES 
('Ivan Ivanov', 'password123456', '/img/pic1.jpg', '2001-01-15', 0),
('Anna Petrova', 'pass23456', '/img/pic2.jpg', '2002-11-23', 0),
('Maria Ivanova', '123456', '/img/pic3.jpg', '2001-12-12', 0),
('Stoyan Petrov', 'password', '/img/pic4.jpg', '2003-09-24', 0),
('Iva Stefanova', 'pass3456', '/img/pic5.jpg', '2002-05-01', 0)
;

SELECT * FROM Users;

-- 9 Change Primary Key
ALTER TABLE Users DROP CONSTRAINT [PK__Users__3214EC071BBD9FB2];
ALTER TABLE Users ADD CONSTRAINT PK_IdUsername PRIMARY KEY (Id, Username);

-- 10 Add Check Constraint
ALTER TABLE Users ADD CONSTRAINT CH_IsPasswordAreMoreThanFiveChars CHECK (LEN([Password]) >= 5);

-- 11 Set Default Value of a Field
ALTER TABLE Users ADD CONSTRAINT DF_LastLoginTime DEFAULT GETDATE() FOR LastLoginTime;

-- 12 Set Unique Field
ALTER TABLE Users DROP CONSTRAINT [PK_IdUsername];
ALTER TABLE Users ADD CONSTRAINT PK_Id PRIMARY KEY (Id);
ALTER TABLE Users ADD CONSTRAINT UN_Username UNIQUE (Username);
ALTER TABLE Users ADD CONSTRAINT CH_UsernameLenght CHECK (LEN(Username) >= 3);