using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondoProgetto.school
{
    internal class Student
    {
        public int Id;
        public string Name;
        public string Study;
        public DateTime Birthday;


        //public Student ()
        //{
        //    Id = id;
        //    Name = name;
        //    Study = study;
        //    Birthday = birthday;
        //}


        public Student(int id, string name, string study, DateTime birthday)
        {
            Id = id;
            Name = name;
            Study = study;
            Birthday = birthday;

        }

        public string Getbirthday() => this.Birthday.ToString("dd/MM/yyyy");
    }
}
