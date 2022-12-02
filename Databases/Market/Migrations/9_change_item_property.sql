BEGIN;

UPDATE db_version SET version = 9 WHERE TRUE;

DROP TABLE IF EXISTS property_data_type;

CREATE TABLE property_data_type(
    Id SMALLSERIAL PRIMARY KEY,
    Name VARCHAR(10));

INSERT INTO property_data_type(Id, Name) VALUES (1, 'string'), (2, 'int');

ALTER TABLE item_properties
DROP COLUMN property_data_type;

ALTER TABLE item_properties
ADD COLUMN data_type_id INT NOT NULL DEFAULT 1;

ALTER TABLE item_properties
ADD CONSTRAINT property_data_type_id_fkey FOREIGN KEY (data_type_id) REFERENCES property_data_type(Id);

COMMIT;