using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace NestedLoopJoinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeDataGridViews();
        }

        private void InitializeDataGridViews()
        {
            // Inicializa las tablas de ejemplo
            DataTable table1 = new DataTable();
            table1.Columns.Add("ID", typeof(int));
            table1.Columns.Add("Name", typeof(string));
            table1.Rows.Add(1, "Alice");
            table1.Rows.Add(2, "Bob");
            table1.Rows.Add(3, "Charlie");

            DataTable table2 = new DataTable();
            table2.Columns.Add("ID", typeof(int));
            table2.Columns.Add("Age", typeof(int));
            table2.Rows.Add(1, 25);
            table2.Rows.Add(2, 30);
            table2.Rows.Add(4, 35);

            dataGridView1.DataSource = table1;
            dataGridView2.DataSource = table2;
        }

        private void buttonJoin_Click(object sender, EventArgs e)
        {
            DataTable result = NestedLoopJoin(
                (DataTable)dataGridView1.DataSource,
                (DataTable)dataGridView2.DataSource,
                "ID",
                "ID"
            );

            dataGridViewResult.DataSource = result;
        }

        private DataTable NestedLoopJoin(DataTable leftTable, DataTable rightTable, string leftJoinColumn, string rightJoinColumn)
        {
            DataTable resultTable = new DataTable();

            // Crear columnas para la tabla de resultados
            foreach (DataColumn col in leftTable.Columns)
                resultTable.Columns.Add(col.ColumnName, col.DataType);
            foreach (DataColumn col in rightTable.Columns)
                if (col.ColumnName != rightJoinColumn)
                    resultTable.Columns.Add(col.ColumnName, col.DataType);

            // Realizar el Nested Loop Join
            foreach (DataRow leftRow in leftTable.Rows)
            {
                foreach (DataRow rightRow in rightTable.Rows)
                {
                    if (leftRow[leftJoinColumn].Equals(rightRow[rightJoinColumn]))
                    {
                        DataRow newRow = resultTable.NewRow();
                        foreach (DataColumn col in leftTable.Columns)
                            newRow[col.ColumnName] = leftRow[col.ColumnName];
                        foreach (DataColumn col in rightTable.Columns)
                            if (col.ColumnName != rightJoinColumn)
                                newRow[col.ColumnName] = rightRow[col.ColumnName];
                        resultTable.Rows.Add(newRow);
                    }
                }
            }

            return resultTable;
        }
    }
}