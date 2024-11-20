using CommunityToolkit.Mvvm.DependencyInjection;
using EnerSync.Data;
using EnerSync.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace EnerSync.Views.Controls
{
    /// <summary>
    /// Interaction logic for FilterControl.xaml
    /// </summary>
    public partial class FilterControl : UserControl
    {

        public FilterControl()
        {
            InitializeComponent();
        }

        private void RowCountTextBlock_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var rowCount = 0;
            {
                if (DataContext is WellsFilterViewModel wellsFilterViewModel)
                {
                    rowCount = wellsFilterViewModel.SelectedRowCount;
                }
                else if (DataContext is FacilitiesFilterViewModel facilitiesFilterViewModel)
                {
                    rowCount = facilitiesFilterViewModel.SelectedRowCount;
                }
            }

            var textBlock = sender as TextBlock;

            if (e.LeftButton == MouseButtonState.Pressed && textBlock != null && rowCount > 0)
            {
                var serializedData = string.Empty;
                var data = new Tuple<List<EnerSync.Models.Well>, List<EnerSync.Models.WellWiki.Well>, List<EnerSync.Models.Facility>>([], [], []);

                if (DataContext is WellsFilterViewModel wellsFilterViewModel)
                {
                    var enerSyncContext = Ioc.Default.GetService<EnerSyncContext>();

                    foreach (Models.Well well in wellsFilterViewModel.SelectedItems)
                    {
                        data.Item1.Add(well);
                    }

                    foreach (Models.WellWiki.Well wellWiki in enerSyncContext!.WellsWiki.Where(w => data.Item1.Select(ww => ww.WellIdentifier).Contains(w.AlternateWellId ?? string.Empty)).Include("WellHistories").Include("WellDirectionalDrillings").Include("WellPerforationTreatments").Include("WellProductionData"))
                    {
                        data.Item2.Add(wellWiki);
                    }

                }
                else if (DataContext is FacilitiesFilterViewModel facilitiesFilterViewModel)
                {
                    foreach (Models.Facility facility in facilitiesFilterViewModel.SelectedItems)
                    {
                        data.Item3.Add(facility);
                    }
                }

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                    WriteIndented = true // Optional, for better readability
                };

                // Serialize selected items to JSON for cross-application compatibility
                string jsonData = JsonSerializer.Serialize(data, options);

                // Create a DataObject and add JSON data
                var dataObject = new DataObject();
                dataObject.SetData(DataFormats.Text, jsonData);

                // Start drag-and-drop operation
                DragDrop.DoDragDrop(textBlock, dataObject, DragDropEffects.Copy);
            }
        }
    }
}
