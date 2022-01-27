using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._1.Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====Part 1 start====");
            var cor = new Coordinates(1, 1);
            Console.WriteLine(cor.ToString());
            cor = cor.SetCoordinates(2, cor.Y);
            Console.WriteLine(cor.ToString());
            Console.WriteLine("====Part 1 end====");
            Console.WriteLine("Press enter to continue\n");
            Console.ReadKey();

            Console.WriteLine("====Part 2 start====");
            IBase Ibase = new A();
            Console.WriteLine(Ibase.GetInfo());
            Ibase = new B();
            Console.WriteLine(Ibase.GetInfo());
            Console.WriteLine("====Part 2 end====");
            Console.ReadKey();
        }
    }

    class Coordinates
    {
        public Coordinates(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double X { get; }
        public double Y { get; }
        public Coordinates GetCoordinates()
        {
            return this;
        }

        public Coordinates SetCoordinates(double x, double y)
        {
            return new Coordinates(x, y);
        }


        public override string ToString()
        {
            return $"x = {X}, y = {Y} ";
        }
    }

    public interface IBase
    {
        string GetInfo();
    }
    public struct A : IBase
    {
        string IBase.GetInfo()
        {
            return "A class";
        }
    }
    public struct B : IBase
    {
        string IBase.GetInfo()
        {
            return "B class";
        }
    }
}
