using System.ComponentModel;

namespace EnerSync.Filters
{
    public class FilterCriteria : INotifyPropertyChanged
    {
        private string? _operator;
        private string? _fieldName;

        public bool IsValueReadOnly => Operator == "IsNull" || Operator == "IsNotNull" || Operator == "IsEmpty" || Operator == "IsNotEmpty";

        public string FieldName // Name of the property being filtered
        {
            get => _fieldName!;
            set
            {
                if (_fieldName != value)
                {
                    _fieldName = value;
                    OnPropertyChanged(nameof(FieldName));
                }
            }
        }

        public string Operator
        {
            get => _operator!;
            set
            {
                if (_operator != value)
                {
                    _operator = value;
                    OnPropertyChanged(nameof(Operator));
                    OnPropertyChanged(nameof(IsValueReadOnly)); // Notify that IsValueReadOnly might change

                    // Clear the Value if the operator is set to IsNull or IsNotNull
                    if (IsValueReadOnly)
                    {
                        Value = null;
                        OnPropertyChanged(nameof(Value));
                    }
                }
            }
        }
        public string? LogicalOperator { get; set; } // AND, OR, NOT
        public string? Value { get; set; } // Value to filter by, as a string for flexibility
        public Type? FieldType { get; set; } // Type of the field (int, double, etc.)

        public object GetTypedValue()
        {
            if (string.IsNullOrEmpty(Value))
                return null!;

            try
            {
                if (FieldType == typeof(int) && int.TryParse(Value, out int intValue))
                    return intValue;
                else if (FieldType == typeof(int?) && int.TryParse(Value, out intValue))
                    return (int?)intValue;
                else if (FieldType == typeof(double) && double.TryParse(Value, out double doubleValue))
                    return doubleValue;
                else if (FieldType == typeof(double?) && double.TryParse(Value, out doubleValue))
                    return (double?)doubleValue;
                else if (FieldType == typeof(decimal) && decimal.TryParse(Value, out decimal decimalValue))
                    return decimalValue;
                else if (FieldType == typeof(decimal?) && decimal.TryParse(Value, out decimalValue))
                    return (decimal?)decimalValue;
                else if (FieldType == typeof(DateTime) && DateTime.TryParse(Value, out DateTime dateTimeValue))
                    return dateTimeValue;
                else if (FieldType == typeof(DateTime?) && DateTime.TryParse(Value, out dateTimeValue))
                    return (DateTime?)dateTimeValue;
                else if (FieldType == typeof(DateOnly) && DateOnly.TryParse(Value, out DateOnly dateOnlyValue))
                    return dateOnlyValue;
                else if (FieldType == typeof(DateOnly?) && DateOnly.TryParse(Value, out dateOnlyValue))
                    return (DateOnly?)dateOnlyValue;
                else if (FieldType == typeof(float) && float.TryParse(Value, out float floatValue))
                    return floatValue;
                else if (FieldType == typeof(float?) && float.TryParse(Value, out floatValue))
                    return (float?)floatValue;
                else if (FieldType == typeof(bool) && bool.TryParse(Value, out bool boolValue))
                    return boolValue;
                else if (FieldType == typeof(bool?) && bool.TryParse(Value, out boolValue))
                    return (bool?)boolValue;
                else if (FieldType == typeof(string))
                    return Value; // Return the string directly
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to convert Value '{Value}' to type '{FieldType?.Name}'", ex);
            }

            // Return the original string if no conversion is possible
            return Value;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

