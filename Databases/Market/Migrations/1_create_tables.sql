
begin;

DROP TABLE IF EXISTS items;
DROP TABLE IF EXISTS item_type;
DROP TABLE IF EXISTS item_properties;
DROP TABLE IF EXISTS property_group;


CREATE TABLE property_group(Id int primary key, Name varchar(20));

CREATE TABLE item_properties(
    Id int primary key,
    Name varchar(20) not null,
    Group_Id int null default null,
        foreign key (Group_Id) references property_group(Id),
    Is_Filterable bool not null default false,
    PropertyDataType varchar(10) not null default 'varchar'
    );

CREATE TABLE item_type(
    Id int primary key,
    Name varchar(20)
);

CREATE TABLE items(
    Id int primary key,
    Name varchar(40),
    Type_Id int not null ,
        foreign key (Type_Id) references item_type(Id)

);



commit;