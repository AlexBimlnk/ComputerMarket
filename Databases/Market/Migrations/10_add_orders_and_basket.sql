BEGIN;

UPDATE db_version SET version = 10 WHERE TRUE;

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
(5,'Ready');

DROP TABLE IF EXISTS orders;

CREATE TABLE orders(
  Id BIGSERIAL PRIMARY KEY,
  UserId BIGINT NOT NULL,
  Date DATE NOT NULL DEFAULT NOW(),
  StateId INT NOT NULL,
    FOREIGN KEY (StateId) REFERENCES order_state(Id) ON DELETE RESTRICT,
    FOREIGN KEY (UserId) REFERENCES users(Id) ON DELETE CASCADE
);

DROP TABLE IF EXISTS order_fill;

CREATE TABLE order_fill(
    OrderId BIGINT NOT NULL,
    ProviderId BIGINT NOT NULL,
    ItemId BIGINT NOT NULL,
    Quantity INT NOT NULL,
    PaidPrice DECIMAL(20,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProviderId) REFERENCES providers(Id) ON DELETE CASCADE,
    FOREIGN KEY (ItemId) REFERENCES items(Id) ON DELETE CASCADE,
    CHECK ( PaidPrice >= 0 AND Quantity > 0),
    PRIMARY KEY (OrderId, ProviderId, ItemId)
);

DROP TABLE IF EXISTS basket_items;

CREATE TABLE basket_items(
  UserId BIGINT NOT NULL,
  ProviderId BIGINT NOT NULL,
  ItemId BIGINT NOT NULL,
  Quantity INT NOT NULL,
  FOREIGN KEY (UserId) REFERENCES users(Id) ON DELETE CASCADE,
  FOREIGN KEY (ProviderId) REFERENCES providers(Id) ON DELETE CASCADE,
  FOREIGN KEY (ItemId) REFERENCES items(Id) ON DELETE CASCADE,
  CHECK ( Quantity> 0 ),
  PRIMARY KEY(UserId, ProviderId, ItemId)
);

COMMIT;