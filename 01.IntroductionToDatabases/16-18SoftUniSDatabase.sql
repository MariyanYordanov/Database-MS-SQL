-- 16 Softuni Database
CREATE DATABASE SoftUni;
USE SoftUni

CREATE TABLE Towns
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	[Name]  NVARCHAR(100) NOT NULL,
);

CREATE TABLE Addresses 
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	AddressText NVARCHAR(200) NOT NULL,
	TownId INT FOREIGN KEY (TownId) REFERENCES Towns(Id),
);

CREATE TABLE Departments 
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(50) NOT NULL,
);

CREATE TABLE Employees 
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	FirstName NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	JobTitle NVARCHAR(50) NOT NULL,
	DepartmentId INT FOREIGN KEY (DepartmentId) REFERENCES Departments(Id),
	HireDate DATETIME,
	Salary DECIMAL(6,2),
	AddressId INT FOREIGN KEY (AddressId) REFERENCES Addresses(Id)
);

-- 17 Backup Database
USE SoftUni
BACKUP DATABASE SoftUni
TO DISK = 'H:\backups\softuni-backup.bak';

-- 18 Basic Insert
USE SoftUni
INSERT INTO Towns
VALUES
('Sofia'),
('Plovdiv'),
('Varna'),
('Burgas');

SELECT * FROM Towns;

INSERT INTO Departments
VALUES
('Engineering'),
('Sales'),
('Marketing'),
('Software Development'),
('Quality Assurance');

SELECT * FROM Departments;

INSERT INTO Employees(FirstName,MiddleName,LastName,JobTitle,DepartmentId,HireDate,Salary)
VALUES
('Ivan','Ivanov','Ivanov','.NET Developer',4,'02/01/2013',3500.00),
('Petar','Petrov','Petrov','Senior Engineer',1,'03/02/2004',4000.00),
('Maria','Petrova','Ivanova','Intern',5,'08/28/2016',525.25),
('Georgi','Teziev','Ivanov','CEO',2,'12/09/2007',3000.00),
('Peter','Pan','Pan','Intern',3,'08/28/2016',599.88);