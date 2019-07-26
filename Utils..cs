using System;
using Integral.Library.GuardianClient;

namespace SagePayPEDTest
{

    public enum DiagnosticErrMsg : short
    {
        OK = 0,
        NOTOK = 1
    }

    public class Utils
    {

        /// <summary>
        /// Check the numeric value of the amount
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static int GetNumericAmountValue(int amount)
        {

            if (amount <= 0)
            {
                Console.WriteLine("Invalid pay amount");
                amount = 0;
            }

            return amount;
        }

        /// <summary>
        /// Create Customer Ticket to output the reciept
        /// </summary>
        /// <param name="ticket"></param>
        public static void CreateCustomerTicket(string ticket)
        {

        }
    }
}
