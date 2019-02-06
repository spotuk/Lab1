CREATE TABLE [dbo].[Autoes] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (100)  NULL,
    [Manufacturer]  NVARCHAR (50)  NULL,
    [Description]   NVARCHAR (MAX)  NULL,
    [Type]          NVARCHAR (50)  NULL,
    [Price]         DECIMAL (18, 2) NOT NULL,
    [ImageData]     VARBINARY (MAX) NULL,
    [ImageMimeType] VARCHAR (50)    NULL,
    CONSTRAINT [PK_dbo.Autoes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

