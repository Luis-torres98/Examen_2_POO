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

namespace Examen02
{
    public partial class Form1 : Form
    {

        // Declarar la lista de participantes
        List<TRABAJADOR> Trabajador = new List<TRABAJADOR>();
        public Form1()
        {
            InitializeComponent();
            GenerarCodigo();
        }
        void GenerarCodigo()
        {
            // Genera código del trabajador: TRA001
            int cod = 0;
            if (Trabajador.Count != 0)
            {
                // Obtener el código del último participante
                string ult = Trabajador[Trabajador.Count - 1].Codigo;
                
                cod = Convert.ToInt32(ult.Substring(3, 3));
            }
            cod++;
            txtCodigo.Text = "TRA" + cod.ToString().PadLeft(3, '0');
        }

        bool ValidarDatos()
        {
            if (string.IsNullOrEmpty(txtNombres.Text.Trim()))
            {
                MessageBox.Show("Ingresa el nombre del trabajador", "Registrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombres.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtApellidos.Text.Trim()))
            {
                MessageBox.Show("Ingresa el apellido del trabajador", "Registrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellidos.Focus();
                return false;
            }
            
            return true;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                // Crear un objeto
                TRABAJADOR p = new TRABAJADOR();
                p.Codigo = txtCodigo.Text.Trim();
                p.Nombre = txtNombres.Text.Trim();
                p.Apellido = txtApellidos.Text.Trim();
                p.Turno = cboTurno.SelectedIndex;

                if (p.Turno == 0)
                {
                    p.Sueldo = 1500.00;
                }
                else if (p.Turno == 1) 
                {
                    p.Sueldo = 1700.00;
                }else
                    p.Sueldo = 1900.00;
                


                Trabajador.Add(p);
                LimpiarDatos();
                Listar();
                GenerarCodigo();
            }
        }

        void LimpiarDatos()
        {
            txtNombres.Clear();
            txtApellidos.Clear();
            txtNombres.Focus();
        }

        void Listar()
        {
            lvwTrabajador.Items.Clear();
            foreach (var p in Trabajador)
            {
                ListViewItem item = new ListViewItem(p.Codigo);
                item.SubItems.Add(p.Nombre.ToString());
                item.SubItems.Add(p.Apellido.ToString());
                item.SubItems.Add(cboTurno.Text);
                item.SubItems.Add(p.Sueldo.ToString());


                lvwTrabajador.Items.Add(item);
            }

            
        }

        string ObtenerContenido()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var p in Trabajador)
            {
                sb.AppendLine(p.Codigo + ";" +
                             p.Nombre.ToString() + ";" +
                             p.Apellido.ToString() + ";" +
                             cboTurno.Text + ";" +
                             p.Sueldo.ToString() + ";");
                
            }
            return sb.ToString();
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            string contenido = ObtenerContenido();
            if (string.IsNullOrEmpty(contenido))
            {
                MessageBox.Show("No existen datos para exportar", "Exportar TXT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                
                String ruta = "C:\\Users\\Harold MV\\Desktop\\Ciclo III\\POO\\Examen 2\\Reportes\\Reporte.txt";

                StreamWriter escritor = new StreamWriter(ruta);
                escritor.Write(contenido);
                escritor.Close();
                MessageBox.Show("Se generó el archivo en " + ruta, "Exportar TXT", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    
}
