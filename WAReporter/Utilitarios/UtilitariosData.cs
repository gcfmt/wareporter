using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAReporter.Utilitarios
{
    public static class UtilitariosData
    {
        public static DateTime TimeStampParaDateTime(this Int64 timestampSqlite)
        {

            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(timestampSqlite / 1000d)).ToLocalTime();
        }
    }
}
