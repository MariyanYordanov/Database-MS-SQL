CREATE DATABASE Hotel;

USE Hotel
CREATE TABLE Employees 
(
	Id INT PRIMARY KEY,
	FirstName VARCHAR(20) NOT NULL,
	LastName VARCHAR(20) NOT NULL,
	Title VARCHAR(20) NOT NULL,
	Notes VARCHAR(20)
);

INSERT INTO Employees
VALUES
(1,'Ivan','Ivanov','Worker','Something'),
(2,'Ivan','Ivanov','Worker','Something'),
(3,'Ivan','Ivanov','Worker','Something');

CREATE TABLE Customers 
(
	AccountNumber INT PRIMARY KEY,
	FirstName VARCHAR(20) NOT NULL,
	LastName VARCHAR(20) NOT NULL,
	PhoneNumber INT NOT NULL,
	EmergencyName VARCHAR(50) NOT NULL,
	EmergencyNumber VARCHAR(50) NOT NULL,
	Notes  VARCHAR(150)
);

INSERT INTO Customers 
VALUES
(1122,'Anna','Smith',0878123456,'Pep Smith',0877324252,'Something'),
(2212,'Boby','Johnes',0878213243,'Valery',08871298475,'Something'),
(3213,'Ceci','Ivanova',0878322143,'John',0987142356,'Something');

CREATE TABLE RoomStatus 
(
	RoomStatus CHAR(20) PRIMARY KEY,
	Notes VARCHAR(50)
);

INSERT INTO RoomStatus
VALUES
('Free','Clean'),
('Cleaning','Mess'),
('Occupied',NULL);

CREATE TABLE RoomTypes 
(
	RoomType CHAR(20) PRIMARY KEY,
	Notes VARCHAR(50)
);

INSERT INTO RoomTypes
VALUES
('Bedroom','Room for two people'),
('Apartment','Full extras'),
('Single bed','Another bed is option');

CREATE TABLE BedTypes 
(
	BedType CHAR(20) PRIMARY KEY,
	Notes VARCHAR(50)
);

INSERT INTO BedTypes
VALUES
('One person','200/144 sm'),
('Two person','200/200 sm'),
('King size', '250/350 sm');

CREATE TABLE Rooms
(
	RoomNumber INT PRIMARY KEY,
	RoomType CHAR(20) FOREIGN KEY (RoomType) REFERENCES RoomTypes(RoomType),
	BedType CHAR(20) FOREIGN KEY (BedType) REFERENCES BedTypes(BedType),
	Rate TINYINT NOT NULL,
	RoomStatus CHAR(20) FOREIGN KEY (RoomStatus) REFERENCES RoomStatus(RoomStatus),
	Notes VARCHAR(50)
);

INSERT INTO Rooms
VALUES
(101,'Bedroom','Two person',2,'Free','Something'),
(102,'Apartment','King size',1,'Cleaning','Something else'),
(103,'Single bed','One person',3,'Occupied',NULL);

CREATE TABLE Payments 
(
	Id INT PRIMARY KEY,
	EmployeeId INT FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
	PaymentDate DATETIME,
	AccountNumber INT FOREIGN KEY (AccountNumber) REFERENCES Customers(AccountNumber),
	FirstDateOccupied DATETIME NOT NULL,
	LastDateOccupied DATETIME NOT NULL,
	TotalDays INT NOT NULL,
	AmountCharged DECIMAL(7,2),
	TaxRate DECIMAL(7,2),
	TaxAmount DECIMAL(7,2),
	PaymentTotal DECIMAL(7,2),
	Notes VARCHAR(50)
);

INSERT INTO Payments
VALUES
(1,1,'6/2/2022',1122,'3/2/2022','6/2/2022',3,40.00,4.20,1.20,125.40,'Cash'),
(2,2,'12/2/2022',2212,'4/2/2022','12/2/2022',8,50.00,8.00,1.50,409.50,'Cash'),
(3,3,'5/2/2022',3213,'2/2/2022','5/2/2022',3,80.00,12.20,2.50,174.70,'With card');

CREATE TABLE Occupancies 
(
	Id INT PRIMARY KEY,
	EmployeeId INT FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
	DateOccupied DATETIME,
	AccountNumber INT FOREIGN KEY (AccountNumber) REFERENCES Customers(AccountNumber),
	RoomNumber INT FOREIGN KEY (RoomNumber) REFERENCES Rooms(RoomNumber),
	RateApplied TINYINT NOT NULL,
	PhoneCharge DECIMAL(7,2),
	Notes VARCHAR(20)
);

INSERT INTO Occupancies
VALUES
(1,1,'6/2/2022',1122,101,2,0.00,NULL),
(2,2,'5/2/2022',2212,102,1,0.00,NULL),
(3,3,'6/2/2022',3213,103,3,10.55,'Phone is used');