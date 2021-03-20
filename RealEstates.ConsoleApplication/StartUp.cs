using System;
using System.Collections.Generic;
using RealEstates.ConsoleApplication.Core;

namespace RealEstates.ConsoleApplication
{
    public class StartUp
    {
        public static void Main()
        {
            try
            {
                var engine = new Engine();
                engine.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}