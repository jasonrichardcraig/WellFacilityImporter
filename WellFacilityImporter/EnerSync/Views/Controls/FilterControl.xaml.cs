using EnerSync.ViewModels;
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

            if (DataContext is WellsFilterViewModel)
            {
                var wellsFilterViewModel = DataContext as WellsFilterViewModel;

                rowCount = wellsFilterViewModel!.SelectedRowCount;
            }
            else if (DataContext is WellsFilterViewModel)
            {
                var facilitiesFilterViewModel = DataContext as FacilitiesFilterViewModel;
                rowCount = facilitiesFilterViewModel!.SelectedRowCount;
            }

            var textBlock = sender as TextBlock;

            if (e.LeftButton == MouseButtonState.Pressed && textBlock != null && rowCount > 0)
            {

                // Serialize selected items to JSON for cross-application compatibility
                string jsonData = "poop"; // JsonConvert.SerializeObject(selectedItems);

                // Create a DataObject and add JSON data
                var dataObject = new DataObject();
                dataObject.SetData(DataFormats.Text, jsonData);

                // Start drag-and-drop operation
                DragDrop.DoDragDrop(RowCountTextBlock, dataObject, DragDropEffects.Copy);

            }
        }
    }
}
