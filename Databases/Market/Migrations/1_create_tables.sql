BEGIN;

DROP TABLE IF EXISTS item_description;
DROP TABLE IF EXISTS product;
DROP TABLE IF EXISTS providers;
DROP TABLE IF EXISTS items;
DROP TABLE IF EXISTS type_description;
DROP TABLE IF EXISTS item_type;
DROP TABLE IF EXISTS item_properties;
DROP TABLE IF EXISTS property_group;


CREATE TABLE property_group(Id BIGSERIAL PRIMARY KEY, Name VARCHAR(40));

CREATE TABLE item_properties(
    Id BIGSERIAL PRIMARY KEY ,
    Name VARCHAR(40) NOT NULL,
    Group_id INT NULL DEFAULT NULL,
        FOREIGN KEY (Group_id) REFERENCES property_group(Id) ON DELETE SET DEFAULT,
    Is_filterable BOOL NOT NULL DEFAULT FALSE,
    Property_data_type VARCHAR(10) NOT NULL DEFAULT 'varchar'
    );

CREATE TABLE item_type(
    Id BIGSERIAL PRIMARY KEY,
    Name VARCHAR(40)
);

CREATE TABLE items(
    Id BIGSERIAL PRIMARY KEY,
    Name VARCHAR(40),
    Type_id INT NOT NULL,
        FOREIGN KEY (Type_id) REFERENCES item_type(Id) ON DELETE RESTRICT

);

CREATE TABLE type_description(
    Type_id INT NOT NULL,
        FOREIGN KEY (Type_id) REFERENCES item_type(Id) ON DELETE RESTRICT,
    Property_id INT NOT NULL ,
        FOREIGN KEY (Property_id) REFERENCES item_properties(Id) ON DELETE CASCADE
);

CREATE TABLE item_description(
    Item_id INT NOT NULL ,
        FOREIGN KEY (Item_id) REFERENCES items(Id) ON DELETE RESTRICT,
    Property_Id INT NOT NULL ,
        FOREIGN KEY (Property_Id) REFERENCES item_properties(Id) ON DELETE CASCADE,
    Property_value VARCHAR(40) NULL DEFAULT NULL
);

CREATE TABLE providers(
    Id BIGSERIAL PRIMARY KEY,
    Name VARCHAR(30) NOT NULL,
    Margin DECIMAL(5,4) NOT NULL DEFAULT 1.0000,
    Bank_account varchar(20) NOT NULL,
    CHECK ( Margin >= 1 )
);


CREATE TABLE product(
    Item_id INT NOT NULL ,
        FOREIGN KEY (Item_id) REFERENCES items(Id),
    Base_cost DECIMAL(20,2) NOT NULL ,
    Final_cost DECIMAL(20,2) NOT NULL ,
    Quantity INT NOT NULL DEFAULT 0,
    Provider_id INT NOT NULL,
        FOREIGN KEY (Provider_id) REFERENCES providers(Id) ON DELETE CASCADE,
    CHECK ( Base_cost >= 0 AND Final_cost >= 0 AND Quantity >= 0)
);

COMMIT;