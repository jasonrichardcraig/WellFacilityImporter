using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellFacilityClrUtilities.Functions;

namespace TestHarness
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double testLatitude = 53.023333;
            double testLongitude = -114.918916;

            Console.WriteLine($"Testing conversion for Latitude: {testLatitude}, Longitude: {testLongitude}\n");

            DlsCoordinate dlsCoordinate = LatLongToDlsConverter.ConvertLatLongToDls(testLatitude, testLongitude);

            Console.WriteLine($"\nCalculated DLS Coordinate: {dlsCoordinate}");
            Console.WriteLine("Actual DLS Coordinate is 06-02-047-07W5");
            Console.ReadKey();
        }
    }
}
