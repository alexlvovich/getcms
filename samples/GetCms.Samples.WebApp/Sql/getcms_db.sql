/****** Object:  Table [dbo].[Content]    Script Date: 2/8/2019 6:28:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Content](
	[ContentId] [int] IDENTITY(1,1) NOT NULL,
	[ContentTypeId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Title] [nvarchar](512) NULL,
	[Body] [ntext] NULL,
	[SiteId] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](64) NULL,
	[IsActive] [bit] NOT NULL,
	[ExternalLink] [nvarchar](128) NULL,
	[RefContentId] [int] NULL,
	[OrderNumber] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](64) NULL,
	[PublishedOn] [datetime] NULL,
	[PublishedBy] [nvarchar](64) NULL,
 CONSTRAINT [PK_Content] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContentToPages]    Script Date: 2/8/2019 6:28:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentToPages](
	[ContentId] [int] NOT NULL,
	[PageId] [int] NOT NULL,
	[Order] [tinyint] NULL,
 CONSTRAINT [PK_ContentToPages] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 2/8/2019 6:28:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[SiteId] [int] NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](64) NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](64) NULL,
	[PublishedOn] [datetime] NULL,
	[PublishedBy] [nvarchar](64) NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuItems]    Script Date: 2/8/2019 6:28:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuItems](
	[MenuId] [int] NOT NULL,
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[ParentItemId] [int] NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Title] [nvarchar](256) NULL,
	[Url] [nvarchar](256) NULL,
	[IsActive] [bit] NOT NULL,
	[ImagePath] [nvarchar](128) NULL,
	[ImagePathAct] [nvarchar](128) NULL,
	[Order] [tinyint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](64) NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](64) NULL,
	[PublishedOn] [datetime] NULL,
	[PublishedBy] [nvarchar](64) NULL,
 CONSTRAINT [PK_MenuItems] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MessagingTemplates]    Script Date: 2/8/2019 6:28:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessagingTemplates](
	[ContentId] [int] NOT NULL,
	[EmailPriorityId] [tinyint] NOT NULL,
	[EmailBodyFormatId] [tinyint] NOT NULL,
	[TemplateTypeId] [tinyint] NOT NULL,
	[Parameters] [nvarchar](512) NULL,
	[Subject] [nvarchar](256) NOT NULL,
	[Target] [tinyint] NOT NULL,
	[MessagingTemplateId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_MessagingTemplates] PRIMARY KEY CLUSTERED 
(
	[MessagingTemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Metadata]    Script Date: 2/8/2019 6:28:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Metadata](
	[MetaDataId] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NOT NULL,
	[MetaDataTypeId] [tinyint] NOT NULL,
	[SiteId] [int] NOT NULL,
	[Key] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](1024) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](64) NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](64) NULL,
	[PublishedOn] [datetime] NULL,
	[PublishedBy] [nvarchar](64) NULL,
 CONSTRAINT [PK_Metadata] PRIMARY KEY CLUSTERED 
(
	[MetaDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pages]    Script Date: 2/8/2019 6:28:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pages](
	[SiteId] [int] NOT NULL,
	[PageId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NULL,
	[Slug] [nvarchar](128) NULL,
	[Title] [nvarchar](256) NULL,
	[PageTypeId] [tinyint] NULL,
	[MasterPageId] [int] NULL,
	[Css] [nvarchar](64) NULL,
	[IsEditable] [bit] NOT NULL,
	[MenuIndex] [tinyint] NULL,
	[IsActive] [bit] NOT NULL,
	[ParentPageId] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](64) NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](64) NULL,
	[PublishedOn] [datetime] NULL,
	[PublishedBy] [nvarchar](64) NULL,
 CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED 
(
	[PageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sites]    Script Date: 2/8/2019 6:28:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sites](
	[SiteId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Host] [nvarchar](64) NOT NULL,
	[Language] [char](3) NOT NULL,
	[TimeZoneOffset] [decimal](18, 0) NULL,
	[PageTitleSeparator] [nvarchar](128) NULL,
	[ContentType] [nvarchar](64) NULL,
	[IsActive] [bit] NOT NULL,
	[ParentSiteId] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](64) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](64) NULL,
	[PublishedOn] [datetime] NULL,
	[PublishedBy] [nvarchar](64) NULL,
 CONSTRAINT [PK_Sites] PRIMARY KEY CLUSTERED 
(
	[SiteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Content] ADD  CONSTRAINT [DF_Content_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Pages] ADD  CONSTRAINT [DF_Pages_IsEditable]  DEFAULT ((1)) FOR [IsEditable]
GO
ALTER TABLE [dbo].[Content]  WITH CHECK ADD  CONSTRAINT [FK_Content_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Sites] ([SiteId])
GO
ALTER TABLE [dbo].[Content] CHECK CONSTRAINT [FK_Content_Sites]
GO
ALTER TABLE [dbo].[ContentToPages]  WITH CHECK ADD  CONSTRAINT [FK_ContentToPages_Content] FOREIGN KEY([ContentId])
REFERENCES [dbo].[Content] ([ContentId])
GO
ALTER TABLE [dbo].[ContentToPages] CHECK CONSTRAINT [FK_ContentToPages_Content]
GO
ALTER TABLE [dbo].[ContentToPages]  WITH CHECK ADD  CONSTRAINT [FK_ContentToPages_Pages] FOREIGN KEY([PageId])
REFERENCES [dbo].[Pages] ([PageId])
GO
ALTER TABLE [dbo].[ContentToPages] CHECK CONSTRAINT [FK_ContentToPages_Pages]
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_Menu_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Sites] ([SiteId])
GO
ALTER TABLE [dbo].[Menu] CHECK CONSTRAINT [FK_Menu_Sites]
GO
ALTER TABLE [dbo].[MenuItems]  WITH CHECK ADD  CONSTRAINT [FK_MenuItems_Menu] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menu] ([MenuId])
GO
ALTER TABLE [dbo].[MenuItems] CHECK CONSTRAINT [FK_MenuItems_Menu]
GO
ALTER TABLE [dbo].[MessagingTemplates]  WITH CHECK ADD  CONSTRAINT [FK_MessagingTemplates_Content] FOREIGN KEY([ContentId])
REFERENCES [dbo].[Content] ([ContentId])
GO
ALTER TABLE [dbo].[MessagingTemplates] CHECK CONSTRAINT [FK_MessagingTemplates_Content]
GO
ALTER TABLE [dbo].[Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Metadata_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Sites] ([SiteId])
GO
ALTER TABLE [dbo].[Metadata] CHECK CONSTRAINT [FK_Metadata_Sites]
GO
ALTER TABLE [dbo].[Pages]  WITH CHECK ADD  CONSTRAINT [FK_Pages_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Sites] ([SiteId])
GO
ALTER TABLE [dbo].[Pages] CHECK CONSTRAINT [FK_Pages_Sites]
GO
/****** Object:  StoredProcedure [dbo].[Content_CreateUpdateDelete]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Content_CreateUpdateDelete] 
	@Action TINYINT,
	@ContentTypeId TINYINT,
	@Name NVARCHAR(128),
	@Title NVARCHAR(512),
	@Body NVARCHAR(MAX),
	@IsActive BIT,
	@CreatedOn DATETIME,
	@CreatedBy NVARCHAR(64),
	@ModifiedOn DATETIME = NULL,
	@ModifiedBy NVARCHAR(64),
	@PublishedOn DATETIME = NULL,
	@PublishedBy NVARCHAR(64),
	@SiteId INT,
	@ContentId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Path VARCHAR(1024) -- parameter path

    IF @Action = 1
	BEGIN
		INSERT [dbo].[Content]  ([Name], [Title], Body, IsActive, ContentTypeId, CreatedOn, CreatedBy, SiteId)
		VALUES (@Name, @Title, @Body, @IsActive, @ContentTypeId, @CreatedOn, @CreatedBy, @SiteId)

		SET @ContentId = SCOPE_IDENTITY()
	END
	ELSE IF @Action = 2
	BEGIN 
			UPDATE [dbo].[Content] 
			SET 
			ContentTypeId = @ContentTypeId,
			[Name] = @Name,
			[Title] = @Title,
		    Body = @Body,
			IsActive = @IsActive,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			PublishedOn = @PublishedOn,
			PublishedBy = @PublishedBy 
			WHERE ContentId = @ContentId
	END
	ELSE
	BEGIN
		-- update only name for now
		
		BEGIN TRAN t1
			DELETE [dbo].[Content] WHERE ContentId = @ContentId AND ContentTypeId<>2
		COMMIT TRAN t1
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Content_GetBy]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Content_GetBy] 
	@Id INT = NULL,
	@ContentTypeId TINYINT = NULL,
	@Name NVARCHAR(128) = NULL,
	@IsActive BIT = NULL,
	@PageId INT = NULL,
	@SiteId INT = NULL,
	@From INT,
	@To INT
AS
BEGIN

IF (@PageId is not null) -- get by page
	BEGIN
		SELECT c.*, cp.[Order], Count(*) OVER() AS Total FROM [dbo].[Content] c, [dbo].[ContentToPages] cp
		WHERE cp.ContentId = c.ContentId AND cp.PageId = @PageId
	END
ELSE
	BEGIN

		SELECT * FROM (
			SELECT c.*, ROW_NUMBER() OVER(ORDER BY c.ContentId) AS RowId, Count(*) OVER() AS Total FROM [dbo].[Content] c
			WHERE (c.ContentId = @Id OR @Id = 0) AND
			(ContentTypeId = @ContentTypeId OR @ContentTypeId = 0) AND
			((Name = @Name OR Name LIKE '%' + @Name + '%' ) OR @Name is null) AND
			(IsActive = @IsActive OR @IsActive = 0) AND -- only active if specified overwise all
			(SiteId = @SiteId OR @SiteId = 0) 
		) AS tbl
		WHERE RowId BETWEEN @From AND @To
		ORDER BY RowId

	END
END
GO
/****** Object:  StoredProcedure [dbo].[ContentToPage_Map]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ContentToPage_Map] 
	@Action TINYINT,
	@ContentId INT,
	@PageId INT
AS
BEGIN
	IF @Action = 1
	BEGIN

		IF(NOT EXISTS (SELECT * FROM [dbo].[ContentToPages] WHERE ContentId = @ContentId AND PageId = @PageId))
			INSERT [dbo].[ContentToPages]  (ContentId, PageId)
			VALUES (@ContentId, @PageId)

	END

	ELSE
	BEGIN
		-- update only name for now
		DELETE [dbo].[ContentToPages] WHERE ContentId = @ContentId AND PageId = @PageId

	END


END
GO
/****** Object:  StoredProcedure [dbo].[Menu_CreateUpdateDelete]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Menu_CreateUpdateDelete] 
	@Action TINYINT,
	@Name NVARCHAR(128),
	@IsActive BIT,
	@SiteId INT,
	@CreatedOn DATETIME,
	@CreatedBy NVARCHAR(64),
	@ModifiedOn DATETIME = NULL,
	@ModifiedBy NVARCHAR(64),
	@PublishedOn DATETIME = NULL,
	@PublishedBy NVARCHAR(64),
	@MenuId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @Action = 1
	BEGIN
		INSERT [dbo].[Menu]  ([Name], IsActive, SiteId, CreatedOn, CreatedBy)
		VALUES (@Name, @IsActive, @SiteId, @CreatedOn, @CreatedBy)

		SET @MenuId = SCOPE_IDENTITY()
	END
	ELSE IF @Action = 2
	BEGIN 
			UPDATE [dbo].[Menu] 
			SET 
			[Name] = @Name,
			IsActive = @IsActive,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			PublishedBy = @PublishedBy,
			PublishedOn = @PublishedOn
			WHERE MenuId = @MenuId
	END
	ELSE
	BEGIN
		-- update only name for now
		
		BEGIN TRAN t1
			DELETE [dbo].[Menu] WHERE MenuId = @MenuId
		COMMIT TRAN t1
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Menu_GetBy]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Menu_GetBy]
	@MenuId INT = NULL, 
	@SiteId INT = null,
	@Name NVARCHAR(128) = null,
	@IsActive BIT = null,
	@From INT,
	@To INT
AS
SELECT
* FROM
(
	SELECT m.*, (ROW_NUMBER() OVER (ORDER BY m.MenuId
		   )) AS RowNum, Count(*) OVER() AS Total FROM [dbo].[Menu] m
    
    WHERE (m.MenuId = @MenuId OR @MenuId is null) AND
	(m.SiteId = @SiteId OR @SiteId is null) AND
	(m.[Name] = @Name OR @Name is null) AND
	(m.IsActive = @IsActive OR @IsActive is null)
) AS Menus
WHERE Menus.RowNum BETWEEN @From AND @To


	
GO
/****** Object:  StoredProcedure [dbo].[MenuItems_CreateUpdateDelete]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MenuItems_CreateUpdateDelete] 
	@Action TINYINT,
	@MenuId INT,
	@Name NVARCHAR(256),
	@Title NVARCHAR(256),
	@Url NVARCHAR(256),
	@IsActive BIT,
	@CreatedOn DATETIME,
	@CreatedBy NVARCHAR(64),
	@ModifiedOn DATETIME = NULL,
	@ModifiedBy NVARCHAR(64),
	@PublishedOn DATETIME = NULL,
	@PublishedBy NVARCHAR(64),
	@ImagePath NVARCHAR(128),
	@ImagePathAct NVARCHAR(128),
	@Order TINYINT,
	@ItemId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @Action = 1
	BEGIN
		INSERT [dbo].[MenuItems]  (MenuId, [Name], Title, [Url], IsActive, CreatedOn, CreatedBy, ImagePath, ImagePathAct, [Order])
		VALUES (@MenuId, @Name, @Title, @Url, @IsActive, @CreatedOn, @CreatedBy, @ImagePath, @ImagePathAct, @Order)

		SET @ItemId = SCOPE_IDENTITY()
	END
	ELSE IF @Action = 2
	BEGIN 
			UPDATE [dbo].[MenuItems]
			SET 
			[Name] = @Name,
			Title = @Title,
			[Url] = @Url,
			IsActive = @IsActive,
			ImagePath = @ImagePath, 
			ImagePathAct = @ImagePathAct,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			PublishedBy = @PublishedBy,
			PublishedOn = @PublishedOn,
			[Order] = @Order
			WHERE ItemId = @ItemId
	END
	ELSE
	BEGIN
		-- update only name for now
		
		BEGIN TRAN t1
			DELETE [dbo].[Menu] WHERE MenuId = @MenuId
		COMMIT TRAN t1
	END
END
GO
/****** Object:  StoredProcedure [dbo].[MenuItems_GetBy]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MenuItems_GetBy]
	@MenuId INT = NULL, 
	@ItemId INT = null,
	@IsActive BIT = null,
	@From INT,
	@To INT
AS
SELECT
* FROM
(
	SELECT m.*, (ROW_NUMBER() OVER (ORDER BY m.[Order]
		   )) AS RowNum, Count(*) OVER() AS Total FROM [dbo].[MenuItems] m
    
    WHERE (m.MenuId = @MenuId OR @MenuId is null) AND

	(m.ItemId = @ItemId OR @ItemId is null) AND
	(m.IsActive = @IsActive OR @IsActive is null)
) AS MenuItems
WHERE MenuItems.RowNum BETWEEN @From AND @To


	
GO
/****** Object:  StoredProcedure [dbo].[MessagingTemplate_CreateUpdateDelete]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MessagingTemplate_CreateUpdateDelete] 
	@Action TINYINT,
	@Subject NVARCHAR(128) = null,
	@Parameters NVARCHAR(512) = null,
	@TemplateTypeId INT = 0,
	@Target INT = 0,
	@ContentId INT,
	@EmailPriorityId TINYINT,
	@EmailBodyFormatId TINYINT,
	@Id INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @Action = 1
	BEGIN
		INSERT [dbo].[MessagingTemplates]  (ContentId, TemplateTypeId, [Parameters], [Target], [Subject], [EmailPriorityId], [EmailBodyFormatId])
		VALUES (@ContentId, @TemplateTypeId, @Parameters, @Target, @Subject, @EmailPriorityId, @EmailBodyFormatId)

		SET @Id = SCOPE_IDENTITY()
	END
	ELSE IF @Action = 2
	BEGIN 
			UPDATE [dbo].[MessagingTemplates] 
			SET 
			[Subject] = @Subject,
			[Parameters] = @Parameters,
			[EmailPriorityId] = @EmailPriorityId, 
			[EmailBodyFormatId] = @EmailBodyFormatId,
			/*[Target] = @Target,*/
			TemplateTypeId = @TemplateTypeId
			WHERE MessagingTemplateId = @Id
	END
	ELSE
	BEGIN
		-- update only name for now
		BEGIN TRAN t1
		DELETE [dbo].[MessagingTemplates] WHERE ContentId = @ContentId
		DELETE [dbo].[Content] WHERE ContentId = @ContentId
		

		COMMIT TRAN t1
	END
END
GO
/****** Object:  StoredProcedure [dbo].[MessagingTemplate_GetBy]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MessagingTemplate_GetBy] 
	@ContentId INT = NULL,
	@Target TINYINT = NULL,
	@TemplateTypeId TINYINT = NULL,
	@IsActive BIT = NULL,
	@SiteId INT = NULL,
	@From INT,
	@To INT

AS
BEGIN


	BEGIN

		SELECT * FROM (
			SELECT c.*, et.[Target], et.[Subject], et.[Parameters], et.TemplateTypeId, ROW_NUMBER() OVER(ORDER BY c.ContentId) AS RowId, Count(*) OVER() AS Total FROM [dbo].[Content] c, [dbo].[MessageTemplates] et
			WHERE c.ContentId = et.ContentId AND (c.ContentId = @ContentId OR @ContentId is null) AND
			(et.[Target] = @Target OR @Target is null) AND
			(et.[TemplateTypeId] = @TemplateTypeId OR @TemplateTypeId is null) AND
			(IsActive = @IsActive OR @IsActive is null) AND -- only active if specified overwise all
			(SiteId = @SiteId OR @SiteId is null) 
		) AS tbl
		WHERE RowId BETWEEN @From AND @To
		ORDER BY RowId

	END
END
GO
/****** Object:  StoredProcedure [dbo].[MetaData_CreateUpdateDelete]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MetaData_CreateUpdateDelete] 
	@Action TINYINT,
	@ItemId INT,
	@MetaDataTypeId TINYINT,
	@Key NVARCHAR(128),
	@Value NVARCHAR(1024),
	@CreatedOn DATETIME,
	@CreatedBy NVARCHAR(64),
	@ModifiedOn DATETIME = NULL,
	@ModifiedBy NVARCHAR(64),
	@PublishedOn DATETIME = NULL,
	@PublishedBy NVARCHAR(64),
	@SiteId INT,
	@MetaDataId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF @Action = 1
	BEGIN
		INSERT [dbo].[MetaData] (ItemId, MetaDataTypeId, SiteId, [Key], Value, CreatedOn, CreatedBy)
		VALUES (@ItemId, @MetaDataTypeId, @SiteId, @Key, @Value, @CreatedOn, @CreatedBy)

		SET @MetaDataId = SCOPE_IDENTITY()
	END
	ELSE IF @Action = 2
	BEGIN 
			UPDATE [dbo].[MetaData]
			SET 
			[Key] = @Key,
			Value = @Value,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			PublishedOn = @PublishedOn,
			PublishedBy = @PublishedBy
		
			WHERE MetaDataId = @MetaDataId
	END
	ELSE
	BEGIN
	

		DELETE [dbo].[MetaData] WHERE MetaDataId = @MetaDataId

	END
END
GO
/****** Object:  StoredProcedure [dbo].[MetaData_GetBy]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MetaData_GetBy] 
	@ItemId INT = null,
	@SiteId INT = null,
	@MetaDataTypeId TINYINT = null
AS
BEGIN


SELECT * FROM [dbo].[MetaData] WHERE (ItemId = @ItemId OR @ItemId is null) AND (SiteId = @SiteId OR @SiteId is null)
AND (MetaDataTypeId = @MetaDataTypeId OR @MetaDataTypeId is null)
END
GO
/****** Object:  StoredProcedure [dbo].[Pages_CreateUpdateDelete]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Pages_CreateUpdateDelete] 
	@Action TINYINT,
	@PageTypeId INT,
	@ParentPageId INT = null,
	@MasterPageId INT = NULL,
	@Css NVARCHAR(64) = NULL,
	@Name NVARCHAR(128),
	@Slug NVARCHAR(128),
	@MenuIndex TINYINT = null, 
	@IsActive BIT,
	@CreatedOn DATETIME,
	@CreatedBy NVARCHAR(64),
	@ModifiedOn DATETIME = NULL,
	@ModifiedBy NVARCHAR(64),
	@PublishedOn DATETIME = NULL,
	@PublishedBy NVARCHAR(64),
	@SiteId INT,
	@Title NVARCHAR(128),
	@PageId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Path VARCHAR(1024) -- parameter path

    IF @Action = 1
	BEGIN
		INSERT [dbo].[Pages] (ParentPageId, PageTypeId, MasterPageId, Css, IsActive, Name, Slug, CreatedOn, CreatedBy, PublishedOn, PublishedBy, SiteId, ModifiedOn, ModifiedBy, Title, MenuIndex)
		VALUES (@ParentPageId, @PageTypeId, @MasterPageId, @Css, @IsActive, @Name, @Slug, @CreatedOn, @CreatedBy, @PublishedOn, @PublishedBy, @SiteId, @ModifiedOn, @ModifiedBy, @Title, @MenuIndex)

		SET @PageId = SCOPE_IDENTITY()
	END
	ELSE IF @Action = 2
	BEGIN
			UPDATE [dbo].[Pages] 
			SET 
			PageTypeId = @PageTypeId,
			ParentPageId = @ParentPageId,
			MasterPageId = @MasterPageId,
			Css = @Css,
			Name = @Name, 
			Slug = @Slug,
			MenuIndex = @MenuIndex,
			IsActive = @IsActive,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy,
			PublishedOn = @PublishedOn,
			PublishedBy = @PublishedBy,
			Title = @Title
			WHERE PageId = @PageId

			UPDATE [dbo].[Sites] SET ModifiedOn = @ModifiedOn WHERE SiteId = @SiteId
	END
	ELSE
	BEGIN
		-- update only name for now
		DECLARE @tbl TABLE (ContentId INT)

		INSERT INTO @tbl SELECT ContentId FROM [dbo].[ContentToPages] WHERE PageId= @PageId
		DELETE [dbo].[ContentToPages] WHERE PageId= @PageId
		DELETE [dbo].[Content] WHERE ContentId IN (SELECT ContentId FROM @tbl)
		DELETE [dbo].[MetaData] WHERE ItemId = @PageId AND MetaDataTypeId = 1 /* page*/
		DELETE [dbo].[Pages] WHERE PageId = @PageId

	END
END
GO
/****** Object:  StoredProcedure [dbo].[Pages_GetBy]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Pages_GetBy]
	@PageId INT = NULL, 
	@SiteId INT = null,
	@Name NVARCHAR(128) = null,
	@Slug NVARCHAR(128) = null,
	@PageTypeId TINYINT = NULL,
	@ParentId INT = NULL,
	@IsActive BIT = null,
	@Published BIT = NULL,
	@From INT,
	@To INT
AS
SELECT
* FROM
(
	SELECT p.*, (ROW_NUMBER() OVER (ORDER BY CreatedOn
		   )) AS RowNum, Count(*) OVER() AS Total FROM [dbo].[Pages] p
    
    WHERE (p.PageId = @PageId OR @PageId is null) AND
	(p.SiteId = @SiteId OR @SiteId is null) AND
	(p.[Name] = @Name OR @Name is null) AND
	(p.[slug] = @slug OR @slug is null) AND
	(p.PageTypeId = @PageTypeId OR @PageTypeId is null) AND
	(p.ParentPageId = @ParentId OR @ParentId is null) AND
	(((p.PublishedOn is null AND @Published=0) OR (p.PublishedOn is not null AND @Published=1)) OR @Published is null) AND
	(p.IsActive = @IsActive OR @IsActive is null)
) AS Pages
WHERE Pages.RowNum BETWEEN @From AND @To


	
GO
/****** Object:  StoredProcedure [dbo].[Sites_CreateUpdate]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sites_CreateUpdate] 
	@Action TINYINT,
	@Name NVARCHAR(64),
	@Host NVARCHAR(64),
	@Language CHAR(3),
	@TimeZoneOffset DECIMAL,
    @PageTitleSeparator NVARCHAR(128),
    @IsActive BIT,
	@ParentSiteId INT = NULL,
	@ContentType NVARCHAR(64),
	@CreatedOn DATETIME,
	@CreatedBy NVARCHAR(64),
	@ModifiedOn DATETIME,
	@ModifiedBy NVARCHAR(64),
	@SiteId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Path VARCHAR(1024) -- parameter path

	IF @Action = 1 -- creaete
	BEGIN 
			INSERT [dbo].[Sites] 
			([Name], Host, [Language], TimeZoneOffset, PageTitleSeparator, ContentType, IsActive, ParentSiteId,  CreatedOn, CreatedBy)
			 VALUES
			 (@Name,@Host,@Language,@TimeZoneOffset,@PageTitleSeparator,@ContentType, @IsActive, @ParentSiteId, @CreatedOn, @CreatedBy)

			 SET @SiteId = SCOPE_IDENTITY()
	END

    ELSE IF @Action = 2
	BEGIN 
			UPDATE [dbo].[Sites] 
			SET 
			TimeZoneOffset = @TimeZoneOffset,
			PageTitleSeparator = @PageTitleSeparator,
			Host = @Host,
			IsActive = @IsActive,
			ContentType = @ContentType,
			ModifiedOn = @ModifiedOn,
			ModifiedBy = @ModifiedBy
			WHERE SiteId = @SiteId
	END
	
END
GO
/****** Object:  StoredProcedure [dbo].[Sites_GetBy]    Script Date: 2/8/2019 6:28:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sites_GetBy] 
	@SiteId INT = null,
	@ParentSiteId INT = null,
	@Name NVARCHAR(128) = null,
	@Host NVARCHAR(128) = null,
	@Lang NCHAR(3) = null,
	@IsActive BIT = null,
	@From INT,
	@To INT
AS
SELECT
* FROM
(
	SELECT s.*, (ROW_NUMBER() OVER (ORDER BY CreatedOn
		   )) AS RowNum, Count(*) OVER() AS Total FROM [dbo].[Sites] s
    
    WHERE (s.SiteId = @SiteId OR @SiteId is null) AND
	(s.[Name] = @Name OR @Name is null) AND
	(s.[Host] = @Host OR @Host is null) AND
	(s.ParentSiteId = @ParentSiteId OR @ParentSiteId is null) AND
	(s.[Language] = @Lang OR @Lang is null) AND
	(s.IsActive = @IsActive OR @IsActive is null)
) AS Sites
WHERE Sites.RowNum BETWEEN @From AND @To


	
GO
