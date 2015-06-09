using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAReporter.Modelo
{
    public class GroupParticipantHistory
    {
        //Esses campos correspondem aos existentes na tabela group_participants_history dos bancos msgstore.db

        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public String GJid { get; set; }
        public String Jid { get; set; }
        public int Action { get; set; }
        public String OldPhash { get; set; }
        public String NewPhash { get; set; }
    }
}
