USE [WellFacilityRepository]
GO

-- Longitude / Latitude from DLS 06-02-047-07W5 -> 53.023333, -114.918916

DECLARE @latitude float = 53.023333;
DECLARE @longitude float = -114.918916;
DECLARE @addressDetail [Geocoding].[AddressDetail];

SET @addressDetail = [Geocoding].[GetAddressFromCoordinates] (@latitude, @longitude);

PRINT 'Country: ' + @addressDetail.Country;
PRINT 'State: ' + @addressDetail.State;
PRINT 'County: ' + @addressDetail.County;
PRINT 'City: ' + @addressDetail.City;
PRINT 'Town/Borough: ' + @addressDetail.TownBorough;
PRINT 'Village/Suburb: ' + @addressDetail.VillageSuburb;
PRINT 'Neighbourhood: ' + @addressDetail.Neighbourhood;
PRINT 'Any Settlement: ' + @addressDetail.AnySettlement;
PRINT 'Major Streets: ' + @addressDetail.MajorStreets;
PRINT 'Major and Minor Streets: ' + @addressDetail.MajorMinorStreets;
PRINT 'Building: ' + @addressDetail.Building;


