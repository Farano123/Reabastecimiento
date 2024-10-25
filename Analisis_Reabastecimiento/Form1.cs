// AUTOR: ANTONIO ESTRADA   
// CONSTRURAMA BLANQUITA
// FECHA CREADO: 21 DE FEBRERO DEL 2020
//
// DESCRIPCION: WINDOWS FORM EN LA CUAL INCLUYE CON FUNCTIONES DE BUSQUEDA
// EN LA BASE DE DATOS USANDO STORED PROCEDURE spNvoDinamico_2 PARA HACER UN
// ANALISIS DE REABASTECIMIENTO. LA FUNCION DE IMPRESION SOLO ES LIBERADA CON
// LA CONTRASEÑA ASIGNADA ("1234"); MOMENTARIA
//
// FUNCIONES: 
//  - BUSCAR INFORMACION POR CATEGORIAS, SUCURSAL(ALMACEN) Y PROVEDOR
//  - SELECCIONAR LOS ARTICULOS QUE QUIERES IMPRIMIR O CREAR ARCHIVO
//  - AUTORIZACION DE IMPRESION POR CONTRASEÑA
//  - LOS ARTICULOS MARCADOS EN COLOR ROJO SON LOS QUE TIENEN MENOS DE 10 DIAS DE INVENTARIO
//
// LENGUAGE DE PROGRAMACION: C#   


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
    // COMIENZO DE PROGRAMA
    public partial class Form1 : Form
    {
        private PrintDocument docToPrint = new PrintDocument();
        string pathToPrint = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        PrintDialog PrintDialog1 = new PrintDialog();
        string passwordIngresado = "";
        string passwordCorrecto = "1234";
        bool otherButtonPresent = false;
        bool gridOnePresent = true;
        bool gridTwoPresent = false;
        bool gridThreePresent = false;

        // FUNCION CREADA POR EL SISTEMA DONDE GUARDA VARIABLES NECESARIAS PARA CREAR EL WINDOWS FORM
        public Form1()
        {
            InitializeComponent();

            dataGridView1.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            sucursalLabel.Font = new System.Drawing.Font(sucursalLabel.Font.Name, 10F);
            fechaLabel.Font = new System.Drawing.Font(fechaLabel.Font.Name, 10F);
            fechaFinalLabel.Font = new System.Drawing.Font(fechaFinalLabel.Font.Name, 10F);
            categoriaLabel.Font = new System.Drawing.Font(categoriaLabel.Font.Name, 10F);
            provedorLabel.Font = new System.Drawing.Font(fechaLabel.Font.Name, 10F);


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        // FUNCION EJECUTADA CUANDO EL PROGRAMA EMPIEZA: AQUI SE ESCONDEN ALGUNOS BOTONES AL IGUAL SE DETERMINAN ESTILOS PARA LOS LABELS
        private void Form1_Load(object sender, EventArgs e)
        {
            comboProvedor.Items.Add("");
            mostrarProvedores();
            mostrarCategorias();
            mostrarSucursales();
            buttonExcel.Show();
            labelAnuncio.Font = new Font("Arial", 10, FontStyle.Bold);
            buttonImprimir.Hide();
            loadingPictureBox.Hide();
            dataGridView2.Hide();
            checkedListBoxClasificador.Hide();
            comboBasico.Hide();
            labelBasico.Hide();
            labelClasificador.Hide();
            dataGridView4.Hide();
            dataGridView3.Hide();
            pruebaBase.Hide();
            textBoxDiasInv.Hide();
            labelDiasDeInventario.Hide();
            //pruebaBase.Hide();
            buttonImprimirGrid2.Hide();
            //buttonImprimirGrid3.Hide();
            dataGridView4.Hide();


            fechaFinalTimePicker1.Value = DateTime.Today.AddDays(-0);
            fechaInicialTimePicker1.Value = DateTime.Today.AddDays(-90);

            comboBasico.Items.Add(""); comboBasico.Items.Add("No"); comboBasico.Items.Add("Si");
            checkedListBoxClasificador.Items.Add("");
            checkedListBoxClasificador.Items.Add("Descontinuado"); checkedListBoxClasificador.Items.Add("Sobre Pedido"); checkedListBoxClasificador.Items.Add("Oportunidad");
            checkedListBoxClasificador.Items.Add("No resurtible"); checkedListBoxClasificador.Items.Add("Art. Produccion"); checkedListBoxClasificador.Items.Add("Art. Introduccion");
            checkedListBoxClasificador.Items.Add("Art. Temp- Verano"); checkedListBoxClasificador.Items.Add("Art. Temp- Inviern"); checkedListBoxClasificador.Items.Add("Art. Linea");


            DataGridViewColumn column = dataGridView1.Columns[1];
            column.Width = 400;

            DataGridViewColumn column2 = dataGridView2.Columns[1];
            column2.Width = 400;
        }

        private void fechaInicialTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        // RESPUESTA AL CLICK DEL BOTON DE BUSQUEDA, AQUI SE EJECUTA LA FUNCION DE LLAMADA A LA BASE DE DATOS


        private async void button1_Click_1(object sender, EventArgs e) // este es el botón buscar
        {
            try
            {
                // Mostrar el GIF de "Cargando..." al inicio de la operación
                loadingPictureBox.Show(); // Mostrar la imagen de carga
                loadingPictureBox.Refresh(); // Forzar la actualización de la interfaz para asegurar que se muestre

                // Limpiar el DataGridView y restablecer controles
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                comboBasico.SelectedIndex = -1;
                checkedListBoxClasificador.ClearSelected(); // Limpiar la selección del CheckedListBox

                dataGridView2.Hide(); // Ocultar el DataGridView si es necesario

                // Obtener las fechas del DateTimePicker
                string fechaInicial = fechaInicialTimePicker1.Value.ToString("MM/dd/yyyy");
                string fechaFinal = fechaFinalTimePicker1.Value.ToString("MM/dd/yyyy");

                DateTime fechaInicialDate = fechaInicialTimePicker1.Value.Date;
                DateTime fechaFinalDate = fechaFinalTimePicker1.Value.Date;

                string diff2 = (fechaFinalDate - fechaInicialDate).TotalDays.ToString();

                // Ejecutar la carga de datos en segundo plano usando Task.Run()
                await Task.Run(() =>
                {
                    // Acceder a la UI usando Invoke si es necesario
                    this.Invoke(new Action(() =>
                    {
                        // Llamar a la función que carga los datos en el DataGridView
                        testNewDatabase(fechaInicial, fechaFinal, diff2);
                    }));
                });

                // Contar las filas cargadas en el DataGridView1 después de la búsqueda
                int numeroDeFilas = 0;
                this.Invoke(new Action(() =>
                {
                    numeroDeFilas = dataGridView1.Rows.Count;
                }));

                // Mostrar el número de filas en el Label llamado NoFilas
                this.Invoke(new Action(() =>
                {
                    NoFilas.Text = numeroDeFilas.ToString();
                }));
            }
            finally
            {
                // Ocultar el GIF de "Cargando..." al finalizar la operación
                loadingPictureBox.Hide(); // Ocultar la imagen de carga
            }
        }




        // FUNCION QUE LLAMA A LA BASE DE DATOS PARA BUSCAR TODOS LOS PROVEEDORES
        private void mostrarProvedores()
        {
            using (SqlConnection conn = new SqlConnection("Data Source = 192.168.102.10; Initial Catalog = BqtApplicationsNew; User ID = sa; Password = SAP123x"))
            {
                using (SqlCommand cmd = new SqlCommand("AppSAP_ListProveedores", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    //ArrayList rowList = new ArrayList();
                    ArrayList cardCodeNombreProvedor = new ArrayList();

                    while (reader.Read())
                    {
                        cardCodeNombreProvedor.Add(reader.GetValue(reader.GetOrdinal("CARDNAME")) + " - " + reader.GetValue(reader.GetOrdinal("CARDCODE")));

                    }

                    int countCode = cardCodeNombreProvedor.Count;

                    for (int i = 0; i < countCode; i++)
                    {
                        comboProvedor.Items.Add(cardCodeNombreProvedor[i]);
                    }

                    reader.Close();
                    conn.Close();
                }
            }
        }

        // FUNCION QUE LLAMA A LA BASE DE DATOS PARA BUSCAR TODAS LAS CATEGORIAS
        private void mostrarCategorias()
        {
            using (SqlConnection conn = new SqlConnection("Data Source = 192.168.102.10; Initial Catalog = BqtApplicationsNew; User ID = sa; Password = SAP123x"))
            {
                using (SqlCommand cmd = new SqlCommand("AppSAP_ListCategoriasArticulos", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    //ArrayList rowList = new ArrayList();
                    ArrayList listaCategorias = new ArrayList();

                    while (reader.Read())
                    {
                        listaCategorias.Add(reader.GetValue(reader.GetOrdinal("ITMSGRPNAM")));

                    }

                    int countCode = listaCategorias.Count;

                    for (int i = 0; i < countCode; i++)
                    {
                        comboCategorias.Items.Add(listaCategorias[i]);
                    }

                    reader.Close();
                    conn.Close();
                }
            }
        }

        // FUNCION QUE LLAMA A LA BASE DE DATOS PARA BUSCAR TODAS LAS SUCURSALES
        private void mostrarSucursales()
        {
            string[] sucursales = { "1011", "1021", "1031", "1041", "1051", "2011", "1061" };
            comboSucursal.Items.AddRange(sucursales);
        }

        // FUNCION QUE LLAMA A LA BASE DE DATOS SIN NECESIDAD DE FECHA. NOTA: ESTA FUNCION QUEDO OBSOLETA CON LA IMPLEMENTACION DE UN NUEVO STORED PROCEDURE spNvoDinamico_2
        // ESTA FUNCION ASIGNA EL VALOR A LAS CELDAS DEL DATAGRIDVIEW CON LA INFORMACION DE LA BASE DE DATOS
     // private void obtenerBusquedaSinFecha(string surcursal_, string categoria_, string proveedor_)
    //  {
      //    using (SqlConnection conn = new SqlConnection("Data Source = 192.168.102.10; Initial Catalog = SistemasTI; User ID = sa; Password = SAP123x"))
        //  {
          //    using (SqlCommand cmd = new SqlCommand("SP_ArtCriticos_v2", conn))
            //  {
              //    cmd.CommandType = CommandType.StoredProcedure;

//                  cmd.Parameters.AddWithValue("@ALM", surcursal_);
   //               cmd.Parameters.AddWithValue("@CAT", categoria_);
     //             cmd.Parameters.AddWithValue("@PROV", proveedor_);
     //
       //           conn.Open();

                    //cmd.CommandTimeout = 60000;
         //         SqlDataReader reader = cmd.ExecuteReader();

           //       if (!reader.HasRows) //The key Word is **.HasRows**

             //     {

               //       MessageBox.Show("INFORMACION NO ENCONTRADA");

                 // }

                //  ArrayList RowsToBePrinted = new ArrayList();
                 // string[] row = new string[20];
                //  int rowSelected = 0;
                //  while (reader.Read())
               //   {
                 //     row = new string[] {reader.GetValue(reader.GetOrdinal("itemCode")).ToString(),
                   //       reader.GetValue(reader.GetOrdinal("itemName")).ToString(), reader.GetValue(reader.GetOrdinal("UomInv")).ToString() ,reader.GetValue(reader.GetOrdinal("WhsCode")).ToString(), reader.GetValue(reader.GetOrdinal("Cant")).ToString(),
                     //     reader.GetValue(reader.GetOrdinal("Stock")).ToString(), reader.GetValue(reader.GetOrdinal("Pedido")).ToString(), reader.GetValue(reader.GetOrdinal("Comprometido")).ToString(),
                       //   reader.GetValue(reader.GetOrdinal("Dias")).ToString(), reader.GetValue(reader.GetOrdinal("VtaDiaria")).ToString(), reader.GetValue(reader.GetOrdinal("DiasInv")).ToString(),
                         // reader.GetValue(reader.GetOrdinal("Vta7Dias")).ToString(), reader.GetValue(reader.GetOrdinal("ExistCEDIS")).ToString(), reader.GetValue(reader.GetOrdinal("OrdCedis")).ToString(),
                        //  reader.GetValue(reader.GetOrdinal("U_Basico")).ToString(), reader.GetValue(reader.GetOrdinal("U_ABC")).ToString(), reader.GetValue(reader.GetOrdinal("Clasificacion")).ToString(),
                         // categoria_ , reader.GetValue(reader.GetOrdinal("CardCode")).ToString(), reader.GetValue(reader.GetOrdinal("CardName")).ToString()};

                  //    dataGridView1.Rows.Add(row);

                    //  string diasDeInventarioString = reader.GetValue(reader.GetOrdinal("DiasInv")).ToString();
                   //   int diasDeInventarioNumero = Int32.Parse(diasDeInventarioString);

                     // if (diasDeInventarioNumero <= 10)
                    //  {
                      //    dataGridView1.Rows[rowSelected].DefaultCellStyle.BackColor = Color.Red;
                      //}

                //      rowSelected++;
             //     }

          //        reader.Close();
           //       conn.Close();
            //  }
      //    }
    //  }

        private void comboSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(comboSucursal.SelectedItem.ToString());
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // RESPUESTA QUE EJECUTA LA FUNCION QUE OBTIENE LOS ARTICULOS SELECCIONADOS PARA IMPRIR, EN ESTA FUNCION SE CREA EL TXT FILE EN EL CUAL INCLUYE LA INFORMACION DE LOS ARTICULOS SELECCIONADOS
        private void buttonImprimir_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("GRID 2");
            string codigoDeArticuloSelec = "";
            string descripcionDeArticuloSelec = "";
            string unidadMedidaDeArticuloSelec = "";
            string almacenDeArticuloSelec = "";
            string cantidadPromedioDeArticuloSelec = "";
            string prvlgPromedioDeArticuloSelec = "";
            string cantInvDeArticuloSelec = "";
            string stockCedisDeArticuloSelec = "";
            string textoCompleto = "";

            Int32 selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (selectedRowCount > 0)
            {
                StringBuilder sb = new StringBuilder();
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "AnalisisReabastesimiento.txt")))
                {
                    outputFile.WriteLine("ITEM CODE                         DESCRIPCION            UNIDAD MEDIDA            ALMACEN            CANTIDAD PROMEDIO            PRVLG            CANTIDAD INV            STOCK CEDIS");
                    for (int i = 0; i < selectedRowCount; i++)
                    {
                        DataGridViewRow selectedRow = dataGridView1.Rows[dataGridView1.SelectedRows[i].Index];
                        codigoDeArticuloSelec = selectedRow.Cells[0].Value.ToString();
                        descripcionDeArticuloSelec = selectedRow.Cells[1].Value.ToString();
                        unidadMedidaDeArticuloSelec = selectedRow.Cells[2].Value.ToString();
                        almacenDeArticuloSelec = selectedRow.Cells[3].Value.ToString();
                        cantidadPromedioDeArticuloSelec = selectedRow.Cells[4].Value.ToString();
                        prvlgPromedioDeArticuloSelec = selectedRow.Cells[6].Value.ToString();
                        cantInvDeArticuloSelec = selectedRow.Cells[7].Value.ToString();
                        stockCedisDeArticuloSelec = selectedRow.Cells[12].Value.ToString();

                        sb.Append("Item Seleccionado " + dataGridView1.SelectedRows[i].Index.ToString());
                        sb.Append(Environment.NewLine);
                        sb.Append("Codigo: " + codigoDeArticuloSelec + " Desc: " + descripcionDeArticuloSelec + " Unidad: " + unidadMedidaDeArticuloSelec + " Almacen: " + almacenDeArticuloSelec + " Cant_prom: " + cantidadPromedioDeArticuloSelec + " Prvlg: " + prvlgPromedioDeArticuloSelec + " Cant_inv: " + cantInvDeArticuloSelec + " Stock CEDIS " + stockCedisDeArticuloSelec);
                        textoCompleto = codigoDeArticuloSelec.ToString() + "        " + descripcionDeArticuloSelec.ToString() + "       " + unidadMedidaDeArticuloSelec.ToString() + "       " + almacenDeArticuloSelec.ToString() + "       " + cantidadPromedioDeArticuloSelec.ToString() + "       " + prvlgPromedioDeArticuloSelec.ToString() + "       " + cantInvDeArticuloSelec.ToString() + "       " + stockCedisDeArticuloSelec.ToString();
                        outputFile.WriteLine(textoCompleto);
                        sb.Append(Environment.NewLine);
                    }
                }

                openFileToPrint();
            }
            else
            {
                MessageBox.Show("Selecciona algun producto");
            }
        }
        // FUNCION QUE IMPRIME LOS ARTICULOS SELECCIONADOS
        private void PrintFile()
        {
            PrintDialog printDlg = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = Path.Combine(pathToPrint, "AnalisisReabastesimiento.txt");
            printDlg.Document = printDoc;
            printDlg.AllowSelection = true;
            printDlg.AllowSomePages = true;
            printDoc.Print();
        }

        // FUNCION QUE ABRE EL ARCHIVO DE TEXTO INSTANTANEAMENTE CUANDO SE MANDA A IMPRIMIR POR EL BOTON DE IMPRESION
        private void openFileToPrint()
        {
            try
            {
                string filePath = Path.Combine(pathToPrint, "AnalisisReabastesimiento.txt");

                // Verificar si el archivo existe antes de intentar abrirlo
                if (File.Exists(filePath))
                {
                    Process.Start("notepad.exe", filePath);
                }
                else
                {
                    MessageBox.Show("El archivo no existe: " + filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al intentar abrir el archivo: {ex.Message}");
            }
        }

        // FUNCION QUE ABRE EL ARCHIVO EXCEL INSTANTANEAMENTE CUANDO SE MANDA A IMPRIMIR POR EL BOTON DE IMPRESION
        


        // RESPUESTA AL BOTON QUE OBTIENE LA CONTRASEÑA INGRESADA POR EL USUARIO PARA LIBERAR LA OPCION DE IMPRESION DISPONIBLE
        private void botonDePassword_Click(object sender, EventArgs e)
        {
            passwordIngresado = textboxPassword.Text;
            //MessageBox.Show("Password Correcto");
            if (passwordCorrecto == passwordIngresado)  
            {
                labelPassword.Hide();
                botonDePassword.Hide();
                textboxPassword.Hide();
                labelAnuncio.Hide();
                buttonExcel.Show();
                if (gridTwoPresent == false)
                {
                    buttonImprimir.Show();
                }
                else if (gridTwoPresent == true)
                {
                    buttonImprimirGrid2.Show();
                }

                MessageBox.Show("Contraseña Correcta");
            }
            else
            {
                MessageBox.Show("Contraseña incorrecta");
            }

        }

        // RESPUESTA AL BOTON QUE LIMPIA LA BUSQUEDA PREVIA. LIMPIA LAS COMBO BOX DE SUCURSAL, CATEGORIA Y PROVEEDOR
        private void bottonLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            comboCategorias.SelectedIndex = -1;
            comboProvedor.SelectedIndex = -1;
            comboSucursal.SelectedIndex = -1;
            comboBasico.SelectedIndex = -1;
            checkedListBoxClasificador.SelectedIndex = -1;
            textBoxDiasInv.Text = "";
            labelBasico.Hide();
            labelClasificador.Hide();
            checkedListBoxClasificador.Hide();
            comboBasico.Hide();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            dataGridView2.Hide();
            labelDiasDeInventario.Hide();
            textBoxDiasInv.Hide();
            //dataGridView3.Rows.Clear();
            //dataGridView3.Refresh();
            dataGridView3.Hide();
            //buttonImprimirGrid3.Hide();
            buttonImprimirGrid2.Hide();
            pruebaBase.Hide();

            if (passwordCorrecto == passwordIngresado)
            {
                buttonImprimir.Show();
                buttonImprimirGrid2.Hide();
            }
        }

        // FUNCION QUE LLAMA LA STORED PROCEDURE spNvoDinamico_2 LA CUAL ES LA NUEVA STORED PROCEDURE LA CUAL QUE CONTIENE TODA LA INFORMACION NECESARIA POR EL DATAGRIDVIEW

        // SE CAMBIO VENTA TOTAL POR VENTA PROMEDIO MENSUAL HACIENDO EL CALCULO PZA/DIAS * 30.5
        private void testNewDatabase(string fechaInicial, string fechaFinal, string diasDeDiferencia)
        {
            string sucursal_ = comboSucursal.Text;
            string categoria_ = comboCategorias.Text;
            string numeroDeProveedor = "";
            string proveedor = "";
            string[] numeroProv = { };
            if (!string.IsNullOrEmpty(comboProvedor.Text))
            {
                proveedor = comboProvedor.Text;
                numeroProv = proveedor.Split(' ');
                numeroDeProveedor = numeroProv[numeroProv.Length - 1].ToString();
            }

            string ventaAb = "Vta_";
            string venta = ventaAb + sucursal_;
            string prvlAb = "PRVLG_";
            string prvlg = prvlAb + sucursal_;
            string existAb = "Exist_";
            string exist = existAb + sucursal_;
            string ordenAb = "Orden_";
            string orden = ordenAb + sucursal_;
            string comproAb = "Compro_";
            string compro = comproAb + sucursal_;
            string diasInvAb = "DiasInv_";
            string diasInv = diasInvAb + sucursal_;
            string ofvlg = "OFVLG_";
            string ofvlgCompleto = ofvlg + sucursal_;
            string hlvlg = "HLVLG_";
            string hlvlgCompleto = hlvlg + sucursal_;

            int diferenciasFecha = Int32.Parse(diasDeDiferencia);
            float mesFijo = 30.5f;
            float ventaDiaria = 0f, valorDeVentaTotal = 0f, horizonte = 0f, ofvlgCompletofloat = 0f, hlvlgCompletofloat = 0f;
            string prueba = "", prueba2 = "";

            if (string.IsNullOrEmpty(comboCategorias.Text) || string.IsNullOrEmpty(comboSucursal.Text))
            {
                MessageBox.Show("Selecciona Sucursal y Categoria");
                return;
            }

            if (passwordCorrecto == passwordIngresado)
            {
                buttonImprimir.Show();
                buttonImprimirGrid2.Hide();
            }

            checkedListBoxClasificador.Show();
            comboBasico.Show();
            labelBasico.Show();
            pruebaBase.Show();
            labelDiasDeInventario.Show();
            textBoxDiasInv.Show();
            labelClasificador.Show();

            dataGridView1.Rows.Clear();
            dataGridView1.SuspendLayout(); // Optimización: suspender el renderizado

            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=192.168.120.13; Initial Catalog=SistemasTI; User ID=sa; Password=Pass@123x"))
                {
                    using (SqlCommand cmd = new SqlCommand("spNvoDinamico_3CL", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FechaD", fechaInicial);
                        cmd.Parameters.AddWithValue("@FechaA", fechaFinal);

                        conn.Open();
                        cmd.CommandTimeout = 100000;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();  // Optimización: cargar datos en DataTable
                            dt.Load(reader);
                         
                                foreach (DataRow row in dt.Rows)
                            {
                                if (string.IsNullOrEmpty(comboProvedor.Text))
                                {
                                    valorDeVentaTotal = Convert.ToSingle(row[venta]);
                                    ventaDiaria = valorDeVentaTotal / diferenciasFecha;

                                    prueba = row[ofvlgCompleto].ToString();
                                    prueba2 = row[hlvlgCompleto].ToString();
                                    ofvlgCompletofloat = !string.IsNullOrEmpty(prueba) ? Convert.ToSingle(prueba) : 0;
                                    hlvlgCompletofloat = !string.IsNullOrEmpty(prueba2) ? Convert.ToSingle(prueba2) : 0;
                                    horizonte = ofvlgCompletofloat + hlvlgCompletofloat;

                                    if (categoria_ == row["Familia"].ToString())
                                    {
                                        string diasDeInventarioString = row[diasInv].ToString();
                                        int diasDeInventarioNumero = Int32.Parse(diasDeInventarioString);

                                        

                                        dataGridView1.Rows.Add(new string[]
                                        {
                                    row["itemcode"].ToString(),
                                    row["itemname"].ToString(),
                                    row["UomCode"].ToString(),
                                    sucursal_,
                                    valorDeVentaTotal.ToString(),
                                    horizonte.ToString(),
                                    row[prvlg].ToString(),
                                    row[exist].ToString(),
                                    row["Exist_TRA1011"].ToString(),
                                    row[orden].ToString(),
                                    row[compro].ToString(),
                                    ventaDiaria.ToString(),
                                    diasDeInventarioString,
                                    row["Exist_3011"].ToString(),
                               
                                    row["Basico"].ToString(),
                                    row["ABC"].ToString(),
                                    row["Clasificacion"].ToString(),
                                    row["Familia"].ToString(),
                                    row["Proveedor"].ToString(),
                                    row["NomProveedor"].ToString()
                                        });

                                        if (diasDeInventarioNumero <= 8)
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                        }
                                    }
                                }
                                // Parte del proveedor también debe incluir la columna de tránsito
                                else if (!string.IsNullOrEmpty(comboProvedor.Text))
                                {
                                    if (categoria_ == row["Familia"].ToString() && numeroDeProveedor == row["Proveedor"].ToString())
                                    {
                                        valorDeVentaTotal = Convert.ToSingle(row[venta]);
                                        ventaDiaria = valorDeVentaTotal / diferenciasFecha;

                                        prueba = row[ofvlgCompleto].ToString();
                                        prueba2 = row[hlvlgCompleto].ToString();
                                        ofvlgCompletofloat = !string.IsNullOrEmpty(prueba) ? Convert.ToSingle(prueba) : 0;
                                        hlvlgCompletofloat = !string.IsNullOrEmpty(prueba2) ? Convert.ToSingle(prueba2) : 0;
                                        horizonte = ofvlgCompletofloat + hlvlgCompletofloat;

                                        string diasDeInventarioString = row[diasInv].ToString();
                                        int diasDeInventarioNumero = Int32.Parse(diasDeInventarioString);

                                      

                                        dataGridView1.Rows.Add(new string[]
                                        {
                                    row["itemcode"].ToString(),
                                    row["itemname"].ToString(),
                                    row["UomCode"].ToString(),
                                    sucursal_,
                                    valorDeVentaTotal.ToString(),
                                    horizonte.ToString(),
                                    row[prvlg].ToString(),
                                    row[exist].ToString(),
                                    row[orden].ToString(),
                                    row[compro].ToString(),
                                    ventaDiaria.ToString(),
                                    diasDeInventarioString,
                                    row["Exist_3011"].ToString(),
                                  
                                    row["Basico"].ToString(),
                                    row["ABC"].ToString(),
                                    row["Clasificacion"].ToString(),
                                    row["Familia"].ToString(),
                                    row["Proveedor"].ToString(),
                                    row["NomProveedor"].ToString()
                                        });

                                        if (diasDeInventarioNumero <= 8)
                                        {
                                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                dataGridView1.ResumeLayout();  // Reanudar el renderizado
                textBoxDiasInv.Show();
            }
        }




        private void pruebaBase_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.SuspendLayout();  // Suspender el renderizado del DataGridView

            int rowSelected = 0;
            bool hideData = false;
            gridTwoPresent = true;

            if (passwordCorrecto == passwordIngresado)
            {
                buttonImprimir.Hide();
                buttonImprimirGrid2.Show();
            }

            // Obtener las selecciones del CheckedListBoxClasificador
            List<string> clasificadorSeleccionado = new List<string>();
            foreach (var item in checkedListBoxClasificador.CheckedItems)
            {
                clasificadorSeleccionado.Add(item.ToString());
            }

            // Si el CheckedListBox no tiene selecciones, ignora el filtro de clasificador
            foreach (DataGridViewRow r in this.dataGridView1.Rows)
            {
                bool agregarFila = false;
                int diasEnTabla = Int32.Parse(r.Cells[11].Value.ToString());
                string[] row = new string[20]; // Aumentar el tamaño si agregas más columnas (20 + 1 para la columna de tránsito "Trancito")

                // Si no hay ningún clasificador seleccionado
                if (clasificadorSeleccionado.Count == 0)
                {
                    // Solo filtrar por comboBasico y textBoxDiasInv
                    if (comboBasico.Text != "" && textBoxDiasInv.Text != "")
                    {
                        int diaInsertado = Int32.Parse(textBoxDiasInv.Text);
                        if (r.Cells[14].Value.ToString().StartsWith(this.comboBasico.Text.Trim()) && diasEnTabla < diaInsertado)
                        {
                            agregarFila = true;
                        }
                    }
                    else if (comboBasico.Text != "" && textBoxDiasInv.Text == "")
                    {
                        if (r.Cells[14].Value.ToString().StartsWith(this.comboBasico.Text.Trim()))
                        {
                            agregarFila = true;
                        }
                    }
                    else if (comboBasico.Text == "" && textBoxDiasInv.Text != "")
                    {
                        int diaInsertado = Int32.Parse(textBoxDiasInv.Text);
                        if (diasEnTabla < diaInsertado)
                        {
                            agregarFila = true;
                        }
                    }
                    else
                    {
                        // Si no hay filtros de comboBasico o DiasInv, agregar todas las filas
                        agregarFila = true;
                    }
                }
                else
                {
                    // Aplicar los filtros si hay selección en CheckedListBoxClasificador
                    if (comboBasico.Text != "" && textBoxDiasInv.Text != "")
                    {
                        int diaInsertado = Int32.Parse(textBoxDiasInv.Text);
                        if (r.Cells[14].Value.ToString().StartsWith(this.comboBasico.Text.Trim()) &&
                            clasificadorSeleccionado.Contains(r.Cells[16].Value.ToString()) && diasEnTabla < diaInsertado)
                        {
                            agregarFila = true;
                        }
                    }
                    else if (comboBasico.Text != "" && textBoxDiasInv.Text == "")
                    {
                        if (r.Cells[14].Value.ToString().StartsWith(this.comboBasico.Text.Trim()) &&
                            clasificadorSeleccionado.Contains(r.Cells[16].Value.ToString()))
                        {
                            agregarFila = true;
                        }
                    }
                    else if (comboBasico.Text == "" && textBoxDiasInv.Text != "")
                    {
                        int diaInsertado = Int32.Parse(textBoxDiasInv.Text);
                        if (clasificadorSeleccionado.Contains(r.Cells[16].Value.ToString()) && diasEnTabla < diaInsertado)
                        {
                            agregarFila = true;
                        }
                    }
                    else if (comboBasico.Text == "" && textBoxDiasInv.Text == "")
                    {
                        if (clasificadorSeleccionado.Contains(r.Cells[16].Value.ToString()))
                        {
                            agregarFila = true;
                        }
                    }
                }

                if (agregarFila)
                {
                    // Construir fila incluyendo la columna de tránsito (por ejemplo, la columna 13 de dataGridView1)
                    for (int i = 0; i < 20; i++) // Ajusta la longitud de la fila según sea necesario
                    {
                        row[i] = r.Cells[i].Value.ToString();
                    }

             
                    // Colorear fila si los días de inventario son <= 8
                    if (diasEnTabla <= 8)
                    {
                        dataGridView2.Rows[rowSelected].DefaultCellStyle.BackColor = Color.Red;
                    }

                    rowSelected++;
                }
            }

            if (!hideData)
            {
                dataGridView2.Show();
            }
            else
            {
                MessageBox.Show("Seleccione Filtro");
                dataGridView2.Hide();
            }

            dataGridView2.ResumeLayout();  // Reanudar el renderizado del DataGridView
            dataGridView1.ClearSelection(); // Limpiar selección para evitar resaltar filas innecesarias

            // Contar las filas agregadas en dataGridView2 y mostrar en el Label NoFilas
            int numeroDeFilas = dataGridView2.Rows.Count;
            NoFilas.Text = numeroDeFilas.ToString();
        }






        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void filtroDeClasificacion(string clasificacion)
        {
            gridTwoPresent = true;
            gridOnePresent = false;
            gridThreePresent = false;
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            string searchValue = clasificacion;
            string[] rows = new string[23]; // Cambiado a 24 para incluir "Trancito"
            int rowSelected = 0;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[17].Value.ToString().Equals(searchValue))
                    {
                        row.Selected = true;
                        rows = new string[]
                        {
                    dataGridView1.SelectedCells[0].Value.ToString(),
                    dataGridView1.SelectedCells[1].Value.ToString(),
                    dataGridView1.SelectedCells[2].Value.ToString(),
                    dataGridView1.SelectedCells[3].Value.ToString(),
                    dataGridView1.SelectedCells[4].Value.ToString(),
                    dataGridView1.SelectedCells[5].Value.ToString(),
                    dataGridView1.SelectedCells[6].Value.ToString(),
                    dataGridView1.SelectedCells[7].Value.ToString(),
                    dataGridView1.SelectedCells[8].Value.ToString(),
                    dataGridView1.SelectedCells[9].Value.ToString(),
                    dataGridView1.SelectedCells[10].Value.ToString(),
                    dataGridView1.SelectedCells[11].Value.ToString(),
                    dataGridView1.SelectedCells[12].Value.ToString(),
                    dataGridView1.SelectedCells[13].Value.ToString(),
                    dataGridView1.SelectedCells[14].Value.ToString(),
                    dataGridView1.SelectedCells[15].Value.ToString(),
                    dataGridView1.SelectedCells[16].Value.ToString(),
                    dataGridView1.SelectedCells[17].Value.ToString(),
                    dataGridView1.SelectedCells[18].Value.ToString(),
                    dataGridView1.SelectedCells[19].Value.ToString(),
                    dataGridView1.SelectedCells[20].Value.ToString(),
                   
                        };

                        dataGridView2.Rows.Add(rows);

                        string diasDeInventarioString = dataGridView1.SelectedCells[12].Value.ToString();
                        int diasDeInventarioNumero = Int32.Parse(diasDeInventarioString);

                        if (diasDeInventarioNumero <= 8)
                        {
                            dataGridView2.Rows[rowSelected].DefaultCellStyle.BackColor = Color.Red;
                        }

                        rowSelected++;
                    }
                }
                dataGridView2.Show();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void filtrosDeBasicos(string basico)
        {
            gridTwoPresent = true;
            gridOnePresent = false;
            gridThreePresent = false;
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            string searchValue = basico;
            string[] rows = new string[20]; // Cambiado a 21 para incluir "Trancito"
            int rowSelected = 0;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[15].Value.ToString().Equals(searchValue))
                    {
                        row.Selected = true;
                        rows = new string[]
                        {
                    dataGridView1.SelectedCells[0].Value.ToString(),
                    dataGridView1.SelectedCells[1].Value.ToString(),
                    dataGridView1.SelectedCells[2].Value.ToString(),
                    dataGridView1.SelectedCells[3].Value.ToString(),
                    dataGridView1.SelectedCells[4].Value.ToString(),
                    dataGridView1.SelectedCells[5].Value.ToString(),
                    dataGridView1.SelectedCells[6].Value.ToString(),
                    dataGridView1.SelectedCells[7].Value.ToString(),
                    dataGridView1.SelectedCells[8].Value.ToString(),
                    dataGridView1.SelectedCells[9].Value.ToString(),
                    dataGridView1.SelectedCells[10].Value.ToString(),
                    dataGridView1.SelectedCells[11].Value.ToString(),
                    dataGridView1.SelectedCells[12].Value.ToString(),
                    dataGridView1.SelectedCells[13].Value.ToString(),
                    dataGridView1.SelectedCells[14].Value.ToString(),
                    dataGridView1.SelectedCells[15].Value.ToString(),
                    dataGridView1.SelectedCells[16].Value.ToString(),
                    dataGridView1.SelectedCells[17].Value.ToString(),
                    dataGridView1.SelectedCells[18].Value.ToString(),
                    dataGridView1.SelectedCells[19].Value.ToString(),
                   
                        };

                        dataGridView2.Rows.Add(rows);

                        string diasDeInventarioString = dataGridView1.SelectedCells[10].Value.ToString();
                        int diasDeInventarioNumero = Int32.Parse(diasDeInventarioString);

                        if (diasDeInventarioNumero <= 8)
                        {
                            dataGridView2.Rows[rowSelected].DefaultCellStyle.BackColor = Color.Red;
                        }

                        rowSelected++;
                    }
                }
                dataGridView2.Show();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }


        private void comboBasico_SelectedIndexChanged(object sender, EventArgs e)
        {
            //otherButtonPresent = false;
            //passwordIngresado = textboxPassword.Text;
            ////MessageBox.Show("Password Correcto");

            //if (comboBasico.Text != "" && comboClasificador.Text != "")
            //{
            //    if (passwordCorrecto == passwordIngresado)
            //    {
            //        if (otherButtonPresent == false)
            //        {
            //            buttonImprimirGrid3.Show();
            //            buttonImprimirGrid2.Hide();
            //            buttonImprimir.Hide();
            //            otherButtonPresent = true;
            //        }
            //    }

            //    multipleFilter(comboBasico.Text, comboClasificador.Text);               
            //}
            //else
            //{
            //    if (passwordCorrecto == passwordIngresado)
            //    {
            //        if (otherButtonPresent == false)
            //        {
            //            //MessageBox.Show("BASICO");
            //            buttonImprimirGrid3.Hide();
            //            buttonImprimirGrid2.Show();
            //            buttonImprimir.Hide();
            //            otherButtonPresent = true;
            //        }
            //    }

            //    filtrosDeBasicos(comboBasico.Text);
            //}         
        }

        private void comboClasificador_SelectedIndexChanged(object sender, EventArgs e)
        {
            //otherButtonPresent = false;
            //if (comboBasico.Text != "" && comboClasificador.Text != "")
            //{
            //    //MessageBox.Show("CLASIFICADOR Y BASICO");

            //    if (passwordCorrecto == passwordIngresado)
            //    {
            //        if (otherButtonPresent == false)
            //        {
            //            buttonImprimirGrid3.Show();
            //            buttonImprimirGrid2.Hide();
            //            buttonImprimir.Hide();
            //            otherButtonPresent = true;
            //        }
            //    }

            //    multipleFilter(comboBasico.Text, comboClasificador.Text);
            //}
            //else
            //{

            //    if (passwordCorrecto == passwordIngresado)
            //    {
            //        //MessageBox.Show("CLASIFICADOR SOLO");
            //        if (otherButtonPresent == false)
            //        {
            //            buttonImprimirGrid3.Hide();
            //            buttonImprimirGrid2.Show();
            //            buttonImprimir.Hide();
            //            otherButtonPresent = true;
            //        }

            //    }

            //    filtroDeClasificacion(comboClasificador.Text);
            //}
        }

        private void multipleFilter(string basico, string clasificacion)
        {
            gridThreePresent = true;
            gridOnePresent = false;
            gridTwoPresent = false;
            //dataGridView3.Rows.Clear();
            //dataGridView3.Refresh();
            string basicoValue = basico;
            string categoriaValue = clasificacion;
            string[] rows = new string[20];
            int rowSelected = 0;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    MessageBox.Show(basicoValue);
                    MessageBox.Show(categoriaValue);
                    if (row.Cells[13].Value.ToString().Equals(basicoValue) && row.Cells[15].Value.ToString().Equals(categoriaValue))
                    {
                        MessageBox.Show(dataGridView1.SelectedCells[0].Value.ToString() + " " + dataGridView1.SelectedCells[1].Value.ToString());
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        } // cambio a la seleccion de valores se cambio de datagridview2 a datagridview1, de no funcionar regresar a estadp previo

        private void buttonImprimirGrid2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("GRID 2");
            string codigoDeArticuloSelec = "";
            string descripcionDeArticuloSelec = "";
            string unidadMedidaDeArticuloSelec = "";
            string almacenDeArticuloSelec = "";
            string cantidadPromedioDeArticuloSelec = "";
            string prvlgPromedioDeArticuloSelec = "";
            string cantInvDeArticuloSelec = "";
            string stockCedisDeArticuloSelec = "";
            string textoCompleto = "";

            Int32 selectedRowCount = dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (selectedRowCount > 0)
            {
                StringBuilder sb = new StringBuilder();
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "AnalisisReabastesimiento.txt")))
                {
                    outputFile.WriteLine("ITEM CODE                         DESCRIPCION            UNIDAD MEDIDA            ALMACEN            CANTIDAD PROMEDIO            PRVLG            CANTIDAD INV            STOCK CEDIS");
                    for (int i = 0; i < selectedRowCount; i++)
                    {
                        DataGridViewRow selectedRow = dataGridView2.Rows[dataGridView2.SelectedRows[i].Index];
                        codigoDeArticuloSelec = selectedRow.Cells[0].Value.ToString();
                        descripcionDeArticuloSelec = selectedRow.Cells[1].Value.ToString();
                        unidadMedidaDeArticuloSelec = selectedRow.Cells[2].Value.ToString();
                        almacenDeArticuloSelec = selectedRow.Cells[3].Value.ToString();
                        cantidadPromedioDeArticuloSelec = selectedRow.Cells[4].Value.ToString();
                        prvlgPromedioDeArticuloSelec = selectedRow.Cells[6].Value.ToString();
                        cantInvDeArticuloSelec = selectedRow.Cells[7].Value.ToString();
                        stockCedisDeArticuloSelec = selectedRow.Cells[12].Value.ToString();

                        sb.Append("Item Seleccionado " + dataGridView2.SelectedRows[i].Index.ToString());
                        sb.Append(Environment.NewLine);
                        sb.Append("Codigo: " + codigoDeArticuloSelec + " Desc: " + descripcionDeArticuloSelec + " Unidad: " + unidadMedidaDeArticuloSelec + " Almacen: " + almacenDeArticuloSelec + " Cant_prom: " + cantidadPromedioDeArticuloSelec + " Prvlg: " + prvlgPromedioDeArticuloSelec + " Cant_inv: " + cantInvDeArticuloSelec + " Stock CEDIS " + stockCedisDeArticuloSelec);
                        textoCompleto = codigoDeArticuloSelec.ToString() + "        " + descripcionDeArticuloSelec.ToString() + "       " + unidadMedidaDeArticuloSelec.ToString() + "       " + almacenDeArticuloSelec.ToString() + "       " + cantidadPromedioDeArticuloSelec.ToString() + "       " + prvlgPromedioDeArticuloSelec.ToString() + "       " + cantInvDeArticuloSelec.ToString() + "       " + stockCedisDeArticuloSelec.ToString();
                        outputFile.WriteLine(textoCompleto);
                        sb.Append(Environment.NewLine);
                    }
                }

                openFileToPrint();
            }
            else
            {
                MessageBox.Show("Selecciona algun producto");
            }
        }

        private void buttonImprimirGrid3_Click(object sender, EventArgs e)
        {
            ////MessageBox.Show("GRID 3");
            //string codigoDeArticuloSelec = "";
            //string descripcionDeArticuloSelec = "";
            //string unidadMedidaDeArticuloSelec = "";
            //string textoCompleto = "";

            //Int32 selectedRowCount = dataGridView3.Rows.GetRowCount(DataGridViewElementStates.Selected);
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //if (selectedRowCount > 0)
            //{

            //    StringBuilder sb = new StringBuilder();
            //    using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "AnalisisReabastesimiento.txt")))
            //    {
            //        outputFile.WriteLine("ITEM CODE                         DESCRIPCION            UNIDAD MEDIDA");
            //        for (int i = 0; i < selectedRowCount; i++)
            //        {
            //            DataGridViewRow selectedRow = dataGridView3.Rows[dataGridView3.SelectedRows[i].Index];
            //            codigoDeArticuloSelec = selectedRow.Cells[0].Value.ToString();
            //            descripcionDeArticuloSelec = selectedRow.Cells[1].Value.ToString();
            //            unidadMedidaDeArticuloSelec = selectedRow.Cells[2].Value.ToString();

            //            sb.Append("Item Seleccionado " + dataGridView3.SelectedRows[i].Index.ToString());
            //            sb.Append(Environment.NewLine);
            //            sb.Append("Codigo: " + codigoDeArticuloSelec + " Desc: " + descripcionDeArticuloSelec + " Unidad: " + unidadMedidaDeArticuloSelec);
            //            textoCompleto = codigoDeArticuloSelec.ToString() + "        " + descripcionDeArticuloSelec.ToString() + "       " + unidadMedidaDeArticuloSelec.ToString();
            //            outputFile.WriteLine(textoCompleto);
            //            sb.Append(Environment.NewLine);
            //        }
            //    }

            //    openFileToPrint();
            //}
            //else
            //{
            //    MessageBox.Show("Selecciona algun producto");
            //}
        }

        private void textBoxDiasInv_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // If you want, you can allow decimal (float) numbers
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void filterByDiasInventory(int dias)
        {
            gridTwoPresent = true;
            gridOnePresent = false;
            gridThreePresent = false;
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            string searchValue = dias.ToString();
            string[] rows = new string[20];
            int rowSelected = 0;
            int numeroDeDias = 0;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    numeroDeDias = Int32.Parse(row.Cells[10].Value.ToString());
                    if (numeroDeDias < dias)
                    {
                        row.Selected = true;
                        //dataGridView2.Rows[0].Cells[0].Value = dataGridView1.SelectedCells[0].Value.ToString();
                        rows = new string[] {dataGridView1.SelectedCells[0].Value.ToString(),
                            dataGridView1.SelectedCells[1].Value.ToString(),dataGridView1.SelectedCells[2].Value.ToString() ,dataGridView1.SelectedCells[3].Value.ToString(),dataGridView1.SelectedCells[4].Value.ToString(),
                            dataGridView1.SelectedCells[5].Value.ToString(),dataGridView1.SelectedCells[6].Value.ToString(),dataGridView1.SelectedCells[7].Value.ToString(),
                            dataGridView1.SelectedCells[8].Value.ToString(), dataGridView1.SelectedCells[9].Value.ToString(),dataGridView1.SelectedCells[10].Value.ToString(),
                            dataGridView1.SelectedCells[11].Value.ToString(), dataGridView1.SelectedCells[12].Value.ToString(), dataGridView1.SelectedCells[13].Value.ToString(),
                            dataGridView1.SelectedCells[14].Value.ToString(), dataGridView1.SelectedCells[15].Value.ToString(), dataGridView1.SelectedCells[16].Value.ToString(),
                            dataGridView1.SelectedCells[17].Value.ToString(), dataGridView1.SelectedCells[18].Value.ToString()};

                        dataGridView2.Rows.Add(rows);

                        string diasDeInventarioString = dataGridView1.SelectedCells[10].Value.ToString();
                        int diasDeInventarioNumero = Int32.Parse(diasDeInventarioString);

                        if (diasDeInventarioNumero <= 8)
                        {
                            dataGridView2.Rows[rowSelected].DefaultCellStyle.BackColor = Color.Red;
                        }

                        rowSelected++;
                    }
                }


                dataGridView2.Show();
            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void mostrarTodasLasSucursales(string fechaInicial, string fechaFinal, string diasDeDiferencia)
        {
            dataGridView4.Rows.Clear();
            dataGridView4.Refresh();
            dataGridView4.Show();
            dataGridView2.Hide();

            int diferenciasFecha = Int32.Parse(diasDeDiferencia);
            float[] ventaDiaria = new float[8];
            float[] valorDeVentaTotal = new float[8];

            if (string.IsNullOrEmpty(comboCategorias.Text) || string.IsNullOrEmpty(comboSucursal.Text))
            {
                MessageBox.Show("Selecciona Sucursal y Categoria");
                return;
            }

            using (SqlConnection conn = new SqlConnection("Data Source=192.168.120.13; Initial Catalog=SistemasTI; User ID=sa; Password=Pass@123x"))
            using (SqlCommand cmd = new SqlCommand("spNvoDinamico_3CL", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaD", fechaInicial);
                cmd.Parameters.AddWithValue("@FechaA", fechaFinal);

                string sucursal_ = comboSucursal.Text;
                string categoria_ = comboCategorias.Text;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    string[] ventaColumnas = { "Vta_1011", "Vta_1021", "Vta_1031", "Vta_1041", "Vta_1051", "Vta_2011", "Vta_3011", "Vta_1061" };
                    string[] diasInvColumnas = { "DiasInv_1011", "DiasInv_1021", "DiasInv_1031", "DiasInv_1041", "DiasInv_1051", "DiasInv_2011", "DiasInv_1061" };
               

                    while (reader.Read())
                    {
                        // Validación de categoría y proveedor
                        if (categoria_ != reader["Familia"].ToString() || (!string.IsNullOrEmpty(comboProvedor.Text) && comboProvedor.Text != reader["NomProveedor"].ToString()))
                            continue;

                        // Cargar las ventas y calcular ventas diarias
                        for (int i = 0; i < ventaColumnas.Length; i++)
                        {
                            float.TryParse(reader[ventaColumnas[i]].ToString(), out valorDeVentaTotal[i]);
                            ventaDiaria[i] = valorDeVentaTotal[i] / diferenciasFecha;
                        }

                        // Construcción de la fila
                        var row = new object[]
                        {
                    reader["itemcode"].ToString(),
                    reader["itemname"].ToString(),
                    reader["UomCode"].ToString(),
                    valorDeVentaTotal[0].ToString(), valorDeVentaTotal[1].ToString(), valorDeVentaTotal[2].ToString(),
                    valorDeVentaTotal[3].ToString(), valorDeVentaTotal[4].ToString(), valorDeVentaTotal[5].ToString(),
                    valorDeVentaTotal[6].ToString(), valorDeVentaTotal[7].ToString(),
                    reader["PRVLG_1011"].ToString(), reader["PRVLG_1021"].ToString(), reader["PRVLG_1031"].ToString(),
                    reader["PRVLG_1041"].ToString(), reader["PRVLG_1051"].ToString(), reader["PRVLG_2011"].ToString(),
                    reader["PRVLG_3011"].ToString(), reader["PRVLG_1061"].ToString(),
                    reader["Exist_1011"].ToString(), reader["Exist_1021"].ToString(), reader["Exist_1031"].ToString(),
                    reader["Exist_1041"].ToString(), reader["Exist_1051"].ToString(), reader["Exist_2011"].ToString(),
                    reader["Exist_3011"].ToString(), reader["Exist_1061"].ToString(),
                    reader["Orden_1011"].ToString(), reader["Orden_1021"].ToString(), reader["Orden_1031"].ToString(),
                    reader["Orden_1041"].ToString(), reader["Orden_1051"].ToString(), reader["Orden_2011"].ToString(),
                    reader["Orden_3011"].ToString(), reader["Orden_1061"].ToString(),
                    reader["Compro_1011"].ToString(), reader["Compro_1021"].ToString(), reader["Compro_1031"].ToString(),
                    reader["Compro_1041"].ToString(), reader["Compro_1051"].ToString(), reader["Compro_2011"].ToString(),
                    reader["Compro_3011"].ToString(), reader["Compro_1061"].ToString(),
                    ventaDiaria[0].ToString(), ventaDiaria[1].ToString(), ventaDiaria[2].ToString(),
                    ventaDiaria[3].ToString(), ventaDiaria[4].ToString(), ventaDiaria[5].ToString(),
                    ventaDiaria[6].ToString(), ventaDiaria[7].ToString(),
                    reader[diasInvColumnas[0]].ToString(), reader[diasInvColumnas[1]].ToString(), reader[diasInvColumnas[2]].ToString(),
                    reader[diasInvColumnas[3]].ToString(), reader[diasInvColumnas[4]].ToString(), reader[diasInvColumnas[5]].ToString(),
                    reader[diasInvColumnas[6]].ToString(),
                  
                    reader["Basico"].ToString(), reader["ABC"].ToString(), reader["Clasificacion"].ToString(),
                    reader["Familia"].ToString(), reader["Proveedor"].ToString(), reader["NomProveedor"].ToString()
                        };

                        dataGridView4.Rows.Add(row);
                    }
                }
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            mostrarTodasLasSucursales("05/01/2019", "08/29/2019", "90");
        }

        private void buttonExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Random random = new Random();
                string datesSelected = random.Next(1, 100000).ToString();
                string nameOfFile = "AnalisisReabastesimiento-" + datesSelected + ".xlsx";

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
                new string[] {"ITEM CODE", "DESCRIPCION", "UNIDAD MEDIDA", "ALMACEN", "CANT_PROM_MENSUAL", "PRVLG", "CANT_INV", "STOCK_CEDIS", "FECHA SELECCIONADA: " + fechaInicialTimePicker1.Text + " A " + fechaFinalTimePicker1.Text}
            };

                    string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                    var worksheet = excel.Workbook.Worksheets["Worksheet1"];

                    // Popular los datos del encabezado
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<string[]>();
                    int gridTwoCount = dataGridView2.RowCount;

                    if (gridTwoCount > 0)
                    {
                        foreach (DataGridViewRow r in this.dataGridView2.Rows)
                        {
                            cellData.Add(new string[]
                            {
                        r.Cells[0]?.Value?.ToString() ?? "",
                        r.Cells[1]?.Value?.ToString() ?? "",
                        r.Cells[2]?.Value?.ToString() ?? "",
                        r.Cells[3]?.Value?.ToString() ?? "",
                        r.Cells[4]?.Value?.ToString() ?? "",
                        r.Cells[6]?.Value?.ToString() ?? "",
                        r.Cells[7]?.Value?.ToString() ?? "",
                        r.Cells[12]?.Value?.ToString() ?? ""
                            });
                        }
                    }
                    else if (gridTwoCount < 1)
                    {
                        foreach (DataGridViewRow r in this.dataGridView1.Rows)
                        {
                            cellData.Add(new string[]
                            {
                        r.Cells[0]?.Value?.ToString() ?? "",
                        r.Cells[1]?.Value?.ToString() ?? "",
                        r.Cells[2]?.Value?.ToString() ?? "",
                        r.Cells[3]?.Value?.ToString() ?? "",
                        r.Cells[4]?.Value?.ToString() ?? "",
                        r.Cells[6]?.Value?.ToString() ?? "",
                        r.Cells[7]?.Value?.ToString() ?? "",
                        r.Cells[12]?.Value?.ToString() ?? ""
                            });
                        }
                    }

                    // Popular los datos de la tabla
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





        private void buttonUpload_Click(object sender, EventArgs e)
        {
            string finalDate = fechaFinalTimePicker1.Value.ToShortDateString();
            string rangoFecha = Convert.ToDateTime(finalDate).ToString("dd/MM/yyyy");
            Random random = new Random();
            int folio = random.Next(1, 10000);
            int selectedRowCountGrid1 = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            int selectedRowCountGrid2 = dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (selectedRowCountGrid1 > 0)
            {
                InsertarDatos(selectedRowCountGrid1, dataGridView1, rangoFecha, folio);
            }
            else if (selectedRowCountGrid2 > 0)
            {
                InsertarDatos(selectedRowCountGrid2, dataGridView2, rangoFecha, folio);
            }
        }

        private void InsertarDatos(int selectedRowCount, DataGridView gridView, string rangoFecha, int folio)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=192.168.102.10; Initial Catalog=SistemasTI; User ID=sa; Password=SAP123x"))
            {
                string query = "INSERT INTO dbo.SeguimientoArticulos (itemCode, descripcion, unidad_medida, almacen, cant_prom_mes, prvlg, cant_Inv, stockCedis, fecha, completo, folio) " +
                               "VALUES (@itemCode, @descripcion, @unidad, @almacen, @cantProm, @prvlg, @cantInv, @stockCedis, @fecha, @completo, @folio)";
                conn.Open();

                for (int i = 0; i < selectedRowCount; i++)
                {
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        DataGridViewRow selectedRow = gridView.Rows[gridView.SelectedRows[i].Index];
                        command.Parameters.AddWithValue("@itemCode", selectedRow.Cells[0].Value.ToString());
                        command.Parameters.AddWithValue("@descripcion", selectedRow.Cells[1].Value.ToString());
                        command.Parameters.AddWithValue("@unidad", selectedRow.Cells[2].Value.ToString());
                        command.Parameters.AddWithValue("@almacen", selectedRow.Cells[3].Value.ToString());
                        command.Parameters.AddWithValue("@cantProm", selectedRow.Cells[4].Value.ToString());
                        command.Parameters.AddWithValue("@prvlg", selectedRow.Cells[6].Value.ToString());
                        command.Parameters.AddWithValue("@cantInv", selectedRow.Cells[7].Value.ToString());
                        command.Parameters.AddWithValue("@stockCedis", selectedRow.Cells[12].Value.ToString());
                        command.Parameters.AddWithValue("@fecha", rangoFecha);
                        command.Parameters.AddWithValue("@completo", "no");
                        command.Parameters.AddWithValue("@folio", folio);

                        command.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Datos insertados");
        }


        private void buttonForm2_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private void labelAnuncio_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
