using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Ejer02
{
    public partial class Form1 : Form
    {
   
        ListaProducto coleccion = new ListaProducto();

        public Form1()
        {
            InitializeComponent();
        }

        bool ValidarDatos()
        {
           
            if (string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                MessageBox.Show("Ingresa el Codigo del Producto", "Registrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                 return false;
            }
            if (string.IsNullOrEmpty(txtNombres.Text.Trim()))
            {
                MessageBox.Show("Ingresa el nombre del Producto", "Registrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombres.Focus();
                return false;

            }
            if (Convert.ToDouble(txtPrecio.Text.Trim()) <= 0) 
            {
                MessageBox.Show("El Precio debe ser mayor a cero", "Registrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return false;
                
            }
            

            return true;
        }

        

        void Listar()
        {
            lvwProductos.Items.Clear();
            foreach (var p in coleccion.lista)
            {
                ListViewItem item = new ListViewItem(p.Codigo.ToString());
                item.SubItems.Add(p.Nombre.ToString());
                item.SubItems.Add(p.Marca.ToString());
                item.SubItems.Add(p.FechaRegistro.ToShortDateString());
                item.SubItems.Add(p.Precio.ToString());


                lvwProductos.Items.Add(item);
            }

        }

        private void btnAgragar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos()) 
            {
                Producto productCode = coleccion.lista.Find(prod => prod.Codigo == Convert.ToInt32(txtCodigo.Text));
                if (productCode != null) 
                {
                    MessageBox.Show("El codigo ya existe", "Añadir", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCodigo.Focus();

                }if(txtCodigo.Text.Length <= 5) 
                {
                    MessageBox.Show("El codigo debe tener 6 digitos", "Añadir", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCodigo.Focus();
                }
                else { 
                // Crear un objeto
                Producto p = new Producto();
                p.Codigo = Convert.ToInt32(txtCodigo.Text.Trim());
                p.Nombre = txtNombres.Text.Trim();
                p.Marca = cboMarca.SelectedItem.ToString();
                 p.FechaRegistro = dtpFechaRegistro.Value;
                p.Precio = Convert.ToDouble( txtPrecio.Text.Trim());
                

                coleccion.lista.Add(p);
                LimpiarDatos();
                Listar();
                }
            }
        }

        void LimpiarDatos()
        {
            txtCodigo.Clear();
            txtNombres.Clear();
            cboMarca.SelectedItem = 0;
            dtpFechaRegistro.Value = DateTime.Today;
            txtPrecio.Clear();
            txtCodigo.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                SaveFileDialog dialogo = new SaveFileDialog();
                dialogo.Filter = "Archivo XML (*.xml)|*.xml";
                dialogo.Title = "Generar XML";
                if (dialogo.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(dialogo.FileName))
                    {
                        FileStream fs = new FileStream(dialogo.FileName, FileMode.Create);
                        XmlSerializer xml = new XmlSerializer(typeof(ListaProducto));
                        xml.Serialize(fs, coleccion);
                        fs.Close();
                    }
                }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Filter = "Archivo XML (*.xml)|*.xml";
            dialogo.Title = "Cargar XML...";

            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(dialogo.FileName))
                {
                    FileStream fs = new FileStream(dialogo.FileName, FileMode.Open);
                    XmlSerializer xml = new XmlSerializer(typeof(ListaProducto));
                    // 'casteo': convertir un objeto a otro tipo
                    coleccion = (ListaProducto)xml.Deserialize(fs);
                    Listar();
                    LimpiarDatos();
                    
                }
            }
        }
        }
    }

