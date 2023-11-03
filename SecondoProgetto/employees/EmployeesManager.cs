using Libprogetto;
using Newtonsoft.Json;
using SecondoProgetto.User;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecondoProgetto.employees
{
    public abstract class EmployeesData
    {
        public static void ShowEmployees()
        {

        }

        /// <summary>
        /// Modifica dei dati di un dipendente
        /// </summary>
        public static void EditEmployeeData()
        {

        }

        /// <summary>
        /// Eliminare i dati dell'utente
        /// </summary>
        public static void DeleteEmployee()
        {

        }
    }
    internal class EmployeesManager : EmployeesData
    {
        public static List<Employees> employees = new();

        LibraryProject project = new();

        SqlConnection sqlConnection = new(ConfigurationManager.AppSettings["DBServer"]);
        public void ImportDatafromDB()
        {
            SqlTransaction transaction = null;
            try
            {
                if (project.CheckDb())
                {
                    sqlConnection.Open();
                    transaction = sqlConnection.BeginTransaction();

                    using (SqlCommand cmd = new("SELECT * FROM AnagraficaGenerale", sqlConnection))
                    {
                        cmd.Transaction = transaction;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader[1].ToString().Substring(0, reader[1].ToString().IndexOf(' '));
                                string surname = reader[1].ToString().Substring(reader[1].ToString().IndexOf(' '));
                                employees.Add(new(reader[0].ToString(), name, surname, int.Parse(reader[4].ToString()), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), reader[9].ToString(), reader[2].ToString(), reader[3].ToString()));
                            }
                            reader.Close();
                        }
                    }
                    using (SqlCommand cmd = new("SELECT * FROM AttivitaDipendente", sqlConnection))
                    {
                        cmd.Transaction = transaction;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AddWorkData(reader[4].ToString(), DateTime.Parse(reader[1].ToString()), reader[2].ToString(), double.Parse(reader[3].ToString()));
                            }
                        }
                    }
                    transaction.Commit();
                    sqlConnection.Close();
                }


            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex.Message);
                LibraryProject.StampError(ex.Message);
            }


        }


        /// <summary>
        /// Aggiunge i dati da un file txt, Anagrafica e ruoli se la riga contiene 10 parametri, Attività se ne contiene 4
        /// </summary>
        public static void ImportDataFromTXT()
        {
            try
            {
                Console.WriteLine("Inserisci il path del file");
                string path = Console.ReadLine();
                while (!File.Exists(path))
                {
                    Console.Clear();
                    Console.WriteLine("Inserisci il path corretto del file");
                    path = Console.ReadLine();

                }

                string[] line = File.ReadAllLines(path);

                foreach (string dati in line)
                {
                    string[] datiPerson = dati.Split(';');
                    if (datiPerson.Length == 10)
                    {
                        if (datiPerson[0].Length == 4 && Char.IsLetter(datiPerson[0][0]) && int.TryParse(datiPerson[0][1..], out _))
                        {
                            Employees employee = employees.Find(x => x.Id == datiPerson[0]);

                            if (employee != null)
                            {
                                continue;
                            }
                            else
                            {
                                string name = datiPerson[1].Substring(0, datiPerson[1].IndexOf(' '));
                                string surname = datiPerson[1].Substring(datiPerson[1].IndexOf(' '));

                                employees.Add(new(datiPerson[0], name, surname, int.Parse(datiPerson[4]), datiPerson[5], datiPerson[6], datiPerson[7], datiPerson[8], datiPerson[9], datiPerson[2], datiPerson[3]));
                            }
                        }
                    }
                    else if (datiPerson.Length == 4)
                    {
                        if (datiPerson[3].Length == 4 && Char.IsLetter(datiPerson[3][0]) && int.TryParse(datiPerson[3][1..], out _))
                        {
                            AddWorkData(datiPerson[3], DateTime.Parse(datiPerson[0]), datiPerson[1], int.Parse(datiPerson[2]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Messaggio d'errore {ex.Message}");
                LibraryProject.StampError(ex.Message);
            }

        }

        /// <summary>
        /// Aggiunge le attività ai lavoratori
        /// </summary>
        /// <param matricola="id"></param>
        /// <param Giorno del lavoro="Date"></param>
        /// <param attività="Activity"></param>
        /// <param ore="Hours"></param>
        public static void AddWorkData(string id, DateTime Date, string Activity, double Hours)
        {
            try
            {
                Employees employee = employees.Find(x => x.Id == id);
                employee?.Works.Add(new(Date, Activity, Hours));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Messaggio d'errore {ex.Message}");
                LibraryProject.StampError(ex.Message);
            }
        }

        /// <summary>
        /// Mostra a video l'anagrafica dei dipendenti e la logica
        /// </summary>
        public void ShowEmployees()
        {
            try
            {
                Console.Clear();
                foreach (Employees employee in employees)
                {
                    Console.WriteLine($"Matricola : {employee.Id}\n" +
                        $"Nome : {employee.Name}   Cognome : {employee.Surname}   Età : {employee.Age}\n" +
                        $"Indirizzo : {employee.Address}, {employee.City}, {employee.PostalCode}, {employee.Province}\n" +
                        $"Telefono : {employee.Phone}\n");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Ruolo : {employee.Role}   Reparto : {employee.Department}\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("-----------------------------------------------------------------------------------------\n");
                }
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Messaggio d'errore {ex.Message}");
                LibraryProject.StampError(ex.Message);
            }

        }

        /// <summary>
        /// Modifica dei dati di un dipendente
        /// </summary>
        public void EditEmployeeData()
        {
            try
            {
                Console.Clear();
                bool matricola = false;
                while (matricola != true)
                {
                    Console.WriteLine("Inserisci la matricola del dipendente di cui vuoi modificare i dati o premi x per uscire");

                    string id = LibraryProject.Capitalize(Console.ReadLine());
                    Employees employee = employees.Find(x => x.Id == id);
                    if (employee != null)
                    {
                        Dictionary<string, string> dict = new()
                        {
                            { "Matricola", employee.Id },
                            { "Indirizzo", employee.Address },
                            { "Città", employee.City },
                            { "CAP", employee.PostalCode },
                            { "Provincia", employee.Province },
                            { "Telefono", employee.Phone },
                            { "Ruolo", employee.Role },
                            { "Reparto", employee.Department }
                        };

                        Console.Clear();
                        bool edit = false;
                        while (edit != true)
                        {
                            int numero = 1;
                            Console.WriteLine($"Nominativo: {employee.Name} {employee.Surname}");
                            foreach (KeyValuePair<string, string> valore in dict)
                            {
                                if (valore.Key == "Matricola")
                                {
                                    continue;
                                }
                                Console.Write($"{numero}) {valore.Key} : {valore.Value} \n");
                                numero++;
                            }
                            Console.WriteLine("X) Torna al menù");
                            string choice = Console.ReadLine().ToLower();

                            if (choice == "1")
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci il nuovo indirizzo");
                                dict["Indirizzo"] = Console.ReadLine();
                                Console.Clear();
                            }
                            else if (choice == "2")
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci la nuova città");
                                dict["Città"] = Console.ReadLine();
                                Console.Clear();

                            }
                            else if (choice == "3")
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci il nuovo CAP");
                                dict["CAP"] = Console.ReadLine();
                                Console.Clear();

                            }
                            else if (choice == "4")
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci la nuova provincia");
                                dict["Provincia"] = Console.ReadLine();
                                Console.Clear();

                            }
                            else if (choice == "5")
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci il nuovo numero di telefono");
                                dict["Telefono"] = Console.ReadLine();
                                Console.Clear();

                            }
                            else if (choice == "6")
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci il nuovo ruolo");
                                dict["Ruolo"] = Console.ReadLine();
                                Console.Clear();

                            }
                            else if (choice == "7")
                            {
                                Console.Clear();
                                Console.WriteLine("Inserisci il nuovo reparto");
                                dict["Reparto"] = Console.ReadLine();
                                Console.Clear();

                            }
                            else if (choice == "x")
                            {
                                bool select = false;
                                while (select != true)
                                {
                                    Console.WriteLine("Vuoi confermare le modifiche? Y/N");
                                    string confirm = Console.ReadLine().ToLower();
                                    if (confirm == "y")
                                    {
                                        employee.Address = dict["Indirizzo"];
                                        employee.City = dict["Città"];
                                        employee.Province = dict["Provincia"];
                                        employee.PostalCode = dict["CAP"];
                                        employee.Phone = dict["Telefono"];
                                        employee.Role = dict["Ruolo"];
                                        employee.Department = dict["Reparto"];
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Modifiche effettuate con successo");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.ReadKey();
                                        EditFromDB(dict);
                                        edit = true;
                                        matricola = true;
                                        select = true;
                                    }
                                    else if (confirm == "n")
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Modifiche annullate");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.ReadKey();
                                        edit = true;
                                        matricola = true;
                                        select = true;
                                    }
                                }

                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Comando non corretto\n");
                            }

                        }

                    }
                    else if (id == "X")
                    {
                        Console.Clear();
                        matricola = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Non è stato trovato nessun dipendente con questa matricola\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Messaggio d'errore {ex.Message}");
                LibraryProject.StampError(ex.Message);
            }

        }

        /// <summary>
        /// Modifica dei dati di un database
        /// </summary>
        /// <param name="dict"></param>
        public void EditFromDB(Dictionary<string, string> dict)
        {
            SqlTransaction transaction = null;
            try
            {
                if (project.CheckDb())
                {
                    sqlConnection.Open();
                    transaction = sqlConnection.BeginTransaction();

                    using (SqlCommand cmd = new("UPDATE [dbo].[AnagraficaGenerale]" +
                        "   SET " +
                        "       [Ruolo] = @Ruolo" +
                        "      ,[Reparto] = @Reparto" +
                        "      ,[Indirizzo] = @Indirizzo" +
                        "      ,[Città] = @Città" +
                        "      ,[Provincia] = @Provincia" +
                        "      ,[CAP] = @CAP" +
                        "      ,[Telefono] = @Telefono" +
                        " WHERE Matricola = @Matricola", sqlConnection))
                    {
                        cmd.Transaction = transaction;
                        foreach (KeyValuePair<string, string> kvp in dict)
                        {
                            SqlParameter sqlParameter = new()
                            {
                                ParameterName = kvp.Key,
                                Value = kvp.Value
                            };
                            cmd.Parameters.Add(sqlParameter);
                        }

                        cmd.ExecuteNonQuery();
                        transaction.Commit();

                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LibraryProject.StampError(ex.Message);
                transaction.Rollback();
            }
        }

        /// <summary>
        /// Eliminare i dati dell'utente
        /// </summary>
        public void DeleteEmployee()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Inserisci la matricola del dipendente di cui vuoi eliminare i dati o premi x per uscire");
                bool delete = false;
                while (delete != true)
                {
                    string id = LibraryProject.Capitalize(Console.ReadLine());

                    if (id.Length == 4)
                    {
                        Employees employee = employees.Find(x => x.Id == id);
                        if (employee != null)
                        {
                            employees.Remove(employee);
                            Console.WriteLine("Utente Eliminato\n");
                            DeleteEmployeeFromDB(employee.Id);
                            delete = true;
                        }
                        else
                            Console.WriteLine("Non è stato trovato nessun dipendente con questa matricola");

                    }
                    else if (id == "X")
                    {
                        delete = true;
                    }
                    else { Console.WriteLine("La matricola inserita non è corretta"); }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Messaggio d'errore {ex.Message}");
                LibraryProject.StampError(ex.Message);
            }
        }

        /// <summary>
        /// Eliminazione da un database dei dati di una persona passata tramite matricola
        /// </summary>
        /// <param name="matricola"></param>
        public void DeleteEmployeeFromDB(string matricola)
        {

            SqlTransaction transaction = null;
            try
            {
                if (project.CheckDb())
                {
                    sqlConnection.Open();
                    transaction = sqlConnection.BeginTransaction();
                    using (SqlCommand cmd = new("DELETE FROM [dbo].[AnagraficaGenerale]" +
                        "WHERE Matricola = @Matricola",sqlConnection))
                    {
                        cmd.Transaction = transaction;
                        SqlParameter sqlParameter = new()
                        {
                            ParameterName = "@Matricola",
                            Value = matricola
                        };
                        cmd.Parameters.Add(sqlParameter);
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LibraryProject.StampError(ex.Message);
                transaction.Rollback();

            }
        }
        /// <summary>
        /// Menù dei dettagli
        /// </summary>
        public static void MenuDetail()
        {
            bool choice = false;
            Console.Clear();
            do
            {

                Console.WriteLine("1) Età media personale\n" +
                    "2) Età media per reparto\n" +
                    "3) Totale ore lavoro Reparto\n" +
                    "4) Totale ore lavoro per Nominativo\n" +
                    "5) Totale ore straordinarie\n" +
                    "6) Totale Ore straordinarie per Nominativo\n" +
                    "7) Totale ore ferie\n" +
                    "8) Ore ferie per Nominativo\n" +
                    "9) Ore Prefestive con Data e Nominativo\n" +
                    "X) Torna indietro");
                string detail = Console.ReadLine().ToLower();
                if (detail == "1")
                {
                    AverageEmployees();
                }
                else if (detail == "2")
                {
                    AverageEmployeesDepartures();
                }
                else if (detail == "3")
                {
                    SumHoursDepartments();
                }
                else if (detail == "4")
                {
                    SumHoursPerson();
                }
                else if (detail == "5")
                {
                    SumExtraordinaryHours();
                }
                else if (detail == "6")
                {
                    SumExtraorinaryHoursForPerson();
                }
                else if (detail == "7")
                {
                    SumHolidays();
                }
                else if (detail == "8")
                {
                    SumHolidaysForPerson();
                }
                else if (detail == "9")
                {
                    PreHolidaysData();
                }
                else if (detail == "x")
                {
                    choice = true;
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Hai inserito un parametro non corretto\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }

            } while (choice != true);

        }

        /// <summary>
        /// Calcola l'età media dei dipendenti
        /// </summary>
        public static void AverageEmployees()
        {
            Console.Clear();
            Console.WriteLine("Età media personale : " + employees.Average(x => x.Age).ToString("#.##"));
        }

        /// <summary>
        /// Calcola età media per reparto
        /// </summary>
        public static void AverageEmployeesDepartures()
        {
            var groupedEmployees = employees.GroupBy(employee => employee.Department);
            Console.Clear();
            foreach (var group in groupedEmployees)
            {
                // Ottieni il nome del dipartimento dal raggruppamento
                string department = group.Key;
                // Calcola la media delle età nel gruppo
                double averageAge = group.Average(employee => employee.Age);

                Console.WriteLine("Reparto : " + department + ",    età media : " + averageAge.ToString("#.##"));
            }
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Somma del totale ore per reparto
        /// </summary>
        public static void SumHoursDepartments()
        {
            var groupedEmployees = employees.GroupBy(employees => employees.Department);

            Console.Clear();
            foreach (var group in groupedEmployees)
            {
                string department = group.Key;
                double TotalHours = 0;
                foreach (Employees employees in group)
                {
                    foreach (EmployeeWork work in employees.Works)
                    {
                        if (work.Activity != "Ferie")
                            TotalHours += work.Hours;
                    }
                }
                if (TotalHours > 0)
                    Console.WriteLine("Reparto : " + department + ",    Totale ore : " + TotalHours.ToString("#.##"));
            }
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Somma ore totale per persona 
        /// </summary>
        public static void SumHoursPerson()
        {
            Console.Clear();
            foreach (Employees employee in employees)
            {
                double hoursTotal = 0;

                if (employee.Works != null)
                {
                    foreach (EmployeeWork work in employee.Works)
                    {
                        if (work.Activity != "Ferie")
                        {
                            hoursTotal += work.Hours;
                        }
                    }
                }
                if (hoursTotal > 0)
                    Console.WriteLine($"{employee.Name} {employee.Surname} ha lavorato {hoursTotal} ore");

            }
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Somma le ore di straordinario totali
        /// </summary>
        public static void SumExtraordinaryHours()
        {
            double totalHours = 0;
            foreach (Employees employee in employees)
            {
                foreach (EmployeeWork work in employee.Works)
                {
                    if (work.Activity == "Pre Festivo")
                    {
                        totalHours += work.Hours;
                    }
                    else if (work.Hours > 8 && work.Activity != "Ferie")
                    {
                        totalHours += work.Hours - 8;
                    }
                }
            }
            Console.Clear();
            Console.WriteLine("Le ore di straordinario totali sono : " + totalHours.ToString("#.##"));
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Somma le ore di straordinario per persona
        /// </summary>
        public static void SumExtraorinaryHoursForPerson()
        {
            Console.Clear();
            foreach (Employees employee in employees)
            {
                double hoursTotal = 0;
                if (employee.Works != null)
                {
                    foreach (EmployeeWork work in employee.Works)
                    {
                        if (work.Activity == "Pre Festivo")
                        {
                            hoursTotal += work.Hours;
                        }
                        else if (work.Activity != "Ferie" && work.Hours > 8)
                        {
                            hoursTotal += work.Hours - 8;
                        }
                    }
                    if (hoursTotal > 0)
                    {
                        Console.WriteLine($"{employee.Name} {employee.Surname} ha fatto {hoursTotal} ore di straordinario.");
                    }
                }
            }
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Somma delle ferie totali
        /// </summary>
        public static void SumHolidays()
        {
            double totalHours = 0;
            foreach (Employees employee in employees)
            {
                foreach (EmployeeWork work in employee.Works)
                {
                    if (work.Activity == "Ferie")
                    {
                        totalHours += work.Hours;
                    }
                }
            }
            Console.Clear();
            Console.WriteLine("Le ore totali di ferie sono : " + totalHours);
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Somma delle ore di ferie per persona 
        /// </summary>
        public static void SumHolidaysForPerson()
        {
            Console.Clear();
            foreach (Employees employee in employees)
            {
                double totalHours = 0;
                foreach (EmployeeWork work in employee.Works)
                {
                    if (work.Activity == "Ferie")
                        totalHours += work.Hours;
                }
                if (totalHours > 0)
                    Console.WriteLine($"{employee.Name} {employee.Surname} ha fatto {totalHours} di ferie");
            }
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Torna data/ Nominativo e ore di lavoro in Pre festivo
        /// </summary>
        public static void PreHolidaysData()
        {
            Console.Clear();
            Console.WriteLine("Lavoro in giorni pre festivo :");
            foreach (Employees employee in employees)
            {
                foreach (EmployeeWork work in employee.Works)
                {
                    if (work.Activity == "Pre Festivo")
                    {
                        Console.WriteLine($"In data {work.Date.ToString("dd/MM/yyyy")} , {employee.Name} {employee.Surname} ha lavorato {work.Hours} ore.");
                    }
                }
            }
            Console.ReadKey();
            Console.Clear();
        }
    }


}
