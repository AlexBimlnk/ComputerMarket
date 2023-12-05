\c market
BEGIN;

DROP TABLE IF EXISTS db_version;

CREATE TABLE db_version(
    version INT NOT NULL,
	update_date TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

INSERT INTO db_version
VALUES (8);

COMMIT;