using BitCoin;
using System.Net;
using System.Text.RegularExpressions;

namespace BitcoinInputValueThenSign
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("please input secrecKey of AES");
            var aesKey = Console.ReadLine();
            Console.WriteLine("please drag the filePath to here!");
            var filePath = Console.ReadLine();
            var content = File.ReadAllText(filePath);
            var privateKeyString = CommonClass.AES.AesDecrypt(content,aesKey);
            var wifs = privateKeyString.Split(",");

            int selectIndex = 0;
            for (int i = 0; i < wifs.Length; i++)
            {
                /*
                 * be careful;notice not to show privateKeys!
                 */
                System.Numerics.BigInteger privateBiginterger;
                BitCoin.PrivateKeyF.Check(wifs[i], out privateBiginterger);

                var address = PublicKeyF.GetAddressOfcompressed(Calculate.getPublicByPrivate(privateBiginterger));

                Console.WriteLine($"{i}--{address}");

                //  privateKeyIsRight = address == ri.RewardBtcAddr;
            }


            var regex = new Regex("^[0-9a-f]{32}$");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == null)
                {
                    continue;
                }
                else if (input == "exit")
                {
                    break;
                }
                else if (regex.IsMatch(input))
                {
                    var priveteKey = wifs[selectIndex];
                    System.Numerics.BigInteger privateBiginterger;
                    BitCoin.PrivateKeyF.Check(wifs[selectIndex], out privateBiginterger);
                    var address = PublicKeyF.GetAddressOfcompressed(Calculate.getPublicByPrivate(privateBiginterger));
                    var sigh = BitCoin.Sign.SignMessage(priveteKey, input, address);
                    Console.WriteLine($"{selectIndex}");
                    Console.WriteLine($"{address}");
                    Console.WriteLine($"{sigh}");
                }
                else
                {
                    if (int.TryParse(input, out selectIndex))
                        if (selectIndex < 0)
                            selectIndex = 0;
                        else
                            selectIndex = selectIndex % wifs.Length;

                }
            }

            //   int.TryParse(input, out selectIndex);
        }
    }
}
