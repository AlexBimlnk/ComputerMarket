\c import
BEGIN;

ALTER TABLE histories
ALTER COLUMN provider_id TYPE SMALLINT;

ALTER TABLE histories
ALTER COLUMN external_id TYPE BIGINT;

COMMIT;