USE [Biblioteca]
GO
/****** Object:  Table [dbo].[Asignaturas]    Script Date: 15/11/2021 19:41:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asignaturas](
	[Codigo] [varchar](10) NOT NULL,
	[Nombre] [varchar](100) NULL,
 CONSTRAINT [PK_Asignaturas] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Libros]    Script Date: 15/11/2021 19:41:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Libros](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[IDProfesor] [uniqueidentifier] NOT NULL,
	[CodigoAsignatura] [varchar](10) NOT NULL,
	[ISBN] [varchar](13) NOT NULL,
	[Titulo] [varchar](256) NOT NULL,
	[Autor] [varchar](100) NOT NULL,
	[NombreArchivo] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Libros] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__FILESTRE__3214EC26108B795B] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 15/11/2021 19:41:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login](
	[Username] [varchar](32) NOT NULL,
	[Password] [varchar](256) NOT NULL,
	[Profesor] [tinyint] NOT NULL,
 CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PDF]    Script Date: 15/11/2021 19:41:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PDF] AS FILETABLE ON [PRIMARY] FILESTREAM_ON [PDFFileStream]
WITH
(
FILETABLE_DIRECTORY = N'PDFDirectory', FILETABLE_COLLATE_FILENAME = Latin1_General_CI_AS
)
GO
/****** Object:  Table [dbo].[Perfiles]    Script Date: 15/11/2021 19:41:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Perfiles](
	[ID] [uniqueidentifier] NOT NULL,
	[Username] [varchar](32) NOT NULL,
	[Nombre] [nvarchar](200) NOT NULL,
	[Apellido] [nvarchar](200) NOT NULL,
	[Telefono] [varchar](20) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[FotoPerfil] [varchar](max) NULL,
 CONSTRAINT [PK_Perfiles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Login] ADD  CONSTRAINT [DF_Login_Profesor]  DEFAULT ((0)) FOR [Profesor]
GO
ALTER TABLE [dbo].[Libros]  WITH CHECK ADD  CONSTRAINT [FK_Asignatura_Libros] FOREIGN KEY([CodigoAsignatura])
REFERENCES [dbo].[Asignaturas] ([Codigo])
GO
ALTER TABLE [dbo].[Libros] CHECK CONSTRAINT [FK_Asignatura_Libros]
GO
ALTER TABLE [dbo].[Libros]  WITH CHECK ADD  CONSTRAINT [FK_Libros_Perfiles] FOREIGN KEY([IDProfesor])
REFERENCES [dbo].[Perfiles] ([ID])
GO
ALTER TABLE [dbo].[Libros] CHECK CONSTRAINT [FK_Libros_Perfiles]
GO
