CREATE EXTENSION vector;

CREATE TABLE Dialog_histroy (
                dialog_id INTEGER NOT NULL,
                npc_id INTEGER NOT NULL,
                dialog_content VARCHAR NOT NULL,
                dialog_vetcor vector(1536) NOT NULL,
                CONSTRAINT dialog_histroy_id PRIMARY KEY (dialog_id, npc_id)
);


CREATE TABLE Konwledge (
                konwledge_id INTEGER NOT NULL,
                konwledge_content VARCHAR NOT NULL,
                konwledge_vector vector(1536) NOT NULL,
                CONSTRAINT konwledge_id PRIMARY KEY (konwledge_id)
);


CREATE TABLE NPC (
                npc_id INTEGER NOT NULL,
                name VARCHAR NOT NULL,
                personality VARCHAR NOT NULL,
                person_summrise VARCHAR NOT NULL,
                CONSTRAINT npc_id PRIMARY KEY (npc_id)
);


CREATE TABLE NPCKonwledge (
                konwledge_id INTEGER NOT NULL,
                npc_id INTEGER NOT NULL,
                CONSTRAINT npckonwledge_pk PRIMARY KEY (konwledge_id, npc_id)
);

ALTER TABLE NPCKonwledge ADD CONSTRAINT konwledge_npc_konwledge_fk
FOREIGN KEY (konwledge_id)
REFERENCES Konwledge (konwledge_id)
ON DELETE CASCADE  
ON UPDATE NO ACTION
NOT DEFERRABLE;

ALTER TABLE NPCKonwledge ADD CONSTRAINT npc_npc_konwledge_fk
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