\c import
INSERT INTO providers(provider_name)
VALUES ('Ivanov');

INSERT INTO links(internal_id, external_id, provider_id)
VALUES (1, 2, 1);

INSERT INTO histories(external_id, provider_id, product_metadata)
VALUES (2, 1, 'Intel Pentium G4560 LGA 1151');