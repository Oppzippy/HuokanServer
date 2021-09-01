CREATE TABLE deposit_node (
	node_id INTEGER PRIMARY KEY REFERENCES graph_node(id),
	external_id UUID UNIQUE NOT NULL DEFAULT gen_random_uuid(),
	character_name TEXT NOT NULL,
	deposit_in_copper BIGINT NOT NULL,
	guild_bank_copper BIGINT NOT NULL
);

CREATE INDEX deposit_node_index ON deposit_node (character_name, deposit_in_copper, guild_bank_copper);

CREATE TABLE deposit_node_endorsement (
	id SERIAL PRIMARY KEY,
	node_id INTEGER NOT NULL REFERENCES graph_node(id),
	user_id CHARACTER VARYING(50) NOT NULL,
	created_at TIMESTAMP NOT NULL,
	UNIQUE(node_id, user_id)
);
