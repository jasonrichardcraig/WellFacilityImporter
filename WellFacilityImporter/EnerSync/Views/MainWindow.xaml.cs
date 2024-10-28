using EnerSync.Data;
using System.Windows;

namespace EnerSync.Views
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
            using (var context = new EnerSync.Data.EnerSyncContext())
            {
                var fac = context.Facilities.First();

                var facFmtName = fac.FormattedFacilityName;

                var well = context.WellsInfrastructure.First();

                var welFormatName = well.FormattedWellIdentifier;

                var well1 = context.WellsWiki.First();

                var welFormatName1 = well1.AlternateWellId;

                //var list = context.Facilities.Select(f=> f.ExperimentalConfidential, foo = WellFacilityRepositoryDbContext.ConvertDlsToWellID("100/01/")).ToList();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}