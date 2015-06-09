using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAReporter.Modelo
{
    public class GroupParticipant
    {
        //Esses campos correspondem aos existentes na tabela group_participants dos bancos msgstore.db

        public int Id { get; set; }
        public String Gjid { get; set; }
        public String Jid { get; set; }
        public int Admin { get; set; }
        public int Pending { get; set; }
    }
}
