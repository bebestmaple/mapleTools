using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Utils.SecurityEx
{
    public class SecurityHelper
    {
        #region Md5
        /// <summary>
        /// MD5 16位加密 加密后密码为大写
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <returns></returns>
        public static string GetMd5_16(string ConvertString)
        {
            try
            {
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
                    return t2.Replace("-", "");
                }
            }
            catch { return ""; }
        }


        /// <summary>
        /// MD5 32位加密 加密后密码为大写
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetMd5_32(string text)
        {
            try
            {
                using (MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider())
                {
                    byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(text));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }
                    return sBuilder.ToString();
                }
            }
            catch { return ""; }
        }
        #endregion

        #region SHA

        /// <summary>
        /// 使用 SHA1 加密算法来加密
        /// </summary>
        /// <param name="source">原字符串</param>
        /// <returns>加密后字符串</returns>
        public static string SHA1_Encrypt(string source)
        {
            byte[] StrRes = Encoding.UTF8.GetBytes(source);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
       


        /// <summary>
        /// SHA256 加密
        /// </summary>
        /// <param name="source">原字符串</param>
        /// <returns>加密后字符串</returns>
        public static string SHA256_Encrypt(string source)
        {
            byte[] data = Encoding.UTF8.GetBytes(source);
            SHA256 shaM = SHA256.Create();
            byte[] result = shaM.ComputeHash(data);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in result)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        /// <summary>
        /// SHA384 加密
        /// </summary>
        /// <param name="source">原字符串</param>
        /// <returns>加密后字符串</returns>
        public static string SHA384_Encrypt(string source)
        {
            byte[] data = Encoding.UTF8.GetBytes(source);
            SHA384 shaM = SHA384.Create();
            byte[] result = shaM.ComputeHash(data);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in result)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        /// <summary>
        /// SHA512_加密
        /// </summary>
        /// <param name="source">原字符串</param>
        /// <returns>加密后字符串</returns>
        public static string SHA512_Encrypt(string source)
        {
            byte[] data = Encoding.UTF8.GetBytes(source);
            SHA512 shaM = new SHA512Managed();
            byte[] result = shaM.ComputeHash(data);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in result)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
        #endregion

        #region Hash
        /// <summary>
        /// 获取某个哈希算法对应下的哈希值
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="algorithm">哈希算法</param>
        /// <returns>经过计算的哈希值</returns>
        private static string GetHash(string source, HashAlgorithm algorithm)
        {
            if (algorithm == null)
            {
                throw new ArgumentNullException("algorithm", "algorithm is null.");
            
            }
            byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
            byte[] result = algorithm.ComputeHash(sourceBytes);
            algorithm.Clear();
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        } 
        #endregion

        #region 对称加密
        public class AES_128
        {
            /// <summary>  
            /// AES加密(无向量)  
            /// </summary>  
            /// <param name="plainBytes">被加密的明文</param>  
            /// <param name="key">密钥</param>  
            /// <returns>密文</returns>  
            public static string Encrypt(string source, String Key)
            {
                MemoryStream mStream = new MemoryStream();
                RijndaelManaged aes = new RijndaelManaged();

                byte[] plainBytes = Encoding.UTF8.GetBytes(source);
                Byte[] bKey = new Byte[32];
                Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);

                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;
                aes.Key = bKey;
                CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                try
                {
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    return Convert.ToBase64String(mStream.ToArray());
                }
                finally
                {
                    cryptoStream.Close();
                    mStream.Close();
                    aes.Clear();
                }
            }


            /// AES解密(无向量)  
            /// </summary>  
            /// <param name="encryptedBytes">被加密的明文</param>  
            /// <param name="key">密钥</param>  
            /// <returns>明文</returns>  
            public static string Decrypt(String Data, String Key)
            {
                Byte[] encryptedBytes = Convert.FromBase64String(Data);
                Byte[] bKey = new Byte[32];
                Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);

                MemoryStream mStream = new MemoryStream(encryptedBytes);
                RijndaelManaged aes = new RijndaelManaged();
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;
                aes.Key = bKey;
                CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                try
                {
                    byte[] tmp = new byte[encryptedBytes.Length + 32];
                    int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                    byte[] ret = new byte[len];
                    Array.Copy(tmp, 0, ret, 0, len);
                    return Encoding.UTF8.GetString(ret);
                }
                finally
                {
                    cryptoStream.Close();
                    mStream.Close();
                    aes.Clear();
                }
            }

        }

        public class AES_256
        {

            /// <summary>
            ///  AES 加密
            /// </summary>
            /// <param name="source"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            public static string Encrypt(string source, string key)
            {

                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("Source(or key) is null or empty.");
                }
                return Encrypt(Encoding.UTF8.GetBytes(source), Encoding.UTF8.GetBytes(key));
            }

            public static string Encrypt(byte[] source, byte[] key)
            {
                if (source == null || source.Length == 0 || key == null || key.Length == 0)
                {
                    throw new ArgumentException("Source(or key) is null or empty.");
                }
                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = key,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cTransform = rm.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(source, 0, source.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);

            }

            /// <summary>
            ///  AES 解密
            /// </summary>
            /// <param name="source"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            public static string Decrypt(string source, string key)
            {
                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("Source(or key) is null or empty.");
                }
                return Decrypt(Encoding.UTF8.GetBytes(source), Encoding.UTF8.GetBytes(key));
            }

            public static string Decrypt(byte[] source, byte[] key)
            {
                if (source == null || source.Length == 0 || key == null || key.Length == 0)
                {
                    throw new ArgumentException("Source(or key) is null or empty.");
                }

                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = key,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cTransform = rm.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(source, 0, source.Length);
                return Encoding.UTF8.GetString(resultArray);
            }

        }

        public class DES
        {
            /// <summary>
            /// DES加密方法
            /// </summary>
            /// <param name="source">明文</param>
            /// <param name="key">密钥,且必须为8位</param>
            /// <returns>密文</returns>
            public static string Encrypt(string source, string key)
            {
                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("Source(or key) is null or empty.");
                }
                if (key.Length != 8)
                {
                    throw new ArgumentException("key's length must be 8.");
                }
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    byte[] keyArray = ASCIIEncoding.ASCII.GetBytes(key);
                    byte[] iv = ASCIIEncoding.ASCII.GetBytes(key);
                    byte[] dataByteArray = Encoding.UTF8.GetBytes(source);
                    des.Mode = System.Security.Cryptography.CipherMode.CBC;
                    des.Key = keyArray;
                    des.IV = iv;
                    string encrypt = "";
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(dataByteArray, 0, dataByteArray.Length);
                            cs.FlushFinalBlock();
                            encrypt = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                    return encrypt;
                }
            }

            /// <summary>
            /// 进行DES解密。
            /// </summary>
            /// <param name="pToDecrypt">要解密的base64串</param>
            /// <param name="key">密钥，且必须为8位。</param>
            /// <returns>已解密的字符串。</returns>
            public static string Decrypt(string source, string key)
            {
                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("Source(or key) is null or empty.");
                }
                if (key.Length != 8)
                {
                    throw new ArgumentException("key's length must be 8.");
                }

                byte[] inputByteArray = System.Convert.FromBase64String(source);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                    des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    string str = Encoding.UTF8.GetString(ms.ToArray());
                    ms.Close();
                    return str;
                }
            }
        }

        public class TripleDES
        {
            public static string Encrypt(string source, string key)
            {
                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("Source(or key) is null or empty.");
                }
                return Encrypt(Encoding.UTF8.GetBytes(source), Encoding.ASCII.GetBytes(key));
            }

            public static string Encrypt(byte[] source, byte[] key)
            {
                if (source == null || source.Length == 0 || key == null || key.Length == 0)
                {
                    throw new ArgumentException("Source(or key) is null or empty.");
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    TripleDESCryptoServiceProvider dsp = new TripleDESCryptoServiceProvider();
                    dsp.Mode = CipherMode.ECB;
                    dsp.Padding = PaddingMode.PKCS7;
                    dsp.Key = key;
                    using (CryptoStream cStream = new CryptoStream(ms, dsp.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Write the byte array to the crypto stream and flush it.
                        cStream.Write(source, 0, source.Length);
                        cStream.FlushFinalBlock();
                        // Get an array of bytes from the
                        // MemoryStream that holds the
                        // encrypted data.
                        byte[] ret = ms.ToArray();
                        // Return the encrypted buffer.
                        return Convert.ToBase64String(ret);

                    }
                }

            }

            public static string Decrypt(string source, string key)
            {
                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("Source(or key) is null or empty.");
                }
                return Decrypt(System.Convert.FromBase64String(source), Encoding.ASCII.GetBytes(key));
            }


            public static string Decrypt(byte[] source, byte[] key)
            {
                if (source == null || source.Length == 0 || key == null || key.Length == 0)
                {
                    throw new ArgumentException("Source(or key) is null or empty.");
                }

                // Create a new MemoryStream using the passed
                // array of encrypted data.
                using (MemoryStream ms = new MemoryStream(source))
                {
                    // Create a CryptoStream using the MemoryStream
                    TripleDESCryptoServiceProvider dsp = new TripleDESCryptoServiceProvider();
                    dsp.Key = key;
                    dsp.Padding = PaddingMode.PKCS7;//补位
                    dsp.Mode = CipherMode.ECB;//CipherMode.CBC
                    using (CryptoStream csDecrypt = new CryptoStream(ms, dsp.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        // Create buffer to hold the decrypted data.
                        byte[] fromEncrypt = new byte[source.Length];
                        // Read the decrypted data out of the crypto stream
                        // and place it into the temporary buffer.
                        csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                        //Convert the buffer into a string and return it.
                        return Encoding.UTF8.GetString(fromEncrypt).TrimEnd('\0');
                    }
                }
            }

        }


        /// <summary>
        /// Description of CryptoGraphy.
        /// </summary>
        public class RC4Crypt : IDisposable
        {
            byte[] S;
            byte[] T;
            byte[] K;
            byte[] k;

            public RC4Crypt(byte[] key)
            {
                if (key == null || key.Length == 0)
                {
                    throw new ArgumentException("key is null or empty.");
                }
                this.K = key;
            }

            public RC4Crypt(string key)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("key is null or empty.");
                }
                this.K = Encoding.UTF8.GetBytes(key);
            }

            public byte[] Key
            {
                get
                {
                    return K;
                }
                set
                {
                    K = value;
                }
            }

            /// <summary>
            /// 初始化状态向量S和临时向量T，供keyStream方法调用
            /// </summary>
            private void Initial()
            {
                if (S == null || T == null)
                {
                    S = new byte[256];
                    T = new byte[256];
                }
                for (int i = 0; i < 256; ++i)
                {
                    S[i] = (byte)i;
                    T[i] = K[i % K.Length];
                }
            }


            /// <summary>
            /// 初始排列状态向量S，供keyStream方法调用
            /// </summary>
            private void Ranges()
            {
                int j = 0;
                for (int i = 0; i < 256; ++i)
                {
                    j = (j + S[i] + T[i]) & 0xff;
                    S[i] = (byte)((S[i] + S[j]) & 0xff);
                    S[j] = (byte)((S[i] - S[j]) & 0xff);
                    S[i] = (byte)((S[i] - S[j]) & 0xff);
                }
            }

            //生成密钥流
            //len:明文为len个字节
            private void KeyStream(int len)
            {
                Initial();
                Ranges();
                int i = 0, j = 0, t = 0;
                k = new byte[len];
                for (int r = 0; r < len; r++)
                {
                    i = (i + 1) & 0xff;
                    j = (j + S[i]) & 0xff;

                    S[i] = (byte)((S[i] + S[j]) & 0xff);
                    S[j] = (byte)((S[i] - S[j]) & 0xff);
                    S[i] = (byte)((S[i] - S[j]) & 0xff);

                    t = (S[i] + S[j]) & 0xff;
                    k[r] = S[t];
                }
            }

            public string Encrypt(string source)
            {
                if (string.IsNullOrEmpty(source))
                {
                    throw new ArgumentException("source is null or empty.");
                }
                return Encoding.UTF8.GetString(Encrypt(Encoding.UTF8.GetBytes(source)));
            }

            public byte[] Encrypt(byte[] source)
            {
                if (source == null || source.Length == 0)
                {
                    throw new ArgumentException("source is null or empty.");
                }

                //生产密匙流
                KeyStream(source.Length);
                for (int i = 0; i < source.Length; i++)
                {
                    k[i] = (byte)(source[i] ^ k[i]);
                }
                return k;
            }

            public string Decrypt(string source)
            {
                if (string.IsNullOrEmpty(source))
                {
                    throw new ArgumentException("source is null or empty.");
                }
                return Encoding.UTF8.GetString(Encrypt(Encoding.UTF8.GetBytes(source)));

            }

            public byte[] Decrypt(byte[] source)
            {
                if (source == null || source.Length == 0)
                {
                    throw new ArgumentException("source is null or empty.");
                }
                return Encrypt(source);
            }

            //是否回收完毕
            bool _disposed;
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            ~RC4Crypt()
            {
                Dispose(false);
            }
            //这里的参数表示示是否需要释放那些实现IDisposable接口的托管对象
            protected virtual void Dispose(bool disposing)
            {
                if (_disposed) return;//如果已经被回收，就中断执行
                if (disposing)
                {
                    //TODO:释放那些实现IDisposable接口的托管对象

                }
                //TODO:释放非托管资源，设置对象为null
                S = null;
                T = null;
                K = null;
                k = null;
                _disposed = true;
            }
        }
        #endregion

        #region RSA加密
        public class RSAHelper
        {
            /// <summary>
            /// RSA的容器 可以解密的源字符串长度为 DWKEYSIZE/8-11 
            /// </summary>
            public const int DWKEYSIZE = 1024;

            /// <summary>
            /// RSA加密的密匙结构  公钥和私匙
            /// </summary>
            public struct RSAKey
            {
                public string PublicKey { get; set; }
                public string PrivateKey { get; set; }
            }

            #region 得到RSA密匙对
            /// <summary>
            /// 得到RSA密匙对
            /// </summary>
            /// <returns></returns>
            public static RSAKey GetRASKey()
            {
                RSACryptoServiceProvider.UseMachineKeyStore = true;
                //声明一个指定大小的RSA容器
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(DWKEYSIZE);
                //取得RSA容易里的各种参数
                RSAParameters p = rsaProvider.ExportParameters(true);

                return new RSAKey()
                {
                    PublicKey = ComponentKey(p.Exponent, p.Modulus),
                    PrivateKey = ComponentKey(p.D, p.Modulus)
                };
            }
            #endregion

            #region 检查明文的有效性 DWKEYSIZE/8-11 长度之内为有效 中英文都算一个字符
            /// <summary>
            /// 检查明文的有效性 DWKEYSIZE/8-11 长度之内为有效 中英文都算一个字符
            /// </summary>
            /// <param name="source"></param>
            /// <returns></returns>
            public static bool CheckSourceValidate(string source)
            {
                return (DWKEYSIZE / 8 - 11) >= source.Length;
            }
            #endregion

            #region 组合解析密匙
            /// <summary>
            /// 组合成密匙字符串
            /// </summary>
            /// <param name="b1"></param>
            /// <param name="b2"></param>
            /// <returns></returns>
            private static string ComponentKey(byte[] b1, byte[] b2)
            {
                List<byte> list = new List<byte>();
                //在前端加上第一个数组的长度值 这样今后可以根据这个值分别取出来两个数组
                list.Add((byte)b1.Length);
                list.AddRange(b1);
                list.AddRange(b2);
                byte[] b = list.ToArray<byte>();
                return Convert.ToBase64String(b);
            }

            /// <summary>
            /// 解析密匙
            /// </summary>
            /// <param name="key">密匙</param>
            /// <param name="b1">RSA的相应参数1</param>
            /// <param name="b2">RSA的相应参数2</param>
            private static void ResolveKey(string key, out byte[] b1, out byte[] b2)
            {
                //从base64字符串 解析成原来的字节数组
                byte[] b = Convert.FromBase64String(key);
                //初始化参数的数组长度
                b1 = new byte[b[0]];
                b2 = new byte[b.Length - b[0] - 1];
                //将相应位置是值放进相应的数组
                for (int n = 1, i = 0, j = 0; n < b.Length; n++)
                {
                    if (n <= b[0])
                    {
                        b1[i++] = b[n];
                    }
                    else
                    {
                        b2[j++] = b[n];
                    }
                }
            }
            #endregion

            #region 字符串加密解密 公开方法
            /// <summary>
            /// 字符串加密
            /// </summary>
            /// <param name="source">源字符串 明文</param>
            /// <param name="key">密匙</param>
            /// <returns>加密遇到错误将会返回原字符串</returns>
            public static string Encrypt(string source, string key)
            {
                string encryptString = string.Empty;
                byte[] d;
                byte[] n;
                try
                {
                    if (!CheckSourceValidate(source))
                    {
                        throw new Exception("source string too long");
                    }
                    //解析这个密钥
                    ResolveKey(key, out d, out n);
                    BigInteger biN = new BigInteger(n);
                    BigInteger biD = new BigInteger(d);
                    encryptString = Encrypt(source, biD, biN);
                }
                catch
                {
                    encryptString = source;
                }
                return encryptString;
            }

            /// <summary>
            /// 字符串解密
            /// </summary>
            /// <param name="encryptString">密文</param>
            /// <param name="key">密钥</param>
            /// <returns>遇到解密失败将会返回原字符串</returns>
            public static string Decrypt(string encryptString, string key)
            {
                string source = string.Empty;
                byte[] e;
                byte[] n;
                try
                {
                    //解析这个密钥
                    ResolveKey(key, out e, out n);
                    BigInteger biE = new BigInteger(e);
                    BigInteger biN = new BigInteger(n);
                    source = Decrypt(encryptString, biE, biN);
                }
                catch
                {
                    source = encryptString;
                }
                return source;
            }
            #endregion

            #region 字符串加密解密   实现加解密的实现方法
            /// <summary>
            /// 用指定的密匙加密 
            /// </summary>
            /// <param name="source">明文</param>
            /// <param name="d">可以是RSACryptoServiceProvider生成的D</param>
            /// <param name="n">可以是RSACryptoServiceProvider生成的Modulus</param>
            /// <returns>返回密文</returns>
            private static string Encrypt(string source, BigInteger d, BigInteger n)
            {
                int len = source.Length;
                int len1 = 0;
                int blockLen = 0;
                if ((len % 128) == 0)
                    len1 = len / 128;
                else
                    len1 = len / 128 + 1;
                string block = "";
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < len1; i++)
                {
                    if (len >= 128)
                        blockLen = 128;
                    else
                        blockLen = len;
                    block = source.Substring(i * 128, blockLen);
                    byte[] oText = System.Text.Encoding.Default.GetBytes(block);
                    BigInteger biText = new BigInteger(oText);
                    BigInteger biEnText = biText.modPow(d, n);
                    string temp = biEnText.ToHexString();
                    result.Append(temp).Append("@");
                    len -= blockLen;
                }
                return result.ToString().TrimEnd('@');
            }

            /// <summary>
            /// 用指定的密匙加密 
            /// </summary>
            /// <param name="source">密文</param>
            /// <param name="e">可以是RSACryptoServiceProvider生成的Exponent</param>
            /// <param name="n">可以是RSACryptoServiceProvider生成的Modulus</param>
            /// <returns>返回明文</returns>
            private static string Decrypt(string encryptString, BigInteger e, BigInteger n)
            {
                StringBuilder result = new StringBuilder();
                string[] strarr1 = encryptString.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strarr1.Length; i++)
                {
                    string block = strarr1[i];
                    BigInteger biText = new BigInteger(block, 16);
                    BigInteger biEnText = biText.modPow(e, n);
                    string temp = System.Text.Encoding.Default.GetString(biEnText.getBytes());
                    result.Append(temp);
                }
                return result.ToString();
            }
            #endregion
        }





       #endregion
    }
}
