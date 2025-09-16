use librarydb;

SELECT * from books;
SELECT * from members;
SELECT * from loans;

-- delete loans table
drop table loans;

-- delete all rows from loans table
delete from loans;

create table loans (
Id INT NOT NULL AUTO_INCREMENT,
BookId int NOT null,
MemberId int NOT null,
LoanDate date NOT NULL,
ReturnDate date,
PRIMARY KEY (Id),
FOREIGN KEY(BookId) references books(Id) on delete cascade,
FOREIGN KEY(MemberId) references members(Id) on delete cascade);