using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WAReporter.Utilitarios;

namespace WAReporter
{
    /// <summary>
    /// Interaction logic for JanelaAbrirArquivo.xaml
    /// </summary>
    
    public partial class JanelaExtrairCriptografia : Window
    {
        public event EventHandler SelecaoOk;

        public JanelaExtrairCriptografia()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            arquivoButton.Focus();
        }

        private void arquivoButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".crypt";
            dlg.Filter = "Bancos de dados Criptografados|*.crypt|*.crypt5|*.crypt7|*.crypt8";

            var result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                arquivoTextBox.Text = dlg.FileName;

                var pathWaDb = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(dlg.FileName), "wa.db");
                if (File.Exists(pathWaDb) && String.IsNullOrWhiteSpace(waDbTextBox.Text))
                    waDbTextBox.Text = pathWaDb;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrWhiteSpace(arquivoTextBox.Text))
            {
                MessageBox.Show("É necessário preencher um nome de arquivo de banco de dados.");
                arquivoButton.Focus();
                return;
            }


            //if (File.Exists(arquivoTextBox.Text) && arquivoTextBox.Text.EndsWith("crypt"))
            //{
            //    crypt7TextBox.Text = crypt8TextBox.Text = contaTextBox.Text = "";
            //    crypt7TextBox.IsEnabled = crypt8TextBox.IsEnabled = contaTextBox.IsEnabled = false;
            //} else if (File.Exists(arquivoTextBox.Text) && arquivoTextBox.Text.EndsWith("crypt5"))
            //{
            //    crypt7TextBox.Text = crypt8TextBox.Text = "";
            //    crypt7TextBox.IsEnabled = crypt8TextBox.IsEnabled = false;
            //    contaTextBox.IsEnabled = true;
            //} else if (File.Exists(arquivoTextBox.Text) && arquivoTextBox.Text.EndsWith("crypt5"))
            //{
            //    crypt7TextBox.Text = crypt8TextBox.Text = "";
            //    crypt7TextBox.IsEnabled = crypt8TextBox.IsEnabled = false;
            //    contaTextBox.IsEnabled = true;
            //}


            //var arquivoTextBox.Text;
            

            byte[] test = File.ReadAllBytes(crypt8TextBox.Text).Skip(125).Take(32).ToArray();
            byte[] iv = File.ReadAllBytes(crypt8TextBox.Text).Skip(109).Take(16).ToArray();



            var startInfo = new ProcessStartInfo();
                startInfo.Arguments = "enc -d -aes-192-ecb -in \""+ arquivoTextBox.Text +"\" -out \""+ arquivoTextBox.Text.Replace("db.crypt", "db") + "\" -K 346a23652a46392b4d73257c67317e352e3372482177652c -iv 1";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                var process = new Process();
                process.StartInfo = startInfo;
                startInfo.FileName = "C:\\openssl.exe";
                process.Start();
                process.WaitForExit();
                
                SelecaoOk(null, null);
                this.Close();
            }

            else
                MessageBox.Show("Arquivo \"" + arquivoTextBox.Text + "\" não encontrado.");

            arquivoButton.Focus();
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
