USE [WellFacilityRepository]
GO

-- DlsCoordinate is 06-02-047-07W5 -> 53.023333, -114.918916

DECLARE @lsd int = 6;
DECLARE @section int = 2;
DECLARE @township int = 47;
DECLARE @range int = 7;
DECLARE @meridian int = 5;
DECLARE @point geography;

SET @point = [Converters].[ConvertDlsToLatLong] (@lsd, @section, @township,@range, @meridian);

PRINT 'Latitude: ' +  CAST(@point.Lat as varchar(64));
PRINT 'Longitude: ' +  CAST(@point.Long as varchar(64));

GO


