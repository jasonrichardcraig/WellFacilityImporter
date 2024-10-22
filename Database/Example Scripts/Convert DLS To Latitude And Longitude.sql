USE [WellFacilityRepository]
GO

--06-02-047-07W5


-- The Calculated DlsCoordinate for 53.023333, -114.918916 was 10-34-046-07W5; actual DlsCoordinate is 06-02-047-07W5

DECLARE @lsd int = 6;
DECLARE @section int = 2;
DECLARE @township int = 47;
DECLARE @range int = 7;
DECLARE @meridian int = 5;
DECLARE @point geography;

SET @point = [Converters].[ConvertDlsToPoint] (@lsd, @section, @township,@range, @meridian);

PRINT @point.Lat;
PRINT @point.Long;
GO


