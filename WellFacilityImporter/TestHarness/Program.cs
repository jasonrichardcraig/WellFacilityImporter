using System;

namespace TestHarness
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double testLatitude = 53.023333;
            double testLongitude = -114.918916;

            Console.WriteLine($"Testing Reverse Geocoding for Latitude: {testLatitude}, Longitude: {testLongitude}\n");

            var addressDetails = Geocoder.GetAddressFromCoordinates(testLatitude, testLongitude);

            Console.WriteLine(addressDetails.ToString());

            Console.ReadKey();
        }
    }
}
