using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EnerSync.Filters;
using EnerSync.Models;
using EnerSync.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EnerSync.ViewModels
{
    public partial class FacilitiesFilterViewModel : FilterViewModel
    {

        private readonly FilterService _filterService;
        private readonly MainViewModel _mainViewModel;

        public ICommand AddFilterCommand { get; }
        public ICommand RemoveFilterCommand { get; }
        public ICommand ApplyFiltersCommand { get; }

        public ObservableCollection<Facility> FilteredFacilitiesSearchResults { get; set; } = [];

        public FacilitiesFilterViewModel(MainViewModel mainviewModel, FilterService filterService)
        {
            _mainViewModel = mainviewModel;
            _filterService = filterService;
            AddFilterCommand = new RelayCommand(AddFilter);
            RemoveFilterCommand = new RelayCommand<FilterCriteria>(RemoveFilter!);
            ApplyFiltersCommand = new RelayCommand(async () => await ApplyFilters());
            PopulateAvailableFields(typeof(Facility)); // Populate fields based on Well entity

            FilterCriteria.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (FilterCriteria criteria in e.NewItems)
                    {
                        criteria.PropertyChanged += (sender, args) =>
                        {
                            if (args.PropertyName == nameof(criteria.FieldName))
                            {
                                // Update operators based on the new field's data type
                                var propertyInfo = typeof(Facility).GetProperty(criteria.FieldName!);
                                if (propertyInfo != null)
                                    UpdateAvailableOperators(propertyInfo.PropertyType);
                            }
                        };
                    }
                }
            };
        }

        protected override void AddFilter()
        {
            var defaultField = AvailableFields.FirstOrDefault();
            if (defaultField != null)
            {
                var propertyInfo = typeof(Facility).GetProperty(defaultField); // Adjust to your model type
                if (propertyInfo != null)
                {
                    UpdateAvailableOperators(propertyInfo.PropertyType);
                    FilterCriteria.Add(new FilterCriteria { FieldName = defaultField });
                }
            }
            else
            {
                throw new InvalidOperationException("No available fields to set as default for the new filter.");
            }
        }

        private async Task ApplyFilters()
        {
            _mainViewModel.IsBusy = true;

            try
            {

                foreach (var filter in FilterCriteria)
                {
                    var propertyInfo = typeof(Facility).GetProperty(filter.FieldName!);
                    if (propertyInfo != null)
                    {
                        filter.FieldType = propertyInfo.PropertyType;
                    }
                }
                FilteredFacilitiesSearchResults.Clear();

                await foreach (var item in _filterService.GetFilteredDataAsync<Facility>([.. FilterCriteria]))
                {
                    FilteredFacilitiesSearchResults.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Ioc.Default.GetService<IDialogService>()!.ShowErrorDialog(ex.Message, "Error");
            }

            _mainViewModel.IsBusy = false;
        }
    }
}
