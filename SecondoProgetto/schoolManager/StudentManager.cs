using Libprogetto;
using SecondoProgetto.school;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SecondoProgetto.schoolManager
{
    internal class StudentManager
    {
        //Student student1 = new();
        List<Student> students = new();
        LibraryProject libraryProject = new();
        public void InsertStudent(int id, string name, string study, DateTime birthday)
        {
            
            students.Add(new Student(id, name, study, birthday));
        }
        /// <summary>
        /// Create a new student from program
        /// </summary>
        public void CreateStudent()
        {
            Console.Clear();
            int id = 0;
            if (students.Count() > 0)
            {
                id = students.Last().Id + 1;
            }
            
            Console.WriteLine("Inserisci il nome del nuovo studente");
            string name = Console.ReadLine();
            name = LibraryProject.Capitalize(name);
            while (name.Length<3 || name.Length>15)
            {
                Console.WriteLine("Il nome deve contenere almeno 3 caratteri e non più di 15");
                name = Console.ReadLine();
                name = LibraryProject.Capitalize(name);
            }
            Console.WriteLine("Inserisci il corso di studio dello studente");
            string study = Console.ReadLine();
            study = LibraryProject.Capitalize(study);
            while (study.Length < 3 || study.Length > 15)
            {
                Console.WriteLine("Il nome del corso deve contenere almeno 3 caratteri e non più di 15");
                study = Console.ReadLine();
                study = LibraryProject.Capitalize(study);
            }
            Console.WriteLine("Inserisci il giorno della tua nascita");
            string day = Console.ReadLine();
            double dayB = 0;
            double.TryParse(day, out dayB);
            while (dayB > 31 || dayB == 0)
            {
                Console.WriteLine("Il giorno deve essere compreso tra 1 e 31");
                day = Console.ReadLine();
                dayB = 0;
                double.TryParse(day, out dayB);
            }
            Console.WriteLine("Inserisci il mese della tua nascita");
            string mounth = Console.ReadLine();
            double mounthB = 0;
            double.TryParse(mounth, out mounthB);
            while (mounthB > 12)
            {
                Console.WriteLine("Il giorno deve essere compreso tra 1 e 12");
                mounth = Console.ReadLine();
                mounthB = 0;
                double.TryParse(mounth, out mounthB);
            }
            Console.WriteLine("Inserisci l'anno della tua nascita");
            string year = Console.ReadLine();
            double yearB = 0;
            double.TryParse(year, out yearB);
            double yearToday = DateTime.Now.Year;
            double age = yearToday - yearB;
            while (yearB == 0 || year.Length != 4 || (age < 10 || age>100))
            {
                Console.WriteLine("L'anno deve essere inserito con 4 cifre tra 1923 e 2013");
                year = Console.ReadLine();
                yearB = 0;
                double.TryParse(year, out yearB);
                yearToday = DateTime.Now.Year;
                age = yearToday - yearB;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Studente aggiunto correttamente");
            Console.ForegroundColor = ConsoleColor.White;

            students.Add(new Student(id, name, study, Convert.ToDateTime($"{dayB}/{mounthB}/{yearB}")));
        }

        /// <summary>
        /// Show all student from program
        /// </summary>
        public void ShowStudents()
        {
            students.ForEach(obj => Console.WriteLine($"Dati Studente: {obj.Id} - {obj.Name} - {obj.Study} - {obj.Getbirthday()}"));
        }

        /// <summary>
        /// Found a specific element from the list by the id passed from input
        /// </summary>
        /// <param name="id"></param>
        public void SearchStudent(double id)
        {

            string elementFind = null;

            foreach (Student student in students)
            {
                if (student.Id == id)
                {
                    elementFind = student.Name;
                    Console.WriteLine($"Dati Studente: {student.Id} - {student.Name} - {student.Study} - {student.Getbirthday()} \n");
                    break;
                }

            }
            if (elementFind == null) 
            {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Lo studente cercato non esiste \n");
                    Console.ForegroundColor = ConsoleColor.White;
            }
        }
        
        /// <summary>
        /// Delete a specific element from the list by the id passed from input
        /// </summary>
        /// <param name="id"></param>
        public void DeleteStudent(double id)
        {
            string elementFind = null;
            Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (Student student in students)
                {
                    if (student.Id == id)
                    {
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine($"Sei sicuro di voler eliminare lo studente : \n \n{student.Id} - {student.Name} - {student.Study} - {student.Getbirthday()} \n \ndal database? \n\n" +
                        $"Premi Y per si, premi N per no");
                    Console.ForegroundColor = ConsoleColor.White;
                    string conferm = Console.ReadLine().ToLower();
                        elementFind = student.Name;
                    bool conf = false;
                    while (conf != true)
                    {
                        if (conferm == "n")
                        {
                            Console.Clear();
                            Console.WriteLine("Comando annullato \n");
                            conf = true;
                            break;
                        } else if (conferm == "y")
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Hai eliminato lo studente :  {student.Id} - {student.Name} - {student.Study} - {student.Getbirthday()} \n ");
                            students.RemoveAll(Student => Student.Id == id);
                            Console.ForegroundColor = ConsoleColor.White;
                            conf = true;
                            break;
                        } else
                        {
                            Console.Clear();
                            Console.WriteLine("Inserisci un comando valido \n" +
                                "Premi Y per si, premi N per no");
                            conferm = Console.ReadLine().ToLower();
                            conf = false;
                        }
                    }
                    break;
                        
                    }
                }

                

                
                
            if (elementFind == null)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Lo studente cercato non esiste \n");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        
        /// <summary>
        /// Make capitalize from a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
            
    }
}
