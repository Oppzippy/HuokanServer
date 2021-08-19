CREATE TABLE graph (
	id SERIAL PRIMARY KEY
);

CREATE TABLE graph_node (
	id SERIAL PRIMARY KEY,
	graph_id INTEGER NOT NULL REFERENCES graph(id),
	created_at TIMESTAMP NOT NULL
);

CREATE TABLE graph_edge (
	id SERIAL PRIMARY KEY,
	start_node_id INTEGER NOT NULL REFERENCES graph_node(id),
	end_node_id INTEGER NOT NULL REFERENCES graph_node(id),
	created_at TIMESTAMP NOT NULL
);
