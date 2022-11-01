BEGIN;

ALTER TABLE property_group
ALTER id TYPE INTEGER;

ALTER TABLE item_TYPE
ALTER id TYPE INTEGER;

ALTER TABLE items
ALTER name SET NOT NULL;

ALTER TABLE item_TYPE_properties
ALTER property_id TYPE BIGINT;

ALTER TABLE item_description
ALTER item_id TYPE BIGINT;

ALTER TABLE item_description
ALTER property_id TYPE BIGINT;

ALTER TABLE products
ALTER item_id TYPE BIGINT;

ALTER TABLE products
ALTER provider_id TYPE BIGINT;

ALTER TABLE users
ALTER user_TYPE_id TYPE smallint;

ALTER TABLE providers_agents
ALTER user_id TYPE BIGINT;

ALTER TABLE providers_agents
ALTER provider_id TYPE BIGINT;

ALTER TABLE providers_agents
ADD CONSTRAINT unique_user_check UNIQUE (user_id);

COMMIT;