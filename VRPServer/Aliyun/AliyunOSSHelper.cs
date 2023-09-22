using Aliyun.OSS.Common;
using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyun
{
    public static class Endpoint
    {
        /// <summary>
        /// 华东1（杭州）
        /// </summary>
        const string HangZhou = "oss-cn-hangzhou.aliyuncs.com";
        /// <summary>
        /// 华东2（上海）
        /// </summary>
        const string ShangHai = "oss-cn-shanghai.aliyuncs.com";
        /// <summary>
        /// 华北1（青岛）
        /// </summary>
        const string QingDao = "oss-cn-qingdao.aliyuncs.com";
        /// <summary>
        /// 华北2（北京）
        /// </summary>
        public const string BeiJing = "oss-cn-beijing.aliyuncs.com";
    }
    public class AliyunOSSHelper
    {
        public static bool loadSuccess = false;
        public static void LoadKey()
        {
            if (!loadSuccess)
            {
                var secret = File.ReadAllText("config/AccessKey.txt");
                secret.Split(",");
                AliyunOSSHelper.accessKeyId = secret.Split(",")[0];
                AliyunOSSHelper.accessKeySecret = secret.Split(",")[1];
                loadSuccess = true;
                client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            }
        }
        static string accessKeyId = "你的accessKeyId ";
        static string accessKeySecret = "你的accessKeySecret";
        static string endpoint = Endpoint.BeiJing;

        static OssClient client = null;//= new OssClient(endpoint, accessKeyId, accessKeySecret);


        /// <summary>
        /// 创建存储空间
        /// </summary>
        /// <param name="bucketName">存储空间名称</param>
        /// <returns></returns>
        public static bool CreateBucket(string bucketName)
        {
            try
            {
                // 创建存储空间。
                var bucket = client.CreateBucket(bucketName);
                Console.WriteLine($"创建存储空间《{bucketName}》成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create bucket failed, {ex.Message}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 存储一个字符串
        /// </summary>
        /// <param name="bucketName">存储空间名称</param>
        /// <param name="key">Key</param>
        /// <param name="objectContent">字符内容</param>
        /// <returns></returns>
        public static bool PutString(string bucketName, string key, string objectContent)
        {
            try
            {
                byte[] binaryData = Encoding.UTF8.GetBytes(objectContent);
                using (MemoryStream requestContent = new MemoryStream(binaryData))
                {
                    // 上传文件。
                    client.PutObject(bucketName, key, requestContent);
                }
                Console.WriteLine($"存储成功:{key}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"存储字符串有误, {key},{ex.Message}");
                Console.ReadLine();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 存储一个字符串
        /// </summary>
        /// <param name="bucketName">存储空间名称</param>
        /// <param name="key">Key</param> 
        /// <returns></returns>
        public static string GetString(string bucketName, string key)
        {
            try
            {
                string text;
                var ossObj = client.GetObject(bucketName, key);
                using (var stream = ossObj.ResponseStream)
                {
                    //stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        text = reader.ReadToEnd();
                    }
                }

                Console.WriteLine($"读取成功:{key}，{text}");
                return text;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取成功有误, {key},{ex.Message}");
                // Console.ReadLine();
                return "";
            }
            //   return true;
        }

        public static bool PutByte(string bucketName, string key, byte[] Data)
        {
            try
            {
                byte[] binaryData = Data;
                using (MemoryStream requestContent = new MemoryStream(binaryData))
                {
                    // 上传文件。
                    client.PutObject(bucketName, key, requestContent);
                }
                Console.WriteLine($"存储成功:{key}");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"存储数据有误, {key},{ex.Message}");
                Console.ReadLine();
                return false;
            }
            return true;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="bucketName">存储空间名称</param>
        /// <param name="fileToUpload">文件路径</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static bool PutObject(string bucketName, string fileToUpload, string key)
        {
            //if (string.IsNullOrEmpty(key))
            //{
            //    key = Path.GetFileName(fileToUpload);
            //}
            try
            {
                // 上传文件。
                var result = client.PutObject(bucketName, key, fileToUpload);
                Console.WriteLine($"上传文件《{key}》成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"上传文件有误  {ex.Message}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ExistsObject(string bucketName, string key)
        {
            try
            {
                var exist = client.DoesObjectExist(bucketName, key);
                return exist;
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
                return false;

            }
            return false;
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DeleteObject(string bucketName, string key)
        {
            try
            {
                // 删除文件。
                client.DeleteObject(bucketName, key);
                Console.WriteLine($"删除文件《{key}》成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除文件有误{ex.Message}");
                return false;
            }
            return true;

        }




        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="bucketName">存储空间名称</param>
        /// <returns></returns>
        public static List<OssObjectSummary> ListObjects(string bucketName)
        {
            var objectsList = new List<OssObjectSummary>();
            try
            {
                ObjectListing result = null;
                string nextMarker = string.Empty;
                do
                {
                    var listObjectsRequest = new ListObjectsRequest(bucketName)
                    {
                        Marker = nextMarker,
                    };
                    // 列举文件。
                    result = client.ListObjects(listObjectsRequest);
                    foreach (var summary in result.ObjectSummaries)
                    {
                        objectsList.Add(summary);
                        Console.WriteLine(summary.Key);
                    }
                    nextMarker = result.NextMarker;
                } while (result.IsTruncated);

            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
            return objectsList;
        }


    }
}
