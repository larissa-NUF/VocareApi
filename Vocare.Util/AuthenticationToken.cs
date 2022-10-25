using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocare.Util
{
    public class AuthenticationToken
    {

        #region Propriedades
        public const string KeyString = "DE602AC85C20081F855BA9FDC59FA099";

        private const int TokenValidity = 90; // days
        public string AppId { get; private set; }
        public string UserId { get; private set; }
        public string Timestamp { get; private set; }

        #endregion

        #region Métodos públicos

        public AuthenticationToken(string appId, string userId)
        {
            AppId = appId;
            UserId = userId;
            Timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss.fff");
        }

        public override string ToString()
        {
            return $"UserId={UserId};Timestamp={Timestamp};AppId={AppId}";
        }

        public string Encrypt()
        {
            return CriptografiaHelper.EncryptString(ToString());
        }

        public bool IsExpired
        {
            get
            {
                var tokenCreated = DateTime.ParseExact(Timestamp, "yyyyMMdd-HHmmss.fff", System.Globalization.CultureInfo.InvariantCulture);
                return tokenCreated.AddDays(TokenValidity) < DateTime.Now;
            }
        }

        public static AuthenticationToken Decrypt(string encryptedToken)
        {
            string decrypted = CriptografiaHelper.DecryptString(encryptedToken);
            var dictionary = ToDictionary(decrypted);

            return new AuthenticationToken(dictionary["AppId"], dictionary["UserId"]);
        }

        public static Dictionary<string, string> ToDictionary(string keyValue)
        {
            return keyValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split('='))
                .ToDictionary(split => split[0], split => split[1]);
        }

        #endregion
    }
}
