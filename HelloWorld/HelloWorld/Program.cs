using HelloWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person() {Name ="CharlesWai", Age = 27 };
            Console.WriteLine($"Hello {person.Name}, you are {person.Age} years old");
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
