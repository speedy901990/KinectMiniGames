
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/23/2014 18:38:17
-- Generated from EDMX file: C:\Projects\KinectMiniGames\DatabaseManagement\GameModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [KinectMiniGamesDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_GameGameParams]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GameParams1] DROP CONSTRAINT [FK_GameGameParams];
GO
IF OBJECT_ID(N'[dbo].[FK_GameGameResults]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GameResults] DROP CONSTRAINT [FK_GameGameResults];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerHistory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Histories] DROP CONSTRAINT [FK_PlayerHistory];
GO
IF OBJECT_ID(N'[dbo].[FK_GameHistory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Histories] DROP CONSTRAINT [FK_GameHistory];
GO
IF OBJECT_ID(N'[dbo].[FK_HistoryHistoryResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HistoryResults] DROP CONSTRAINT [FK_HistoryHistoryResult];
GO
IF OBJECT_ID(N'[dbo].[FK_HistoryHistoryParams]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HistoryParams] DROP CONSTRAINT [FK_HistoryHistoryParams];
GO
IF OBJECT_ID(N'[dbo].[FK_GameParamsHistoryParams]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HistoryParams] DROP CONSTRAINT [FK_GameParamsHistoryParams];
GO
IF OBJECT_ID(N'[dbo].[FK_GameResultsHistoryResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HistoryResults] DROP CONSTRAINT [FK_GameResultsHistoryResult];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Players]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Players];
GO
IF OBJECT_ID(N'[dbo].[Games]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Games];
GO
IF OBJECT_ID(N'[dbo].[GameParams1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GameParams1];
GO
IF OBJECT_ID(N'[dbo].[GameResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GameResults];
GO
IF OBJECT_ID(N'[dbo].[Histories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Histories];
GO
IF OBJECT_ID(N'[dbo].[HistoryResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HistoryResults];
GO
IF OBJECT_ID(N'[dbo].[HistoryParams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HistoryParams];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Players'
CREATE TABLE [dbo].[Players] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Surname] nvarchar(max)  NOT NULL,
    [Age] int  NOT NULL
);
GO

-- Creating table 'Games'
CREATE TABLE [dbo].[Games] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'GameParams1'
CREATE TABLE [dbo].[GameParams1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Values] nvarchar(max)  NOT NULL,
    [Game_Id] int  NOT NULL
);
GO

-- Creating table 'GameResults'
CREATE TABLE [dbo].[GameResults] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Game_Id] int  NOT NULL
);
GO

-- Creating table 'Histories'
CREATE TABLE [dbo].[Histories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Player_Id] int  NOT NULL,
    [Game_Id] int  NOT NULL
);
GO

-- Creating table 'HistoryResults'
CREATE TABLE [dbo].[HistoryResults] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] int  NOT NULL,
    [History_Id] int  NOT NULL,
    [GameResult_Id] int  NOT NULL
);
GO

-- Creating table 'HistoryParams'
CREATE TABLE [dbo].[HistoryParams] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [History_Id] int  NOT NULL,
    [GameParam_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Players'
ALTER TABLE [dbo].[Players]
ADD CONSTRAINT [PK_Players]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [PK_Games]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GameParams1'
ALTER TABLE [dbo].[GameParams1]
ADD CONSTRAINT [PK_GameParams1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GameResults'
ALTER TABLE [dbo].[GameResults]
ADD CONSTRAINT [PK_GameResults]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Histories'
ALTER TABLE [dbo].[Histories]
ADD CONSTRAINT [PK_Histories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HistoryResults'
ALTER TABLE [dbo].[HistoryResults]
ADD CONSTRAINT [PK_HistoryResults]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HistoryParams'
ALTER TABLE [dbo].[HistoryParams]
ADD CONSTRAINT [PK_HistoryParams]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Game_Id] in table 'GameParams1'
ALTER TABLE [dbo].[GameParams1]
ADD CONSTRAINT [FK_GameGameParams]
    FOREIGN KEY ([Game_Id])
    REFERENCES [dbo].[Games]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GameGameParams'
CREATE INDEX [IX_FK_GameGameParams]
ON [dbo].[GameParams1]
    ([Game_Id]);
GO

-- Creating foreign key on [Game_Id] in table 'GameResults'
ALTER TABLE [dbo].[GameResults]
ADD CONSTRAINT [FK_GameGameResults]
    FOREIGN KEY ([Game_Id])
    REFERENCES [dbo].[Games]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GameGameResults'
CREATE INDEX [IX_FK_GameGameResults]
ON [dbo].[GameResults]
    ([Game_Id]);
GO

-- Creating foreign key on [Player_Id] in table 'Histories'
ALTER TABLE [dbo].[Histories]
ADD CONSTRAINT [FK_PlayerHistory]
    FOREIGN KEY ([Player_Id])
    REFERENCES [dbo].[Players]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerHistory'
CREATE INDEX [IX_FK_PlayerHistory]
ON [dbo].[Histories]
    ([Player_Id]);
GO

-- Creating foreign key on [Game_Id] in table 'Histories'
ALTER TABLE [dbo].[Histories]
ADD CONSTRAINT [FK_GameHistory]
    FOREIGN KEY ([Game_Id])
    REFERENCES [dbo].[Games]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GameHistory'
CREATE INDEX [IX_FK_GameHistory]
ON [dbo].[Histories]
    ([Game_Id]);
GO

-- Creating foreign key on [History_Id] in table 'HistoryResults'
ALTER TABLE [dbo].[HistoryResults]
ADD CONSTRAINT [FK_HistoryHistoryResult]
    FOREIGN KEY ([History_Id])
    REFERENCES [dbo].[Histories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HistoryHistoryResult'
CREATE INDEX [IX_FK_HistoryHistoryResult]
ON [dbo].[HistoryResults]
    ([History_Id]);
GO

-- Creating foreign key on [History_Id] in table 'HistoryParams'
ALTER TABLE [dbo].[HistoryParams]
ADD CONSTRAINT [FK_HistoryHistoryParams]
    FOREIGN KEY ([History_Id])
    REFERENCES [dbo].[Histories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HistoryHistoryParams'
CREATE INDEX [IX_FK_HistoryHistoryParams]
ON [dbo].[HistoryParams]
    ([History_Id]);
GO

-- Creating foreign key on [GameParam_Id] in table 'HistoryParams'
ALTER TABLE [dbo].[HistoryParams]
ADD CONSTRAINT [FK_GameParamsHistoryParams]
    FOREIGN KEY ([GameParam_Id])
    REFERENCES [dbo].[GameParams1]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GameParamsHistoryParams'
CREATE INDEX [IX_FK_GameParamsHistoryParams]
ON [dbo].[HistoryParams]
    ([GameParam_Id]);
GO

-- Creating foreign key on [GameResult_Id] in table 'HistoryResults'
ALTER TABLE [dbo].[HistoryResults]
ADD CONSTRAINT [FK_GameResultsHistoryResult]
    FOREIGN KEY ([GameResult_Id])
    REFERENCES [dbo].[GameResults]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GameResultsHistoryResult'
CREATE INDEX [IX_FK_GameResultsHistoryResult]
ON [dbo].[HistoryResults]
    ([GameResult_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------