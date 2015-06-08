using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WAReporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Data.SQLite.SQLiteConnection BancoAtual;
        
        public MainWindow()
        {
            InitializeComponent();



        }

        private void AbrirCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var janelaAbrirArquivo = new JanelaAbrirArquivo();
            janelaAbrirArquivo.SelecaoOk += delegate
            {
                BancoAtual = new SQLiteConnection("Data Source="+janelaAbrirArquivo.arquivoTextBox.Text+ ";Version=3;");
                BancoAtual.Open();
                var command = new SQLiteCommand("select * from chat_list;", BancoAtual);
                var reader = command.ExecuteReader();
                while(reader.Read())
                {
                    var tx = reader.GetString(1);
                }
                BancoAtual.Close();

            };
            janelaAbrirArquivo.ShowDialog();
        }

        private void ExtrairCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void SairCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
