using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SagePayPEDTest
{
    class Program
    {
        static void Main(string[] args)
        {

                // Console.Clear();

                Console.WriteLine("\n\tSagePay Payment Simulator");
                Console.WriteLine("\t________________________\n\n");
                int amount = 0;


                using (var api = new GuardianApi())
                {
                    Console.Write("Enter the Amount(no decimal point allowed): ");
                    try
                    {
                      // check the connection to the PED
                        
                        amount = int.Parse(Console.ReadLine());
                        var payResult =  api.Pay(amount);
                        Console.WriteLine($"PayResult = {payResult.ToString()}");

                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Error" + ex.Message);
                    }
                   
            }

            Console.Write("\n\nPress any key to exit...");
            Console.ReadKey(); 
            
        }
    }
}
