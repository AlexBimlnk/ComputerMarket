BEGIN;

UPDATE db_version SET version = 10 WHERE TRUE;

DROP TABLE IF EXISTS order_fill;
DROP TABLE IF EXISTS orders;
DROP TABLE IF EXISTS order_state;

CREATE TABLE order_state(
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(20) NOT NULL
);

INSERT INTO order_state VALUES
(1,'Cancel'),
(2,'PaymentWait'),
(3,'ProviderAnswerWait'),
(4,'ProductDeliveryWait'),
(5,'Ready'),
(6,'Recivied');



CREATE TABLE orders(
  Id BIGSERIAL PRIMARY KEY,
  user_id BIGINT NOT NULL,
  Date DATE NOT NULL DEFAULT NOW(),
  state_id INT NOT NULL,
    FOREIGN KEY (state_id) REFERENCES order_state(Id) ON DELETE RESTRICT,
    FOREIGN KEY (user_id) REFERENCES users(Id) ON DELETE CASCADE
);


CREATE TABLE order_fill(
    order_id BIGINT NOT NULL,
    provider_id BIGINT NOT NULL,
    item_id BIGINT NOT NULL,
    Quantity INT NOT NULL,
    Paid_Price DECIMAL(20,2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (provider_id) REFERENCES providers(Id) ON DELETE CASCADE,
    FOREIGN KEY (item_id) REFERENCES items(Id) ON DELETE CASCADE,
    CHECK ( Paid_Price >= 0 AND Quantity > 0),
    PRIMARY KEY (order_id, provider_id, item_id)
);

DROP TABLE IF EXISTS basket_items;

CREATE TABLE basket_items(
  user_id BIGINT NOT NULL,
  provider_id BIGINT NOT NULL,
  item_id BIGINT NOT NULL,
  Quantity INT NOT NULL,
  FOREIGN KEY (user_id) REFERENCES users(Id) ON DELETE CASCADE,
  FOREIGN KEY (provider_id) REFERENCES providers(Id) ON DELETE CASCADE,
  FOREIGN KEY (item_id) REFERENCES items(Id) ON DELETE CASCADE,
  CHECK ( Quantity> 0 ),
  PRIMARY KEY(user_id, provider_id, item_id)
);

COMMIT;