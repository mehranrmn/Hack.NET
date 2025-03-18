using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace GridPersistence
{
    public class ColumnProxy
    {
        public string UniqueName { get; set; }
        public int DisplayOrder { get; set; }
        public RadHyperlinkButton Header { get; set; }
        //public object Header { get; set; }
        //public string Header { get; set; }
        public GridViewLength Width { get; set; }
    }

    public class SortDescriptorProxy
    {
        public string ColumnUniqueName { get; set; }
        public ListSortDirection SortDirection { get; set; }
    }

    public class GroupDescriptorProxy
    {
        public string ColumnUniqueName { get; set; }
        public ListSortDirection? SortDirection { get; set; }
    }

    public class FilterDescriptorProxy
    {
        public Telerik.Windows.Data.FilterOperator Operator { get; set; }
        public object Value { get; set; }
        public bool IsCaseSensitive { get; set; }
    }

    public class FilterSetting
    {
        public string ColumnUniqueName { get; set; }

        private List<object> selectedDistinctValue;
        public List<object> SelectedDistinctValues
        {
            get
            {
                if (this.selectedDistinctValue == null)
                {
                    this.selectedDistinctValue = new List<object>();
                }

                return this.selectedDistinctValue;
            }
        }

        public FilterDescriptorProxy Filter1 { get; set; }
        public Telerik.Windows.Data.FilterCompositionLogicalOperator FieldFilterLogicalOperator { get; set; }
        public FilterDescriptorProxy Filter2 { get; set; }
    }
}
