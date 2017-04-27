using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JFB.Utils
{
    public class AESHelper
    {
        //默认密钥向量
        private static byte[] Keys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };
        private static string KeysIV = "E4ghj*Ghg7!rNIfb&*5GUY86KfghUb#er57HBh(u%g6HM($jhWk7&!hg4ui%$hjk";

        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = StringHelper.GetSubString(encryptKey, 16, "");

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey);
            rijndaelProvider.IV = Encoding.UTF8.GetBytes(KeysIV);
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }

        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {      

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV = Encoding.UTF8.GetBytes(KeysIV);

                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Encoding.UTF8.GetBytes(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch(Exception ex)
            {
                return "";
            }

        }

        public static string AESDecrypt(string encryptStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes("7b29e94b1b0bbaed");
            byte[] bIV = Encoding.UTF8.GetBytes("E4ghj*Ghg7!rNIfb&*5GUY86KfghUb#er57HBh(u%g6HM($jhWk7&!hg4ui%$hjk");
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch(Exception ex) { }
            aes.Clear();

            return decrypt;
        }
        
    }
}
