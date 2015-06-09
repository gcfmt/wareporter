using System.Data.SQLite;
using System.Windows;
using System.Windows.Input;

namespace WAReporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class JanelaPrincipal : Window
    {
        
        public JanelaPrincipal()
        {
            InitializeComponent();



        }

        private void AbrirCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var janelaAbrirArquivo = new JanelaAbrirArquivo();
            janelaAbrirArquivo.SelecaoOk += delegate
            {
                var resultadoCarregamento = Banco.CarregarBanco(janelaAbrirArquivo.arquivoTextBox.Text, janelaAbrirArquivo.waDbTextBox.Text);
               

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
