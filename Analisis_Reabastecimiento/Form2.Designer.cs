﻿namespace Analisis_Reabastecimiento
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.dataGridView1From2 = new System.Windows.Forms.DataGridView();
            this.itemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.completo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelFecha = new System.Windows.Forms.Label();
            this.dateTimePicker1Form2 = new System.Windows.Forms.DateTimePicker();
            this.labelFolios = new System.Windows.Forms.Label();
            this.comboBoxFolios = new System.Windows.Forms.ComboBox();
            this.buttonGuardar = new System.Windows.Forms.Button();
            this.comboSucursal = new System.Windows.Forms.ComboBox();
            this.labelSucursal = new System.Windows.Forms.Label();
            this.bottonExcel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1From2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1From2
            // 
            this.dataGridView1From2.AllowDrop = true;
            this.dataGridView1From2.AllowUserToAddRows = false;
            this.dataGridView1From2.AllowUserToDeleteRows = false;
            this.dataGridView1From2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1From2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1From2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1From2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemCode,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column11,
            this.Column7,
            this.column8,
            this.Column9,
            this.Column10,
            this.completo});
            this.dataGridView1From2.Location = new System.Drawing.Point(0, 194);
            this.dataGridView1From2.Name = "dataGridView1From2";
            this.dataGridView1From2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1From2.Size = new System.Drawing.Size(1344, 528);
            this.dataGridView1From2.TabIndex = 0;
            // 
            // itemCode
            // 
            this.itemCode.HeaderText = "ITEM CODE";
            this.itemCode.Name = "itemCode";
            // 
            // Column1
            // 
            this.Column1.HeaderText = "DESCRIPCION";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "UNI MEDIDA";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "ALMACEN";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "CAN_PRO_MES";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "PRVLG";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "CANT_INV";
            this.Column6.Name = "Column6";
            // 
            // Column11
            // 
            this.Column11.HeaderText = "STOCK ACTUAL";
            this.Column11.Name = "Column11";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "STK_CEDIS";
            this.Column7.Name = "Column7";
            // 
            // column8
            // 
            this.column8.HeaderText = "FECHA";
            this.column8.Name = "column8";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "SURTIDO";
            this.Column9.Name = "Column9";
            // 
            // Column10
            // 
            this.Column10.HeaderText = "FOLIO";
            this.Column10.Name = "Column10";
            // 
            // completo
            // 
            this.completo.HeaderText = "PEDIDO";
            this.completo.Name = "completo";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Analisis_Reabastecimiento.Properties.Resources.LOGO_HD24;
            this.pictureBox1.Location = new System.Drawing.Point(12, 34);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(262, 76);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // labelFecha
            // 
            this.labelFecha.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFecha.AutoSize = true;
            this.labelFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFecha.ForeColor = System.Drawing.Color.White;
            this.labelFecha.Location = new System.Drawing.Point(359, 95);
            this.labelFecha.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFecha.Name = "labelFecha";
            this.labelFecha.Size = new System.Drawing.Size(94, 15);
            this.labelFecha.TabIndex = 19;
            this.labelFecha.Text = "Fecha de Folios";
            this.labelFecha.Click += new System.EventHandler(this.fechaFinalLabel_Click);
            // 
            // dateTimePicker1Form2
            // 
            this.dateTimePicker1Form2.Location = new System.Drawing.Point(458, 95);
            this.dateTimePicker1Form2.Name = "dateTimePicker1Form2";
            this.dateTimePicker1Form2.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1Form2.TabIndex = 20;
            this.dateTimePicker1Form2.ValueChanged += new System.EventHandler(this.dateTimePicker1Form2_ValueChanged);
            // 
            // labelFolios
            // 
            this.labelFolios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFolios.AutoSize = true;
            this.labelFolios.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFolios.ForeColor = System.Drawing.Color.White;
            this.labelFolios.Location = new System.Drawing.Point(931, 96);
            this.labelFolios.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFolios.Name = "labelFolios";
            this.labelFolios.Size = new System.Drawing.Size(40, 15);
            this.labelFolios.TabIndex = 21;
            this.labelFolios.Text = "Folios";
            // 
            // comboBoxFolios
            // 
            this.comboBoxFolios.FormattingEnabled = true;
            this.comboBoxFolios.Location = new System.Drawing.Point(977, 94);
            this.comboBoxFolios.Name = "comboBoxFolios";
            this.comboBoxFolios.Size = new System.Drawing.Size(130, 21);
            this.comboBoxFolios.TabIndex = 22;
            this.comboBoxFolios.SelectedIndexChanged += new System.EventHandler(this.comboBoxFolios_SelectedIndexChanged);
            // 
            // buttonGuardar
            // 
            this.buttonGuardar.Location = new System.Drawing.Point(1256, 165);
            this.buttonGuardar.Name = "buttonGuardar";
            this.buttonGuardar.Size = new System.Drawing.Size(75, 23);
            this.buttonGuardar.TabIndex = 23;
            this.buttonGuardar.Text = "Guardar";
            this.buttonGuardar.UseVisualStyleBackColor = true;
            this.buttonGuardar.Click += new System.EventHandler(this.buttonGuardar_Click);
            // 
            // comboSucursal
            // 
            this.comboSucursal.FormattingEnabled = true;
            this.comboSucursal.Location = new System.Drawing.Point(759, 94);
            this.comboSucursal.Name = "comboSucursal";
            this.comboSucursal.Size = new System.Drawing.Size(130, 21);
            this.comboSucursal.TabIndex = 24;
            this.comboSucursal.SelectedIndexChanged += new System.EventHandler(this.comboSucursal_SelectedIndexChanged);
            // 
            // labelSucursal
            // 
            this.labelSucursal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSucursal.AutoSize = true;
            this.labelSucursal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSucursal.ForeColor = System.Drawing.Color.White;
            this.labelSucursal.Location = new System.Drawing.Point(700, 95);
            this.labelSucursal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSucursal.Name = "labelSucursal";
            this.labelSucursal.Size = new System.Drawing.Size(55, 15);
            this.labelSucursal.TabIndex = 25;
            this.labelSucursal.Text = "Sucursal";
            // 
            // bottonExcel
            // 
            this.bottonExcel.BackColor = System.Drawing.Color.DimGray;
            this.bottonExcel.Image = ((System.Drawing.Image)(resources.GetObject("bottonExcel.Image")));
            this.bottonExcel.Location = new System.Drawing.Point(1287, 89);
            this.bottonExcel.Name = "bottonExcel";
            this.bottonExcel.Size = new System.Drawing.Size(44, 37);
            this.bottonExcel.TabIndex = 49;
            this.bottonExcel.UseVisualStyleBackColor = false;
            this.bottonExcel.Click += new System.EventHandler(this.bottonExcel_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(58)))), ((int)(((byte)(117)))));
            this.ClientSize = new System.Drawing.Size(1343, 722);
            this.Controls.Add(this.bottonExcel);
            this.Controls.Add(this.labelSucursal);
            this.Controls.Add(this.comboSucursal);
            this.Controls.Add(this.buttonGuardar);
            this.Controls.Add(this.comboBoxFolios);
            this.Controls.Add(this.labelFolios);
            this.Controls.Add(this.dateTimePicker1Form2);
            this.Controls.Add(this.labelFecha);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dataGridView1From2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.Text = "Revision de Pedido";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1From2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1From2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelFecha;
        private System.Windows.Forms.DateTimePicker dateTimePicker1Form2;
        private System.Windows.Forms.Label labelFolios;
        private System.Windows.Forms.ComboBox comboBoxFolios;
        private System.Windows.Forms.Button buttonGuardar;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewCheckBoxColumn completo;
        private System.Windows.Forms.ComboBox comboSucursal;
        private System.Windows.Forms.Label labelSucursal;
        private System.Windows.Forms.Button bottonExcel;
    }
}