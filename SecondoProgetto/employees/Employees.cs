using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondoProgetto.employees
{
    public abstract class EmployeesAnagrafic
    {
        public string Id { get; set; }
        public string Name {  get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        

        public EmployeesAnagrafic() { }

        public EmployeesAnagrafic(string id, string name, string surname, int age, string address, string city, string province, string postalCode, string phone)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Age = age;
            Address = address;
            City = city;
            Province = province;
            PostalCode = postalCode;
            Phone = phone;
        }


    }

    public class Employees : EmployeesAnagrafic
    {
        public string Role { get; set; }
        public string Department { get; set; }

        public List<EmployeeWork> Works = new();

        public Employees() { }

        public Employees(string id, string name, string surname, int age, string address, string city, string province, string postalCode, string phone,string role, string department)
        
            :base(id, name, surname, age, address, city, province, postalCode, phone)
        {
            Role = role;
            Department = department;
        }


    }

    public class EmployeeWork
    {
        public DateTime Date { get; set; }
        public string Activity { get; set; }
        public double Hours { get; set; }
        public EmployeeWork() { }
        public EmployeeWork(DateTime date, string activity, double hours)
        {
            Date = date;

            Activity = activity;
            
            Hours = hours;
        }
    }

}
