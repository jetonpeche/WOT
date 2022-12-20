DROP TABLE ClanWarJoueur;
DROP TABLE TankJoueur;
DROP TABLE Tank;
DROP TABLE Joueur;
DROP TABLE Tier;
DROP TABLE TypeTank;
DROP TABLE TankStatut;
DROP TABLE ClanWar;

-- cascade sur joueur et clan war

CREATE TABLE Tier
(
    id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    nom varchar(10) NOT NULL
);

CREATE TABLE TypeTank
(
    id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,

    nom varchar(20) NOT NULL,
    nomImage varchar(50) NOT NULL
);

CREATE TABLE TankStatut
(
    id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    nom varchar(100) NOT NULL
);

CREATE TABLE ClanWar
(
    id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    date date NOT NULL
);

CREATE TABLE Tank
(
    id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,

    idTier int NOT NULL,

    -- acceptable, meta ...
    idTankStatut int NOT NULL,
    idTypeTank int NOT NULL,

    nom varchar(100) NOT NULL,
    
    -- 1 => true
    estVisible int DEFAULT 1,

    FOREIGN KEY (idTankStatut) REFERENCES TankStatut(id),
    FOREIGN KEY (idTier) REFERENCES Tier(id),
    FOREIGN KEY (idTypeTank) REFERENCES TypeTank(id)
);

CREATE TABLE Joueur
(
    id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    idDiscord varchar(100) NOT NULL,
    pseudo varchar(200) NOT NULL,

    estAdmin int NOT NULL DEFAULT 0,
    estStrateur int NOT NULL DEFAULT 0,
    estActiver int NOT NULL DEFAULT 1
);

CREATE TABLE ClanWarJoueur
(
    idClanWar int NOT NULL,
    idJoueur int NOT NULL,
    idTank int  NULL, 

    PRIMARY KEY(idClanWar, idJoueur),

    FOREIGN KEY (idClanWar) REFERENCES ClanWar(id) ON DELETE CASCADE,
    FOREIGN KEY (idJoueur) REFERENCES Joueur(id) ON DELETE CASCADE,
    FOREIGN KEY (idTank) REFERENCES Tank(id)
);

CREATE TABLE TankJoueur
(
    idJoueur int NOT NULL,
    idTank int NOT NULL,

    PRIMARY KEY(idJoueur, idTank),

    FOREIGN KEY (idJoueur) REFERENCES Joueur(id) ON DELETE CASCADE,
    FOREIGN KEY (idTank) REFERENCES Tank(id)
);

-- clan war
SET IDENTITY_INSERT ClanWar ON;
INSERT INTO ClanWar (id, date) VALUES (1, '14/12/2022'), (2, '20/01/2022');
SET IDENTITY_INSERT ClanWar OFF;

-- type tank
SET IDENTITY_INSERT TypeTank ON;
INSERT INTO TypeTank (id, nom, nomImage) VALUES 
(1, 'Léger', 'iconLeger.png'), (2, 'Medium', 'iconMedium.png'), (3, 'Lourd', 'iconLourd.png'), 
(4, 'Chasseur de char', 'iconTd.png'), (5, 'Artillerie', 'iconArty.png');
SET IDENTITY_INSERT TypeTank OFF;

-- tank statut
SET IDENTITY_INSERT TankStatut ON;
INSERT INTO TankStatut (id, nom) VALUES (1, 'Méta'), (2, 'Accepté'), (3, 'Toléré');
SET IDENTITY_INSERT TankStatut OFF;

-- tier
SET IDENTITY_INSERT Tier ON;
INSERT INTO Tier (id, nom) VALUES (1, 'Tier 6'), (2, 'Tier 8'), (3, 'Tier 10');
SET IDENTITY_INSERT Tier OFF;

-- joueur
SET IDENTITY_INSERT Joueur ON;
INSERT INTO Joueur (id, idDiscord, pseudo, estAdmin, estStrateur, estActiver) VALUES
(1, '341945414318161920', 'JetonPeche', 1, 1, 1);
SET IDENTITY_INSERT Joueur OFF;

-- tank
SET IDENTITY_INSERT Tank ON;
    INSERT INTO Tank (id, idTier, idTypeTank, idTankStatut, nom) VALUES 

    -- meta tier 6
    (1, 1, 2, 1, 'T-34-85M'), (2, 1, 2, 1, 'Cromwell B'), (3, 1, 1, 1, 'Type 64'),

    -- meta tier 8
    (4, 2, 2, 1, 'Progetto 46'), (5, 2, 2, 1, 'Bourrasque'), (6, 2, 3, 1, 'Renegade'), (7, 2, 3, 1, 'Obj 703 II'),
    (8, 2, 1, 1, 'LT-432'), (9, 2, 1, 1, ' EBR 75 (FL10)'), (10, 2, 3, 1, 'BZ-176'), (11, 2, 2, 1, 'M47 Iron Arnie'),

    -- Acceptable tier 8
    (12, 2, 3, 2, 'Object 252U / Defender'), (13, 2, 3, 2, 'AMX M4 49 (L)'), (14, 2, 2, 2, 'CS-52 LIS'),
    (15, 2, 3, 2, 'E 75 TS'), (16, 2, 2, 2, 'T-54 mod 1'),

    -- Toloré tier 8
    (17, 2, 3, 3, 'Bisonte C45'), (18, 2, 3, 3, 'Tiger II'), (19, 2, 3, 3, 'IS-3 / IS-3A'),

    -- méta tier 10 
    (20, 3, 2, 1, 'Object 907'), (21, 3, 2, 1, 'Object 140'), (22, 3, 2, 1, 'T-62A'), (23, 3, 2, 1, 'STB-1'),
    (24, 3, 1, 1, 'EBR 105'), (25, 3, 3, 1, 'T95 FV4201 Chieftain'), (26, 3, 3, 1, 'Object 279(e)'), (27, 3, 3, 1, 'Object 260'),
    (28, 3, 3, 1, 'Object 277'), (29, 3, 3, 1, 'WZ-111 5A'), (32, 3, 4, 1, 'T110E3'), (34, 3, 4, 1, 'Obj 268/4'), (35, 3, 3, 1, 'Obj 277'),

    -- acceptable tier 10
    (30, 3, 1, 2, 'T-100 LT'), (31, 3, 3, 2, 'S.Conqueror'), (33, 3, 4, 2, 'T110E4'),

    -- toléré tier 10
    (36, 3, 3, 3, 'T110E5'), (37, 3, 3, 3, 'Maus'), (38, 3, 3, 3, 'IS-7'), (39, 3, 3, 3, 'E100');
SET IDENTITY_INSERT Tank OFF;

-- tank joueur
INSERT INTO TankJoueur (idJoueur, idTank) VALUES
(1, 4), (1, 5), (1, 12), (1, 13), (1, 16), (1, 22), (1, 29);

-- clanWarJoueur
INSERT INTO ClanWarJoueur (idJoueur, idClanWar, idTank) VALUES (1, 1, null), (1, 2, 4);