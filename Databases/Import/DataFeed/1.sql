\c import
BEGIN;

INSERT INTO providers(id, provider_name)
VALUES (2, 'Horns and Hooves');

INSERT INTO links(internal_id, external_id, provider_id)
VALUES (1, 3, 2);

INSERT INTO histories(external_id, provider_id, product_metadata)
VALUES (3, 2, 'AMD RYZEN 5600 AM4');

COMMIT;