begin;

CREATE or REPLACE FUNCTION update_provider_product_price()
RETURNS TRIGGER
AS
$trg$
BEGIN
	UPDATE product
        SET final_cost = tg_argv[0] * base_cost
            WHERE product.provider_id = tg_argv[0];
END
$trg$
LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_update_provider_product_price ON providers ;
CREATE TRIGGER trg_update_provider_product_price AFTER UPDATE OF margin ON providers
    FOR EACH ROW
    EXECUTE PROCEDURE update_provider_product_price(margin, id);

commit;