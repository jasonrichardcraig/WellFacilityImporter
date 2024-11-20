using CommunityToolkit.Mvvm.DependencyInjection;
using EnerSync.Data;
using EnerSync.Services;
using EnerSync.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        private void WellSearchResultsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var wellsFilterViewModel = Ioc.Default.GetRequiredService<WellsFilterViewModel>();

            if (WellSearchResultsDataGrid.SelectedItems.Count > 16384)
            {
                Ioc.Default.GetRequiredService<IDialogService>().ShowErrorDialog("You have selected more than 16,384 wells. Please select fewer wells to continue.", "Selection Limit Exceeded");
                WellSearchResultsDataGrid.SelectedItems.Clear();
            }

            wellsFilterViewModel.SelectedItems = WellSearchResultsDataGrid.SelectedItems;

        }

        private void FacilitySearchResultsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var facilitiesFilterViewModel = Ioc.Default.GetRequiredService<FacilitiesFilterViewModel>();

            if (FacilitySearchResultsDataGrid.SelectedItems.Count > 16384)
            {
                Ioc.Default.GetRequiredService<IDialogService>().ShowErrorDialog("You have selected more than 16,384 facilities. Please select fewer facilities to continue.", "Selection Limit Exceeded");
                FacilitySearchResultsDataGrid.SelectedItems.Clear();
            }

            facilitiesFilterViewModel.SelectedItems = FacilitySearchResultsDataGrid.SelectedItems;
        }

        private async void LookupWellAddressMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (WellSearchResultsDataGrid.SelectedItem is Models.Well well)
            {
                var address = await Ioc.Default.GetService<IDataService>()!.Context.GetAddressFromCoordinatesAsync(well.Latitude!.Value, well.Longitude!.Value);

                Dialogs.AddressDialog addressDialog = new Dialogs.AddressDialog();
                addressDialog.DataContext = address;
                addressDialog.Show();
            }
        }

        private async void LookupFacilityAddressMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (FacilitySearchResultsDataGrid.SelectedItem is Models.Facility facility)
            {
                var address = await Ioc.Default.GetService<IDataService>()!.Context.GetAddressFromCoordinatesAsync(facility.Latitude!.Value, facility.Longitude!.Value);

                Dialogs.AddressDialog addressDialog = new Dialogs.AddressDialog();
                addressDialog.DataContext = address;
                addressDialog.Show();
            }
        }

        private async void ImportWellWikiDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var importWellWikiDataDialog = new Dialogs.ImportWellWikiDataDialog();
            var wellWikiImporterViewModel = importWellWikiDataDialog.DataContext as WellWikiImporterViewModel;

            foreach (Models.Well well in WellSearchResultsDataGrid.SelectedItems)
            {
                if (well.WellWikiRowExists == false)
                {
                    wellWikiImporterViewModel!.Wells.Add(well.FormattedWellIdentifier!);
                }
            }

            importWellWikiDataDialog.Show();

            await wellWikiImporterViewModel!.ImportData();

            importWellWikiDataDialog.Close();

            // Get the scroll viewer from the DataGrid
            var scrollViewer = GetScrollViewer(WellSearchResultsDataGrid);
            if (scrollViewer != null)
            {
                // Store current vertical offset
                double verticalOffset = scrollViewer.VerticalOffset;

                await Ioc.Default.GetService<ViewModels.WellsFilterViewModel>()!.ApplyFilters();

                // Restore the vertical offset after the data is refreshed
                WellSearchResultsDataGrid.Dispatcher.Invoke(() =>
                {
                    scrollViewer.ScrollToVerticalOffset(verticalOffset);
                }, System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        // Helper method to get the ScrollViewer from DataGrid
        private ScrollViewer GetScrollViewer(DependencyObject depObj)
        {
            if (depObj is ScrollViewer) return (ScrollViewer)depObj;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null!;
        }
    }
}