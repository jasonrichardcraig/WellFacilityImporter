namespace WellFacilityImporter
{
    internal class Program
    {
        private static readonly string defaultConnectionString = "Data Source=(local);Initial Catalog=WellFacilityRepository;Integrated Security=True;Encrypt=False";
        private static readonly string defaultBaseDataDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\WellFacilityImporter\\Data";
        static void Main()
        {

            Console.WriteLine("Welcome to the Well Facility Importer!");
            Console.WriteLine("=======================================");
            Console.WriteLine();
            Console.WriteLine("This application allows you to import data from various sources into the Well Facility database.");
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
            Console.Write("Please enter the base data directory or press enter for default:");
            string? baseDataDirectory = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(baseDataDirectory))
            {
                baseDataDirectory = defaultBaseDataDirectory;
            }
            Console.WriteLine();
            Console.WriteLine($"Using base data directory: {baseDataDirectory}");
            Console.WriteLine();

            while (true)
            {
                // Display menu options
                Console.WriteLine("1: Import Business Associate Data");
                Console.WriteLine("2: Import Well Infrastructure Data");
                Console.WriteLine("3: Import Well Licence Data");
                Console.WriteLine("4: Import Facility Infrastructure Data");
                Console.WriteLine("5: Import Facility Operator History Data");
                Console.WriteLine("6: Import Well to Facility Link Data");
                Console.WriteLine("7: Import Facility Licence Data");
                Console.WriteLine("8: Import Well Wiki Data");
                Console.WriteLine("9: Update Computed Fields");
                Console.WriteLine("10: Exit");
                Console.WriteLine();
                Console.Write("Select an operation:");
                // Get user input
                string? choice = Console.ReadLine();
                Console.WriteLine();

                // Process based on user input
                switch (choice)
                {
                    case "1":
                        ImportBusinessAssociateData(connectionString, baseDataDirectory);
                        break;
                    case "2":
                        ImportWellInfrastructureData(connectionString, baseDataDirectory);
                        break;
                    case "3":
                        ImportWellLicenceData(connectionString, baseDataDirectory);
                        break;
                    case "4":
                        ImportFacilityInfrastructureData(connectionString, baseDataDirectory);
                        break;
                    case "5":
                        ImportFacilityOperatorHistoryData(connectionString, baseDataDirectory);
                        break;
                    case "6":
                        ImportWellToFacilityLinkData(connectionString, baseDataDirectory);
                        break;
                    case "7":
                        ImportFacilityLicenceData(connectionString, baseDataDirectory);
                        break;
                    case "8":
                        ImportWellWikiData(connectionString);
                        break;
                    case "9":
                        ComputedFieldsUpdater.UpdateData(connectionString);
                        break;
                    case "10":
                        Console.WriteLine("Exiting application...");
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Please choose a number from 1 to 8.");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static void ImportBusinessAssociateData(string connectionString, string baseDataDirectory)
        {
            var defaultFileName = "Business Associate-AB.xml";
            Console.Write("Please enter file name or use default:");
            string? fileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = defaultFileName;
            }
            Console.WriteLine();
            Console.WriteLine($"Using file name: {fileName}");
            Console.WriteLine();
            Console.WriteLine("Importing Business Associate Data...");
            Console.WriteLine();
            BusinessAssociateDataImporter.ImportData(connectionString, $"{baseDataDirectory}\\{fileName}");
        }

        private static void ImportWellInfrastructureData(string connectionString, string baseDataDirectory)
        {
            var defaultFileName = "Well Infrastructure-AB.xml";
            Console.Write("Please enter file name or use default:");
            string? fileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = defaultFileName;
            }
            Console.WriteLine();
            Console.WriteLine($"Using file name: {fileName}");
            Console.WriteLine();
            Console.WriteLine("Importing Well Infrastructure Data...");
            Console.WriteLine();
            WellInfrastructureDataImporter.ImportData(connectionString, $"{baseDataDirectory}\\{fileName}");
        }

        private static void ImportWellLicenceData(string connectionString, string baseDataDirectory)
        {
            var defaultFileName = "Well Licence-AB.xml";
            Console.Write("Please enter file name or use default:");
            string? fileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = defaultFileName;
            }
            Console.WriteLine();
            Console.WriteLine($"Using file name: {fileName}");
            Console.WriteLine();
            Console.WriteLine("Importing Well Licence Data...");
            Console.WriteLine();
            WellLicenceDataImporter.ImportData(connectionString, $"{baseDataDirectory}\\{fileName}");
        }

        private static void ImportFacilityInfrastructureData(string connectionString, string baseDataDirectory)
        {
            var defaultFileName = "Facility Infrastructure-AB.xml";
            Console.Write("Please enter file name or use default:");
            string? fileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = defaultFileName;
            }
            Console.WriteLine();
            Console.WriteLine($"Using file name: {fileName}");
            Console.WriteLine();
            Console.WriteLine("Importing Facility Infrastructure Data...");
            Console.WriteLine();
            FacilityInfrastructureDataImporter.ImportData(connectionString, $"{baseDataDirectory}\\{fileName}");
        }

        private static void ImportFacilityOperatorHistoryData(string connectionString, string baseDataDirectory)
        {
            var defaultFileName = "Facility Operator History-AB.xml";
            Console.Write("Please enter file name or use default:");
            string? fileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = defaultFileName;
            }
            Console.WriteLine();
            Console.WriteLine($"Using file name: {fileName}");
            Console.WriteLine();
            Console.WriteLine("Importing Facility Operator History Data...");
            Console.WriteLine();
            FacilityOperatorHistoryDataImporter.ImportData(connectionString, $"{baseDataDirectory}\\{fileName}");
        }

        private static void ImportWellToFacilityLinkData(string connectionString, string baseDataDirectory)
        {
            var defaultFileName = "Well to Facility Link-AB.xml";
            Console.Write("Please enter file name or use default:");
            string? fileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = defaultFileName;
            }
            Console.WriteLine();
            Console.WriteLine($"Using file name: {fileName}");
            Console.WriteLine();
            Console.WriteLine("Importing Well to Facility Link Data...");
            Console.WriteLine();
            WellFacilityLinkDataImporter.ImportData(connectionString, $"{baseDataDirectory}\\{fileName}");
        }

        private static void ImportFacilityLicenceData(string connectionString, string baseDataDirectory)
        {
            var defaultFileName = "Facility Licence-AB.xml";
            Console.Write("Please enter file name or use default:");
            string? fileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = defaultFileName;
            }
            Console.WriteLine();
            Console.WriteLine($"Using file name: {fileName}");
            Console.WriteLine();
            Console.WriteLine("Importing Facility Licence Data...");
            Console.WriteLine();
            FacilityLicenceDataImporter.ImportData(connectionString, $"{baseDataDirectory}\\{fileName}");
        }

        private static void ImportWellWikiData(string connectionString)
        {
            var defaultCompanyName = "Logic_Energy_Ltd.";
            Console.Write("Please enter a company name or use default:");
            string? fileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = defaultCompanyName;
            }
            Console.WriteLine();
            Console.WriteLine($"Using file name: {fileName}");
            Console.WriteLine();
            Console.WriteLine("Importing Well Wiki Data...");
            Console.WriteLine();
            WellWikiDataImporter.ImportData(defaultCompanyName, connectionString).GetAwaiter().GetResult();
        }
    }
}

