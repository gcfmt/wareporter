using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAReporter.Modelo
{
    public class Chat
    {
        //Esses campos correspondem aos existentes na tabela chat_list dos bancos msgstore.db

        public int Id { get; set; }
        public String KeyRemoteJid { get; set; }
        public int? MessageTableId { get; set; }
        public String Subject { get; set; }
        public DateTime? Creation { get; set; }
        public int? LastReadMessageTableId { get; set; }
        public int? LastReadReceiptSentMessageTableId { get; set; }
        public int? Archived { get; set; }
        public int? SortTimestamp { get; set; }
        public int? ModTag { get; set; }

        public WaContact Contato { get; set; }
        public List<Message> Mensagens { get; set; }
        public List<GroupParticipant> ParticipantesGrupo { get; set; }


    }
}
