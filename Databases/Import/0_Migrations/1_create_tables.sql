\c import
BEGIN;

CREATE TABLE providers
(
    id SMALLSERIAL PRIMARY KEY,
    provider_name CHARACTER VARYING(30) NOT NULL
);

CREATE TABLE links
(
    internal_id INTEGER NOT NULL,
	external_id INTEGER NOT NULL,
    provider_id INTEGER NOT NULL,
	FOREIGN KEY (provider_id) REFERENCES providers (id) ON DELETE CASCADE
);

CREATE TABLE histories
(
	external_id INTEGER NOT NULL,
    provider_id INTEGER NOT NULL,
	product_name CHARACTER VARYING(40) NOT NULL,
	product_description CHARACTER VARYING(50) DEFAULT NULL,
	receive_date TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

COMMIT;