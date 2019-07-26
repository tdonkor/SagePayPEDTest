using System;
using Integral.Library.GuardianClient;
using static SagePayPEDTest.Utils;

namespace SagePayPEDTest
{
    public class GuardianApi : IDisposable 
    {

        TransactionInfo transactionInfo;
        VoidTransactionHook voidTransactionHook;
        TillInformation tillInformation;
        NonGuiTransactionHook nonGuiTransactionHook;
        
        TransactionHook.TRANSACTIONHOOK_TRANSACTIONTYPE transactiontype;
        NonGuiTransactionHook.NONGUITRANSACTION_CONFIRMTYPE confirmationType;

     
        /// <summary>
        /// construtor
        /// </summary>
        public GuardianApi()
        {
            
            Console.WriteLine("SagePay Driver Transaction Started.....");

            voidTransactionHook = new VoidTransactionHook();
            transactionInfo = new TransactionInfo();
            tillInformation = new TillInformation();
            nonGuiTransactionHook = new NonGuiTransactionHook();
            //default confirmation type
            confirmationType = NonGuiTransactionHook.NONGUITRANSACTION_CONFIRMTYPE.CONFIRMTYPE_AUTHORISED;
            
        }

        public void Dispose()
        {
          
        }

        /// <summary>
        /// Payment 
        /// </summary>
        public DiagnosticErrMsg Pay(int amount )
        {

            int intAmount;
            DiagnosticErrMsg isSuccessful = DiagnosticErrMsg.OK;

            //we only do sales
            transactiontype = TransactionHook.TRANSACTIONHOOK_TRANSACTIONTYPE.INT_TT_SALE;

            //check amount is valid
            intAmount = Utils.GetNumericAmountValue(amount);

            if (intAmount == 0)
            {
                throw new Exception("Error in Amount value");
            }

            Console.WriteLine($"Valid payment amount: {intAmount}");

            //add the address of the store for the reciept
            AddAddress();

            //Use a non GUI transaction - if transaction is true proceed to the CardEnquiry stage.
            if (nonGuiTransactionHook.StartTransaction(ref tillInformation) == true)
            {
                Console.WriteLine("Transaction Started....");

                //check the card enquiry returns: RETURNCODE_SUCCESS or RETURNCODE_CASHBACKALLOWED
                if (nonGuiTransactionHook.CardEnquiry(transactiontype,
                                                intAmount,
                                                NonGuiTransactionHook.NONGUITRANSACTION_DATAENTRY.DATAENTRY_CHIPORSWIPEORTAP) == NonGuiTransactionHook.NONGUITRANSACTION_RETURNCODE.RETURNCODE_SUCCESS)
                {

                    //authorise the transaction
                    Console.WriteLine("Transaction Authorisation Started....");

                    if (nonGuiTransactionHook.AuthoriseTransaction(TransactionHook.TRANSACTIONHOOK_TRANSACTIONTYPE.INT_TT_SALE, intAmount, 0, "SALE", ref transactionInfo) == NonGuiTransactionHook.NONGUITRANSACTION_RETURNCODE.RETURNCODE_SUCCESS)
                    {

                        if (transactionInfo.DataEntryMethod == TransactionInfo.TRANSINFO_DATAENTRYMETHOD.TRANSINFO_DE_SWIPED)
                        {

                            Console.WriteLine("Swipe transaction - CANCEL the transaction.");
                            confirmationType = NonGuiTransactionHook.NONGUITRANSACTION_CONFIRMTYPE.CONFIRMTYPE_CANCELLED;
                            isSuccessful = DiagnosticErrMsg.NOTOK;
                        }


                        // confirm the transaction
                        if (nonGuiTransactionHook.ConfirmTransaction(confirmationType,
                                                                   nonGuiTransactionHook.TransactionReference,
                                                                   transactionInfo.AuthorisationCode,
                                                                   ref transactionInfo) == true)
                        {

                            
                   
                            //display transactionInfo
                            Console.WriteLine("\nTransaction Information");
                            Console.WriteLine("-----------------------\n");
                            Console.WriteLine($" AuthCode: {transactionInfo.AuthorisationCode}");
                            Console.WriteLine($" CardHolder Name: {transactionInfo.CardHolderName}");
                            Console.WriteLine($" Currency Code: {transactionInfo.CurrencyCode}");
                            Console.WriteLine($" Data Entry Method: {transactionInfo.DataEntryMethod}");
                            Console.WriteLine($" Merchant Number: {transactionInfo.MerchantNo}");
                            Console.WriteLine($" Response Code: {transactionInfo.ResponseCode}");
                            Console.WriteLine($" Scheme Number.: {transactionInfo.SchemeName}");
                            Console.WriteLine($" Transaction Amount: {transactionInfo.TransactionAmount}");
                            Console.WriteLine($" Transaction Ref Number: {transactionInfo.TransactionRefNo.ToString()}");
                            Console.WriteLine($" TerminalId: {transactionInfo.TerminalId}");


                            //customer receipt
                            Console.WriteLine("\n\nCustomer Receipt");
                            Console.WriteLine("===================\n");

                            Console.WriteLine($" Customer Reciept: {transactionInfo.CustomerReceipt}");

                        }


                    }
                }

            }

            //end the transaction
            nonGuiTransactionHook.EndTransaction();
            Console.WriteLine("SagePay Driver Transaction Finished.....");

            return isSuccessful;

        }

        private void AddAddress()
        {
            // Populate the till information object
            tillInformation.MerchantName = "Acrelec";
            tillInformation.Address1 = "East Wing , Focus 31";
            tillInformation.Address2 = "Mark Road";
            tillInformation.Address3 = "Hemel Hempstead";
            tillInformation.Address4 = "HP2 7BW";
            tillInformation.PhoneNumber = "1234567890";
        }

        private void Populate
    }
}
