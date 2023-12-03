\c import
BEGIN;

ALTER TABLE histories
DROP COLUMN product_name;

ALTER TABLE histories
DROP COLUMN product_description;

ALTER TABLE histories
ADD product_metadata CHARACTER VARYING(80) DEFAULT NULL;

COMMIT;