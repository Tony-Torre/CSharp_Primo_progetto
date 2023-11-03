using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Libprogetto
{
    public class LibraryProject
    {

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        const string path = @"C:\Users\mondo\OneDrive\Desktop\ErrorCars.txt";

        private SqlConnection sqlConnection = new();

        public bool isConnected = false;
        public void onlineCheckDB(string dbCnnString)
        {
            try
            {
                sqlConnection.ConnectionString = dbCnnString;
                sqlConnection.Open();
                isConnected = true;
                sqlConnection.Close();


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                StampError(ex.Message);
            }
        }

        public bool CheckDb()
        {
            try
            {
                if(sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.ConnectionString = ConfigurationManager.AppSettings["DBServer"];
                    sqlConnection.Open();
                    return true;
                } else if(sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    return true;
                }else
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                StampError(ex.Message);
                return false;
            }
            
        }


        public void CreateFileError()
        {
            try
            {
                if (!File.Exists(path))
                    File.Create(path);
            }
            catch (Exception ex)
            {

                StampError(ex.Message);
            }

        }
        

        public static void StampError(string message)
        {
            try
            {
                using StreamWriter sw = File.AppendText(path);
                sw.WriteLine(DateTime.Now + " " + message);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

        }

        public void ExportDataInFileXML<T>(List<T> list, string path, string nameFile)
        {


            XmlSerializer xmlSerializer = new XmlSerializer(list.GetType());
            using (var writer = new StreamWriter(path + nameFile))
            {
                xmlSerializer.Serialize(writer, list);
            }
        }

        /// <summary>
        /// Prendere il nome utente per il login
        /// </summary>
        /// <returns></returns>
        public string UsernameLogin()
        {
            string username = string.Empty;
            try
            {
                Console.Clear();

                Console.WriteLine("Inserisci il nome utente");
                username = Console.ReadLine();
                bool user = false;
                while (username.Length < 4 || username.Length > 15)
                {
                    Console.WriteLine("Il nome utente deve avere almeno 4 caratteri e non più di 15");
                    username = Console.ReadLine();
                }

            }
            catch (Exception ex)
            {

                StampError(ex.Message);
            }

            return username;

        }



        /// <summary>
        /// Prendere la password hashata
        /// </summary>
        /// <returns></returns>
        public string Hashpassword()
        {
            Console.WriteLine("Inserisci la password");
            string password = Console.ReadLine();
            string encryptPassword = string.Empty;
            try
            {
                SHA256 sHA256 = SHA256.Create();

                byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder sb = new();

                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                encryptPassword = sb.ToString();


            }
            catch (Exception ex)
            {

                StampError(ex.Message);
            }

            return encryptPassword;
        }

        public (string,string) SaltPassword (string Password)
        {
            byte[] byteSalt = new byte[8];
            RandomNumberGenerator.Fill(byteSalt);
            string encryptResult= Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: Password,
                    salt: byteSalt,
                    prf:KeyDerivationPrf.HMACSHA256,
                    iterationCount:10000,
                    numBytesRequested:16)
                );
            string salt = Convert.ToBase64String(byteSalt);
            Console.WriteLine(encryptResult);
            Console.WriteLine(salt);

            return (encryptResult, salt);
        }

        public bool CheckSaltPassword(string Password, string pass1, string saltPass)
        {
            byte[] storedPass = Convert.FromBase64String(pass1);
            byte[] storedSalt = Convert.FromBase64String(saltPass);

            string encryptResult = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: Password,
                    salt: storedSalt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 16)
                );
            if (storedPass.Length != Convert.FromBase64String(encryptResult).Length)
            {
                Console.WriteLine("Password non corretta");
                return false;
            }
            else
            {
                for (int i = 0; i < storedPass.Length; i++)
                {
                    if (storedPass[i] != Convert.FromBase64String(encryptResult)[i])
                    {
                        return false;
                    }
                }
                Console.WriteLine("password corretta");
                return true;
            }
        }

        public static void ExportDataInFileJson<T>(List<T> list)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Inserisci il path dove vuoi salvare il file");
                string path = Console.ReadLine();
                Console.WriteLine("inserisci il nome del file");
                string nameFile = Console.ReadLine();
                object jsonOut = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                using StreamWriter streamWriter = new(Path.Combine(path, nameFile + ".json"));
                streamWriter.Write(jsonOut);
                if (File.Exists(path + nameFile + ".json"))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("File creato con successo\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else 
                {
                    Console.Clear();
                    Console.WriteLine("File non creato"); 
                }
            }
            catch (Exception ex)
            {
                Console.Clear() ;
                Console.WriteLine(ex.Message);
                StampError(ex.Message);  
            }
           
        }

        /// <summary>
        /// Mettere il primo carattere maiuscolo
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Capitalize(string s) => char.ToUpper(s[0]) + s[1..];

    }
}