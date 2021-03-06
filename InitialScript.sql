ALTER TABLE [dbo].[ServiceTitanResponse]

DROP CONSTRAINT [FK__ServiceTi__syncI__72C60C4A]
GO

ALTER TABLE [dbo].[ServiceTitanResponse]

DROP CONSTRAINT [FK__ServiceTi__Clien__71D1E811]
GO

ALTER TABLE [dbo].[CapManagement]

DROP CONSTRAINT [FK__CapManage__Clien__70DDC3D8]
GO

ALTER TABLE [dbo].[SyncHistory]

DROP CONSTRAINT [DF__SyncHisto__creat__7B5B524B]
GO

ALTER TABLE [dbo].[ServiceTitanResponse]

DROP CONSTRAINT [DF__ServiceTi__creat__6D0D32F4]
GO

ALTER TABLE [dbo].[ServiceStatusHistory]

DROP CONSTRAINT [DF__ServiceSt__Creat__797309D9]
GO

ALTER TABLE [dbo].[ClientMaster]

DROP CONSTRAINT [DF_Client_Credentials_createdon]
GO

/****** Object:  Table [dbo].[SyncHistory]    Script Date: 29-06-2022 10:43:52 AM ******/
IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'[dbo].[SyncHistory]')
			AND type IN (N'U')
		)
	DROP TABLE [dbo].[SyncHistory]
GO

/****** Object:  Table [dbo].[ServiceTitanResponse]    Script Date: 29-06-2022 10:43:52 AM ******/
IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'[dbo].[ServiceTitanResponse]')
			AND type IN (N'U')
		)
	DROP TABLE [dbo].[ServiceTitanResponse]
GO

/****** Object:  Table [dbo].[ServiceStatusHistory]    Script Date: 29-06-2022 10:43:52 AM ******/
IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'[dbo].[ServiceStatusHistory]')
			AND type IN (N'U')
		)
	DROP TABLE [dbo].[ServiceStatusHistory]
GO

/****** Object:  Table [dbo].[ErrorHandler]    Script Date: 29-06-2022 10:43:52 AM ******/
IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'[dbo].[ErrorHandler]')
			AND type IN (N'U')
		)
	DROP TABLE [dbo].[ErrorHandler]
GO

/****** Object:  Table [dbo].[ClientMaster]    Script Date: 29-06-2022 10:43:52 AM ******/
IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'[dbo].[ClientMaster]')
			AND type IN (N'U')
		)
	DROP TABLE [dbo].[ClientMaster]
GO

/****** Object:  Table [dbo].[CapManagement]    Script Date: 29-06-2022 10:43:52 AM ******/
IF EXISTS (
		SELECT *
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'[dbo].[CapManagement]')
			AND type IN (N'U')
		)
	DROP TABLE [dbo].[CapManagement]
GO

/****** Object:  Table [dbo].[CapManagement]    Script Date: 29-06-2022 10:43:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CapManagement] (
	[id] [int] IDENTITY(1, 1) NOT NULL
	,[ClientId] [int] NULL
	,[date] [date] NULL
	,[InitalCap] [time](7) NULL
	,[PendingCap] [time](7) NULL
	,PRIMARY KEY CLUSTERED ([id] ASC) WITH (
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
		,OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ClientMaster]    Script Date: 29-06-2022 10:43:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClientMaster] (
	[Id] [int] NOT NULL
	,[client_name] [varchar](max) NULL
	,[agent_group] [varchar](max) NULL
	,[tenant] [int] NULL
	,[STClientSecret] [varchar](max) NULL
	,[Cap] [int] NULL
	,[createdon] [datetime] NULL
	,[STClientID] [varchar](100) NULL
	,[MappedAgents] [varchar](max) NULL
	,CONSTRAINT [PK_ClientMaster] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
		,OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
		) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ErrorHandler]    Script Date: 29-06-2022 10:43:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ErrorHandler] (
	[Id] [int] IDENTITY(1, 1) NOT NULL
	,[ClientId] [int] NULL
	,[syncId] [int] NULL
	,[ErrorMessage] [varchar](max) NULL
	,[ErrorStacktrace] [varchar](max) NULL
	,[createdOn] [datetime] NULL
	,[ServiceTitanResponseId] [int] NULL
	,PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
		,OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
		) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ServiceStatusHistory]    Script Date: 29-06-2022 10:43:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ServiceStatusHistory] (
	[ID] [int] IDENTITY(1, 1) NOT NULL
	,[ClientId] [int] NULL
	,[SyncHistoryId] [int] NULL
	,[ServiceTitanResponseId] [int] NULL
	,[Status] [int] NULL
	,[CreatedOn] [datetime] NULL
	,PRIMARY KEY CLUSTERED ([ID] ASC) WITH (
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
		,OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ServiceTitanResponse]    Script Date: 29-06-2022 10:43:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ServiceTitanResponse] (
	[Id] [int] IDENTITY(1, 1) NOT NULL
	,[ClientId] [int] NULL
	,[LeadCallid] [int] NULL
	,[duration] [time](7) NULL
	,[from] [varchar](100) NULL
	,[to] [varchar](100) NULL
	,[RowServiceTitanJson] [varchar](max) NULL
	,[FilePath] [varchar](100) NULL
	,[assemblyairesponse] [varchar](max) NULL
	,[createdOn] [datetime] NULL
	,[syncId] [int] NULL
	,[ResponseStatus] [int] NULL
	,[ErrorCode] [varchar](max) NULL
	,[ReceivedOn] [datetime] NULL
	,PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
		,OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
		) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SyncHistory]    Script Date: 29-06-2022 10:43:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SyncHistory] (
	[id] [int] IDENTITY(1, 1) NOT NULL
	,[syncStartOn] [datetime] NULL
	,[syncEndOn] [datetime] NULL
	,[createdOn] [datetime] NULL
	,PRIMARY KEY CLUSTERED ([id] ASC) WITH (
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
		,OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	50
	,N'hammondservices'
	,N'Hammond Services'
	,371174286
	,N'cs2.60iyo4h1nbi89nuih7yyifmlhx05k2axsqs05dt1t8zuc4jaet'
	,1250
	,CAST(N'2022-06-23T13:30:35.427' AS DATETIME)
	,N'cid.yvvgl5ksu414cswueokuup9nc'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	226
	,N'bonney'
	,N'Bonney Plumbing, Inc.'
	,215810824
	,N'cs1.1bzr123y7w83gu4ttkb1otrbtgki2gjvxm759w77r7uu1wp4em'
	,4250
	,CAST(N'2022-06-23T13:30:35.430' AS DATETIME)
	,N'cid.jn0ywx0i3hnh4kr60co53sz47'
	,N'aamaro, acarmichael@bonney.com, ajackson@bonney.com, ccookman@bonney.com, cmurphy1, ewickert, Kayjohnson, kerriem, lwoods@bonney.com, mariaro, mcastr0, melissaa, mhaley1, MKNAPPENBERGER'
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	294
	,N'ccheatingandairconditioning'
	,N'C and C Heating and Air Conditioning STV'
	,462597653
	,N'cs1.7ryxb7h2x6q9ddkjz3fyecgxwgmg6omezzgq7d0oaman9lp6se'
	,0
	,CAST(N'2022-06-23T13:30:35.433' AS DATETIME)
	,N'cid.hsufiq0sjh4xfps00lp6zl545'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	371
	,N'starcityheatingcooling'
	,N'Star City Heating and Cooling'
	,903157930
	,N'cs1.tuw40tam1mg6gkljpxvq6komh76fhlf9wf9yhk0h5v4rzumixs'
	,750
	,CAST(N'2022-06-23T13:30:35.433' AS DATETIME)
	,N'cid.jyjwumamgdwt4no8c8gkboqp9'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	412
	,N'foxsonsplumbingheating'
	,N'Fox Plumbing Heating Cooling Electrical STV'
	,365083280
	,N'cs1.7ryxb7h2x6q9ddkjz3fyecgxwgmg6omezzgq7d0oaman9lp6se'
	,750
	,CAST(N'2022-06-23T13:30:35.440' AS DATETIME)
	,N'cid.8zqd1red230qvik86gu58i19m'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	426
	,N'universeappliance'
	,N'Universe Home Services STV'
	,358244379
	,N'cs1.5utjcru0npof9t18eperw4x7vaw25ctfjk2y6vnn53358x5h78'
	,0
	,CAST(N'2022-06-23T13:30:35.440' AS DATETIME)
	,N'cid.m8doakg7bv6onjqtt06oy69bz'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	463
	,N'a1plumbingperfectair'
	,N'Perfect Plumbing, Heating and Air STV'
	,235169027
	,N'cs1.vd8wlmg1la2tkzdg6fqu4hehr34bmgfjniit74fmuv09ktfnmq'
	,0
	,CAST(N'2022-06-23T13:30:35.440' AS DATETIME)
	,N'cid.jjuzw0lo7qzkrn9e1yaacn2ho'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	512
	,N'mattioni'
	,N'Mattioni Plumbing Heating and Cooling STV'
	,227669019
	,N'cs1.va9lgxv72hg68efgkqyzfd1bhvkojpfe77zk35cks97pdei2q0'
	,0
	,CAST(N'2022-06-23T13:30:35.440' AS DATETIME)
	,N'cid.ye8j9967rav0gm411jkfil559'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	520
	,N'donnellysplumbingandheating'
	,N'Donnellys Plumbing Heating and Cooling STV'
	,372608904
	,N'cs1.6jp4ukib0kpnfcy3ho6c9uptjuqjqfakp0m5v2iqnooq2kx9nh'
	,0
	,CAST(N'2022-06-23T13:30:35.440' AS DATETIME)
	,N'cid.4eezfturhzvkluezz8rzkpgot'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	529
	,N'myplumber'
	,N'My Plumber Plus STV'
	,446993669
	,N'cs1.wyojsbkh1z8syziaigcxxvuk7s9kwlixfklhu0fhqhci5dcv2z'
	,0
	,CAST(N'2022-06-23T13:30:35.440' AS DATETIME)
	,N'cid.gl13369t8h9uxyq6ffs7sv88d'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	564
	,N'barkermechanicalservices'
	,N'Barker Heating & Cooling Outbound'
	,308650755
	,N'cs1.dn88b2igck7zffgqtx9h80m2ih1s0jqx9olvm2r506vvzmn63n'
	,750
	,CAST(N'2022-06-23T13:30:35.443' AS DATETIME)
	,N'cid.vwyi47dh2oge75pn2wx25y09f'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	591
	,N'myplumber'
	,N'My Plumber Plus OB'
	,446993669
	,N'cs1.wyojsbkh1z8syziaigcxxvuk7s9kwlixfklhu0fhqhci5dcv2z'
	,1250
	,CAST(N'2022-06-23T13:30:35.443' AS DATETIME)
	,N'cid.gl13369t8h9uxyq6ffs7sv88d'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	601
	,N'silaservicesny'
	,N'Sila Heating Air Conditioning and Plumbing NY STV'
	,857364776
	,N'cs1.gubokqezmyyhzkj1197gweiktppwroqkbex2ylyh6v37yj03xf'
	,0
	,CAST(N'2022-06-23T13:30:35.443' AS DATETIME)
	,N'cid.wu0508hrjw820vizqp4y3z7gw'
	,NULL
	)

INSERT [dbo].[ClientMaster] (
	[Id]
	,[client_name]
	,[agent_group]
	,[tenant]
	,[STClientSecret]
	,[Cap]
	,[createdon]
	,[STClientID]
	,[MappedAgents]
	)
VALUES (
	658
	,N'plumbingplus'
	,N'Plumbing Plus STV'
	,357303835
	,N'cs1.kw8tn8fvfbyi4dwfc6w5r5wpv7twxxv5yo8wzoe4mcwik6a7ap'
	,0
	,CAST(N'2022-06-23T13:30:35.443' AS DATETIME)
	,N'cid.xsqgpdb3gfr64189fuxit7y9e'
	,NULL
	)
GO

ALTER TABLE [dbo].[ClientMaster] ADD CONSTRAINT [DF_Client_Credentials_createdon] DEFAULT(getdate())
FOR [createdon]
GO

ALTER TABLE [dbo].[ServiceStatusHistory] ADD DEFAULT(getdate())
FOR [CreatedOn]
GO

ALTER TABLE [dbo].[ServiceTitanResponse] ADD DEFAULT(getdate())
FOR [createdOn]
GO

ALTER TABLE [dbo].[SyncHistory] ADD DEFAULT(getdate())
FOR [createdOn]
GO

ALTER TABLE [dbo].[CapManagement]
	WITH CHECK ADD FOREIGN KEY ([ClientId]) REFERENCES [dbo].[ClientMaster]([Id])
GO

ALTER TABLE [dbo].[ServiceTitanResponse]
	WITH CHECK ADD FOREIGN KEY ([ClientId]) REFERENCES [dbo].[ClientMaster]([Id])
GO

ALTER TABLE [dbo].[ServiceTitanResponse]
	WITH CHECK ADD FOREIGN KEY ([syncId]) REFERENCES [dbo].[SyncHistory]([id])
GO


