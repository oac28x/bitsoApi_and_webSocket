using System;
using WebSocket.DataModels;

namespace WebSocket.Interfaces
{
    public interface IAPIPrivate
    {
        AccountStatus GetAccountStatus();
        Balance[] GetAccountBalance();
        FeeInfo GetFees();

        MobilePhoneNumber RegisterMobilePhoneNumber(string mobilePhoneNumber);
        MobilePhoneNumber VerifyMobilePhoneNumber(string verificationCode);
        
        Operation[] GetLedger(OperationType operationType, string marker = "", string sort = "desc", int limit = 25);
        Operation[] GetLedger(string marker = "", string sort = "desc", int limit = 25);

        Withdrawal[] GetWithdrawals(int limit = 25);
        Withdrawal GetWithdrawal(string wid);
        Withdrawal[] GetWithdrawals(params string[] wids);

        Funding[] GetFundings(int limit = 25);
        Funding GetFunding(string fid);
        Funding[] GetFundings(params string[] fids);

        UserTrade[] GetUserTrades(string book = "btc_mxn", string marker = "", string sort = "desc", int limit = 25);
        UserTrade GetUserTrade(string tid);
        UserTrade[] GetUserTrades(params string[] tids);

        OpenOrder[] GetOpenOrders(string book = "btc_mxn", string marker = "", string sort = "desc", int limit = 25);
        OpenOrder LookupOrder(string oid);
        OpenOrder[] LookupOrders(params string[] oids);

        OpenOrder PlaceOrder(string book, string side, string type, decimal price, decimal? minorAmount = null, decimal? majorAmount = null);

        string[] CancelAllOpenOrders();
        string[] CancelOpenOrder(string oid);
        string[] CancelOpenOrders(params string[] oids);

        FundingDestination GetFundingDestination(string fundCurrency);
        BankCode[] GetMexicanBankCodes();

        Withdrawal WithdrawToBitcoinAddress(decimal amount, string address);
        Withdrawal WithdrawToEtherAddress(decimal amount, string address);
        Withdrawal WithdrawToSPEI(decimal amount, string recipientGivenNames, string recipientFamilyNames, string clabe, string notesRef, string numericRef);
        Withdrawal WithdrawToDebitCard(double amount, string recipientGivenNames, string recipientFamilyNames, string cardNumber, string bankCode);
        Withdrawal WithdrawToPhoneNumber(double amount, string recipientGivenNames, string recipientFamilyNames, string phoneNumber, string bankCode);  
    }
}
