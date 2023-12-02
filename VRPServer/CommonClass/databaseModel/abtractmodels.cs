using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass.databaseModel
{
    public class abtractmodels
    {
        public string modelName { get; set; }
        public string modelType { get; set; }
        //public string imageBase64 { get; set; }
        //public string objText { get; set; }
        //public string mtlText { get; set; }
        //public string animation { get; set; }
        public string amID { get; set; }
        public string author { get; set; }
        public int amState { get; set; }
    }
    public class abtractmodelsPassData
    {
        public string mtlText
        {
            get
            {
                //var result = "";
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < this.mtlTexts.Length; i++)
                {
                    sb.Append(this.mtlTexts[i]);

                    //result += mtlTexts[i];
                    if (i != this.mtlTexts.Length - 1)
                    {
                        sb.Append(Environment.NewLine);
                        //  result += Environment.NewLine;
                    }
                }
                return sb.ToString();
                //  return result;
            }
        }

        public string objText
        {
            get
            {
                //var result = "";
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < this.objTexts.Length; i++)
                {
                    sb.Append(this.objTexts[i]);
                    //     result += objTexts[i];
                    if (i != this.objTexts.Length - 1)
                    {
                        sb.Append(Environment.NewLine);
                        //  result += Environment.NewLine;
                    }
                }
                return sb.ToString();
            }
        }

        public string imageBase64
        {
            get
            {
                //  var result = "";
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < imgBase64.Length; i++)
                {
                    sb.Append(imgBase64[i]);
                    //result += imgBase64[i];
                }
                return sb.ToString();
                // return result;
            }
        }

        public string modelName { get; set; }
        public string modelType { get; set; }
        //public string imageBase64 { get; set; }
        public string[] objTexts { get; set; }
        public string[] mtlTexts { get; set; }
        public string[] imgBase64 { get; set; }
        // public string animation { get; set; }
    }
}
