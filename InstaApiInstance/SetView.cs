using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstaApi.Entities.Concrate;

namespace InstaApiInstance
{
    public static class SetView
    {
        public static void DgvAddRowsAccount<T>(ref DataGridView dataGridView, List<T> list, List<string> newColumnName, params int[] columnCount)
        {
            DgvDeleteRows(ref dataGridView);
       
            int i = 0;
            if (dataGridView.ColumnCount == 0)
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

        public static void DgvAddRowsMessageBox(ref DataGridView messageGridView, ref DataGridView pendingGridView, List<EMessageBox> messageBoxs, List<string> newColumnName)
        {
            DgvDeleteRows(ref messageGridView);
            DgvDeleteRows(ref pendingGridView);
            int i = 0;

            if (messageGridView.ColumnCount ==0)
                foreach (var columnName in newColumnName)
                {
                    messageGridView.Columns.Add("col" + i, columnName);
                    messageGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    pendingGridView.Columns.Add("col" + i, "Pending" + columnName);
                    pendingGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }


            foreach (var message in messageBoxs)
            {
                if (!message.IsPeddingUser)
                    messageGridView.Rows.Add(message.UserName);
                else
                    pendingGridView.Rows.Add(message.UserName);
            }
        }

        public static void DgvDeleteRows(ref DataGridView dataGridView)
        {
            int rowsCount = dataGridView.RowCount;
            int j = rowsCount;
            for (int i = 0; i < rowsCount; i++)
            {
                dataGridView.Rows.RemoveAt(j - 1);
                j--;
            }
        }

        public static void DgvMessageGetMessage(ref DataGridView getMessageDataGridView, List<EMessage> messages, string newColumnName )
        {
            DgvDeleteRows(ref getMessageDataGridView);

            if (getMessageDataGridView.ColumnCount == 0)
            {
                getMessageDataGridView.Columns.Add("col1", newColumnName);
                getMessageDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }


            foreach (var message in messages)
            {
                if (message.IsSendedMessage)
                {

                    var i = getMessageDataGridView.Rows.Add(message.Message);
                    getMessageDataGridView.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    getMessageDataGridView.Rows[i].DefaultCellStyle.BackColor = Color.GreenYellow;
                }
                else
                {
                    getMessageDataGridView.Rows.Add(message.Message);
                }
            }
            getMessageDataGridView.FirstDisplayedScrollingRowIndex = getMessageDataGridView.RowCount - 1;
        }
    }
}
