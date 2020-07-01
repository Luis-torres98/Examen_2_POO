using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO.Compression;
using System.IO;


namespace Ejer03
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[] key = { 60, 61, 62, 63, 64, 65, 66, 67 };
        byte[] vi = { 70, 71, 72, 73, 74, 75, 76, 77 };
        DESCryptoServiceProvider serviceProvider = new DESCryptoServiceProvider();

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtEnunciado.Clear();
            txtEnunciado.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo comprimido (.zip)|.zip",
                Title = "Comprimir"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    StreamWriter streamWriter = new StreamWriter(memoryStream);
                    streamWriter.Write(txtEnunciado.Text);
                    streamWriter.Flush();

                    FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
                    GZipStream gZipStream = new GZipStream(fileStream, CompressionMode.Compress);
                    gZipStream.Write(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);

                    gZipStream.Close();
                    fileStream.Close();

                    MessageBox.Show("El archivo se comprimió correctamente", "Comprimir", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnEncriptar_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo encriptado (.txt)|.txt"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] data = Encoding.UTF8.GetBytes(txtEnunciado.Text);

                FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);

                CryptoStream cryptoStream = new CryptoStream(fileStream, serviceProvider.CreateEncryptor(key, vi), CryptoStreamMode.Write);

                cryptoStream.Write(data, 0, data.Length);

                cryptoStream.Close();
                fileStream.Close();

                MessageBox.Show("El archivo se encriptó correctamente", "Encriptar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnDesencriptar_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Archivo de Texto (*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileStream file = new FileStream(dialog.FileName, FileMode.Open);

                CryptoStream crypto = new CryptoStream(file, serviceProvider.CreateDecryptor(key, vi), CryptoStreamMode.Read);

                byte[] data = new byte[4096];
                crypto.Read(data, 0, data.Length);

                MemoryStream memory = new MemoryStream();
                memory.Write(data, 0, data.Length);
                memory.Position = 0;
                txtEnunciado.Text = new StreamReader(memory).ReadToEnd();
                crypto.Close();
                file.Close();
            }
        }
    }
}
