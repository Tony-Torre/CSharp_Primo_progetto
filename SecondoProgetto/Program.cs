using SecondoProgetto.car;
using SecondoProgetto.carMnager;
using SecondoProgetto.school;
using SecondoProgetto.schoolManager;
using SecondoProgetto.User;
using Libprogetto;
using NLog;
using SecondoProgetto.employees;
using System.Configuration;

namespace SecondoProgetto
{
    internal class Program
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static DateTime dateTime = DateTime.Now;
        static void Main(string[] args)
        {
            //Console.WriteLine("data formato normale " + dateTime + "\n formato data short " + dateTime.ToShortDateString() + "\n data tra 3 giorni " + dateTime.AddDays(3) + "\n test " + dateTime.GetDateTimeFormats());





            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);

            // Apply config           
            NLog.LogManager.Configuration = config;

            StudentManager studentManager = new StudentManager();
            CarManager carManager = new();
            UserManager userManager = new();
            LibraryProject library = new();
            EmployeesManager employeesManager = new();
            library.CreateFileError();
            userManager.CreateFileUser(userManager);
            library.onlineCheckDB(ConfigurationManager.AppSettings["DBServer"]);
            employeesManager.ImportDatafromDB();
            studentManager.InsertStudent(1, "Davide", "History", Convert.ToDateTime("05/07/1999"));
            studentManager.InsertStudent(2, "Sofia", "Math", Convert.ToDateTime("05/07/1999"));
            studentManager.InsertStudent(3, "Giulia", "English", Convert.ToDateTime("05/07/1999"));
            studentManager.InsertStudent(4, "Armando", "History", Convert.ToDateTime("05/07/1999"));
            bool studentBool = false;

            carManager.InsertCar("qwe45678", "Bmw", "M5", 1995);
            carManager.InsertCar("asd12345", "Lancia", "Ypsilon", 1995);

            //esercizio lampo
            //****************************************************
            //bool reverse = false;
            //string newParola = "";
            //string parola = Console.ReadLine();
            //for (int i =0 ; i <= parola.Length - 1; i++)
            //{
            //    newParola = parola[i] + newParola;
            //}
            //if ( parola == newParola )
            //{
            //    Console.WriteLine("la parola è palindroma " +parola);
            //} else Console.WriteLine(reverse);
            //****************************************************

            bool carBool = false;
            bool program = false;
            Console.WriteLine("Menù");
            Console.WriteLine("Inserisci un comando :");
            try
            {

                while (program != true)
                {
                    Console.WriteLine("A) Gestione auto \n" +
                        "B) Gestione studenti \n" +
                        "C) Gestione Dipendenti\n" +
                        "D) Accedi\n" +
                        "E) Esci dal programma");
                    string firstCommand = Console.ReadLine().ToLower();

                    //Gestione Auto
                    if (firstCommand == "a")
                    {
                        Console.Clear();
                        while (carBool != true)
                        {

                            Console.WriteLine(
                                "A) Inserisci uno auto \n" +
                                "B) Visualizza tutte le auto \n" +
                                "C) Cerca un'auto \n" +
                                "D) Inserisci auto da File esterno\n" +
                                "E) Inserisci caratteristiche auto già esistente da file esterno\n" +
                                "F) Inserisci caratteristiche auto già esistente \n" +
                                "G) Elimina un auto \n" +
                                "H) Modifica auto \n" +
                                "I) Esporta auto in un file\n" +
                                "L) Torna indietro\n" +
                                "X) Esci dal programma");
                            string command = Console.ReadLine().ToLower();
                            //Inserire un auto e caratteristiche
                            if (command == "a")
                            {
                                Console.Clear();
                                carManager.CreateCar();
                            }
                            //Visualizza tutte le auto
                            else if (command == "b")
                            {
                                Console.Clear();
                                carManager.ShowCars();
                            }
                            //Cerca singola auto per modello o anno di produzione e mostra le caratteristiche
                            else if (command == "c")
                            {
                                Console.Clear();
                                carManager.SearchCar();

                            }
                            //Inserire auto da file esterno
                            else if (command == "d")
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci il percorso del file che vuoi caricare");
                                string path = Console.ReadLine();
                                carManager.InsertCarFromFile(path);
                            }
                            //Inserire caratteristiche auto da file esterno
                            else if (command == "e")
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci il percorso del file che vuoi caricare");
                                string path1 = Console.ReadLine();
                                carManager.InsertCharacteristFromFile(path1);
                            }
                            //Inserire caratteristiche auto
                            else if (command == "f")
                            {
                                Console.Clear();
                                carManager.InsertCharacterist();

                            }
                            //Eliminare auto tramite numero telaio
                            else if (command == "g")
                            {
                                carManager.DeleteCar();

                            }
                            //Modificare parametri auto
                            else if (command == "h")
                            {
                                carManager.ChangeParameter();
                            }
                            //Esportare file auto in un file
                            else if (command == "i")
                            {
                                Console.WriteLine("Inserisci il path");
                                string path = Console.ReadLine();
                                library.ExportDataInFileXML(CarManager.cars, path, "auto.xml");
                            }
                            //Torna indietro
                            else if (command == "l")
                            {
                                Console.Clear();
                                carBool = true;
                            }
                            //Esci dal programma
                            else if (command == "x")
                            {
                                Console.Clear();
                                Console.WriteLine("Sei uscito dal programma");
                                carBool = true;
                                program = true;
                            }
                            //Parametro non valido
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci un comando valido");
                            }
                        }
                    }
                    //Gestione Studenti
                    else if (firstCommand == "b")
                    {
                        Console.Clear();
                        while (studentBool != true)
                        {
                            Console.WriteLine(
                                "A) Inserisci uno studente \n" +
                                "B) Visualizza gli studenti \n" +
                                "C) Cerca uno studente \n" +
                                "D) Elimina uno studente \n" +
                                "E) Torna indietro");
                            string command = Console.ReadLine().ToLower();
                            if (command == "a")
                            {
                                studentManager.CreateStudent();

                            }
                            else if (command == "b")
                            {
                                Console.Clear();
                                Console.WriteLine("Lista degli studenti : \n");
                                studentManager.ShowStudents();
                                Console.WriteLine();
                                Console.WriteLine("Inserisci un comando :");

                            }
                            else if (command == "c")
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci l'ID dello studente da cercare");
                                string code = Console.ReadLine();
                                double id = 0;
                                double.TryParse(code, out id);
                                while (id == 0)
                                {
                                    Console.WriteLine("L'ID deve essere valido");
                                    code = Console.ReadLine();
                                    id = 0;
                                    double.TryParse(code, out id);
                                }

                                studentManager.SearchStudent(id);

                            }
                            else if (command == "d")
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci l'ID dello studente da eliminare");
                                string code = Console.ReadLine();
                                double id = 0;
                                double.TryParse(code, out id);
                                while (id == 0)
                                {
                                    Console.WriteLine("L'ID deve essere valido");
                                    code = Console.ReadLine();
                                    id = 0;
                                    double.TryParse(code, out id);
                                }
                                studentManager.DeleteStudent(id);
                            }
                            else if (command == "e")
                            {
                                Console.Clear();
                                Console.WriteLine("Sei uscito dal programma");
                                studentBool = true;

                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Hai inserito un commando sbagliato. Inserisci un comando :");
                            }
                        }
                    }
                    //Gestione dipendenti
                    else if (firstCommand == "c")
                    {
                        bool employeeBool = false;
                        Console.Clear();
                        while (employeeBool != true)
                        {
                            Console.WriteLine(
                                "A) Inserisci dati dei dipendenti \n" +
                                "B) Visualizza i dati dei dipendenti \n" +
                                "C) Visualizza statistiche dei dipendenti \n" +
                                "D) Copia i dati in un file Json \n" +
                                "E) Torna indietro\n" +
                                "X) Esci dal programma");
                            string command = Console.ReadLine().ToLower();
                            if (command == "a")
                            {
                                EmployeesManager.ImportDataFromTXT();
                            }
                            else if (command == "b")
                            {
                                
                                bool choice = false;
                                do
                                {
                                    Console.Clear();
                                    Console.WriteLine("Inserisci un comando\n" +
                                        "A) Visualizza i dipendenti\n" +
                                        "B) Modifica i dati di un dipendente\n" +
                                        "C) Elimina i dati di un dipendente\n" +
                                        "D) Torna indietro");
                                    string select = Console.ReadLine().ToLower();
                                    if (select == "a")
                                    {
                                        employeesManager.ShowEmployees();
                                    }
                                    else if (select == "b")
                                    {
                                        employeesManager.EditEmployeeData();
                                    }
                                    else if (select == "c")
                                    {
                                        employeesManager.DeleteEmployee();
                                    }
                                    else if (select == "d")
                                    {
                                        choice = true;
                                    } else
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Parametro non corretto\n");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                } while (choice != true);
                                  
                            }
                            else if (command == "c")
                            {
                                EmployeesManager.MenuDetail();
                            }
                            else if (command == "d")
                            {
                                LibraryProject.ExportDataInFileJson(EmployeesManager.employees);
                            }
                            else if (command == "e")
                            {
                                Console.Clear();
                                employeeBool = true;

                            }
                            else if (command == "x")
                            {
                                Console.Clear();
                                Console.WriteLine("Sei uscito dal programma");
                                employeeBool = true;
                                program = true;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Hai inserito un commando sbagliato. Inserisci un comando :");
                            }
                        }
                    }
                    //Login utente
                    else if (firstCommand == "d")
                    {
                        Console.Clear();
                        

                        bool login = false;

                        try
                        {

                            
                            while (login != true)
                            {
                                Console.WriteLine("A) Registrati\n" +
                                    "B) Login\n" +
                                    "C) Torna indietro\n" +
                                    "D) Esci dal programma");
                                string choice = string.Empty;
                                choice = Console.ReadLine().ToLower();
                                //registrazione
                                if (choice == "a")
                                {
                                    string user = userManager.Username();
                                    string password = library.Hashpassword();

                                    userManager.CreateUser(user, password);
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Utente {user} creato\n");
                                    Console.ForegroundColor = ConsoleColor.White;

                                }
                                //Login
                                else if (choice == "b")
                                {
                                    Console.Clear();
                                    User.User? user;
                                    string username = library.UsernameLogin();
                                    if((user = userManager.users.Find(person => person.Username == username)) == null)
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor= ConsoleColor.Red;
                                        Console.WriteLine("Utente non trovato\n");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else
                                    {
                                        string password = library.Hashpassword();
                                        if (user.Password == password)
                                        {
                                            Console.Clear();
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine($"{username} ti sei loggato\n");
                                            Console.ForegroundColor = ConsoleColor.White;
                                            login = true;
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("Parametri non corretti");
                                        }
                                    }
                                }
                                //Torna indietro
                                else if (choice == "c")
                                {
                                    Console.Clear ();
                                    login = true;
                                }
                                //Esci dal programma
                                else if (choice == "d")
                                {
                                    Console.Clear ();
                                    Console.WriteLine("Sei uscito dal programma");
                                    login = true;
                                    program = true;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Comando Errato\n");
                                    Console.ForegroundColor= ConsoleColor.White;
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine (ex.Message);
                            LibraryProject.StampError(ex.Message);
                        }
                    }
                    //Uscita dal programma
                    else if (firstCommand == "e")
                    {
                        Console.Clear();
                        Console.WriteLine("Sei uscito dal programma");
                        program = true;

                    }
                    else { Console.WriteLine("Inserisci un comando corretto"); }



                }

                return;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                LibraryProject.StampError(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}