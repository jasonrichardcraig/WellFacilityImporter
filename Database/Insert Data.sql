USE [WellFacilityRepository]
GO
INSERT [AlbertaTownshipSystem].[ElevationAccuracyLookup] ([ElevationAccuracy], [Description]) VALUES (N'1', N'< 1 metre')
INSERT [AlbertaTownshipSystem].[ElevationAccuracyLookup] ([ElevationAccuracy], [Description]) VALUES (N'2', N'< 5 metre')
INSERT [AlbertaTownshipSystem].[ElevationAccuracyLookup] ([ElevationAccuracy], [Description]) VALUES (N'3', N'<10 metre')
INSERT [AlbertaTownshipSystem].[ElevationAccuracyLookup] ([ElevationAccuracy], [Description]) VALUES (N'4', N'>10 metre')
INSERT [AlbertaTownshipSystem].[ElevationAccuracyLookup] ([ElevationAccuracy], [Description]) VALUES (N'Z', N'Unclassified, consult AEP')
GO
INSERT [AlbertaTownshipSystem].[ElevationMethodLookup] ([ElevationMethod], [Description]) VALUES (N'1', N'Spirit levels')
INSERT [AlbertaTownshipSystem].[ElevationMethodLookup] ([ElevationMethod], [Description]) VALUES (N'2', N'GPS')
INSERT [AlbertaTownshipSystem].[ElevationMethodLookup] ([ElevationMethod], [Description]) VALUES (N'3', N'ISS')
INSERT [AlbertaTownshipSystem].[ElevationMethodLookup] ([ElevationMethod], [Description]) VALUES (N'4', N'DEM')
INSERT [AlbertaTownshipSystem].[ElevationMethodLookup] ([ElevationMethod], [Description]) VALUES (N'5', N'Trigonometric levels')
INSERT [AlbertaTownshipSystem].[ElevationMethodLookup] ([ElevationMethod], [Description]) VALUES (N'6', N'Spot elevation from contour map')
INSERT [AlbertaTownshipSystem].[ElevationMethodLookup] ([ElevationMethod], [Description]) VALUES (N'7', N'Interpolated from contour map')
INSERT [AlbertaTownshipSystem].[ElevationMethodLookup] ([ElevationMethod], [Description]) VALUES (N'8', N'Other methods (stadia, barometric)')
INSERT [AlbertaTownshipSystem].[ElevationMethodLookup] ([ElevationMethod], [Description]) VALUES (N'Z', N'Consult SRD')
GO
INSERT [AlbertaTownshipSystem].[HorizontalClassificationLookup] ([HorizontalClassification], [Description]) VALUES (N'1', N'1st order EMR classification')
INSERT [AlbertaTownshipSystem].[HorizontalClassificationLookup] ([HorizontalClassification], [Description]) VALUES (N'2', N'2nd order EMR classification')
INSERT [AlbertaTownshipSystem].[HorizontalClassificationLookup] ([HorizontalClassification], [Description]) VALUES (N'3', N'3rd order EMR classification')
INSERT [AlbertaTownshipSystem].[HorizontalClassificationLookup] ([HorizontalClassification], [Description]) VALUES (N'4', N'4th order EMR classification')
INSERT [AlbertaTownshipSystem].[HorizontalClassificationLookup] ([HorizontalClassification], [Description]) VALUES (N'5', N'Non-monumented governing point')
INSERT [AlbertaTownshipSystem].[HorizontalClassificationLookup] ([HorizontalClassification], [Description]) VALUES (N'6', N'Cadastral station, coordinates confirmed by double solution')
INSERT [AlbertaTownshipSystem].[HorizontalClassificationLookup] ([HorizontalClassification], [Description]) VALUES (N'7', N'Cadastral station, coordinates calculated from plan dimensions')
INSERT [AlbertaTownshipSystem].[HorizontalClassificationLookup] ([HorizontalClassification], [Description]) VALUES (N'8', N'Cadastral station, single solution')
INSERT [AlbertaTownshipSystem].[HorizontalClassificationLookup] ([HorizontalClassification], [Description]) VALUES (N'9', N'Theoretical coordinates within Canada Lands')
GO
INSERT [AlbertaTownshipSystem].[HorizontalDatumLookup] ([HorizontalDatum], [Description]) VALUES (N'1', N'NAD 27')
INSERT [AlbertaTownshipSystem].[HorizontalDatumLookup] ([HorizontalDatum], [Description]) VALUES (N'2', N'NAD 83')
INSERT [AlbertaTownshipSystem].[HorizontalDatumLookup] ([HorizontalDatum], [Description]) VALUES (N'3', N'Reserved for future use')
INSERT [AlbertaTownshipSystem].[HorizontalDatumLookup] ([HorizontalDatum], [Description]) VALUES (N'4', N'Reserved for future use')
INSERT [AlbertaTownshipSystem].[HorizontalDatumLookup] ([HorizontalDatum], [Description]) VALUES (N'5', N'Reserved for future use')
INSERT [AlbertaTownshipSystem].[HorizontalDatumLookup] ([HorizontalDatum], [Description]) VALUES (N'6', N'Reserved for future use')
GO
INSERT [AlbertaTownshipSystem].[MeridianLookup] ([MeridianID], [MeridianValue]) VALUES (1, 4)
INSERT [AlbertaTownshipSystem].[MeridianLookup] ([MeridianID], [MeridianValue]) VALUES (2, 5)
INSERT [AlbertaTownshipSystem].[MeridianLookup] ([MeridianID], [MeridianValue]) VALUES (3, 6)
GO
INSERT [AlbertaTownshipSystem].[RoadAllowanceCodeLookup] ([RoadAllowanceCode], [Description]) VALUES (N'1', N'66 foot road allowance, N-S and E-W')
INSERT [AlbertaTownshipSystem].[RoadAllowanceCodeLookup] ([RoadAllowanceCode], [Description]) VALUES (N'2', N'99 foot road allowance, N-S and E-W')
INSERT [AlbertaTownshipSystem].[RoadAllowanceCodeLookup] ([RoadAllowanceCode], [Description]) VALUES (N'3', N'66 foot N-S; 99 foot E-W')
INSERT [AlbertaTownshipSystem].[RoadAllowanceCodeLookup] ([RoadAllowanceCode], [Description]) VALUES (N'4', N'99 foot N-S; 66 foot E-W')
INSERT [AlbertaTownshipSystem].[RoadAllowanceCodeLookup] ([RoadAllowanceCode], [Description]) VALUES (N'5', N'Undefined road allowance')
GO
INSERT [AlbertaTownshipSystem].[StationCodeLookup] ([StationCode], [Description]) VALUES (N'S', N'Surveyed station')
INSERT [AlbertaTownshipSystem].[StationCodeLookup] ([StationCode], [Description]) VALUES (N'U', N'Unsurveyed station')
GO
INSERT [AlbertaTownshipSystem].[StatusCodeLookup] ([StatusCode], [Description]) VALUES (N'0', N'Current')
INSERT [AlbertaTownshipSystem].[StatusCodeLookup] ([StatusCode], [Description]) VALUES (N'5', N'Superseded')
GO
INSERT [AlbertaTownshipSystem].[VerticalDatumLookup] ([VerticalDatum], [Description]) VALUES (N'1', N'NAVD''29')
INSERT [AlbertaTownshipSystem].[VerticalDatumLookup] ([VerticalDatum], [Description]) VALUES (N'2', N'NAVD''88')
GO
