begin;

DROP TABLE IF EXISTS item_description;
DROP TABLE IF EXISTS product;
DROP TABLE IF EXISTS providers;
DROP TABLE IF EXISTS items;
DROP TABLE IF EXISTS type_description;
DROP TABLE IF EXISTS item_type;
DROP TABLE IF EXISTS item_properties;
DROP TABLE IF EXISTS property_group;


CREATE TABLE property_group(Id BIGSERIAL primary key, Name varchar(40));

CREATE TABLE item_properties(
    Id BIGSERIAL primary key,
    Name varchar(40) not null,
    Group_id int null default null,
        foreign key (Group_id) references property_group(Id),
    Is_filterable bool not null default false,
    Property_data_type varchar(10) not null default 'varchar'
    );

CREATE TABLE item_type(
    Id BIGSERIAL primary key,
    Name varchar(40)
);

CREATE TABLE items(
    Id BIGSERIAL primary key,
    Name varchar(40),
    Type_id int not null ,
        foreign key (Type_id) references item_type(Id)

);

CREATE TABLE type_description(
    Type_id int not null,
        foreign key (Type_id) references item_type(Id),
    Property_id int not null,
        foreign key (Property_id) references item_properties(Id)
);

CREATE TABLE item_description(
    Item_id int not null,
        foreign key (Item_id) references items(Id),
    Property_Id int not null,
        foreign key (Property_Id) references item_properties(Id),
    Property_value varchar(40) null default null
);

CREATE TABLE providers(
    Id bigserial primary key,
    Name varchar(30) not null,
    Margin decimal(5,4) not null default 1.0000,
    Bank_account varchar(20) not null
);


CREATE TABLE product(
    Item_id int not null,
        foreign key (Item_id) references items(Id),
    Base_cost decimal(20,2) not null,
    Final_cost decimal(20,2) not null,
    Quantity int not null default 0,
    Provider_id int not null,
        foreign key (Provider_id) references providers(Id)
);

commit;