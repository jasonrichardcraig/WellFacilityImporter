using System.Windows;

namespace EnerSync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new EnerSync.Data.WellFacilityRepositoryDbContext())
            {
                var well = context.WellsInfrastructure.First();
            }
        }
    }
}