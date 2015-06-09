using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAReporter.Modelo
{
    public class MediaRef
    {
        //Esses campos correspondem aos existentes na tabela media_refs dos bancos msgstore.db

        public int Id { get; set; }
        public String Path { get; set; }
        public int RefCount { get; set; }
    }
}
