USE [WellFacilityRepository]
GO

UPDATE [BusinessAssociate].[BusinessAssociate]
   SET [FormattedLegalName] = [Converters].[CamelCaseString]([BALegalName])
GO

UPDATE [FacilityInfrastructure].[Facility]
   SET [FormattedFacilityName] = [Converters].[CamelCaseString]([FacilityName])
GO

UPDATE [WellInfrastructure].[Well]
   SET [FormattedWellIdentifier] = [Converters].[FormatWellIdentifier]([WellIdentifier])
      ,[FormattedWellName] = [Converters].[CamelCaseString]([WellName])
      ,[FormattedFieldName] = [Converters].[CamelCaseString]([FieldName])
      ,[FormattedPoolDepositName] = [Converters].[CamelCaseString]([PoolDepositName])
	  ,[FormattedLicenseeName] = [Converters].[CamelCaseString]([LicenseeName])

GO

UPDATE [WellWiki].[Well]
   SET [AlternateWellID] = [Converters].[ConvertDlsToWellID]([Location])
      ,[FormattedWellName] = [Converters].[CamelCaseString]([WellName])
GO



