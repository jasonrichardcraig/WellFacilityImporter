using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnerSync.Models.Filters;
using EnerSync.Models.WellInfrastructure;
using EnerSync.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnerSync.ViewModels
{
    public class MainViewModel : ObservableValidator
    {
        private readonly FilterService _filterService;
        public ObservableCollection<FilterCriteria> FilterCriteria { get; set; } = new ObservableCollection<FilterCriteria>();
        public ObservableCollection<Well> FilteredCollection { get; set; } = new ObservableCollection<Well>();
        public List<string> AvailableFields { get; private set; } = new List<string>();
        public List<string> AvailableOperators { get; } = new List<string> { "Equals", "Contains", "GreaterThan", "LessThan" };
        public List<string> AvailableLogicalOperators { get; } = new List<string> { "AND", "OR", "NOT" };

        public ICommand AddFilterCommand { get; }
        public ICommand RemoveFilterCommand { get; }
        public ICommand ApplyFiltersCommand { get; }

        public MainViewModel(FilterService filterService)
        {
            _filterService = filterService;
            AddFilterCommand = new RelayCommand(AddFilter);
            RemoveFilterCommand = new RelayCommand<FilterCriteria>(RemoveFilter!);
            ApplyFiltersCommand = new RelayCommand(async () => await ApplyFilters());
            PopulateAvailableFields(typeof(Well)); // Dynamically populate fields based on Well entity
        }

        private void AddFilter() => FilterCriteria.Add(new FilterCriteria());

        private void RemoveFilter(FilterCriteria criteria) => FilterCriteria.Remove(criteria);

        private void PopulateAvailableFields(Type entityType)
        {
            // Use reflection to get public properties of the entity type
            AvailableFields = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(prop => prop.Name)
                .ToList();

            // Notify UI of the updated AvailableFields list
            OnPropertyChanged(nameof(AvailableFields));
        }

        private async Task ApplyFilters()
        {
            var filteredData = await _filterService.GetFilteredDataAsync<Well>(FilterCriteria.ToList());
            FilteredCollection.Clear();
            foreach (var item in filteredData)
            {
                FilteredCollection.Add(item);
            }
        }
    }
}
