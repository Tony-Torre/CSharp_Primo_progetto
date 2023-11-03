using SecondoProgetto.car;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Libprogetto;

namespace SecondoProgetto.User
{
    internal class UserManager
    {
        LibraryProject libraryProject = new();
        public List<User> users = new();

        const string pathUser = @"C:\Users\mondo\OneDrive\Desktop\User.xml";

        public void CreateFileUser(UserManager usermanager)
        {
            try
            {
                if (!File.Exists(pathUser))
                    File.Create(pathUser);
                else
                {
                    usermanager.TakeUserFromXML();

                }
            }
            catch (Exception ex)
            {
                LibraryProject.StampError(ex.Message);
                
            }

        }
        public void TakeUserFromXML()
        {
            try
            {
                XDocument document = XDocument.Load(pathUser);

                            foreach (XElement element in document.Root.Elements("User"))
                            {
                                User user1 = new User(
                                    element.Element("Username").Value,
                                    element.Element("Password").Value               
                                );

                                users.Add(user1);
                            }
            }
            catch (Exception ex)
            {

                LibraryProject.StampError(ex.Message);
            }
            

        }


        public void CreateUser(string username, string password)
        {
            try
            {
                User user1 = new User(username, password);
                users.Add(user1);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
                using (var writer = new StreamWriter(@"C:\Users\mondo\OneDrive\Desktop\User.xml"))
                {
                    xmlSerializer.Serialize(writer, users);
                }
            }
            catch (Exception ex)
            {

                LibraryProject.StampError(ex.Message);
            }
            
        }        
    



        //Registrare il nome utente unico
        public string Username()
        {
            string username = string.Empty;
            try
            {
                Console.Clear();
               
                Console.WriteLine("Inserisci il nome utente");
                username = Console.ReadLine();
                bool user = false;
                while (user != true)
                {
                    while (username.Length < 4 || username.Length > 15)
                    {
                        Console.WriteLine("Il nome utente deve avere almeno 4 caratteri e non più di 15");
                        username = Console.ReadLine();
                    }
                    if (users.Find(x => x.Username == username) != null)
                    {
                        Console.Clear();
                        Console.WriteLine("L'utente risulta già registrato");
                        username = string.Empty;
                    }
                    else
                    {
                        user = true;
                        return username;
                    }
                }
                
            }
            catch (Exception ex)
            {

                LibraryProject.StampError(ex.Message);
            }
            return username;
        }


        
    }
}
