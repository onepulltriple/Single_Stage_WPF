USE SINGLE_STAGE
GO

DELETE FROM SEATROWS
INSERT INTO SEATROWS
	(Row)
	VALUES
	('A'),
	('B'),
	('C'),
	('D'),
	('E'),
	('F'),
	('G'),
	('H'),
	('I'),
	('J'),
	('K')

SELECT * FROM SEATROWS ORDER BY id


DELETE FROM SEATNUMBERS
INSERT INTO SEATNUMBERS
	(Number)
	VALUES
	('01'),('02'),('03'),('04'),('05'),
	('06'),('07'),('08'),('09'),('10'),
	('11'),('12'),('13'),('14'),('15'),
	('16'),('17'),('18'),('19'),('20'),
	('21'),('22'),('23'),('24'),('25')

SELECT * FROM SEATNUMBERS ORDER BY id
