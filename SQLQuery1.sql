CREATE DATABASE HaatBazar

Use HaatBazar


CREATE TABLE OnlineShop(
ShopId int primary key,
ShopName varchar(50) not null,
ShopeAdd varchar(50) not null,
ShopPhone varchar(15) not null
);

//---------------Admin Table----------------

CREATE TABLE admin_table 
(
admin_id int identity primary key,
admin_username varchar(20) not null unique,
admin_pass varchar(20) not null 
)

insert into admin_table values
('musfiq','shahed2020'),
('ziaul','ziaul1234'),
('zarin','zarin1234'),
('sayma','sayma1234');

select * from admin_table

//---------------Catagories Table----------------

create table catagories(
catId int identity(1,1) primary key,
catName varchar(50) not null unique,
catImage nvarchar(max) not null,
catStatus int not null,
admin_id int foreign key references admin_table(admin_id)
)


//---------------Customer Table----------------

create table customers(
customerId int identity(1,1) primary key,
customerName varchar(50) not null,
customerEmail varchar(50) null unique,
customerPhone varchar(50) not null unique,
customerPass varchar(50) not null,
customerImage varchar(50) 
)

---------------Product Table----------------

CREATE TABLE Products(
ProductId int identity(1,1) primary key,
ProductName varchar(50) not null,
ProductImage varchar(max) not null,
ProductDes varchar(max) not null,
ProductPrice int not null,
ProductStock int not null,
Discount int not null,
CatagorieId int not null foreign key references Catagories(catId),
admin_id int foreign key references admin_table(admin_id)
);

CREATE TABLE Courrier(
CourrierId int identity(1,1) primary key,
CourrierName varchar(50) not null,
CourrierPhone varchar(50) not null,
CourrierAddress varchar(50) not null
);

CREATE TABLE Orders(
OrderId int identity(1,1) primary key,
OrderDate date not null,
Quantity int not null,
ProductId int not null foreign key references Products(ProductId),
InvoiceId int not null foreign key references Invoice(InvoiceId),
Bill int not null,
UnitPrice int not null
);


CREATE TABLE Payment(
PaymentId int identity(1,1) primary key,
CustomerId int not null foreign key references Customers(CustomerId),
OrderId int not null foreign key references Orders(OrderId),
PaymentDate date not null,
PaymentMethod varchar(50) not null,
PaymentAmount int not null,
PaymentDescription varchar(50) not null
);


CREATE TABLE Invoice(
InvoiceId int identity(1,1) primary key,
CustomerId int not null foreign key references Customers(CustomerId),
InvoiceDate date not null,
TotalBill int not null,

);

select * from OnlineShop
select * from Customers
select * from Products
select * from admin_table
select * from Catagories
select * from Orders
select * from Courrier
select * from Payment
select * from Invoice

Truncate table Catagories

delete from catagories where admin_id = 2

drop table Orders
drop table catagories
drop table Payment
drop table Products
Drop table customers
Drop table Courrier
