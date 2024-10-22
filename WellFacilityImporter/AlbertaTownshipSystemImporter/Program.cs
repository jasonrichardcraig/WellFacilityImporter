namespace AlbertaTownshipSystemImporter
{
    internal class Program
    {
        private static readonly string defaultConnectionString = "Data Source=(local);Initial Catalog=WellFacilityRepository;Integrated Security=True;Encrypt=False";
        // The data is available at https://www.altalis.com/ 
        private static readonly string defaultDataDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\WellFacilityImporter\\Data";

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Well Facility Importer!");
            Console.WriteLine("=======================================");
            Console.WriteLine();
            Console.WriteLine("This application allows you to import data from the Alberta Township System.");
            Console.WriteLine();
            Console.Write("Please enter a database connection string or press enter for the default:");
            string? connectionString = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = defaultConnectionString;
            }
            Console.WriteLine();
            Console.WriteLine($"Using connection string: {connectionString}");
            Console.WriteLine();
            Console.Write("Please enter the data directory or press enter for default:");
            string? baseDataDirectory = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(baseDataDirectory))
            {
                baseDataDirectory = defaultDataDirectory;
            }
            Console.WriteLine();
            Console.WriteLine($"Using base data directory: {baseDataDirectory}");
            Console.WriteLine();

            var importer = new AtsCoordinateImporter(connectionString);

            importer.ImportFromFile($"{baseDataDirectory}\\ATS_V4_1.SEQ");
        }
    }
}
