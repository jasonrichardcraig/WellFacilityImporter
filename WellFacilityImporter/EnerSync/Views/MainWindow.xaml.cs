using CommunityToolkit.Mvvm.DependencyInjection;
using EnerSync.Data;
using EnerSync.Services;
using EnerSync.ViewModels;
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void WellSearchResultsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var wellsFilterViewModel = Ioc.Default.GetRequiredService<WellsFilterViewModel>();

            if (WellSearchResultsDataGrid.SelectedItems.Count > 16384)
            {
                Ioc.Default.GetRequiredService<IDialogService>().ShowErrorDialog("You have selected more than 16,384 wells. Please select fewer wells to continue.", "Selection Limit Exceeded");
                WellSearchResultsDataGrid.SelectedItems.Clear();
            }

            wellsFilterViewModel.SelectedRowCount = WellSearchResultsDataGrid.SelectedItems.Count;

        }

        private void FacilitySearchResultsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var facilitiesFilterViewModel = Ioc.Default.GetRequiredService<FacilitiesFilterViewModel>();

            if (WellSearchResultsDataGrid.SelectedItems.Count > 16384)
            {
                Ioc.Default.GetRequiredService<IDialogService>().ShowErrorDialog("You have selected more than 16,384 facilities. Please select fewer facilities to continue.", "Selection Limit Exceeded");
                WellSearchResultsDataGrid.SelectedItems.Clear();
            }

            facilitiesFilterViewModel.SelectedRowCount = WellSearchResultsDataGrid.SelectedItems.Count;
        }

        private async void LookupWellAddressMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(WellSearchResultsDataGrid.SelectedItem is Models.Well well)
            {
                var address = await Ioc.Default.GetService<IDataService>()!.Context.GetAddressFromCoordinatesAsync(well.Latitude!.Value, well.Longitude!.Value);

                Dialogs.AddressDialog addressDialog = new Dialogs.AddressDialog();
                addressDialog.DataContext = address;
                addressDialog.Show();
            }
        }
    }
}