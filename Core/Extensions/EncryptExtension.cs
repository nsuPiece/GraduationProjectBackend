using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EncryptExtension
    {
        #region Pbkdf2

        /// <summary>
        /// Pbkdf2配置
        /// </summary>
        private static readonly int _pbkdf2RequestedNum = 512 / 8;
        private static readonly KeyDerivationPrf _keyDerivation = KeyDerivationPrf.HMACSHA256;
        private static readonly int _pbkdf2IterationCount = 500;

        /// <summary>
        /// 使用 PBKDF2 算法执行密钥派生，推荐所有密码都用此方法进行加密，使用 <see cref="Pbkdf2Compare" /> 进行验证
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Pbkdf2Encrypt(this string text)
        {
            // 生成salt
            var salt = Guid.NewGuid().ToByteArray();
            // 加密密码，返回base64
            return Convert.ToBase64String(Encrypt(text, salt, _keyDerivation));
        }

        /// <summary>
        /// 使用 PBKDF2 验证文本（字符串）是否正确
        /// </summary>
        /// <param name="text">待验证的原始文本（字符串）</param>
        /// <param name="encryptText">已加密的 base64 字符串</param>
        /// <returns></returns>
        public static bool Pbkdf2Compare(this string text, string encryptText)
        {
            var decodeEncryptText = Convert.FromBase64String(encryptText);

            var salt = new byte[128 / 8];
            Buffer.BlockCopy(decodeEncryptText, 0, salt, 0, salt.Length);

            var encryptSubkey = new byte[_pbkdf2RequestedNum];
            Buffer.BlockCopy(decodeEncryptText, salt.Length + 1, encryptSubkey, 0, encryptSubkey.Length);

            var actualSubkey = Encrypt(text, salt, (KeyDerivationPrf)decodeEncryptText[salt.Length]);

            return CryptographicOperations.FixedTimeEquals(actualSubkey, decodeEncryptText);
        }

        /// <summary>
        /// 使用 PBKDF2 进行加密
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <param name="salt">salt</param>
        /// <param name="prf">加密使用的伪随机函数</param>
        /// <returns></returns>
        private static byte[] Encrypt(string text, byte[] salt, KeyDerivationPrf prf)
        {
            var output = new byte[salt.Length + 1 + _pbkdf2RequestedNum];
            // 将salt添加到输出结果中
            Buffer.BlockCopy(salt, 0, output, 0, salt.Length);
            // 添加使用的伪随机函数
            output[salt.Length] = (byte)prf;

            // 字节偏移量
            var dstOffset = salt[3] % 10 + 2;
            // 打乱salt
            Buffer.BlockCopy(salt, 0, salt, dstOffset, dstOffset / 2);

            // 加密结果
            var encryptByte = KeyDerivation.Pbkdf2(
                password: text,
                salt: salt,
                prf: prf,
                iterationCount: (salt[dstOffset + 1] + _pbkdf2IterationCount) << 6,
                numBytesRequested: _pbkdf2RequestedNum);

            Buffer.BlockCopy(encryptByte, 0, output, salt.Length + 1, encryptByte.Length);

            return output;
        }
        #endregion
    }
}
