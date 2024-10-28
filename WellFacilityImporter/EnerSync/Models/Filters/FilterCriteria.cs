using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnerSync.Models.Filters
{
    public class FilterCriteria
    {
        public string? FieldName { get; set; }
        public string? Operator { get; set; } // e.g., Equals, Contains, GreaterThan
        public object? Value { get; set; }
        public string? LogicalOperator { get; set; } // e.g., AND, OR, NOT
    }
}
