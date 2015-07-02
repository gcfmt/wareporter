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

            //foreach (var arquivoAvatar in Directory.GetFiles(@"I:\whatsApp_private\profpics"))
            //    if (!File.Exists(arquivoAvatar + ".jpg"))
            //        File.Copy(arquivoAvatar, arquivoAvatar + ".jpg");
        }

        private void AbrirAndroidCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var janelaAbrirArquivoAndroid = new JanelaAbrirArquivoAndroid();
            janelaAbrirArquivoAndroid.SelecaoOk += delegate
            {
                CaminhoMsgStoreDb = janelaAbrirArquivoAndroid.arquivoTextBox.Text.Replace(".db.crypt", ".db");
                CaminhoWaDb = janelaAbrirArquivoAndroid.waDbTextBox.Text;
                var resultadoCarregamento = Banco.CarregarBancoAndroid(CaminhoMsgStoreDb, CaminhoWaDb);
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

                    Midia.Procurar(Directory.GetParent(System.IO.Path.GetDirectoryName(janelaAbrirArquivoAndroid.arquivoTextBox.Text)).FullName);

                }
            };
            janelaAbrirArquivoAndroid.ShowDialog();
        }

        private void AbrirIPhoneCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var janelaAbrirArquivoIPhone = new JanelaAbrirArquivoIPhone();
            janelaAbrirArquivoIPhone.SelecaoOk += delegate
            {
                CaminhoMsgStoreDb = janelaAbrirArquivoIPhone.arquivoTextBox.Text.Replace(".db.crypt", ".db");
                CaminhoWaDb = janelaAbrirArquivoIPhone.contactsSqliteTextBox.Text;
                var resultadoCarregamento = Banco.CarregarBancoIPhone(CaminhoMsgStoreDb, CaminhoWaDb);
                if (resultadoCarregamento.StartsWith("ERRO"))
                {
                    MessageBox.Show(resultadoCarregamento);
                }
                else
                {
                    ItensDataGrid = new List<DataGridItem>();
                    foreach (var chat in Banco.Chats)
                        ItensDataGrid.Add(new DataGridItem
                        {
                            ChatItem = chat,
                            NomeContato = chat.Contato.NomeContato + chat.Subject,
                            UltimaMensagem = chat.Mensagens.Any() ? chat.Mensagens.Max(p => p.Timestamp).ToString() : "",
                            IsSelecionado = false
                        });


                    contatosDataGrid.ItemsSource = ItensDataGrid;

                    Midia.Procurar(Directory.GetParent(System.IO.Path.GetDirectoryName(janelaAbrirArquivoIPhone.arquivoTextBox.Text)).FullName);

                }
            };
            janelaAbrirArquivoIPhone.ShowDialog();
        }


        private void ExtrairCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var janelaExtrairCriptografia = new JanelaExtrairCriptografia();
            janelaExtrairCriptografia.SelecaoOk += delegate
            {
                CaminhoMsgStoreDb = janelaExtrairCriptografia.arquivoTextBox.Text.Replace(".db.crypt", ".db");
                //CaminhoWaDb = janelaExtrairCriptografia.waDbTextBox.Text;
                var resultadoCarregamento = Banco.CarregarBancoAndroid(CaminhoMsgStoreDb, CaminhoWaDb);
                if (resultadoCarregamento.StartsWith("ERRO"))
                {
                    MessageBox.Show(resultadoCarregamento);
                }
                else
                {
                    ItensDataGrid = new List<DataGridItem>();
                    foreach (var chat in Banco.Chats)
                        ItensDataGrid.Add(new DataGridItem
                        {
                            ChatItem = chat,
                            NomeContato = chat.Contato.NomeContato + chat.Subject,
                            UltimaMensagem = chat.Mensagens.Any() ? chat.Mensagens.Max(p => p.Timestamp).ToString() : "",
                            IsSelecionado = false
                        });


                    contatosDataGrid.ItemsSource = ItensDataGrid;

                    Midia.Procurar(Directory.GetParent(System.IO.Path.GetDirectoryName(janelaExtrairCriptografia.arquivoTextBox.Text)).FullName);

                }
            };
            janelaExtrairCriptografia.ShowDialog();
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
