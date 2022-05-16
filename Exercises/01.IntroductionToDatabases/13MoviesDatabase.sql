-- 13 Movies Database
CREATE DATABASE Movies;

USE Movies
CREATE TABLE Directors
(
	Id INT PRIMARY KEY NOT NULL,
	DirectorName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX),
);

USE Movies
CREATE TABLE Genres 
(
	Id INT PRIMARY KEY NOT NULL,
	GenreName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX),
);

USE Movies
CREATE TABLE Categories  
(
	Id INT PRIMARY KEY NOT NULL,
	CategoryName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX),
);

USE Movies
CREATE TABLE Movies   
(
	Id INT PRIMARY KEY NOT NULL,
	Title NVARCHAR(200) NOT NULL,
	DirectorId INT NOT NULL,
	CopyrightYear INT NOT NULL,
	[Length] TIME NOT NULL,
	GenreId INT NOT NULL,
	CategoryId INT,
	Rating TINYINT NOT NULL,
	Notes NVARCHAR(MAX),
);

INSERT INTO Directors (Id,DirectorName,Notes)
VALUES
(1, 'Frank Darabont', 'The Shawshank Redemption'),
(2, 'Francis Ford Copola', 'The Godfhather'),
(3, 'Christopher Nolan', 'The Dark Knight'),
(4, 'Peter Jackson', 'The Lord of the Rings'),
(5, 'Steven Spielberg', 'Schindlers List');

INSERT INTO Genres (Id,GenreName)
VALUES
(1, 'Drama'),
(2, 'Action'),
(3, 'Biography'),
(4, 'Adventure'),
(5, 'Crime');

INSERT INTO Categories (Id,CategoryName,Notes)
VALUES
(1, 'Horor', 'Film that seeks to elicit fear or disgust in its audience for entertainment purposes'),
(2, 'Fiction', 'Science fiction (sometimes shortened to sci-fi or SF) is a genre of speculative fiction which typically deals with imaginative and futuristic concepts such as advanced science and technology, space exploration, time travel, parallel universes, and extraterrestrial life. It has been called the "literature of ideas", and it often explores the potential consequences of scientific, social, and technological innovations'),
(3, 'Art', 'An art film (or art house film) is typically an independent film, aimed at a niche market rather than a mass market audience.[1] It is "intended to be a serious, artistic work, often experimental and not designed for mass appeal",[2] "made primarily for aesthetic reasons rather than commercial profit",[3] and contains "unconventional or highly symbolic content"'),
(4, 'Musical', 'Musical film is a film genre in which songs by the characters are interwoven into the narrative, sometimes accompanied by singing and dancing. The songs usually advance the plot or develop the films characters, but in some cases, they serve merely as breaks in the storyline, often as elaborate "production numbers".'),
(5, 'Amateur', 'Amateur film is the low-budget hobbyist art of film practised for passion and enjoyment and not for business purposes.');

ALTER TABLE Movies ALTER COLUMN Rating DECIMAL(3,1);

INSERT INTO Movies (Id,Title,DirectorId,CopyrightYear,[Length],GenreId,CategoryId,Rating,Notes)
VALUES
(1, 'The Shawshank Redemption', 1, 1994, '01:30:01', 1, NULL, 9.3,
'Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.'),
(2, 'The Godfhather', 2, 1972, '02:15:21', 5, 1, 9.2,
'The aging patriarch of an organized crime dynasty in postwar New York City transfers control of his clandestine empire to his reluctant youngest son.'),
(3, 'The Dark Knight', 3, 2008, '02:32:12', 2, 2, 9.0,
'When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, 
Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.'),
(4,'The Lord of the Rings', 4, 2003, '03:21:36', 4, 2, 9.0,
'Gandalf and Aragorn lead the World of Men against Sauron`s army to draw his gaze from Frodo and Sam as they approach Mount Doom with the One Ring.'),
(5, 'Schindlers List', 5, 1993, '03:11:56', 1, 1, 9.0,
'In German-occupied Poland during World War II, industrialist Oskar Schindler gradually becomes concerned for his Jewish workforce after witnessing their persecution by the Nazis.');

SELECT * FROM Movies;