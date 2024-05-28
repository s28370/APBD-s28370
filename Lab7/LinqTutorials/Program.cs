using System;

namespace LinqTutorials
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = LinqTasks.Task3();
            var t2 = LinqTasks.Task15();
            foreach (var o in t2)
            {
                Console.WriteLine(o);
            }
        }
    }
}
