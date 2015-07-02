using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

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
            dlg.Filter = "All files (*.*)|*.*";

            var result = dlg.ShowDialog();
            
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                arquivoTextBox.Text = dlg.FileName;

                //var pathWaDb = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(dlg.FileName), "wa.db");
                //if (File.Exists(pathWaDb) && String.IsNullOrWhiteSpace(waDbTextBox.Text))
                //    waDbTextBox.Text = pathWaDb;
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
            //chave = 126 a 157
            //< v2.12.38
            //iv = 110 a 125
                
            //se > v2.12.38
            //    iv = 51 a 66

            //ambos strip 67 bytes do crypt8


            byte[] key = File.ReadAllBytes(crypt8TextBox.Text).Skip(125).Take(32).ToArray();
            byte[] iv = File.ReadAllBytes(crypt8TextBox.Text).Skip(110).Take(16).ToArray();
            if (iv[0] == 0)
                iv = File.ReadAllBytes(arquivoTextBox.Text).Skip(50).Take(16).ToArray();
            var keyString = BitConverter.ToString(key).Replace("-","");
            var ivString = BitConverter.ToString(iv).Replace("-", "");


            var startInfo = new ProcessStartInfo();
            startInfo.Arguments = "enc -aes-256-cbc -d -nosalt -bufsize 16384 -in \""+arquivoTextBox.Text+ "\" -out \"" + arquivoTextBox.Text.Replace("db.crypt", "db") + "\" -K "+keyString+" -iv "+ivString;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            var process = new Process();
            process.StartInfo = startInfo;
            startInfo.FileName = "openssl.exe";
            process.Start();
            process.WaitForExit();
                
            SelecaoOk(null, null);
            this.Close();
        
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void crypt8Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".crypt8";
            dlg.Filter = "All files (*.*)|*.*";

            var result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                crypt8TextBox.Text = dlg.FileName;

                //var pathWaDb = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(dlg.FileName), "wa.db");
                //if (File.Exists(pathWaDb) && String.IsNullOrWhiteSpace(waDbTextBox.Text))
                //    waDbTextBox.Text = pathWaDb;
            }
        }

        private void crypt7Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".crypt7";
            dlg.Filter = "All files (*.*)|*.*";

            var result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                crypt7TextBox.Text = dlg.FileName;

                //var pathWaDb = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(dlg.FileName), "wa.db");
                //if (File.Exists(pathWaDb) && String.IsNullOrWhiteSpace(waDbTextBox.Text))
                //    waDbTextBox.Text = pathWaDb;
            }
        }
    }
}
