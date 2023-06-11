using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReadSqlLiteAndWebPass.SqlLitett
{
    internal class ObjFormat
    {
        public class log
        {
            public int id { get; set; }
            public string uid { get; set; }
            public string nickname { get; set; }
            public string ctype { get; set; }
            public string msg { get; set; }
            public string roomid { get; set; }
            public long addtime { get; set; }

        }
    }
}
