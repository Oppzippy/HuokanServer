CREATE TYPE global_permission_level AS ENUM ('USER', 'ADMINISTRATOR');

ALTER TABLE user_account ADD COLUMN permission_level global_permission_level NOT NULL DEFAULT 'USER';
