CREATE EXTENSION IF NOT EXISTS vector;

CREATE SEQUENCE IF NOT EXISTS dialog_id_seq START 1;
CREATE SEQUENCE IF NOT EXISTS knowledge_id_seq START 1;
CREATE SEQUENCE IF NOT EXISTS  npc_id_seq START 1;
CREATE SEQUENCE IF NOT EXISTS  quest_id_seq START 1;

CREATE TABLE Dialog_history (
    dialog_id INTEGER NOT NULL DEFAULT nextval('dialog_id_seq'),
    npc_id INTEGER NOT NULL,
    dialog_content VARCHAR NOT NULL,
    dialog_vetcor vector(1536) NOT NULL,
    CONSTRAINT dialog_history_id PRIMARY KEY (dialog_id, npc_id)
);

CREATE TABLE Knowledge (
    knowledge_id INTEGER NOT NULL DEFAULT nextval('knowledge_id_seq'),
    knowledge_content VARCHAR NOT NULL,
    knowledge_vector vector(1536) NOT NULL,
    CONSTRAINT knowledge_id PRIMARY KEY (knowledge_id)
);

CREATE TABLE NPC (
    npc_id INTEGER NOT NULL DEFAULT nextval('npc_id_seq'),
    npc_name VARCHAR NOT NULL,
    personality VARCHAR NOT NULL,
    person_summarise VARCHAR NOT NULL,
    CONSTRAINT npc_id PRIMARY KEY (npc_id)
);

CREATE TABLE NPCKnowledge (
    knowledge_id INTEGER NOT NULL,
    npc_id INTEGER NOT NULL,
    CONSTRAINT npcknowledge_pk PRIMARY KEY (knowledge_id, npc_id)
);

CREATE TABLE Quest (
    quest_id INT DEFAULT nextval('quest_id_seq'),
    quest_goal VARCHAR NOT NULL,
	quest_urgency INT,
	quest_description VARCHAR,
    quest_vector vector(1536) NOT NULL,
	CONSTRAINT quest_id PRIMARY KEY (quest_id)
);

CREATE TABLE NPCQuest (
	quest_id INTEGER NOT NULL,
    npc_id INTEGER NOT NULL,
    CONSTRAINT npcquest_pk PRIMARY KEY (quest_id, npc_id)
);

ALTER TABLE NPCQuest ADD CONSTRAINT quset_npcquest_fk
FOREIGN KEY (quest_id)
REFERENCES Quest (quest_id)
ON DELETE CASCADE  
ON UPDATE NO ACTION
NOT DEFERRABLE;

ALTER TABLE NPCQuest ADD CONSTRAINT npc_npcquest_fk
FOREIGN KEY (npc_id)
REFERENCES NPC (npc_id)
ON DELETE CASCADE  
ON UPDATE NO ACTION
NOT DEFERRABLE;

ALTER TABLE NPCKnowledge ADD CONSTRAINT knowledge_npc_knowledge_fk
FOREIGN KEY (knowledge_id)
REFERENCES Knowledge (knowledge_id)
ON DELETE CASCADE  
ON UPDATE NO ACTION
NOT DEFERRABLE;

ALTER TABLE NPCKnowledge ADD CONSTRAINT npc_npc_knowledge_fk
FOREIGN KEY (npc_id)
REFERENCES NPC (npc_id)
ON DELETE CASCADE  
ON UPDATE NO ACTION
NOT DEFERRABLE;

ALTER TABLE Dialog_histroy ADD CONSTRAINT fk_npc_id
FOREIGN KEY (npc_id)
REFERENCES NPC(npc_id)
ON DELETE CASCADE
ON UPDATE NO ACTION
NOT DEFERRABLE;


INSERT INTO NPC (npc_name, personality, person_summrise) VALUES
('Alice', 'Friendly', 'Alice is a friendly and outgoing person.'),
('Bob', 'Serious', 'Bob is a serious and hardworking individual.'),
('Charlie', 'Funny', 'Charlie has a great sense of humor and is always making people laugh.'),
('David', 'Loyal', 'David is loyal and always stands by his friends.'),
('Emma', 'Adventurous', 'Emma loves adventure and exploring new places.');
