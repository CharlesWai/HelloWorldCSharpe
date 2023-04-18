using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppForAndroid.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ToBytesFromHexString(this string hexString)
        {
            //以 ' ' 分割字符串，并去掉空字符
            string[] chars = hexString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] returnBytes = new byte[chars.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < chars.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(chars[i], 16);
            }
            return returnBytes;
        }
    }
}
