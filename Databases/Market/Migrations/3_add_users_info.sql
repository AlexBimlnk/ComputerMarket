BEGIN;

ALTER TABLE providers
ADD COLUMN Inn VARCHAR(10) NOT NULL;

DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS user_type;
DROP TABLE IF EXISTS providers_agents;

CREATE TABLE user_type(
    Id SMALLSERIAL PRIMARY KEY,
    Name varchar(8)
);

CREATE TABLE users(
    Id BIGSERIAL PRIMARY KEY,
    Login VARCHAR(20),
    Password VARCHAR(20),
    User_type_id INT NOT NULL,
        FOREIGN KEY(User_type_id) REFERENCES user_type(Id) ON DELETE RESTRICT
);

CREATE TABLE providers_agents(
    User_id INT NOT NULL,
        FOREIGN KEY(User_id) REFERENCES users(Id) ON DELETE CASCADE,
    Provider_id INT NOT NULL,
        FOREIGN KEY (Provider_id) REFERENCES  providers(Id) ON DELETE RESTRICT,
    PRIMARY KEY (User_id, Provider_id)
);

COMMIT;