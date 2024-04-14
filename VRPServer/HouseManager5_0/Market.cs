using BitCoin;
using CommonClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HouseManager5_0
{
    public class Market
    {
        PriceChanged _priceChanged;

        public long? mile_Price { get; private set; }
        public long? business_Price { get; private set; }
        public long? volume_Price { get; private set; }
        public long? speed_Price { get; private set; }
        //   else if (!(sp.pType == "mile" || sp.pType == "business" || sp.pType == "volume" || sp.pType == "speed"))
        //{
        //    return $"wrong pType:{sp.pType}"; ;
        //}
        string IP { get; set; }
        int port { get; set; }
        public string TradingCenterAddr { get; private set; }
        string PrivateKey { get; set; }

        public long? satoshi_score_price { get; private set; }
        public long SatoshiScorePrice
        {
            get
            {
                if (this.satoshi_score_price == null)
                    return 1;
                else return this.satoshi_score_price.Value;
            }
        }

        public Market(PriceChanged priceChanged)
        {
            this.mile_Price = null;
            this.business_Price = null;
            this.volume_Price = null;
            this.speed_Price = null;
            this._priceChanged = priceChanged;
            var rootPath = System.IO.Directory.GetCurrentDirectory();
            //Consol.WriteLine($"path:{rootPath}");

            if (File.Exists($"{rootPath}\\config\\MarketIP.txt"))
            {
                var text = File.ReadAllText($"{rootPath}\\config\\MarketIP.txt");
                var ipAndPort = text.Split(':');
                this.IP = ipAndPort[0];
                this.port = int.Parse(ipAndPort[1]);
            }
            else
            {
                Console.WriteLine($"请market输入IP");
                this.IP = Console.ReadLine();
                Console.WriteLine("请market输入端口");
                this.port = int.Parse(Console.ReadLine());
                var text = $"{this.IP}:{this.port}";
                File.WriteAllText($"{rootPath}\\config\\MarketIP.txt", text);
            }
            this.TradingCenterAddr = "**";
            if (File.Exists($"{rootPath}\\config\\StockAddrPrivate.secr"))
            {
                string privateKeyResult;
                ECCMain.Deciphering.Decrypt($"{rootPath}\\config\\StockAddrPrivate.secr", DalOfAddress.Connection.PasswordStr, out privateKeyResult);
                // var PrivateKeyContent = File.ReadAllText($"{rootPath}\\config\\StockAddrPrivate.txt");
                // CommonClass.AES.AesDecrypt(content, password);
                //  this.PrivateKey = CommonClass.AES.AesDecrypt(PrivateKeyContent, DalOfAddress.Connection.PasswordStr);
                this.PrivateKey = privateKeyResult;
                System.Numerics.BigInteger privateValue;
                if (BitCoin.PrivateKeyF.Check(this.PrivateKey, out privateValue))
                {
                    this.TradingCenterAddr = PublicKeyF.GetAddressOfP2SH(Calculate.getPublicByPrivate(privateValue));
                    Console.WriteLine($"股份交易地址为:{this.TradingCenterAddr}");
                }
                else
                {
                    throw new Exception("股份交易私钥加载错误！");
                }
            }
            else
            {
                this.PrivateKey = "**";
                this.TradingCenterAddr = "**";
            }
        }

        internal void Update(MarketPrice sa)
        {
            switch (sa.sellType)
            {
                case "mile":
                    {
                        this.mile_Price = sa.price;
                        LogClass.Class1.Add($"收到{sa.sellType}市场价:{sa.price}");
                        this._priceChanged(sa.sellType, this.mile_Price.Value);
                    }; break;
                case "business":
                    {
                        this.business_Price = sa.price;
                        LogClass.Class1.Add($"收到{sa.sellType}市场价:{sa.price}");
                        this._priceChanged(sa.sellType, this.business_Price.Value);
                    }; break;
                case "volume":
                    {
                        this.volume_Price = sa.price;
                        LogClass.Class1.Add($"收到{sa.sellType}市场价:{sa.price}");
                        this._priceChanged(sa.sellType, this.volume_Price.Value);
                    }; break;
                case "speed":
                    {
                        this.speed_Price = sa.price;
                        LogClass.Class1.Add($"收到{sa.sellType}市场价:{sa.price}");
                        this._priceChanged(sa.sellType, this.speed_Price.Value);
                    }; break;
                case "stock":
                    {
                        this.satoshi_score_price = sa.price;
                    }; break;
            }
            // throw new NotImplementedException();
        }

        ///// <summary>
        ///// 玩家卖，市场收
        ///// </summary>
        ///// <param name="pType"></param>
        ///// <param name="v"></param>
        //internal void Receive(string pType, int v)
        //{

        //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(new MarketIn()
        //    {
        //        c = "MarketIn",
        //        pType = pType,
        //        count = v
        //    });
        //    Startup.sendSingleMsg($"{this.IP}:{this.port}", json);

        //}
        public delegate void sendMsgF(string ipAndPort, string json);

        /// <summary>
        /// 玩家买，市场发
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="v"></param>
        internal void Send(string pType, int v)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new MarketOut()
            {
                c = "MarketOut",
                pType = pType,
                count = v
            });
            Startup.sendSingleMsg($"{this.IP}:{this.port}", json);
        }


        //internal void Send(string pType, int v)
        //{
        //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(new MarketOut()
        //    {
        //        c = "MarketOut",
        //        pType = pType,
        //        count = v
        //    });
        //    Startup.sendMsg($"{this.IP}:{this.port}", json);
        //}
        internal void Buy(BuyDiamondInMarket bd)
        {
            //switch (bd.buyType)
            //{
            //    case "mile":
            //        {
            //            if (this.mile_Price != null) 
            //            {

            //            }
            //            //this.mile_Price = sa.price;
            //            //this._priceChanged(sa.sellType, this.mile_Price.Value);
            //        }; break;
            //    case "business":
            //        {
            //            this.business_Price = sa.price;
            //            this._priceChanged(sa.sellType, this.business_Price.Value);
            //        }; break;
            //    case "volume":
            //        {
            //            this.volume_Price = sa.price;
            //            this._priceChanged(sa.sellType, this.volume_Price.Value);
            //        }; break;
            //    case "speed":
            //        {
            //            this.speed_Price = sa.price;
            //            this._priceChanged(sa.sellType, this.speed_Price.Value);
            //        }; break;
            //}
            // throw new NotImplementedException();
        }

        internal string Send(ModelTranstraction.GetTransctionFromChain gtfc)
        {
            var data = Program.dt.GetDataOfOriginalStock(gtfc.bussinessAddr);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return json;
        }
    }

    public delegate void PriceChanged(string priceType, long value);
}
