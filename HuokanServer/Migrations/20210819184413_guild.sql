CREATE TABLE organization (
	id SERIAL PRIMARY KEY,
	external_id UUID UNIQUE NOT NULL DEFAULT gen_random_uuid(),
	"name" TEXT NOT NULL,
	slug TEXT NOT NULL UNIQUE,
	discord_guild_id NUMERIC NOT NULL UNIQUE,
	created_at TIMESTAMP NOT NULL
);

CREATE TABLE guild (
	id SERIAL PRIMARY KEY,
	external_id UUID UNIQUE NOT NULL DEFAULT gen_random_uuid(),
	organization_id INTEGER NOT NULL REFERENCES organization(id),
	"name" TEXT NOT NULL,
	realm TEXT NOT NULL,
	guild_bank_graph_id INTEGER NULL REFERENCES graph(id),
	created_at TIMESTAMP NOT NULL,
	deleted_at TIMESTAMP NULL,
	is_not_deleted BOOLEAN GENERATED ALWAYS AS (deleted_at IS NULL) STORED,
	UNIQUE(organization_id, "name", realm, is_not_deleted)
);


CREATE TABLE user_account (
	id SERIAL PRIMARY KEY,
	external_id UUID UNIQUE NOT NULL DEFAULT gen_random_uuid(),
	discord_user_id NUMERIC UNIQUE NOT NULL,
	created_at TIMESTAMP NOT NULL
);

CREATE TABLE user_discord_token (
	id SERIAL PRIMARY KEY,
	user_id INTEGER UNIQUE NOT NULL REFERENCES user_account(id),
	token TEXT NULL,
	refresh_token TEXT NULL,
	expires_at TIMESTAMP NOT NULL,
	created_at TIMESTAMP NOT NULL
);

CREATE TABLE api_key (
	id SERIAL PRIMARY KEY,
	external_id UUID UNIQUE NOT NULL DEFAULT gen_random_uuid(),
	hashed_key TEXT NOT NULL UNIQUE,
	user_id INTEGER NOT NULL REFERENCES user_account(id),
	created_at TIMESTAMP NOT NULL,
	expires_at TIMESTAMP NULL
);

CREATE VIEW unexpired_api_key AS SELECT * FROM api_key WHERE expires_at IS NULL OR expires_at < NOW() AT TIME ZONE 'UTC';

CREATE TABLE organization_user_membership (
	id SERIAL PRIMARY KEY,
	organization_id INTEGER NOT NULL REFERENCES organization(id),
	user_id INTEGER NOT NULL REFERENCES user_account(id),
	UNIQUE(organization_id, user_id)
);

CREATE INDEX organization_user_membership_reverse_index ON organization_user_membership (user_id, organization_id);

ALTER TABLE deposit_node_endorsement ALTER COLUMN user_id TYPE INTEGER USING user_id::INTEGER;
ALTER TABLE deposit_node_endorsement ALTER COLUMN user_id SET NOT NULL;
