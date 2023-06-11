using Aliyun.Acs.Core.Auth;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aliyun
{
    public class Json
    {
        public static void Add(string path, string json)
        {
            if (!AliyunOSSHelper.loadSuccess)
                AliyunOSSHelper.LoadKey();

            AliyunOSSHelper.PutString("yrqmodeldata", path, json);
        }
    }
    public class ByteData
    {
        public static void Add(string path, byte[] data)
        {
            if (!AliyunOSSHelper.loadSuccess)
                AliyunOSSHelper.LoadKey();

            AliyunOSSHelper.PutByte("yrqmodeldata", path, data);
        }
    }
}
