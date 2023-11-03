using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondoProgetto.car
{
    public class Car
    {
        public  class CarData 
        {
            public string Id { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public int ProductionYear { get; set; }

            public List<CarCharacterist> carCharacterist = new ();

            public CarData() { }
            public CarData(string id, string brand, string model, int productionYear)
            {
                Id = id;

                Brand = brand;

                Model = model;

                ProductionYear = productionYear;
            }
        }

        public class CarCharacterist
        {
            public int Id { get; set; }

            public string CarId { get; set; }

            public int Engine { get; set; }

            public string Fuel { get; set; }


            public CarCharacterist()
            {
            }

        }

    }

    
}
