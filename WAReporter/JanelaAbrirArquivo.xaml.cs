﻿using System;
using System.Collections.Generic;
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

namespace WAReporter
{
    /// <summary>
    /// Interaction logic for JanelaAbrirArquivo.xaml
    /// </summary>
    
    public partial class JanelaAbrirArquivo : Window
    {
        public event EventHandler SelecaoOk;

        public JanelaAbrirArquivo()
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
            dlg.DefaultExt = ".db";
            dlg.Filter = "Bancos de dados SQLite | *.db";

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


            if (File.Exists(arquivoTextBox.Text) && arquivoTextBox.Text.EndsWith("db"))
            {
                if (!String.IsNullOrWhiteSpace(waDbTextBox.Text) && !File.Exists(waDbTextBox.Text))
                {
                    MessageBox.Show("Arquivo \"" + waDbTextBox.Text + "\" não encontrado. Insira um endereço válido ou mantenha o endereço em branco.");
                    waDbTextBox.Focus();
                    return;
                }

                SelecaoOk(null, null);
                this.Close();
            } else
                MessageBox.Show("Arquivo \"" + arquivoTextBox.Text + "\" não encontrado.");

            arquivoButton.Focus();
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
