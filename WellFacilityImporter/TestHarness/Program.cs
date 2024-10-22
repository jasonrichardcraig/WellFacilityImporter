using System;

namespace TestHarness
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double testLatitude = 53.023333;
            double testLongitude = -114.918916;

            Console.WriteLine($"Testing conversion for Latitude: {testLatitude}, Longitude: {testLongitude}\n");

            var dlsCoordinate = LatLongToDlsConverter.ConvertLatLongToDls(testLatitude, testLongitude);

            Console.WriteLine($"\nCalculated DLS Coordinate: {dlsCoordinate.ToString()}");
            Console.WriteLine("Actual DLS Coordinate is 06-02-047-07W5");
            Console.ReadKey();
        }
    }
}
