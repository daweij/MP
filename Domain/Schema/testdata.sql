USE MovieDB;

-- 
-- REMOVE CONNECTION TABLES
IF OBJECT_ID('[GenreMovies]', 'U') IS NOT NULL DROP TABLE [GenreMovies];
IF OBJECT_ID('[ActorMovies]', 'U') IS NOT NULL DROP TABLE [ActorMovies];
IF OBJECT_ID('[DirectorMovies]', 'U') IS NOT NULL DROP TABLE [DirectorMovies];
--
IF OBJECT_ID('[FactRatings]', 'U') IS NOT NULL DROP TABLE [FactRatings];
IF OBJECT_ID('[FactSales]', 'U') IS NOT NULL DROP TABLE [FactSales];
IF OBJECT_ID('[DimMovies]', 'U') IS NOT NULL DROP TABLE [DimMovies];
IF OBJECT_ID('[DimGenres]', 'U') IS NOT NULL DROP TABLE [DimGenres];
IF OBJECT_ID('[DimActors]', 'U') IS NOT NULL DROP TABLE [DimActors];
IF OBJECT_ID('[DimDirectors]', 'U') IS NOT NULL DROP TABLE [DimDirectors];
IF OBJECT_ID('[DimYears]', 'U') IS NOT NULL DROP TABLE [DimYears];
IF OBJECT_ID('[DimCountries]', 'U') IS NOT NULL DROP TABLE [DimCountries];


CREATE TABLE [FactRatings] (
  [Id] INT not null,
  [MovieId] INT not null,
  [YearId] INT,
  [CountryId] INT not null,
  [Rating] DECIMAL not null,
  [Votes] FLOAT not null,
  PRIMARY KEY ([Id])
);

CREATE TABLE [FactSales] (
  [Id] INT not null,
  [MovieId] INT not null,
  [YearId] INT,
  [CountryId] INT not null,
  [Revenue] FLOAT not null,
  PRIMARY KEY ([Id])
);

CREATE TABLE [DimMovies] (
  [Id] INT not null,
  [Title] NVARCHAR(256) not null,
  [Year] INT,
  [Country] NVARCHAR(128) not null,
  PRIMARY KEY ([Id])
);

CREATE TABLE [DimGenres] (
  [Id] INT not null,
  [Name] NVARCHAR(256) not null,
  PRIMARY KEY ([Id])
);

CREATE TABLE [DimActors] (
  [Id] INT not null,
  [Name] NVARCHAR(256) not null,
  PRIMARY KEY ([Id])
);

CREATE TABLE [DimDirectors] (
  [Id] INT not null,
  [Name] NVARCHAR(256) not null,
  PRIMARY KEY ([Id])
);

CREATE TABLE [DimYears] (
  [Id] INT not null,
  [Year] INT not null,
  PRIMARY KEY ([Id])
);

CREATE TABLE [DimCountries] (
  [Id] INT not null,
  [Name] NVARCHAR(256) not null,
  PRIMARY KEY ([Id])
);

CREATE TABLE [GenreMovies] (
  [GenreId] INT not null,
  [MovieId] INT not null,
  PRIMARY KEY ([GenreId], [MovieId])
);

CREATE TABLE [ActorMovies] (
  [ActorId] INT not null,
  [MovieId] INT not null,
  PRIMARY KEY ([ActorId], [MovieId])
);

CREATE TABLE [DirectorMovies] (
  [DirectorId] INT not null,
  [MovieId] INT not null,
  PRIMARY KEY ([DirectorId], [MovieId])
);




ALTER TABLE [FactRatings] ADD FOREIGN KEY ([MovieId]) REFERENCES [DimMovies] ([Id]);
ALTER TABLE [FactRatings] ADD FOREIGN KEY ([YearId]) REFERENCES [DimYears] ([Id]);
ALTER TABLE [FactRatings] ADD FOREIGN KEY ([CountryId]) REFERENCES [DimCountries] ([Id]);
ALTER TABLE [FactSales] ADD FOREIGN KEY ([MovieId]) REFERENCES [DimMovies] ([Id]);
ALTER TABLE [FactSales] ADD FOREIGN KEY ([YearId]) REFERENCES [DimYears] ([Id]);
ALTER TABLE [FactSales] ADD FOREIGN KEY ([CountryId]) REFERENCES [DimCountries] ([Id]);
ALTER TABLE [GenreMovies] ADD FOREIGN KEY ([GenreId]) REFERENCES [DimGenres] ([Id]);
ALTER TABLE [GenreMovies] ADD FOREIGN KEY ([MovieId]) REFERENCES [DimMovies] ([Id]);
ALTER TABLE [ActorMovies] ADD FOREIGN KEY ([ActorId]) REFERENCES [DimActors] ([Id]);
ALTER TABLE [ActorMovies] ADD FOREIGN KEY ([MovieId]) REFERENCES [DimMovies] ([Id]);
ALTER TABLE [DirectorMovies] ADD FOREIGN KEY ([DirectorId]) REFERENCES [DimDirectors] ([Id]);
ALTER TABLE [DirectorMovies] ADD FOREIGN KEY ([MovieId]) REFERENCES [DimMovies] ([Id]);

--
-- ADD FOREIGN KEYS
--ALTER TABLE [Movies] ADD FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]);
--ALTER TABLE [Movies] ADD FOREIGN KEY ([DirectorId]) REFERENCES [Directors] ([Id]);
--ALTER TABLE [MovieGenres] ADD FOREIGN KEY ([MovieId]) REFERENCES [Movies] ([Id]);
--ALTER TABLE [MovieGenres] ADD FOREIGN KEY ([GenreId]) REFERENCES [Genres] ([Id]);
--ALTER TABLE [MovieActors] ADD FOREIGN KEY ([MovieId]) REFERENCES [Movies] ([Id]);
--ALTER TABLE [MovieActors] ADD FOREIGN KEY ([ActorId]) REFERENCES [Actors] ([Id]);

-- dummy data
-- movies
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (1, 'Teletubbies', 1997, 'Ukraine');
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (2, 'Spiderman', 2002, 'USA');
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (3, 'Spider-Man 2', 2004, 'USA');
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (4, 'Spider-Man 3', 2007, 'USA');
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (5, 'The Amazing Spider-Man', 2012, 'USA');
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (6, 'The Amazing Spider-Man 2', 2014, 'USA');
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (7, 'District B13', 2004, 'France');
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (8, 'Johan Falk', 2009, 'Sweden');
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (9, 'Rurouni Kenshin', 2012, 'Japan');
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (10, 'The Last Samurai', 2003, 'USA');
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (11, 'Oldboy', 2003, 'Korea');
INSERT INTO [DimMovies] ([Id], [Title], [Year], [Country])
VALUES (12, 'Trainspotting', 1996, 'United Kingdom');

-- years
INSERT INTO [DimYears] ([Id], [Year])
VALUES (1, 1990);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (2, 1991);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (3, 1992);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (4, 1993);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (5, 1994);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (6, 1995);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (7, 1996);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (8, 1997);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (9, 1998);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (10, 1999);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (11, 2000);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (12, 2001);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (13, 2002);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (14, 2003);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (15, 2004);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (16, 2005);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (17, 2006);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (18, 2007);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (19, 2008);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (20, 2009);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (21, 2010);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (22, 2011);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (23, 2012);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (24, 2013);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (25, 2014);
INSERT INTO [DimYears] ([Id], [Year])
VALUES (26, 2015);

-- countries
INSERT INTO [DimCountries] ([Id], [Name])
VALUES (1, 'Sweden');
INSERT INTO [DimCountries] ([Id], [Name])
VALUES (2, 'USA');
INSERT INTO [DimCountries] ([Id], [Name])
VALUES (3, 'Japan');
INSERT INTO [DimCountries] ([Id], [Name])
VALUES (4, 'Ukraine');
INSERT INTO [DimCountries] ([Id], [Name])
VALUES (5, 'France');
INSERT INTO [DimCountries] ([Id], [Name])
VALUES (6, 'United Kingdom');
INSERT INTO [DimCountries] ([Id], [Name])
VALUES (7, 'Korea');

-- movieid 1, teletubbies 1997
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (1, 1, 8, 1, 2.4, 6234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (2, 1, 8, 2, 2.4, 4345);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (3, 1, 8, 3, 2.4, 5500);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (4, 1, 9, 1, 3.5, 2300);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (5, 1, 10, 1, 3.0, 1119);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (6, 1, 10, 5, 2.4, 4422);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (7, 1, 11, 2, 3.2, 766);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (8, 1, 12, 1, 2.8, 234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (9, 1, 13, 2, 2.2, 123);


-- movieid 2, Spiderman 2002
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (10, 2, 13, 1, 7.8, 5869455);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (11, 2, 13, 2, 7.8, 323323);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (12, 2, 13, 3, 7.8, 454522);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (13, 2, 13, 4, 7.3, 666333);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (14, 2, 13, 5, 7.4, 6664333);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (15, 2, 14, 1, 7.7, 4200911);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (16, 2, 15, 1, 7.4, 3291981);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (17, 2, 15, 4, 7.4, 222331);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (18, 2, 16, 2, 7.6, 2314511);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (19, 2, 16, 3, 7.2, 113334);


-- movieid 3, Spiderman 2 2004
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (20, 3, 15, 3, 6.8, 34234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (21, 3, 15, 2, 6.8, 2342341);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (22, 3, 15, 1, 6.8, 563434);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (23, 3, 15, 5, 6.3, 345234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (24, 3, 15, 4, 6.4, 2346);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (25, 3, 16, 2, 7.6, 2342346);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (26, 3, 16, 1, 6.4, 234236);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (27, 3, 17, 1, 7.4, 234234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (28, 3, 17, 6, 4.4, 2342345);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (29, 3, 18, 7, 5.2, 755675);

-- movieid 4, Spiderman 3 2007
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (30, 4, 18, 1, 6.8, 34234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (31, 4, 18, 2, 6.8, 2342341);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (32, 4, 19, 3, 6.8, 563434);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (33, 4, 19, 1, 6.3, 345234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (34, 4, 19, 4, 6.4, 2346);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (35, 4, 20, 3, 7.6, 2342346);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (36, 4, 20, 2, 6.4, 234236);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (37, 4, 21, 1, 7.4, 234234);

-- movieid 5, The Amazing Spider-Man 2012
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (38, 5, 23, 1, 7.8, 23234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (39, 5, 23, 2, 8.8, 64545);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (40, 5, 23, 3, 7.8, 356745);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (41, 5, 23, 4, 8.3, 34856);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (42, 5, 23, 5, 6.4, 56753);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (43, 5, 24, 1, 7.6, 964344);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (44, 5, 24, 2, 9.4, 3466666);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (45, 5, 24, 3, 6.4, 453456);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (46, 5, 24, 4, 7.8, 345124);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (47, 5, 25, 1, 6.3, 345345);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (48, 5, 25, 2, 8.3, 5345);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (49, 5, 25, 3, 6.3, 7567567);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (50, 5, 26, 4, 7.6, 1233334);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (51, 5, 26, 3, 7.2, 234236);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (52, 5, 26, 2, 7.3, 1235566);

-- movieid 6, The Amazing Spider-Man 2 2014
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (53, 6, 25, 1, 9.9, 323423);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (54, 6, 25, 2, 8.9, 34554);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (55, 6, 25, 3, 8.9, 345345);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (56, 6, 26, 4, 8.3, 6634445);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (57, 6, 26, 5, 3.4, 737455);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (58, 6, 26, 1, 4.6, 745342);

-- movieid 7, District B13 2004
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (59, 7, 15, 1, 5.5, 323423);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (60, 7, 15, 2, 5.2, 34554);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (61, 7, 15, 3, 4.9, 345345);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (62, 7, 15, 4, 8.4, 6634445);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (63, 7, 16, 5, 6.1, 737455);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (64, 7, 17, 1, 5.2, 745342);

-- movieid 8, Johan Falk 2009
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (65, 8, 20, 1, 5.5, 323423);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (66, 8, 21, 1, 5.2, 34554);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (67, 8, 22, 1, 4.9, 345345);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (68, 8, 23, 1, 3.4, 534223);

-- movieid 9, Rurouni Kenshin 2012
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (69, 9, 23, 3, 9.8, 23423);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (70, 9, 23, 3, 5.6, 574545);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (71, 9, 24, 3, 7.3, 85634);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (72, 9, 24, 3, 9.7, 34534);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (73, 9, 25, 3, 8.6, 77332);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (74, 9, 25, 2, 8.6, 6669941);

-- movieid 10, The Last Samurai 2003
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (75, 10, 14, 2, 9.8, 123412);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (76, 10, 14, 1, 5.6, 34234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (77, 10, 15, 2, 7.3, 2562);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (78, 10, 15, 3, 7.7, 234234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (79, 10, 16, 4, 5.6, 2362362);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (80, 10, 16, 5, 5.6, 456456);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (81, 10, 16, 2, 5.6, 222567);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (82, 10, 16, 3, 6.6, 978534);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (83, 10, 16, 1, 6.6, 345457);

-- movieid 11, Oldboy', 2003
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (84, 11, 14, 7, 9.8, 123123);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (85, 11, 14, 3, 5.6, 123234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (86, 11, 15, 7, 7.3, 54322);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (87, 11, 15, 6, 7.7, 23412);

-- movieid 12, Trainspotting', 1996,
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (88, 12, 7, 6, 9.8, 123123);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (89, 12, 7, 1, 7.6, 123234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (90, 12, 8, 6, 6.3, 54322);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (91, 12, 9, 6, 8.7, 23412);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (92, 12, 10, 6, 9.6, 123123);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (93, 12, 11, 6, 5.5, 123234);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (94, 12, 11, 2, 7.4, 54322);
INSERT INTO [FactRatings] ([Id], [MovieId], [YearId], [CountryId], [Rating], [Votes])
VALUES (95, 12, 12, 3, 7.2, 23412);