use UnivDB

create table Universities (
	uni_id int identity(1, 1),
	uni_name varchar(50),
	uni_location varchar(50),
	uni_score float, 
	uni_descr varchar(500),
	[user_id] nvarchar(450),
	constraint PK_uni_id primary key (uni_id)
)

create table Faculties (
	facult_id int identity(1, 1),
	facult_name varchar(50),
	facult_nostud int,
	uni_id int not null,
	[user_id] nvarchar(450),
	constraint PK_facult_id primary key (facult_id),
	constraint FK_uni_id foreign key (uni_id) references Universities(uni_id) on delete cascade
)


alter table [dbo].[Universities]
add constraint FK_user_id foreign key ([user_id]) references [dbo].[AspNetUsers](Id) on delete set null

alter table [dbo].[Faculties]
add constraint FK_user_id foreign key ([user_id]) references [dbo].[AspNetUsers](Id) on delete set null
