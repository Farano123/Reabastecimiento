using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Diagnostics;
using OfficeOpenXml;

namespace Analisis_Reabastecimiento
{
    public partial class Form2 : Form
    {
        string pathToPrint = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBoxFolios.Hide();
            labelFolios.Hide();
            dataGridView1From2.ColumnHeadersHeight = 25;
            dataGridView1From2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            mostrarSucursales();

            DataGridViewColumn column = dataGridView1From2.Columns[1];
            column.Width = 400;
        }

        private void fechaFinalLabel_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1Form2_ValueChanged(object sender, EventArgs e)
        {
            string fecha = dateTimePicker1Form2.Value.ToShortDateString();
            var date = Convert.ToDateTime(fecha);
            //obtenerFolios(date.ToString("dd/MM/yyyy"), comboSucursal.Text);
            //comboBoxFolios.Show();
            //labelFolios.Show();
            comboBoxFolios.SelectedIndex = -1;
            comboBoxFolios.Text = "";
        }

        private void obtenerFolios(string fecha, string almacen)
        {
            comboBoxFolios.Items.Clear();
            SqlConnection conn = new SqlConnection("Data Source = 192.168.102.10; Initial Catalog = SistemasTI; User ID = sa; Password = SAP123x");
            conn.Open();

            SqlCommand command = new SqlCommand("Select folio from  dbo.SeguimientoArticulos where fecha=@fecha AND almacen=@alma", conn);
            command.Parameters.AddWithValue("@fecha", fecha);
            command.Parameters.AddWithValue("@alma", almacen);
            // int result = command.ExecuteNonQuery();

            ArrayList listaFolios = new ArrayList();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (!listaFolios.Contains(reader["folio"].ToString()))
                    {
                        listaFolios.Add(reader["folio"].ToString());
                    }

                }
            }

            int numberOfFolios = listaFolios.Count;

            for (int i = 0; i < numberOfFolios; i++)
            {
                comboBoxFolios.Items.Add(listaFolios[i]);
            }
        }

        private void mostrarArticulos(string fecha, string folio)
        {
            try
            {
                dataGridView1From2.Rows.Clear();
                dataGridView1From2.Refresh();
                string[] rows = new string[15];
                

                ////////////////////////////////////////////////////////////////////////////
                SqlConnection conn = new SqlConnection("Data Source = 192.168.102.10; Initial Catalog = SistemasTI; User ID = sa; Password = SAP123x");
                conn.Open();

                SqlCommand command = new SqlCommand("Select * from  dbo.SeguimientoArticulos where fecha=@fecha and folio=@folio", conn);
                command.Parameters.AddWithValue("@fecha", fecha);
                command.Parameters.AddWithValue("@folio", folio);

                 
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rows = new string[] { reader["itemCode"].ToString(), reader["descripcion"].ToString(), reader["unidad_medida"].ToString(),
                        reader["almacen"].ToString(), reader["cant_prom_mes"].ToString(),reader["prvlg"].ToString(),reader["cant_Inv"].ToString(),
                        secondData(reader["itemCode"].ToString(),  reader["almacen"].ToString()),
                        reader["stockCedis"].ToString(), reader["fecha"].ToString(), reader["completo"].ToString(), reader["folio"].ToString()};
                        dataGridView1From2.Rows.Add(rows);
                    }
                }

                foreach (DataGridViewRow r in this.dataGridView1From2.Rows)
                {
                    if (this.dataGridView1From2.Rows[r.Index].Cells[10].Value.ToString() == "si")
                    {
                        DataGridViewCell cell = dataGridView1From2.Rows[r.Index].Cells[12];
                        DataGridViewCheckBoxCell chkCell = cell as DataGridViewCheckBoxCell;
                        chkCell.Value = false;
                        chkCell.FlatStyle = FlatStyle.Flat;
                        chkCell.Style.ForeColor = Color.DarkGray;
                        cell.ReadOnly = true;
                    }
                }
            }
            catch
            {
                MessageBox.Show("INFORMACION NO ENCONTRADA");
            }
        }

        private void mostrarSucursales()
        {
            string[] sucursales = { "1011", "1021", "1031", "1041", "1051", "2011" };
            comboSucursal.Items.AddRange(sucursales);
        }


        private void comboBoxFolios_SelectedIndexChanged(object sender, EventArgs e)
        {

            string fecha = dateTimePicker1Form2.Value.ToShortDateString();
            string folio = comboBoxFolios.Text;
            var date = Convert.ToDateTime(fecha);
            mostrarArticulos(date.ToString("dd/MM/yyyy"), folio);
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            ArrayList rows_with_checked_column = new ArrayList();
            foreach (DataGridViewRow r in this.dataGridView1From2.Rows)
            {             
               if (Convert.ToBoolean(this.dataGridView1From2.Rows[r.Index].Cells[12].Value) == true)
               {
                    updateDataBase(this.dataGridView1From2.Rows[r.Index].Cells[11].Value.ToString(), this.dataGridView1From2.Rows[r.Index].Cells[9].Value.ToString(), this.dataGridView1From2.Rows[r.Index].Cells[0].Value.ToString());
               }

            }

            MessageBox.Show("Informacion Guardada");

            int count = rows_with_checked_column.Count;
        }

        private void updateDataBase(string folio, string fecha, string itemCode)
        {
            try
            {
                string[] rows = new string[15];

                SqlConnection conn = new SqlConnection("Data Source = 192.168.102.10; Initial Catalog = SistemasTI; User ID = sa; Password = SAP123x");
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE dbo.SeguimientoArticulos SET completo=@surtido where itemCode=@item and fecha=@fecha and folio=@folio", conn);
                command.Parameters.AddWithValue("@surtido", "si");
                command.Parameters.AddWithValue("@item", itemCode);
                command.Parameters.AddWithValue("@fecha", fecha);
                command.Parameters.AddWithValue("@folio", folio);

                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("INFORMACION NO ENCONTRADA");
            }
        }

        private string secondData(string code, string almacen)
        {
            //SqlConnection connection = new SqlConnection("Data Source = 192.168.102.13; Initial Catalog = SBO_ConstruramaBlanquita; User ID = sa; Password = SAP123x");
            //connection.Open();

            //SqlCommand cmd = new SqlCommand("Select OnHand from OITW where itemcode='20141027001' and Whscode='1011'", connection);

            SqlConnection conn = new SqlConnection("Data Source = 192.168.102.13; Initial Catalog = SBO_ConstruramaBlanquita; User ID = sa; Password = SAP123x");
            conn.Open();

            SqlCommand command = new SqlCommand("Select OnHand from OITW where itemcode=@item and Whscode=@alma", conn);
            command.Parameters.AddWithValue("@item", code);
            command.Parameters.AddWithValue("@alma", almacen);
            string output = "";

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    output = reader["OnHand"].ToString();
                } 
            }

            return output;
        }

        private void comboSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fecha = dateTimePicker1Form2.Value.ToShortDateString();
            var date = Convert.ToDateTime(fecha);
            obtenerFolios(date.ToString("dd/MM/yyyy"), comboSucursal.Text);
            labelFolios.Show();
            comboBoxFolios.Show();
        }

        private void bottonExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string nameOfFile = "Seguimiento-" + comboBoxFolios.Text + ".xlsx";

                // Obtener la ruta del escritorio del usuario actual
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fullFilePath = Path.Combine(desktopPath, nameOfFile);

                // Verificar si la ruta es válida (que no sea demasiado larga o tenga caracteres inválidos)
                if (fullFilePath.Length > 218)
                {
                    MessageBox.Show("La ruta es demasiado larga. Intenta guardar en una carpeta con un nombre más corto.");
                    return;
                }

                FileInfo excelFile = new FileInfo(fullFilePath);

                // Verificar si el archivo ya existe y no puede ser sobrescrito
                if (excelFile.Exists)
                {
                    MessageBox.Show("El archivo ya existe y no puede ser sobrescrito.");
                    return;
                }

                using (ExcelPackage excel = new ExcelPackage())
                {
                    excel.Workbook.Worksheets.Add("Worksheet1");

                    var headerRow = new List<string[]>()
            {
                new string[] { "ITEM CODE", "DESCRIPCION", "UNIDAD MEDIDA", "ALMACEN", "CANT_PROM_MENSUAL", "PRVLG", "CANT_INV", "STOCK ACTUAL", "STOCK_CEDIS", "FECHA", "SURTIDO", "FOLIO", "FECHA: " + dateTimePicker1Form2.Value.ToString() }
            };

                    string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                    var worksheet = excel.Workbook.Worksheets["Worksheet1"];

                    // Cargar los datos del encabezado
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<string[]>();

                    foreach (DataGridViewRow r in this.dataGridView1From2.Rows)
                    {
                        cellData.Add(new string[]
                        {
                    r.Cells[0]?.Value?.ToString() ?? "",
                    r.Cells[1]?.Value?.ToString() ?? "",
                    r.Cells[2]?.Value?.ToString() ?? "",
                    r.Cells[3]?.Value?.ToString() ?? "",
                    r.Cells[4]?.Value?.ToString() ?? "",
                    r.Cells[5]?.Value?.ToString() ?? "",
                    r.Cells[6]?.Value?.ToString() ?? "",
                    r.Cells[7]?.Value?.ToString() ?? "",
                    r.Cells[8]?.Value?.ToString() ?? "",
                    r.Cells[9]?.Value?.ToString() ?? "",
                    r.Cells[10]?.Value?.ToString() ?? "",
                    r.Cells[11]?.Value?.ToString() ?? ""
                        });
                    }

                    worksheet.Cells[2, 1].LoadFromArrays(cellData);
                    worksheet.Cells.AutoFitColumns();

                    // Guardar el archivo en el escritorio
                    excel.SaveAs(excelFile);
                }

                // Mensaje de confirmación con botones "Abrir" y "Cerrar"
                DialogResult result = MessageBox.Show("Archivo descargado correctamente. ¿Deseas abrirlo?", "Éxito", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                // Si el usuario elige "Sí", abrir el archivo
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Process.Start(fullFilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"No se pudo abrir el archivo: {ex.Message}");
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("No tienes permisos para guardar el archivo en esta ubicación. Intenta guardarlo en otro lugar o verificar los permisos de la carpeta.");
            }
            catch (IOException ex)
            {
                MessageBox.Show("Ocurrió un error al intentar acceder al archivo. Asegúrate de que no esté abierto en otra aplicación.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}");
            }
        }




    }
}
