\c market
BEGIN;

ALTER TABLE item_description
ADD PRIMARY KEY (item_id, property_id);

ALTER TABLE item_type_properties
ADD PRIMARY KEY (type_id, property_id);

COMMIT;