USE MovieDB;

-- 
-- REMOVE CONNECTION TABLES
IF OBJECT_ID('[GenreMovies]', 'U') IS NOT NULL DROP TABLE [GenreMovies];
IF OBJECT_ID('[CountryMovies]', 'U') IS NOT NULL DROP TABLE [CountryMovies];
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
  [CountryId] INT not null,
  [Rating] DECIMAL not null,
  [Votes] FLOAT not null,
  PRIMARY KEY ([Id])
);

CREATE TABLE [FactSales] (
  [Id] INT not null,
  [MovieId] INT not null,
  [CountryId] INT not null,
  [Revenue] FLOAT not null, 
  [Type] SMALLINT not null,
  PRIMARY KEY ([Id])
);

CREATE TABLE [DimMovies] (
  [Id] INT not null,
  [Title] NVARCHAR(512) not null,
  [Year] INT not null,
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

CREATE TABLE [CountryMovies] (
  [CountryId] INT not null,
  [MovieId] INT not null,
  PRIMARY KEY ([CountryId], [MovieId])
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
ALTER TABLE [FactRatings] ADD FOREIGN KEY ([CountryId]) REFERENCES [DimCountries] ([Id]);
ALTER TABLE [FactSales] ADD FOREIGN KEY ([MovieId]) REFERENCES [DimMovies] ([Id]);
ALTER TABLE [FactSales] ADD FOREIGN KEY ([CountryId]) REFERENCES [DimCountries] ([Id]);
ALTER TABLE [GenreMovies] ADD FOREIGN KEY ([GenreId]) REFERENCES [DimGenres] ([Id]);
ALTER TABLE [GenreMovies] ADD FOREIGN KEY ([MovieId]) REFERENCES [DimMovies] ([Id]);
ALTER TABLE [CountryMovies] ADD FOREIGN KEY ([CountryId]) REFERENCES [DimCountries] ([Id]);
ALTER TABLE [CountryMovies] ADD FOREIGN KEY ([MovieId]) REFERENCES [DimMovies] ([Id]);
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



