CREATE TABLE organization (
	id SERIAL PRIMARY KEY,
	'name' TEXT NOT NULL,
	slug TEXT NOT NULL UNIQUE,
	discord_guild_id NUMERIC NOT NULL UNIQUE,
	created_at TIMESTAMP NOT NULL
);

CREATE TABLE guild (
	id SERIAL PRIMARY KEY,
	organization_id INTEGER NOT NULL REFERENCES organization(id),
	'name' TEXT NOT NULL,
	realm TEXT NOT NULL,
	created_at TIMESTAMP NOT NULL,
	deleted_at TIMESTAMP NULL,
	is_not_deleted BOOLEAN GENERATED ALWAYS AS (deleted_at IS NULL) STORED,
	UNIQUE(organization_id, 'name', realm, is_not_deleted)
);

CREATE TABLE user (
	id SERIAL PRIMARY KEY,
	organization_id INTEGER NOT NULL REFERENCES organization(id),
	discord_user_id NUMERIC NOT NULL,
	discord_token TEXT NULL,
	created_at TIMESTAMP NOT NULL,
	UNIQUE(organization_id, discord_user_id)
);

ALTER TABLE deposit_node_endorsement ALTER COLUMN user_id TYPE INTEGER USING user_id::INTEGER;
ALTER TABLE deposit_node_endorsement ALTER COLUMN user_id SET NOT NULL;
