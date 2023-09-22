using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass
{
    public class Random
    {
        public static string GetMD5HashFromStr(string str)
        {
            try
            {
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(str);
                    using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] retVal = md5.ComputeHash(bytes);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < retVal.Length; i++)
                        {
                            sb.Append(retVal[i].ToString("x2"));
                        }
                        return sb.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        public static string GetMD5HashFromBytes(byte[] bytes)
        {
            try
            {
                {
                    // byte[] bytes = Encoding.UTF8.GetBytes(str);
                    using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] retVal = md5.ComputeHash(bytes);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < retVal.Length; i++)
                        {
                            sb.Append(retVal[i].ToString("x2"));
                        }
                        return sb.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        public static byte[] GetMD5HashByteFromBytes(byte[] bytes)
        {
            try
            {
                {
                    // byte[] bytes = Encoding.UTF8.GetBytes(str);
                    using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                    {
                        byte[] retVal = md5.ComputeHash(bytes);
                        return retVal;
                        //StringBuilder sb = new StringBuilder();
                        //for (int i = 0; i < retVal.Length; i++)
                        //{
                        //    sb.Append(retVal[i].ToString("x2"));
                        //}
                        //return sb.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        public static string getGUID()
        {

            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }
        public static string GetSha256FromStr(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            string hashString;
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
            return hashString;
        }
    }
}
