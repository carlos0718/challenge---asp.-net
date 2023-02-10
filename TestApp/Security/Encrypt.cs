using System.Security.Cryptography;
using System.Text;

namespace TestApp.Security {
    public class Encrypt {
        
        public static string GetSHA256(string password){
            SHA256 sha256 = SHA256.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[]? stream = null;
            stream = sha256.ComputeHash(encoding.GetBytes(password));
            string passEncode = Convert.ToBase64String(stream).ToString();

            return passEncode;
        }
    }
}
