USE [WellFacilityRepository]
GO

-- Longitude / Latitude from DLS 06-02-047-07W5 -> 53.023333, -114.918916

DECLARE @latitude float = 53.023333;
DECLARE @longitude float = -114.918916;

DECLARE @dlsCoordinate [Converters].[DLS];

SET @dlsCoordinate = [Converters].[ConvertCoordinatesToDls] (@latitude, @longitude);

PRINT @dlsCoordinate.ToString();
GO


