using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstaApiInstance
{
    public static class SetView<T>
    {
        public static void DgvAddRows(ref DataGridView dataGridView, List<T> list, List<string> newColumnName, params int[] columnCount)
        {
            int i = 0;
            foreach (var columnName in newColumnName)
            {
                dataGridView.Columns.Add("col"+i, columnName);
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            
             foreach (var data in list)
            {
                dataGridView.Rows.Add(data);
            }

        }
    }
}
