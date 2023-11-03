using Libprogetto;
using SecondoProgetto.car;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SecondoProgetto.carMnager
{
    internal class CarManager
    {
        private static int Id = 0;
        public static List<Car.CarData> cars = new();
        LibraryProject libraryProject = new();

        /// <summary>
        /// Insert cars from external file
        /// </summary>
        /// <param name="id"></param>
        /// <param name="brand"></param>
        /// <param name="model"></param>
        /// <param name="productionYear"></param>
        public void InsertCar(string id, string brand, string model, int productionYear)
        {
            cars.Add(new Car.CarData(id, brand, model, productionYear));
        }

        /// <summary>
        /// Create car from input
        /// </summary>
        public void CreateCar()
        {
            string carId;
            bool firstCar;
            bool secondCar;
            bool control;
            do
            {
                firstCar = false;
                secondCar = false;
                control = false;
                Console.WriteLine("Inserisci il codice di telaio dell'auto");
                carId = Console.ReadLine().ToLower();
                Console.Clear();
                if (carId.Length == 8)
                {
                    firstCar = carId.Substring(0, 2).ToCharArray().ToList().TrueForAll(c => char.IsLetter(c));
                    secondCar = int.TryParse(carId[3..], out _);
                }

                if (firstCar && secondCar && carId.Length == 8)
                {
                    control = true;
                }

                else
                {
                    Console.WriteLine("Parametro non corretto\n");
                }

            } while (carId.Length != 8 || firstCar != true && secondCar != true && control != true);
            //
            string brand = CheckBrand();
            brand = LibraryProject.Capitalize(brand);
            string model = CheckModel();
            model = LibraryProject.Capitalize(model);
            int productionYear = CheckYearProduction();

            cars.Add(new Car.CarData(carId, brand, model, productionYear));
            
            Console.WriteLine("Hai i dati delle caratteristiche dell'auto? \n" +
                "Premi \"Y\" per si, premi \"N\" per no.");
            bool carChar = false;
            while (carChar != true)
            {
                string date = Console.ReadLine().ToLower();
                if (date == "y") 
                {
                    int idop = Id;
                    Id++;
                    string idCar = carId;
                    int engine = CheckEngine();
                    string fuel = CheckFuel();
                    
                    Car.CarCharacterist carCharacterist = new Car.CarCharacterist();

                    carCharacterist.Id = idop;
                    carCharacterist.CarId = idCar;
                    carCharacterist.Engine = engine;
                    carCharacterist.Fuel = fuel;

                    Car.CarData carCh = cars.Find(car => car.Id == carId);
                    if (carCh != null)
                    {
                        carCh.carCharacterist.Add(carCharacterist);
                        carChar = true;
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Dati delle caratteristiche dell'auto non inseriti \n");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"L'auto inserita è : \n" +
                            $"Marca : {brand} \n" +
                            $"Modello : {model} \n" +
                            $"Anno di produzione : {productionYear} \n");
                        Console.ForegroundColor= ConsoleColor.Yellow;
                        Console.WriteLine("Caratteristiche: \n" +
                            $"Id = {carCharacterist.Id}\n" +
                            $"Cilindrata = {carCharacterist.Engine}\n" +
                            $"Alimentazione = {carCharacterist.Fuel}\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                
                } else if (date == "n")
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Dati delle caratteristiche dell'auto non inseriti \n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"L'auto inserita è : \n" +
                        $"Marca : {brand} \n" +
                        $"Modello : {model} \n" +
                        $"Anno di produzione : {productionYear} \n");
                    Console.ForegroundColor= ConsoleColor.White;
                    carChar = true;

                } else
                {
                    Console.Clear();
                    Console.WriteLine("Inserisci un valore corretto");
                    Console.WriteLine("Hai i dati della macchina? \n" +
                        "Premi \"Y\" per si, premi \"N\" per no.");
                }
            }
        }
        
        /// <summary>
        /// Mostrare tutte le auto 
        /// </summary>
        public void ShowCars()
        {
            Console.WriteLine("Le auto sono : \n");
            foreach (Car.CarData car in cars) 
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(car.Id);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" - {car.Brand} - {car.Model} - {car.ProductionYear}\n");

            }
            Console.WriteLine();
        }
        
        /// <summary>
        /// Cercare auto tramite modello o anno di produzione
        /// </summary>
        public void SearchCar()
        {
            //Scegliere un opzione per la ricerca
            bool parameter = false;
            Console.WriteLine("Inserisci un'opzione :\n" +
                "A) Cerca per marca\n" +
                "B) Cerca per anno di produzione\n" +
                "C) Torna al Menù");
            string choice = Console.ReadLine().ToLower();
            Console.Clear();
            //while che mi obbliga la scelta della ricerca
            while (parameter != true)
            {
                //se voglio cercare per modello
                if (choice == "a")
                {
                    
                    //richiede il modello della ricerca
                    Console.WriteLine("Inserisci il modello dell'auto o premi \"x\" per uscire");
                    string brand = LibraryProject.Capitalize(Console.ReadLine());
                    List<Car.CarData> car = cars.FindAll(cars => cars.Brand == brand);
                    
                    // se il modello corrisponde a qualcosa 
                    if (car.Count > 0)
                    {
                        
                        foreach (Car.CarData singleCar in car)
                        {
                            Console.WriteLine("Dati dell'auto : \n" +
                            $"id : {singleCar.Id} \n" +
                            $"Marca : {singleCar.Brand} \n" +
                            $"Modello : {singleCar.Model} \n" +
                            $"Anno di produzione : {singleCar.ProductionYear}");
                            if (singleCar.carCharacterist != null)
                            {
                                foreach (Car.CarCharacterist character in singleCar.carCharacterist)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Caratteristiche :");
                                    Console.WriteLine($"CarId = {character.CarId}\n" +
                                        $"Cilindrata : {character.Engine}\n" +
                                        $"Alimentazione : {character.Fuel}");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                        }
                        
                        
                        Console.WriteLine();
                        parameter = true;
                    }
                    // se si preme x per uscire
                    else if (brand == "X")
                    {
                        Console.Clear();
                        Console.WriteLine("Sei uscito dal programma\n");
                        parameter = true;
                    }
                    // se non viene inserito nessuno dei parametri
                    else
                    {

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Non è stata  trovata nessun auto con questo Id. \n");
                        Console.ForegroundColor = ConsoleColor.White;

                    }

                }
                //se voglio cercare per anno
                else if (choice == "b")
                {
                    Console.Clear();

                    string yearProduction = "";

                    int year = CheckYearProduction();
                    
                    if (year == 0) { yearProduction = "x"; }

                    List<Car.CarData> car = cars.FindAll(cars => cars.ProductionYear == year);

                    // se l'anno corrisponde a qualcosa 
                    if (car.Count > 0)
                    {
                        Console.Clear();
                        foreach (Car.CarData carData in car)
                        {
                            
                            Console.WriteLine("Dati dell'auto : \n" +
                                $"id : {carData.Id} \n" +
                                $"Marca : {carData.Brand} \n" +
                                $"Modello : {carData.Model} \n" +
                                $"Anno di produzione : {carData.ProductionYear}");
                            if (carData.carCharacterist != null)
                            {
                                foreach (Car.CarCharacterist character in carData.carCharacterist)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Caratteristiche :");
                                    Console.WriteLine($"CarId = {character.CarId}\n" +
                                        $"Cilindrata : {character.Engine}\n" +
                                        $"Alimentazione : {character.Fuel}");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            Console.WriteLine();

                        }
                        
                        parameter = true;
                    }
                    // se si preme x per uscire
                    else if (yearProduction == "x")
                    {
                        Console.Clear();
                        Console.WriteLine("Sei uscito dal programma\n");
                        parameter = true;
                        break;
                    }
                    // se non viene inserito nessuno dei parametri
                    else
                    {

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Non è stata  trovata nessun auto con quest'anno di produzione. \n");
                        Console.WriteLine("Inserisci un anno corretto o premi \"x\" per uscire\n");
                        Console.ForegroundColor = ConsoleColor.White;

                    }

                } else if(choice == "c")
                {
                    Console.Clear ();
                    Console.WriteLine("Sei tornato al menù principale");
                    parameter= true;

                } else
                { 
                    Console.Clear();
                    Console.WriteLine("Inserisci un'opzione corretta :\n" +
                    "A) Cerca per modello.\n" +
                    "B) Cerca per anno di produzione\n" +
                    "C) Torna al Menù");
                    choice = Console.ReadLine().ToLower();
                }
                
            }
            
        }
        
        /// <summary>
        /// Metodo per inserire le auto tramite dati da file esterno
        /// </summary>
        /// <param name="path"></param>
        public void InsertCarFromFile(string path)
        {
            try
            {
                int parameter = 0;
                //prendo e stringhe in un array da un file
                string[] data = File.ReadAllLines(path);
                //Faccio un foreach per prendere le singole stringhe con i dati di ogni auto
                foreach (string line in data)
                {
                    //inserisco i singoli dati di ogni auto dentro un array
                    string[] datiCar = line.Split(':');
                    //controllo i dati di ogni riga
                    if (datiCar.Length == 4)
                    {
                        if (datiCar[0].GetType() == typeof(string))
                        {
                            if (datiCar[1].GetType() == typeof(string))
                            {
                                string brand = datiCar[1].ToLower();
                                brand = LibraryProject.Capitalize(brand);
                                if (datiCar[2].GetType() == typeof(string))
                                {
                                    string model = datiCar[2].ToLower();
                                    model = LibraryProject.Capitalize(model);
                                    if (datiCar[3].Length == 4)
                                    {
                                        int yearproduction = 0;
                                        int.TryParse(datiCar[3], out yearproduction);
                                        if (yearproduction != 0)
                                        {
                                            parameter++;
                                            cars.Add(new Car.CarData(datiCar[0], brand, model, yearproduction));
                                        }

                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else { continue; }
                            }
                            else { continue; }
                        }
                        else { continue; }
                    }
                }
                    
                if (parameter >0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Sono state aggiunte {parameter} auto\n");
                    Console.ForegroundColor= ConsoleColor.White;
                } else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Non sono state trovate auto da aggiungere\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception ex)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine(ex.Message);
                LibraryProject.StampError(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Metodo per inserire caratteristiche tramite file esterno di auto già esistenti
        /// </summary>
        /// <param name="path"></param>
        public void InsertCharacteristFromFile(string path)
        {
            try
            {
                int parameter = 0;
                string[] carCh = File.ReadAllLines(path);

                foreach (string car in carCh)
                {
                    string[] strings = car.Split(':');
                    //controllo che esista un codice dell'auto corrispondente al dato che mi viene dato
                    Car.CarData data = cars.Find(data => data.Id == strings[1]);
                    
                    if (data != null)
                    {
                        //controllo che l'engine sia un numero
                        int engine = 0;
                        int.TryParse((string)strings[2], out engine);
                        if ( engine != 0)
                        {
                            //Se è un numero instanzio un oggetto carCharacterist
                            Car.CarCharacterist carCharacterist = new Car.CarCharacterist();
                            //metto un Id dinamico
                            carCharacterist.Id = Id;
                            Id++;
                            carCharacterist.CarId = strings[1];
                            carCharacterist.Engine = engine;
                            carCharacterist.Fuel = strings[3];

                            parameter++;
                            data.carCharacterist.Add(carCharacterist);
                        }
                       
                    }
                    
                }
                if (parameter > 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Sono state aggiunte {parameter} caratteristiche alle auto\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Non sono state trovate caratteristiche da aggiungere\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Metodo per inserire caratteristice per auto già esistente
        /// </summary>
        public void InsertCharacterist()
        {
            Console.Clear();
            string carId;
            bool firstCar;
            bool secondCar;
            bool control;
           do
            {
                firstCar = false;
                secondCar = false;
                control = false;
                Console.WriteLine("Inserisci il codice di telaio dell'auto o premi \"x\" per uscire");
                carId = Console.ReadLine();
                Console.Clear();
                if(carId.Length == 8)
                {
                    firstCar = carId.Substring(0, 2).ToCharArray().ToList().TrueForAll(c => char.IsLetter(c));
                    secondCar = int.TryParse(carId[3..], out _);
                }
                
                if (firstCar && secondCar && carId.Length == 8)
                {
                    Car.CarData car1 = cars.Find(car => car.Id == carId);
                    
                    if(car1 != null)
                    {

                        int engine = CheckEngine();
                        string fuel = CheckFuel();
                        Car.CarCharacterist carCharacterists =new Car.CarCharacterist();
                        carCharacterists.Id = Id;
                        Id++;
                        carCharacterists.CarId= carId;
                        carCharacterists.Engine = engine;
                        carCharacterists.Fuel = fuel;         

                        car1.carCharacterist.Add(carCharacterists);

                    }
                    

                } else if ( carId == "x")
                {
                    carId = "12345678";
                    firstCar =true;
                    secondCar = true;
                    control = true;
                }
                
                else { 
                    Console.WriteLine("Parametro non corretto\n");
                }

            } while (carId.Length != 8 || firstCar != true && secondCar != true && control != true) ;
        }

        /// <summary>
        /// Metodo per eliminare l'auto tramite numero di telaio
        /// </summary>
        public void DeleteCar()
        {
            Console.Clear();
            string carId;
            bool firstCar;
            bool secondCar;
            bool control;
           do
            {
                firstCar = false;
                secondCar = false;
                control = false;
                Console.WriteLine("Inserisci il codice di telaio dell'auto o premi \"x\" per uscire");
                carId = Console.ReadLine();
                Console.Clear();
                if(carId.Length == 8)
                {
                    firstCar = carId.Substring(0, 2).ToCharArray().ToList().TrueForAll(c => char.IsLetter(c));
                    secondCar = int.TryParse(carId[3..], out _);
                }
                
                if (firstCar && secondCar && carId.Length == 8)
                {
                    if (cars.RemoveAll(car => car.Id == carId) > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Auto eliminata\n");
                        cars.RemoveAll(car => car.Id == carId);
                        Console.ForegroundColor= ConsoleColor.White;
                    } else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Auto non trovata\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        firstCar = false;
                        secondCar = false;
                    }

                } else if ( carId == "x")
                {
                    carId = "12345678";
                    firstCar =true;
                    secondCar = true;
                    control = true;
                }
                
                else { 
                    Console.WriteLine("Parametro non corretto\n");
                }

            } while (carId.Length != 8 || firstCar != true && secondCar != true && control != true) ;
        }

        /// <summary>
        /// Mi permette di modificare i parametri singolo per singolo
        /// </summary>
        public void ChangeParameter()
        {
            Console.Clear();
            string carId;
            bool firstCar;
            bool secondCar;
            bool control;
            do
            {
                firstCar = false;
                secondCar = false;
                control = false;
                bool found = false;
                Console.WriteLine("Inserisci il codice di telaio dell'auto che vuoi modificare o premi \"x\" per uscire");
                carId = Console.ReadLine().ToLower();
                Console.Clear();
                if (carId.Length == 8)
                {
                    firstCar = carId.Substring(0, 2).ToCharArray().ToList().TrueForAll(c => char.IsLetter(c));
                    secondCar = int.TryParse(carId[3..], out _);
                }

                if (firstCar && secondCar && carId.Length == 8)
                {
                    Car.CarData car1 = cars.Find(car => car.Id == carId);

                    if(car1 != null)
                    {
                        Console.Clear();
                        Console.WriteLine($"Dati dell'auto trovata : \n" +
                            $"Marca : {car1.Brand}\n" +
                            $"Modello : {car1.Model}\n" +
                            $"Anno di produzione : {car1.ProductionYear}");
                        if (car1.carCharacterist != null)
                        {
                            foreach(Car.CarCharacterist characterist in car1.carCharacterist)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Id : {characterist.Id}\n" +
                                    $"Cilindrata : {characterist.Engine}\n" +
                                    $"Alimentazione : {characterist.Fuel}");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            Console.WriteLine();
                            string parameter;
                            bool parameterOk = false;
                            while ( parameterOk != true)
                            {
                                Console.WriteLine("Cosa vuoi modificare?\n" +
                                    "1)Marca\n" +
                                    "2)Modello\n" +
                                    "3)Anno di produzione");
                                if (car1.carCharacterist.Count != 0)
                                {
                                    Console.WriteLine("4)Cilindrata\n" +
                                        "5)Alimentazione");
                                }
                                Console.WriteLine("x)Torna al menù");
                                parameter = Console.ReadLine();

                                if (parameter == "1")
                                {
                                    Console.Clear();
                                    car1.Brand = CheckBrand();
                                    Console.Clear() ;
                                    Console.ForegroundColor= ConsoleColor.Green;
                                    Console.WriteLine($"Modello auto modificato in \" {car1.Brand} \" \n");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    found = true;
                                } else if (parameter == "2")
                                {
                                    Console.Clear();
                                    car1.Model = CheckModel();
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Marca auto modificata in \" {car1.Model} \" \n");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    found = true;

                                } else if (parameter == "3")
                                {
                                    Console.Clear();
                                    car1.ProductionYear = CheckYearProduction();
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Anno di produzione dell'auto modificato in \" {car1.ProductionYear} \" \n");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    found = true;
                                } 
                                if (car1.carCharacterist.Count != 0)
                                {
                                    if (parameter == "4")
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Inserisci la nuova cilindrata dell'auto");
                                        foreach (Car.CarCharacterist carCharacterist in car1.carCharacterist)
                                        {
                                            carCharacterist.Engine= CheckEngine();
                                            Console.Clear();
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine($"Cilindrata dell'auto modificata in \" {carCharacterist.Engine} \" \n");
                                            Console.ForegroundColor = ConsoleColor.White;
                                            found = true;
                                        }

                                    }
                                    else if (parameter == "5")
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Inserisci la nuova cilindrata dell'auto");
                                        foreach (Car.CarCharacterist carCharacterist in car1.carCharacterist)
                                        {
                                            carCharacterist.Fuel = CheckFuel();
                                            Console.Clear();
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine($"Marca auto modificata in \" {carCharacterist.Fuel} \" \n");
                                            Console.ForegroundColor = ConsoleColor.White;
                                            found = true;
                                        }
                                    }
                                }
                                
                                if (parameter == "x")
                                {
                                    Console.Clear();
                                    parameterOk = true;
                                    found = true;
                                }
                                if (found == false)
                                {
                                    Console.Clear() ;
                                    Console.WriteLine("Inserisci un parametro corretto\n");
                                }
                                found = false;
                            }
                        }

                    } else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Non è stata trovata nessun auto con questo numero di telaio \n");
                        Console.ForegroundColor= ConsoleColor.White;
                        firstCar = false;
                        secondCar = false;
                        control = false;
                    }

                }
                else if (carId == "x")
                {
                    carId = "12345678";
                    firstCar = true;
                    secondCar = true;
                    control = true;
                }

                else
                {
                    Console.WriteLine("Parametro non corretto\n");
                }

            } while (carId.Length != 8 || firstCar != true && secondCar != true && control != true);
        }

        /// <summary>
        /// Metodo che salva in un file.txt tutti i dati delle auto
        /// </summary>
        public void ExportDataInFileTXT()
        {
            Console.WriteLine("Inserisci il path del cartella che vuoi inserire :");
            string path = Console.ReadLine();
            using StreamWriter streamWriter = new StreamWriter(path+"\\carsUpdate.txt");
            
            foreach (Car.CarData auto in cars)
            {

                streamWriter.Write($"{auto.Id}:{auto.Brand}:{auto.Model}:{auto.ProductionYear}");
                if(auto.carCharacterist.Count > 0)
                {
                    foreach (Car.CarCharacterist character in auto.carCharacterist)
                    {
                        streamWriter.Write($":{character.Engine}:{character.Fuel}");
                    }
                }
            
                streamWriter.WriteLine();
            }

            streamWriter.Close();
        }
        

        

        /// <summary>
        /// Metodo per controllare che il brand sia compreso tra 4 e 10 caratteri
        /// </summary>
        /// <returns></returns>
        public  string CheckBrand()
        {
            Console.WriteLine("Inserisci la marca dell'auto : ");
            string brand = LibraryProject.Capitalize(Console.ReadLine());
            while (brand.Length <= 3 || brand.Length >= 10)
            {
                Console.WriteLine("Inserisci la marca dell'auto, può avere minimo 3 caratteri e massimo 10");
                brand = Console.ReadLine();
            }

            return brand;
        }

        /// <summary>
        /// Metodo per controllare che il modello sia compreso tra 4 e 10 caratteri
        /// </summary>
        /// <returns></returns>
        public string CheckModel()
        {
            Console.WriteLine("Inserisci il modello dell'auto : ");
            string model = LibraryProject.Capitalize(Console.ReadLine());
            while (model.Length <= 3 || model.Length >= 10)
            {
                Console.WriteLine("Inserisci il modello dell'auto, può avere minimo 3 caratteri e massimo 10");
                model = Console.ReadLine();
            }

            return model;
        }

        /// <summary>
        /// Controllo per anno compreso tra 1901 e anno corrente
        /// </summary>
        /// <returns>Ritorna l'anno di produzione dell'auto</returns>
        public static int CheckYearProduction()
        {
            DateTime dateTime = DateTime.Now;
            int yearProduction = 0;
            Console.WriteLine("Inserisci l'anno di produzione dell'auto o premi \"x\" per uscire");
            string yearString = Console.ReadLine();
            int.TryParse(yearString, out yearProduction);
            while (yearProduction == 0 || (yearProduction < 1900 || yearProduction > dateTime.Year))
                {
                    Console.WriteLine($"Inserisci un anno compreso tra il 1901 e il {dateTime.Year}");
                    yearString = Console.ReadLine();
                    int.TryParse(yearString, out yearProduction);
                }
            
            

            return yearProduction;

            
        }

        /// <summary>
        /// Metodo per controllare che la cilindrata sia compresa tra 700 e 7000
        /// </summary>
        /// <returns></returns>
        public static int CheckEngine()
        {
            int engine;
            do
            {
                Console.WriteLine($"Inserisci la cilindrata dell'auto");
                engine = 0;
                string engineString = Console.ReadLine();
                int.TryParse(engineString, out engine);

            } while (engine < 50 || engine > 7000);
            
            return engine;

        }

        /// <summary>
        /// Enum con tutti i metodi per alimentazione auto
        /// </summary>
        public enum Fuel
        {
            Benzina = 1 ,
            Diesel,
            Metano,
            GPL,
            Elettrico,
            Hybrid
        }

        /// <summary>
        /// metodo che mi permette di inserire il tipo di alimentazione tra elementi di un Enum
        /// </summary>
        /// <returns></returns>
        public static string CheckFuel()
        {
            string alimentazione = "";
            Console.WriteLine("Inserisci un tipo di alimentazione :");
            foreach(Fuel fuel in Enum.GetValues(typeof(Fuel)))
            {
                Console.WriteLine($"{(int) fuel}){fuel}");
            }
            bool control = false;
            while(control != true)
            {
                int choice = 0;
                string number = Console.ReadLine();
                int.TryParse(number, out choice);
                
                foreach(Fuel fuel in Enum.GetValues(typeof (Fuel)))
                {
                    if (choice == (int)fuel)
                    {
                        alimentazione = $"{fuel}";
                        control = true;
                    }
                }
                if (control == false)
                {
                    Console.WriteLine("Inserisci un parametro corretto");
                }
            }
            return alimentazione;


        }

        /// <summary>
        /// Metodo da richiamare per avere il primo carattere maiuscolo
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        
        
    }
}
