\c market
BEGIN;

UPDATE db_version SET version = 12 WHERE TRUE;

ALTER TABLE providers ADD COLUMN IF NOT EXISTS is_approved BOOl DEFAULT FALSE;

COMMIT;