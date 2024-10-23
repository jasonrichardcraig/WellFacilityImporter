-- Switch to master database
USE [master];
GO
-- Create the certificate from the assembly file
CREATE CERTIFICATE [NetHttpAssemblyCert]
FROM EXECUTABLE FILE = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.Http.dll';
GO
-- Create a login from the certificate
CREATE LOGIN [NetHttpAssemblyLogin] FROM CERTIFICATE [NetHttpAssemblyCert];
GO
-- Grant UNSAFE ASSEMBLY permission to the login
GRANT UNSAFE ASSEMBLY TO [NetHttpAssemblyLogin];
GO
-- Switch to your target database
USE [WellFacilityRepository];
GO
-- Create the assembly in the WellFacilityRepository database
CREATE ASSEMBLY [System.Net.Http]  
FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.Http.dll'
WITH PERMISSION_SET = UNSAFE;
GO
-- Switch to master database
USE [master];
GO
-- Create the asymmetric key from the key file
CREATE ASYMMETRIC KEY WellFacilityClrUtilitiesKey -- Create using: sn -p "../WellFacilityClrUtilities-Key.pfx" WellFacilityClrUtilities-Key.snk
AUTHORIZATION dbo
FROM FILE = 'C:\Users\jason\GitHub\WellFacilityImporter\WellFacilityImporter\WellFacilityClrUtilities\WellFacilityClrUtilities-Key.snk' -- Replace with path to your location
GO
-- Create a login from the asymmetric key
CREATE LOGIN [WellFacilityClrUtilitiesLogin] FROM ASYMMETRIC KEY [WellFacilityClrUtilitiesKey]
GO
-- Grant UNSAFE ASSEMBLY permission to the login
GRANT UNSAFE ASSEMBLY TO [WellFacilityClrUtilitiesLogin]
GO

