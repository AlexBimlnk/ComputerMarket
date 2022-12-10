BEGIN;

UPDATE db_version SET version = 11 WHERE TRUE;

ALTER TABLE order_fill
DROP CONSTRAINT IF EXISTS order_fill_item_id_fkey;

ALTER TABLE order_fill
DROP CONSTRAINT IF EXISTS order_fill_provider_id_fkey;

ALTER TABLE order_fill
ADD CONSTRAINT order_fill_product_id_fkey FOREIGN KEY (item_id, provider_id) REFERENCES products(item_id, provider_id);

ALTER TABLE basket_items
DROP CONSTRAINT IF EXISTS basket_items_item_id_fkey;

ALTER TABLE basket_items
DROP CONSTRAINT IF EXISTS basket_items_provider_id_fkey;

ALTER TABLE basket_items
ADD CONSTRAINT  basket_items_product_id_fkey FOREIGN KEY (item_id, provider_id) REFERENCES products(item_id, provider_id);

COMMIT;