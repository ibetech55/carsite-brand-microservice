using System.Security.Cryptography;

namespace BrandMicroservice.src.Utils
{
    public class GenerateCode
    {
        public static string Execute(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            char[] result = new char[length];

            byte[] randomBytes = new byte[length];

            RandomNumberGenerator.Fill(randomBytes);

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[randomBytes[i] % chars.Length];
            }

            return new string(result);
        }
    }
}
