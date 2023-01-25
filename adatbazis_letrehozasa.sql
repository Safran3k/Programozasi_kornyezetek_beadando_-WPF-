
CREATE TABLE [Filmek] (
    [Id]            INT         IDENTITY (1, 1) NOT NULL,
    [Cim]           NCHAR (100) NOT NULL,
    [PremierDatuma] DATE        NOT NULL,
    [Mufaj]         NCHAR (50)  NOT NULL,
    [Rendezo]       NCHAR (30)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [Felhasznalok] (
    [Id]             INT         IDENTITY (1, 1) NOT NULL,
    [FelhasznaloNev] NCHAR (50)  NOT NULL,
    [Jelszo]         NCHAR (100) NOT NULL,
    [Admin]          BIT         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);