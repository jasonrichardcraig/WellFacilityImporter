USE [master]
GO
/****** Object:  Database [WellFacilityRepository]    Script Date: 2024-10-16 1:10:37 AM ******/
CREATE DATABASE [WellFacilityRepository]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WellFacilityRepository', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\WellFacilityRepository.mdf' , SIZE = 447424KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WellFacilityRepository_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\WellFacilityRepository_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [WellFacilityRepository] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WellFacilityRepository].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WellFacilityRepository] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET ARITHABORT OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WellFacilityRepository] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WellFacilityRepository] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WellFacilityRepository] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WellFacilityRepository] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [WellFacilityRepository] SET  MULTI_USER 
GO
ALTER DATABASE [WellFacilityRepository] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WellFacilityRepository] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WellFacilityRepository] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WellFacilityRepository] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WellFacilityRepository] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WellFacilityRepository] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'WellFacilityRepository', N'ON'
GO
ALTER DATABASE [WellFacilityRepository] SET QUERY_STORE = ON
GO
ALTER DATABASE [WellFacilityRepository] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [WellFacilityRepository]
GO
/****** Object:  Schema [BusinessAssociate]    Script Date: 2024-10-16 1:10:38 AM ******/
CREATE SCHEMA [BusinessAssociate]
GO
/****** Object:  Schema [FacilityInfrastructure]    Script Date: 2024-10-16 1:10:38 AM ******/
CREATE SCHEMA [FacilityInfrastructure]
GO
/****** Object:  Schema [FacilityLicence]    Script Date: 2024-10-16 1:10:38 AM ******/
CREATE SCHEMA [FacilityLicence]
GO
/****** Object:  Schema [FacilityOperatorHistory]    Script Date: 2024-10-16 1:10:38 AM ******/
CREATE SCHEMA [FacilityOperatorHistory]
GO
/****** Object:  Schema [WellFacilityLink]    Script Date: 2024-10-16 1:10:38 AM ******/
CREATE SCHEMA [WellFacilityLink]
GO
/****** Object:  Schema [WellInfrastructure]    Script Date: 2024-10-16 1:10:38 AM ******/
CREATE SCHEMA [WellInfrastructure]
GO
/****** Object:  Schema [WellLicence]    Script Date: 2024-10-16 1:10:38 AM ******/
CREATE SCHEMA [WellLicence]
GO
/****** Object:  Schema [WellWiki]    Script Date: 2024-10-16 1:10:38 AM ******/
CREATE SCHEMA [WellWiki]
GO
/****** Object:  Table [BusinessAssociate].[BusinessAssociate]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BusinessAssociate].[BusinessAssociate](
	[BAIdentifier] [nvarchar](50) NOT NULL,
	[BALegalName] [nvarchar](255) NULL,
	[BAAddress] [nvarchar](500) NULL,
	[BAPhoneNumber] [nvarchar](50) NULL,
	[BACorporateStatus] [nvarchar](50) NULL,
	[BACorporateStatusEffectiveDate] [datetime] NULL,
	[AmalgamatedIntoBAID] [nvarchar](50) NULL,
	[AmalgamatedIntoBALegalName] [nvarchar](255) NULL,
	[BAAmalgamationEstablishedDate] [datetime] NULL,
	[BALicenceEligibilityType] [nvarchar](50) NULL,
	[BALicenceEligibiltyDesc] [nvarchar](100) NULL,
	[BAAbbreviatedName] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[BAIdentifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FacilityInfrastructure].[Facility]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FacilityInfrastructure].[Facility](
	[FacilityID] [nvarchar](50) NULL,
	[FacilityProvinceState] [nvarchar](50) NULL,
	[FacilityType] [nvarchar](50) NULL,
	[FacilityIdentifier] [nvarchar](50) NULL,
	[FacilityName] [nvarchar](255) NULL,
	[FacilitySubType] [nvarchar](50) NULL,
	[FacilitySubTypeDesc] [nvarchar](255) NULL,
	[ExperimentalConfidential] [char](1) NULL,
	[FacilityStartDate] [datetime] NULL,
	[FacilityLocation] [nvarchar](100) NULL,
	[FacilityLegalSubdivision] [nvarchar](50) NULL,
	[FacilitySection] [int] NULL,
	[FacilityTownship] [int] NULL,
	[FacilityRange] [int] NULL,
	[FacilityMeridian] [int] NULL,
	[FacilityLicenceStatus] [nvarchar](50) NULL,
	[FacilityOperationalStatus] [nvarchar](50) NULL,
	[FacilityOperationalStatusDate] [datetime] NULL,
	[LicenceType] [nvarchar](50) NULL,
	[LicenceNumber] [nvarchar](50) NULL,
	[EnergyDevelopmentCategoryType] [nvarchar](50) NULL,
	[LicenceIssueDate] [datetime] NULL,
	[LicenseeBAID] [nvarchar](50) NULL,
	[LicenseeName] [nvarchar](255) NULL,
	[OperatorBAID] [nvarchar](50) NULL,
	[OperatorName] [nvarchar](255) NULL,
	[OperatorStartDate] [datetime] NULL,
	[TerminalPipelineLink] [nvarchar](255) NULL,
	[TPFacilityProvinceState] [nvarchar](50) NULL,
	[TPFacilityType] [nvarchar](50) NULL,
	[TPFacilityIdentifier] [nvarchar](50) NULL,
	[MeterStationPipelineLink] [nvarchar](255) NULL,
	[MPFacilityProvinceState] [nvarchar](50) NULL,
	[MPFacilityType] [nvarchar](50) NULL,
	[MPFacilityIdentifier] [nvarchar](50) NULL,
	[EnergyDevelopmentCategoryID] [nvarchar](50) NULL,
	[OrphanWellFlg] [char](1) NULL,
	[TierAggregateID] [nvarchar](50) NULL,
	[TierAggregatePR] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [FacilityLicence].[Licence]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FacilityLicence].[Licence](
	[LicenceType] [nvarchar](50) NOT NULL,
	[LicenceNumber] [nvarchar](50) NOT NULL,
	[LicenceStatus] [nvarchar](50) NULL,
	[LicenceStatusDate] [datetime] NULL,
	[Licensee] [nvarchar](50) NULL,
	[LicenseeName] [nvarchar](255) NULL,
	[EnergyDevelopmentCategoryType] [nvarchar](100) NULL,
	[LicenceLocation] [nvarchar](100) NULL,
	[LicenceLegalSubdivision] [nvarchar](50) NULL,
	[LicenceSection] [int] NULL,
	[LicenceTownship] [int] NULL,
	[LicenceRange] [int] NULL,
	[LicenceMeridian] [int] NULL,
	[OrphanWellFlg] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[LicenceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FacilityOperatorHistory].[Facility]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FacilityOperatorHistory].[Facility](
	[FacilityID] [nvarchar](50) NOT NULL,
	[FacilityProvinceState] [nvarchar](50) NULL,
	[FacilityType] [nvarchar](50) NULL,
	[FacilityIdentifier] [nvarchar](50) NULL,
	[FacilityName] [nvarchar](255) NULL,
	[FacilitySubType] [nvarchar](50) NULL,
	[FacilitySubTypeDesc] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[FacilityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [FacilityOperatorHistory].[OperatorHistory]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [FacilityOperatorHistory].[OperatorHistory](
	[FacilityID] [nvarchar](50) NOT NULL,
	[OperatorBAID] [nvarchar](50) NOT NULL,
	[OperatorName] [nvarchar](255) NULL,
	[StartDate] [nvarchar](7) NOT NULL,
	[EndDate] [nvarchar](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[FacilityID] ASC,
	[OperatorBAID] ASC,
	[StartDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WellFacilityLink].[LinkedFacility]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WellFacilityLink].[LinkedFacility](
	[LinkedFacilityID] [nvarchar](50) NOT NULL,
	[WellID] [nvarchar](50) NOT NULL,
	[LinkedFacilityProvinceState] [nvarchar](10) NULL,
	[LinkedFacilityType] [nvarchar](10) NULL,
	[LinkedFacilityIdentifier] [nvarchar](50) NULL,
	[LinkedFacilityName] [nvarchar](100) NULL,
	[LinkedFacilitySubType] [nvarchar](50) NULL,
	[LinkedFacilitySubTypeDesc] [nvarchar](100) NULL,
	[LinkedStartDate] [date] NULL,
	[LinkedFacilityOperatorBAID] [nvarchar](50) NULL,
	[LinkedFacilityOperatorName] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[LinkedFacilityID] ASC,
	[WellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WellFacilityLink].[WellFacilityLink]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WellFacilityLink].[WellFacilityLink](
	[WellID] [nvarchar](50) NOT NULL,
	[WellProvinceState] [nvarchar](10) NULL,
	[WellType] [nvarchar](10) NULL,
	[WellIdentifier] [nvarchar](50) NULL,
	[WellLocationException] [nvarchar](50) NULL,
	[WellLegalSubdivision] [nvarchar](10) NULL,
	[WellSection] [int] NULL,
	[WellTownship] [int] NULL,
	[WellRange] [int] NULL,
	[WellMeridian] [int] NULL,
	[WellEventSequence] [int] NULL,
	[WellName] [nvarchar](100) NULL,
	[WellStatusFluid] [nvarchar](50) NULL,
	[WellStatusMode] [nvarchar](50) NULL,
	[WellStatusType] [nvarchar](50) NULL,
	[WellStatusStructure] [nvarchar](50) NULL,
	[WellStatusFluidCode] [nvarchar](10) NULL,
	[WellStatusModeCode] [nvarchar](10) NULL,
	[WellStatusTypeCode] [nvarchar](10) NULL,
	[WellStatusStructureCode] [nvarchar](10) NULL,
	[WellStatusStartDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[WellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WellInfrastructure].[CommingledWell]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WellInfrastructure].[CommingledWell](
	[ComminglingProcessApprovalNumber] [varchar](50) NOT NULL,
	[WellID] [varchar](50) NOT NULL,
	[ComminglingProcess] [varchar](50) NULL,
	[ComminglingEffDate] [date] NULL,
	[CommingledReportingWellID] [varchar](50) NULL,
	[CommingledReportingWellProvinceState] [varchar](50) NULL,
	[CommingledReportingWellType] [varchar](50) NULL,
	[CommingledReportingWellIdentifier] [varchar](50) NULL,
 CONSTRAINT [PK_CommingledWell] PRIMARY KEY CLUSTERED 
(
	[ComminglingProcessApprovalNumber] ASC,
	[WellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WellInfrastructure].[LinkedFacility]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WellInfrastructure].[LinkedFacility](
	[LinkedFacilityID] [varchar](50) NOT NULL,
	[WellID] [varchar](50) NOT NULL,
	[LinkedFacilityProvinceState] [varchar](50) NULL,
	[LinkedFacilityType] [varchar](50) NULL,
	[LinkedFacilityIdentifier] [varchar](50) NULL,
	[LinkedFacilityName] [varchar](255) NULL,
	[LinkedFacilitySubType] [varchar](50) NULL,
	[LinkedFacilitySubTypeDesc] [varchar](255) NULL,
	[LinkedStartDate] [date] NULL,
	[LinkedFacilityOperatorBAID] [varchar](50) NULL,
	[LinkedFacilityOperatorLegalName] [varchar](255) NULL,
 CONSTRAINT [PK_LinkedFacility] PRIMARY KEY CLUSTERED 
(
	[LinkedFacilityID] ASC,
	[WellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WellInfrastructure].[Well]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WellInfrastructure].[Well](
	[WellID] [varchar](50) NOT NULL,
	[WellProvinceState] [varchar](2) NULL,
	[WellType] [varchar](50) NULL,
	[WellIdentifier] [varchar](50) NULL,
	[PreviousWellID] [varchar](50) NULL,
	[WellLocationException] [varchar](10) NULL,
	[WellLegalSubdivision] [varchar](10) NULL,
	[WellSection] [int] NULL,
	[WellTownship] [int] NULL,
	[WellRange] [int] NULL,
	[WellMeridian] [int] NULL,
	[WellEventSequence] [int] NULL,
	[WellName] [varchar](255) NULL,
	[ConfidentialType] [varchar](50) NULL,
	[ExperimentalConfidentialIndicator] [char](1) NULL,
	[ExperimentalConfidentialEffDate] [date] NULL,
	[ExperimentalConfidentialTermDate] [date] NULL,
	[LicenceType] [varchar](50) NULL,
	[LicenceNumber] [varchar](50) NULL,
	[LicenceIssueDate] [date] NULL,
	[LicenceStatusDate] [date] NULL,
	[LicenceStatus] [varchar](50) NULL,
	[Field] [varchar](10) NULL,
	[FieldName] [varchar](100) NULL,
	[Area] [varchar](100) NULL,
	[AreaName] [varchar](100) NULL,
	[PoolDeposit] [varchar](100) NULL,
	[PoolDepositName] [varchar](100) NULL,
	[PoolDepositDensity] [decimal](10, 4) NULL,
	[WellStatusFluid] [varchar](50) NULL,
	[WellStatusMode] [varchar](50) NULL,
	[WellStatusType] [varchar](50) NULL,
	[WellStatusStructure] [varchar](50) NULL,
	[WellStatusFluidCode] [varchar](10) NULL,
	[WellStatusModeCode] [varchar](10) NULL,
	[WellStatusTypeCode] [varchar](10) NULL,
	[WellStatusStructureCode] [varchar](10) NULL,
	[WellStatusDate] [date] NULL,
	[SpudDate] [date] NULL,
	[HorizontalDrill] [varchar](50) NULL,
	[FinishedDrillDate] [date] NULL,
	[FinalTotalDepth] [decimal](10, 4) NULL,
	[MaxTrueVerticalDepth] [decimal](10, 4) NULL,
	[VolumetricGasWellLiquidType] [varchar](50) NULL,
	[VolumetricGasWellLiquidEffDate] [date] NULL,
	[LicenseeID] [varchar](10) NULL,
	[LicenseeName] [varchar](255) NULL,
	[AllowableType] [varchar](50) NULL,
	[BlockNumber] [int] NULL,
	[RecoveryMechanismType] [varchar](50) NULL,
	[OrphanWellFlg] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[WellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WellLicence].[Well]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WellLicence].[Well](
	[LicenceType] [nvarchar](50) NULL,
	[LicenceNumber] [nvarchar](50) NULL,
	[LicenceIssueDate] [datetime] NULL,
	[LicenceStatus] [nvarchar](50) NULL,
	[LicenceStatusDate] [datetime] NULL,
	[LicenseeID] [nvarchar](50) NULL,
	[LicenseeName] [nvarchar](255) NULL,
	[LicenceLocation] [nvarchar](100) NULL,
	[LicenceLegalSubdivision] [nvarchar](50) NULL,
	[LicenceSection] [int] NULL,
	[LicenceTownship] [int] NULL,
	[LicenceRange] [int] NULL,
	[LicenceMeridian] [int] NULL,
	[DrillingOperationType] [nvarchar](100) NULL,
	[WellPurpose] [nvarchar](255) NULL,
	[WellLicenceType] [nvarchar](100) NULL,
	[WellSubstance] [nvarchar](100) NULL,
	[ProjectedFormation] [nvarchar](100) NULL,
	[TerminatingFormation] [nvarchar](100) NULL,
	[ProjectedTotalDepth] [decimal](18, 2) NULL,
	[AERClass] [nvarchar](100) NULL,
	[HeadLessor] [nvarchar](100) NULL,
	[WellCompletionType] [nvarchar](100) NULL,
	[TargetPool] [nvarchar](100) NULL,
	[OrphanWellFlg] [char](1) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [WellWiki].[Well]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WellWiki].[Well](
	[WellID] [varchar](64) NOT NULL,
	[WellName] [varchar](128) NULL,
	[Location] [varchar](128) NULL,
	[LocationAlias] [varchar](128) NULL,
	[LocationAlternateAlias] [varchar](128) NULL,
	[Country] [varchar](128) NULL,
	[Province] [varchar](128) NULL,
	[Township] [int] NULL,
	[Meridian] [int] NULL,
	[Range] [int] NULL,
	[Section] [int] NULL,
	[County] [varchar](128) NULL,
	[SurfaceHoleLatitude] [float] NULL,
	[SurfaceHoleLongitude] [float] NULL,
	[OperatorName] [varchar](128) NULL,
	[LicenseNumber] [varchar](128) NULL,
	[LicenseDate] [date] NULL,
	[LicenseStatus] [varchar](128) NULL,
	[SpudDate] [date] NULL,
	[FinalDrillDate] [date] NULL,
	[WellTotalDepth] [float] NULL,
	[DateTimeCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Well] PRIMARY KEY CLUSTERED 
(
	[WellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WellWiki].[WellDirectionalDrilling]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WellWiki].[WellDirectionalDrilling](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WellID] [varchar](64) NOT NULL,
	[StartDate] [date] NULL,
	[Depth] [float] NULL,
	[Reason] [varchar](255) NULL,
 CONSTRAINT [PK_WellDirectionalDrilling] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WellWiki].[WellHistory]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WellWiki].[WellHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WellID] [varchar](64) NOT NULL,
	[Date] [date] NULL,
	[Event] [varchar](255) NULL,
 CONSTRAINT [PK_WellHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WellWiki].[WellPerforationTreatments]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WellWiki].[WellPerforationTreatments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WellID] [varchar](64) NOT NULL,
	[PerforationDate] [date] NULL,
	[PerforationType] [varchar](128) NULL,
	[IntervalTop] [float] NULL,
	[IntervalBase] [float] NULL,
	[NumberOfShots] [int] NULL,
 CONSTRAINT [PK_WellPerforationTreatments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [WellWiki].[WellProductionData]    Script Date: 2024-10-16 1:10:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [WellWiki].[WellProductionData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WellID] [varchar](64) NOT NULL,
	[Period] [int] NULL,
	[TotalProductionHours] [int] NULL,
	[GasQuantity] [float] NULL,
	[OilQuantity] [float] NULL,
	[WaterQuantity] [float] NULL,
 CONSTRAINT [PK_WellProductionData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [WellWiki].[Well] ADD  CONSTRAINT [DF_Wells_DateTimeCreated]  DEFAULT (getdate()) FOR [DateTimeCreated]
GO
ALTER TABLE [FacilityOperatorHistory].[OperatorHistory]  WITH NOCHECK ADD FOREIGN KEY([FacilityID])
REFERENCES [FacilityOperatorHistory].[Facility] ([FacilityID])
GO
ALTER TABLE [WellFacilityLink].[LinkedFacility]  WITH NOCHECK ADD FOREIGN KEY([WellID])
REFERENCES [WellFacilityLink].[WellFacilityLink] ([WellID])
ON DELETE CASCADE
GO
ALTER TABLE [WellInfrastructure].[CommingledWell]  WITH NOCHECK ADD  CONSTRAINT [FK__Commingle__WellI__4AB81AF0] FOREIGN KEY([WellID])
REFERENCES [WellInfrastructure].[Well] ([WellID])
GO
ALTER TABLE [WellInfrastructure].[CommingledWell] CHECK CONSTRAINT [FK__Commingle__WellI__4AB81AF0]
GO
ALTER TABLE [WellInfrastructure].[LinkedFacility]  WITH NOCHECK ADD  CONSTRAINT [FK__LinkedFac__WellI__48CFD27E] FOREIGN KEY([WellID])
REFERENCES [WellInfrastructure].[Well] ([WellID])
GO
ALTER TABLE [WellInfrastructure].[LinkedFacility] CHECK CONSTRAINT [FK__LinkedFac__WellI__48CFD27E]
GO
ALTER TABLE [WellWiki].[WellDirectionalDrilling]  WITH CHECK ADD  CONSTRAINT [FK_WellDirectionalDrilling_Well] FOREIGN KEY([WellID])
REFERENCES [WellWiki].[Well] ([WellID])
GO
ALTER TABLE [WellWiki].[WellDirectionalDrilling] CHECK CONSTRAINT [FK_WellDirectionalDrilling_Well]
GO
ALTER TABLE [WellWiki].[WellHistory]  WITH CHECK ADD  CONSTRAINT [FK_WellHistory_Well] FOREIGN KEY([WellID])
REFERENCES [WellWiki].[Well] ([WellID])
GO
ALTER TABLE [WellWiki].[WellHistory] CHECK CONSTRAINT [FK_WellHistory_Well]
GO
ALTER TABLE [WellWiki].[WellPerforationTreatments]  WITH CHECK ADD  CONSTRAINT [FK_WellPerforationTreatments_Well] FOREIGN KEY([WellID])
REFERENCES [WellWiki].[Well] ([WellID])
GO
ALTER TABLE [WellWiki].[WellPerforationTreatments] CHECK CONSTRAINT [FK_WellPerforationTreatments_Well]
GO
ALTER TABLE [WellWiki].[WellProductionData]  WITH CHECK ADD  CONSTRAINT [FK_WellProductionData_Well] FOREIGN KEY([WellID])
REFERENCES [WellWiki].[Well] ([WellID])
GO
ALTER TABLE [WellWiki].[WellProductionData] CHECK CONSTRAINT [FK_WellProductionData_Well]
GO
USE [master]
GO
ALTER DATABASE [WellFacilityRepository] SET  READ_WRITE 
GO
