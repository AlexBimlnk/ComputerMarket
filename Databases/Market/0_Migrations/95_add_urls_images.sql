\c market
BEGIN;
UPDATE db_version SET version = 14 WHERE TRUE;

ALTER TABLE item_type
DROP COLUMN IF EXISTS url_image;

ALTER TABLE item_type
ADD COLUMN url_image TEXT NULL;

ALTER TABLE items
DROP COLUMN IF EXISTS url_image;

ALTER TABLE items
ADD COLUMN url_image TEXT NULL;
COMMIT;