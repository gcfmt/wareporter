using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WAReporter.Modelo;

namespace WAReporter
{
    public partial class JanelaPrincipal : Window
    {
        private string CaminhoMsgStoreDb = "";
        private string CaminhoWaDb = "";
        private List<DataGridItem> ItensDataGrid { get; set; }
        
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
                    ItensDataGrid = new List<DataGridItem>();
                    foreach (var chat in Banco.Chats) ItensDataGrid.Add(new DataGridItem {
                        ChatItem = chat,
                        NomeContato = chat.Contato.NomeContato + chat.Subject,
                        UltimaMensagem = chat.Mensagens.Any() ? chat.Mensagens.Max(p => p.Timestamp).ToString() : "",
                        IsSelecionado = false });


                    contatosDataGrid.ItemsSource = ItensDataGrid;

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
            foreach(var itemDataGrid in ItensDataGrid)
                itemDataGrid.IsSelecionado = true;
        }

        private void gerarRelatorioButton_Click(object sender, RoutedEventArgs e)
        {
            var itensSelecionados = ItensDataGrid.Where(p => p.IsSelecionado);

            if(!itensSelecionados.Any())
            {
                MessageBox.Show("Nenhum chat selecionado.");
                return;
            }
            
            var caminhoRelatorio = Path.Combine(Path.GetDirectoryName(CaminhoMsgStoreDb), "WhatsApp - Relatório.html");
            var resultado = GeradorRelatorio.GerarRelatorioHtml(itensSelecionados.Select(p => p.ChatItem).ToList(), caminhoRelatorio);
        }

        private void selecionarNenhumButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var itemDataGrid in ItensDataGrid)
                itemDataGrid.IsSelecionado = false;
        }
    }

    public class DataGridItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Chat ChatItem { get; set; }
        private bool isSelecionado;

        public bool IsSelecionado 
        {
            get { return isSelecionado; }
            set
            {
                isSelecionado = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSelecionado"));
            }
        }


        public String NomeContato { get; set; }
        public String UltimaMensagem { get; set; }
    }
}
