BEGIN;

ALTER TABLE product
ADD PRIMARY KEY (provider_id, item_id);

DROP TRIGGER IF EXISTS trg_update_provider_product_price ON product;

ALTER TABLE product
    DROP COLUMN final_cost;

ALTER TABLE product
    RENAME COLUMN base_cost TO provider_cost;

ALTER TABLE product rename TO products;

DROP FUNCTION update_provider_product_price() CASCADE;

COMMIT;