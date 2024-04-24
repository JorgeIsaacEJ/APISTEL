using System.Text.Json.Serialization;
using System.Text.Json;
using APISTEL.Models;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace APISTEL.Utils
{
    public class Utils
    {
        /// <summary>
        /// DateOnly and TimeOnly serialization is not supported by .NET CORE 6
        /// </summary>
        public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
        {
            public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateOnly.FromDateTime(reader.GetDateTime());
            }

            public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            {
                var isoDate = value.ToString("O");
                writer.WriteStringValue(isoDate);
            }
        }
    }
    public class Functions
    {
        #region Encrypt
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase, bool random)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            if (random)
            {
                var saltStringBytes = Generate256BitsOfRandomEntropy();
                var ivStringBytes = Generate256BitsOfRandomEntropy();

                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(Keysize / 8);
                    var engine = new RijndaelEngine(256);
                    var blockCipher = new CbcBlockCipher(engine);
                    var cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
                    var keyParam = new KeyParameter(keyBytes);
                    var keyParamWithIV = new ParametersWithIV(keyParam, ivStringBytes, 0, 32);

                    cipher.Init(true, keyParamWithIV);
                    var comparisonBytes = new byte[cipher.GetOutputSize(plainTextBytes.Length)];
                    var length = cipher.ProcessBytes(plainTextBytes, comparisonBytes, 0);

                    cipher.DoFinal(comparisonBytes, length);
                    // return Convert.ToBase64String(comparisonBytes);
                    return Convert.ToBase64String(saltStringBytes.Concat(ivStringBytes).Concat(comparisonBytes).ToArray());
                }
            }
            else
            {
                var saltStringBytes = Encoding.ASCII.GetBytes(plainText.PadRight(32, ' '));
                var ivStringBytes = Encoding.ASCII.GetBytes(plainText.PadRight(32, ' '));

                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(Keysize / 8);
                    var engine = new RijndaelEngine(256);
                    var blockCipher = new CbcBlockCipher(engine);
                    var cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
                    var keyParam = new KeyParameter(keyBytes);
                    var keyParamWithIV = new ParametersWithIV(keyParam, ivStringBytes, 0, 32);

                    cipher.Init(true, keyParamWithIV);
                    var comparisonBytes = new byte[cipher.GetOutputSize(plainTextBytes.Length)];
                    var length = cipher.ProcessBytes(plainTextBytes, comparisonBytes, 0);

                    cipher.DoFinal(comparisonBytes, length);
                    // return Convert.ToBase64String(comparisonBytes);
                    return Convert.ToBase64String(saltStringBytes.Concat(ivStringBytes).Concat(comparisonBytes).ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                var engine = new RijndaelEngine(256);
                var blockCipher = new CbcBlockCipher(engine);
                var cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
                var keyParam = new KeyParameter(keyBytes);
                var keyParamWithIV = new ParametersWithIV(keyParam, ivStringBytes, 0, 32);

                cipher.Init(false, keyParamWithIV);
                var comparisonBytes = new byte[cipher.GetOutputSize(cipherTextBytes.Length)];
                var length = cipher.ProcessBytes(cipherTextBytes, comparisonBytes, 0);

                cipher.DoFinal(comparisonBytes, length);
                //return Convert.ToBase64String(saltStringBytes.Concat(ivStringBytes).Concat(comparisonBytes).ToArray());

                var nullIndex = comparisonBytes.Length - 1;
                while (comparisonBytes[nullIndex] == (byte)0)
                    nullIndex--;
                comparisonBytes = comparisonBytes.Take(nullIndex + 1).ToArray();


                var result = Encoding.UTF8.GetString(comparisonBytes, 0, comparisonBytes.Length);

                return result;
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
        #endregion

        /// <summary>
        /// Actualiza cliente/servicios
        /// </summary>
        /// <param name="ppClientsService"></param>
        /// <param name="ppClientsService_old"></param>
        public PpClientsServicesHistoric SetHistoricClientsServices(PpClientsService ppClientsService, PpClientsService? ppClientsService_old = null)
        {
            PpClientsServicesHistoric ppClientsServicesHistoric = new PpClientsServicesHistoric();
            string message = string.Empty;
            ppClientsServicesHistoric.PpcsId = ppClientsService.PpcsId; //ID del cliente/servicio historico
            ppClientsServicesHistoric.PpcId = ppClientsService.PpcId; //ID del cliente historico
            ppClientsServicesHistoric.PpsId = ppClientsService.PpsId; //ID del servicio historico
            ppClientsServicesHistoric.PpscId = ppClientsService.PpscId; //ID del esquema historico
            ppClientsServicesHistoric.PpstId = ppClientsService.PpstId; //Estatus del cliente/servicio historico
            ppClientsServicesHistoric.PpcshPay = ppClientsService.PpcsPay; //Monto de pago del cliente/servicio historico
            ppClientsServicesHistoric.PpcshDatePay = ppClientsService.PpcsDatePay; //Fecha de pago del cliente/servicio historico
            ppClientsServicesHistoric.PpcshDateCrete = DateTime.Now; //Fecha de creacion del cliente/servicio historico

            if (ppClientsService_old == null)
            {
                ppClientsServicesHistoric.PpcshChange = "{Registrado por el Admin}";
            }
            else
            {
                //Cambio de servicio
                if (ppClientsService_old.PpsId != ppClientsServicesHistoric.PpsId)
                {
                    message = "{Se modifico el servicio " + ppClientsService_old.PpsId.ToString() + " por " + ppClientsServicesHistoric.PpsId.ToString() + "}";
                }
                //Cambio de Esquema
                if (ppClientsService_old.PpscId != ppClientsServicesHistoric.PpscId)
                {
                    message = message + "{Se modifico el esquema " + ppClientsServicesHistoric.PpscId.ToString() + " por " + ppClientsServicesHistoric.PpscId.ToString() + "}";
                }
                //Cambio de estatus
                if (ppClientsService_old.PpstId != ppClientsServicesHistoric.PpstId)
                {
                    message = message + "{Se modifico el status " + ppClientsService_old.PpstId.ToString() + " por " + ppClientsServicesHistoric.PpstId.ToString() + "}";
                }
                //Cambio el monto de pago
                if (ppClientsService_old.PpcsPay != ppClientsServicesHistoric.PpcshPay)
                {
                    message = message + "{Se modifico el monto de pago " + ppClientsService_old.PpcsPay.ToString() + " por " + ppClientsServicesHistoric.PpcshPay.ToString() + "}";
                }
                //Cambio la fecha de pago
                if (ppClientsService_old.PpcsDatePay.ToString() != ppClientsServicesHistoric.PpcshDatePay.ToString())
                {
                    message = message + "{Se modifico la fecha de pago " + ppClientsService_old.PpcsDatePay.ToString() + " por " + ppClientsServicesHistoric.PpcshDatePay.ToString() + "}";
                }
                ppClientsServicesHistoric.PpcshChange = message;
            }
            return ppClientsServicesHistoric;
        }
        /// <summary>
        /// Devuelve el historial de cambios
        /// </summary>
        /// <param name="ppClientsServicesHistoric"></param>
        /// <returns></returns>
        public List<PpClientsServicesHistoricDetaill> GetHistoricClientsServices(List<PpClientsServicesHistoric> ppClientsServicesHistoric)
        {
            List<PpClientsServicesHistoricDetaill> PpClientsServicesHistoricDetaill = new List<PpClientsServicesHistoricDetaill>();
            foreach (var historic in ppClientsServicesHistoric)
            {
                PpClientsServicesHistoricDetaill ppClientsServicesHistoricDetaill = new PpClientsServicesHistoricDetaill();
                ppClientsServicesHistoricDetaill.PpcsId = historic.PpcsId; //ID del cliente/servicio historico
                ppClientsServicesHistoricDetaill.PpcId = historic.PpcId; //ID del cliente historico
                ppClientsServicesHistoricDetaill.PpsId = historic.PpsId; //ID del servicio historico
                ppClientsServicesHistoricDetaill.PpscId = historic.PpscId; //ID del esquema historico
                ppClientsServicesHistoricDetaill.PpstId = historic.PpstId; //Estatus del cliente/servicio historico
                ppClientsServicesHistoricDetaill.PpcshPay = historic.PpcshPay; //Monto de pago del cliente/servicio historico
                ppClientsServicesHistoricDetaill.PpcshDatePay = historic.PpcshDatePay; //Fecha de pago del cliente/servicio historico
                ppClientsServicesHistoricDetaill.PpcshDateCrete = historic.PpcshDateCrete; //Fecha de creacion del cliente/servicio historico

                if (historic.PpcshChange.Length > 1)
                {
                    List<string> change = new List<string>();
                    var split = historic.PpcshChange.ToString().Split('{', '}');
                    foreach (var item in split)
                    {
                        if (item.Length > 1)
                        {
                            change.Add(item);
                        }
                    }
                    ppClientsServicesHistoricDetaill.PpcshChange = change;
                }
                PpClientsServicesHistoricDetaill.Add(ppClientsServicesHistoricDetaill);
            }
            return PpClientsServicesHistoricDetaill;
        }
    }
}
