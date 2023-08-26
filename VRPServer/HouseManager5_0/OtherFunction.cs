using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using static HouseManager5_0.Data;

namespace HouseManager5_0
{
    class OtherFunction
    {
        internal static void calMercator()
        {
            //Consol.WriteLine("输入地理经纬度坐标，如 112,37");
            var content = Console.ReadLine();
            var lon = double.Parse(content.Split(',')[0]);
            var lat = double.Parse(content.Split(',')[1]);
            double x, y, z;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(lon, lat, 0, out x, out y, out z);
            Console.WriteLine($"x:{x},y:{y}");
            Console.WriteLine($"E退出，任意键继续");
            if (Console.ReadLine().ToUpper() == "E")
            {

            }
            else
            {
                calMercator();
            }

        }
        internal static void readConnectInfomation()
        {
            Console.WriteLine("输入密文");
            var content = Console.ReadLine();
            Console.WriteLine("输入密钥");
            var secretValue = Console.ReadLine();
            var result = CommonClass.AES.AesDecrypt(content, secretValue);
            Console.WriteLine($"{result}");
            Console.ReadLine();
        }

        class ObjInput
        {
            public string addr { get; set; }
            public string time { get; set; }
            public List<string> list { get; set; }
        }
        class ObjOutput
        {
            public string c { get; set; }
            public string time { get; set; }
            public List<string> list { get; set; }
        }
        internal static void sign()
        {
            while (true)
            {

                Console.WriteLine("输入密钥或exit");
                var privateKey = Console.ReadLine();
                if (privateKey == "exit") 
                {
                    break;
                }


                Console.WriteLine("拖入要加密内容路径");
                var path = Console.ReadLine();
                var content = File.ReadAllText(path);
                var oI = Newtonsoft.Json.JsonConvert.DeserializeObject<ObjInput>(content);
                ObjOutput oo = new ObjOutput()
                {
                    c = "AwardsGiving",
                    time = oI.time,
                    list = new List<string>()
                };
                for (int i = 0; i < oI.list.Count; i++)
                {
                    var msg = oI.list[i];
                    char[] splitOp = { '@', '-', '>', ':' };
                    StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries;
                    var addr = msg.Split(splitOp, options)[1];
                    var sign = BitCoin.Sign.SignMessage(privateKey, msg, addr);
                    oo.list.Add(sign);
                }
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(oo));
                //BitCoin.Sign.SignMessage()
                // var jsonObj = "";
                //  throw new NotImplementedException();
            }
        }

        internal static void writeToAliyun()
        {
            {
                var material = new Dictionary<string, CommonClass.databaseModel.abtractmodelsPassData>();

                var list = DalOfAddress.AbtractModels.GetCategeAm1();
                for (int i = 0; i < list.Count; i += 2)
                {
                    var obj = DalOfAddress.AbtractModels.GetAbtractModelItem(list[i].Trim());
                    {
                        string base64Image = obj.imageBase64;
                        string base64Data = base64Image.Substring(base64Image.IndexOf(',') + 1);
                        byte[] imageBytes = Convert.FromBase64String(base64Data);
                        Aliyun.ByteData.Add($"objmodel/{list[i]}.jpg", imageBytes);
                    }
                    {

                        var returnObj = new
                        {
                            objText = obj.objText,
                            mtlText = obj.mtlText,
                            imgBase64 = "",
                            AmID = list[i],
                            modelType = obj.modelType
                        };
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(returnObj);
                        Aliyun.Json.Add($"objmodel/{list[i]}.json", json);
                    }
                }
            }
            {
                var AllCrossesBGData = DalOfAddress.backgroundjpg.GetAllKey();

                foreach (var item in AllCrossesBGData)
                {
                    //  var key = item.Value;
                    var jpgValue = DalOfAddress.backgroundjpg.GetItemDetail(item.Key);
                    if (jpgValue != null)
                        foreach (var directionItem in jpgValue)
                        {
                            string base64Image = directionItem.Value;
                            string base64Data = base64Image.Substring(base64Image.IndexOf(',') + 1);
                            byte[] imageBytes = Convert.FromBase64String(base64Data);
                            Aliyun.ByteData.Add($"bgimg/{item.Value}/{directionItem.Key}.jpg", imageBytes);
                        }
                }
            }
            {
                var AllFPBGData = DalOfAddress.fpbackground.GetAllKey();

                foreach (var item in AllFPBGData)
                {
                    //  var key = item.Value;
                    var jpgValue = DalOfAddress.fpbackground.GetItemDetail(item.Key);
                    if (jpgValue != null)
                        foreach (var directionItem in jpgValue)
                        {
                            string base64Image = directionItem.Value;
                            string base64Data = base64Image.Substring(base64Image.IndexOf(',') + 1);
                            byte[] imageBytes = Convert.FromBase64String(base64Data);
                            Aliyun.ByteData.Add($"fpbgimg/{item.Key}/{directionItem.Key}.jpg", imageBytes);
                        }
                }
            }
        }
    }
}
