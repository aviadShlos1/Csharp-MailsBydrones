using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome3993();
            Welcome0881();
            Console.ReadKey();
        }
        static partial void Welcome0881();
        private static void Welcome3993()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.Write("{0}, welcome to my first console application", name);
        }
    }
}
