-- 14 Car Rental Database
CREATE DATABASE CarRental;

USE CarRental;
CREATE TABLE Categories 
(
	Id INT PRIMARY KEY,
	CategoryName NVARCHAR(50),
	DailyRate DECIMAL(6,2),
	WeeklyRate DECIMAL(7,2),
	MonthlyRate DECIMAL(8,2),
	WeekendRate DECIMAL(7,2)
);

INSERT INTO Categories
(Id,CategoryName,DailyRate,WeeklyRate,MonthlyRate,WeekendRate)
VALUES
(1,'Limo', 153.20, 609.90, 2021.30, 199.90),
(2,'Caravan', 93.10, 409.90, 1890.50, 159.90),
(3,'Sport car', 109.90, 509.90, 1999.80, 180.90);

CREATE TABLE Cars 
(
	Id INT PRIMARY KEY,
	PlateNumber NVARCHAR(10) NOT NULL,
	Manufacturer NVARCHAR(20) NOT NULL,
	Model NVARCHAR(20) NOT NULL,
	CarYear CHAR(4) NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Doors TINYINT NOT NULL,
	Picture VARCHAR(MAX) NOT NULL,
	Condition NVARCHAR(50),
	Available BIT NOT NULL
);

INSERT INTO Cars 
(Id,PlateNumber,Manufacturer,Model,CarYear,CategoryId,Doors,Picture,Condition,Available)
VALUES
(1,'AB12234AS','Honda','Civic','2021',3,3,'/img/pic1.jpg','Perfect',0),
(2,'AD12334AS','Fiat','Traveler','1994',2,3,'/img/pic2.jpg','Perfect',1),
(3,'CB15434ES','Mercedes','S-Class','2022',1,5,'/img/pic3.jpg','Perfect',0);

CREATE TABLE Employees 
(
	Id INT PRIMARY KEY,
	FirstName NVARCHAR(20) NOT NULL,
	LastName NVARCHAR(20) NOT NULL,
	Title NVARCHAR(20) NOT NULL,
	Notes NVARCHAR(100)
);

INSERT INTO Employees
(Id,FirstName,LastName,Title,Notes)
VALUES
(1,'Ivan','Smith','Cleaner','Hard worker'),
(2,'Pepi','Johnatan','Gard','Lazy, but neccessery'),
(3,'Anna','Ivanova','Receptionist','La Bella :-)');

CREATE TABLE Customers 
(
	Id INT PRIMARY KEY,
	DriverLicenceNumber INT NOT NULL,
	FullName VARCHAR(100) NOT NULL,
	[Address] VARCHAR(200) NOT NULL,
	City VARCHAR(60) NOT NULL,
	ZIPCode INT,
	Notes NVARCHAR(100)
);

INSERT INTO Customers
(Id,DriverLicenceNumber,FullName,[Address],City,ZIPCode,Notes)
VALUES
(1,234567,'John Johnatan','Simeonovo District 34 Str.','Sofia',1000,'Unreal rich'),
(2,123456,'Anna Johnatan','Simeonovo District 34 Str.','Sofia',1000,'Unreal rich'),
(3,345678,'Mary Johnatan','Simeonovo District 34 Str.','Sofia',1000,'Unreal rich');

CREATE TABLE RentalOrders 
(
	Id INT PRIMARY KEY,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id),
	CarId INT FOREIGN KEY REFERENCES Cars(Id),
	TankLevel TINYINT NOT NULL,
	KilometrageStart INT NOT NULL,
	KilometrageEnd INT NOT NULL,
	TotalKilometrage INT NOT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	TotalDays INT NOT NULL,
	RateApplied DECIMAL(7,2) NOT NULL,
	TaxRate DECIMAL(7,2) NOT NULL,
	OrderStatus VARCHAR(50),
	Notes VARCHAR(150),
);

INSERT INTO RentalOrders
(Id,EmployeeId,CustomerId,CarId,TankLevel,KilometrageStart,KilometrageEnd,TotalKilometrage,StartDate,EndDate,TotalDays,RateApplied,TaxRate,OrderStatus,Notes)
VALUES
(1,3,3,2,45,2345,2355,10,'1/3/2022','4/3/2022',3,12.32,34.55,'Finished','Some note'),
(2,3,2,1,12,1230,1290,60,'1/3/2022','4/3/2022',3,12.32,34.55,'Finished','Some note'),
(3,3,1,3,32,1245,1327,82,'1/3/2022','4/3/2022',3,12.32,34.55,'Finished','Some note');
