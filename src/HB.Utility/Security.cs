using NETCore.Encrypt;
using NETCore.Encrypt.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace HB.Utility
{
    public class Security
    {
        #region AES
        public static string AESDecrypt(string data, string key, string vector)
        {
            return EncryptProvider.AESDecrypt(data, key, vector);
        }
        public static byte[] AESDecrypt(byte[] data, string key, string vector)
        {
            return EncryptProvider.AESDecrypt(data, key, vector);
        }
        public static string AESDecrypt(string data, string key)
        {
            return EncryptProvider.AESDecrypt(data, key);
        }
        public static string AESEncrypt(string data, string key, string vector)
        {
            return EncryptProvider.AESEncrypt(data, key, vector);
        }
        public static byte[] AESEncrypt(byte[] data, string key, string vector)
        {
            return EncryptProvider.AESEncrypt(data, key, vector);
        }
        public static string AESEncrypt(string data, string key)
        {
            return EncryptProvider.AESEncrypt(data, key);
        }

        #endregion

        #region BASE64
        public static string Base64Decrypt(string input)
        {
            return EncryptProvider.Base64Decrypt(input);
        }
        public static string Base64Decrypt(string input, Encoding encoding)
        {
            return EncryptProvider.Base64Decrypt(input, encoding);
        }
        public static string Base64Encrypt(string input, Encoding encoding)
        {
            return EncryptProvider.Base64Encrypt( input,  encoding);
        }
        public static string Base64Encrypt(string input)
        {
            return EncryptProvider.Base64Encrypt( input);
        }
        #endregion

        #region  
        //public static AESKey CreateAesKey()
        //{

        //}
        //public static string CreateDecryptionKey(int length)
        //{

        //}
        //public static string CreateDesKey()
        //{

        //}
        //public static RSAKey CreateRsaKey(RsaSize rsaSize = RsaSize.R2048)
        //{

        //}
        //public static RSAKey CreateRsaKey(RSA rsa)
        //{

        //}
        #endregion


        #region createkey
        public static string CreateAesKey()
        {
            //32个字符
            return "1234506789aBCDEf1234506789aBCDEf";
        }

        public static string CreateAesVector()
        {
            //16个字符
            return "1234506789aBCDEf";
        }

        public static string CreateDecryptionKey(int length)
        {
            return "";
        }
        public static string CreateDesKey()
        {
            return "";
        }
        public static string CreateRsaKey(RsaSize rsaSize = RsaSize.R2048)
        {
            return "";
        }
        public static string CreateRsaKey(RSA rsa)
        {
            return "";
        }
        #endregion

        public static string CreateValidationKey(int length)
        {
            return EncryptProvider.CreateValidationKey( length);
        }
        public static string DESDecrypt(string data, string key)
        {
            return EncryptProvider.DESDecrypt( data,  key);
        }
        public static byte[] DESDecrypt(byte[] data, string key)
        {
            return EncryptProvider.DESDecrypt( data,  key);
        }
        public static string DESEncrypt(string data, string key)
        {
            return EncryptProvider.DESEncrypt( data,  key);
        }
        public static byte[] DESEncrypt(byte[] data, string key)
        {
            return EncryptProvider.DESEncrypt(data,  key);
        }
        public static string HMACMD5(string srcString, string key)
        {
            return EncryptProvider.HMACMD5( srcString,  key);
        }
        public static string HMACSHA1(string srcString, string key)
        {
            return EncryptProvider.HMACSHA1( srcString,  key);
        }
        public static string HMACSHA256(string srcString, string key)
        {
            return EncryptProvider.HMACSHA256( srcString,  key);
        }
        public static string HMACSHA384(string srcString, string key)
        {
            return EncryptProvider.HMACSHA384( srcString,  key);
        }
        public static string HMACSHA512(string srcString, string key)
        {
            return EncryptProvider.HMACSHA512( srcString,  key);
        }
        public static string Md5(string srcString, MD5Length length = MD5Length.L32)
        {
            return EncryptProvider.Md5( srcString,  length);
        }
        public static string RSADecrypt(string privateKey, string srcString)
        {
            return EncryptProvider.RSADecrypt( privateKey,  srcString);
        }
        public static string RSADecrypt(string privateKey, string srcString, RSAEncryptionPadding padding, bool isPemKey = false)
        {
            return EncryptProvider.RSADecrypt( privateKey,  srcString,  padding,  isPemKey);
        }
        public static string RSADecryptWithPem(string privateKey, string srcString)
        {
            return EncryptProvider.RSADecryptWithPem( privateKey,  srcString);
        }
        public static string RSAEncrypt(string publicKey, string srcString, RSAEncryptionPadding padding, bool isPemKey = false)
        {
            return EncryptProvider.RSAEncrypt( publicKey,  srcString,  padding,  isPemKey);
        }
        public static string RSAEncrypt(string publicKey, string srcString)
        {
            return EncryptProvider.RSAEncrypt( publicKey,  srcString);
        }
        public static string RSAEncryptWithPem(string publicKey, string srcString)
        {
            return EncryptProvider.RSAEncryptWithPem( publicKey,  srcString);
        }
        public static RSA RSAFromPem(string pem)
        {
            return EncryptProvider.RSAFromPem( pem);
        }
        public static RSA RSAFromString(string rsaKey)
        {
            return EncryptProvider.RSAFromString( rsaKey);
        }
        public static string RSASign(string content, string privateKey, HashAlgorithmName hashAlgorithmName, RSASignaturePadding rSASignaturePadding, Encoding encoding)
        {
            return EncryptProvider.RSASign(content, privateKey, hashAlgorithmName, rSASignaturePadding, encoding);
        }
        public static string RSASign(string conent, string privateKey)
        {
            return EncryptProvider.RSASign( conent,  privateKey);
        }

        public static (string publicPem, string privatePem) RSAToPem(bool isPKCS8)
        {
            return EncryptProvider.RSAToPem(isPKCS8);
        }
        public static bool RSAVerify(string content, string signStr, string publickKey, HashAlgorithmName hashAlgorithmName, RSASignaturePadding rSASignaturePadding, Encoding encoding)
        {
            return EncryptProvider.RSAVerify(content, signStr, publickKey, hashAlgorithmName, rSASignaturePadding, encoding);
        }
        public static bool RSAVerify(string content, string signStr, string publickKey)
        {
            return EncryptProvider.RSAVerify(content, signStr, publickKey);
        }
        public static string Sha1(string str)
        {
            return EncryptProvider.Sha1(str);
        }
        public static string Sha256(string srcString)
        {
            return EncryptProvider.Sha256(srcString);
        }
        public static string Sha384(string srcString)
        {
            return EncryptProvider.Sha384(srcString);
        }
        public static string Sha512(string srcString)
        {
            return EncryptProvider.Sha512(srcString);
        }
    }


}
