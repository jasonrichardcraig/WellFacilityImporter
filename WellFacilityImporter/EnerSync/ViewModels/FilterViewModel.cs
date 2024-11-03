using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnerSync.Filters;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

namespace EnerSync.ViewModels
{
    public class FilterViewModel : ObservableObject
    {

        private int _selectedRowCount = 0;

        public int SelectedRowCount
        {
            get => _selectedRowCount;
            set => SetProperty(ref _selectedRowCount, value);
        }

        public ObservableCollection<FilterCriteria> FilterCriteria { get; set; } = new ObservableCollection<FilterCriteria>();
        public List<string> AvailableFields { get; set; } = new List<string>(); // Can be populated by derived classes
        public List<string> AvailableOperators { get; private set; } = new List<string>();

        protected void UpdateAvailableOperators(Type fieldType)
        {
            // Reset AvailableOperators based on the field type
            AvailableOperators = SupportedOperators.ContainsKey(fieldType) ?
                                 SupportedOperators[fieldType] :
                                 new List<string> { "Equals", "NotEquals" }; // Default operators if type is unrecognized

            OnPropertyChanged(nameof(AvailableOperators)); // Notify UI of changes
        }

        public List<string> AvailableLogicalOperators { get; } = new List<string> { "AND", "OR", "NOT" };

        protected readonly Dictionary<Type, List<string>> SupportedOperators = new()
        {
            { typeof(string), new List<string> { "Equals", "NotEquals", "Contains", "DoesNotContain", "StartsWith", "EndsWith","IsEmpty", "IsNotEmpty", "IsNull", "IsNotNull" } },
            { typeof(int), new List<string> { "Equals", "NotEquals", "GreaterThan", "GreaterThanOrEqual", "LessThan", "LessThanOrEqual" } },
            { typeof(int?), new List<string> { "Equals", "NotEquals", "GreaterThan", "GreaterThanOrEqual", "LessThan", "LessThanOrEqual", "IsNull", "IsNotNull" } },
            { typeof(double), new List<string> { "Equals", "NotEquals", "GreaterThan", "GreaterThanOrEqual", "LessThan", "LessThanOrEqual" } },
            { typeof(double?), new List<string> { "Equals", "NotEquals", "GreaterThan", "GreaterThanOrEqual", "LessThan", "LessThanOrEqual", "IsNull", "IsNotNull" } },
            { typeof(decimal), new List<string> { "Equals", "NotEquals", "GreaterThan", "GreaterThanOrEqual", "LessThan", "LessThanOrEqual" } },
            { typeof(decimal?), new List<string> { "Equals", "NotEquals", "GreaterThan", "GreaterThanOrEqual", "LessThan", "LessThanOrEqual", "IsNull", "IsNotNull" } },
            { typeof(DateTime), new List<string> { "Equals", "NotEquals", "GreaterThan", "GreaterThanOrEqual", "LessThan", "LessThanOrEqual" } },
            { typeof(DateTime?), new List<string> { "Equals", "NotEquals", "GreaterThan", "GreaterThanOrEqual", "LessThan", "LessThanOrEqual", "IsNull", "IsNotNull" } },
            { typeof(DateOnly), new List<string> { "Equals", "NotEquals", "GreaterThan", "GreaterThanOrEqual", "LessThan", "LessThanOrEqual" } },
            { typeof(DateOnly?), new List<string> { "Equals", "NotEquals", "GreaterThan", "GreaterThanOrEqual", "LessThan", "LessThanOrEqual", "IsNull", "IsNotNull" } },
            { typeof(bool), new List<string> { "Equals", "NotEquals" } },
            { typeof(Guid), new List<string> { "Equals", "NotEquals" } },
            { typeof(Guid?), new List<string> { "Equals", "NotEquals", "IsNull", "IsNotNull" } },
            // Add more nullable types as needed
        };

        protected virtual void AddFilter()
        {

        }

        protected virtual void RemoveFilter(FilterCriteria criteria)
        {
            FilterCriteria.Remove(criteria);
        }

        protected void PopulateAvailableFields(Type entityType)
        {
            AvailableFields = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(prop => prop.Name)
                .OrderBy(name => name)
                .ToList();

            OnPropertyChanged(nameof(AvailableFields));
        }
    }
}
