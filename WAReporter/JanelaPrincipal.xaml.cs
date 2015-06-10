using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace WAReporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class JanelaPrincipal : Window
    {
        private string CaminhoMsgStoreDb = "";
        private string CaminhoWaDb = "";
        
        public JanelaPrincipal()
        {
            InitializeComponent();
        }

        private void AbrirCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var janelaAbrirArquivo = new JanelaAbrirArquivo();
            janelaAbrirArquivo.SelecaoOk += delegate
            {
                CaminhoMsgStoreDb = janelaAbrirArquivo.arquivoTextBox.Text;
                CaminhoWaDb = janelaAbrirArquivo.waDbTextBox.Text;
                var resultadoCarregamento = Banco.CarregarBanco(CaminhoMsgStoreDb, CaminhoWaDb);
                if(resultadoCarregamento.StartsWith("ERRO"))
                {
                    MessageBox.Show(resultadoCarregamento);
                }
                else
                {
                    contatosDataGrid.ItemsSource = Banco.Chats;

                    Midia.Procurar(Directory.GetParent(System.IO.Path.GetDirectoryName(janelaAbrirArquivo.arquivoTextBox.Text)).FullName);

                }
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

        private void selecionarTodosButton_Click(object sender, RoutedEventArgs e)
        {
           foreach(var row in contatosDataGrid.Items)
            {
                
            }
        }

        private void gerarRelatorioButton_Click(object sender, RoutedEventArgs e)
        {
            var chats = Banco.Chats;
            var caminhoRelatorio = Path.Combine(Path.GetDirectoryName(CaminhoMsgStoreDb), "WhatsApp - Relatório.html");
            var resultado = GeradorRelatorio.GerarRelatorioHtml(chats, caminhoRelatorio);
        }
    }
}
