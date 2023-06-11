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
            // public int ID { get; set; }
            public string Uid { get; set; }
            public string Nickname { get; set; }
            public string Action { get; set; }
            public string ActionType {
                get {

                    if (this.Action.Contains(":"))
                    {
                        var ss = this.Action.Split(':');
                        if (ss.Length > 0)
                        {
                            return ss[0];
                        }
                    }
                    return this.Action;
                }
            }
            // public string roomid { get; set; }
            public long Addtime { get; set; }

            public int PriceValue { get; set; }

        }
    }
}
