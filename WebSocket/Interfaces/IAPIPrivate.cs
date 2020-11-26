using System;
using WebSocket.DataModels;

namespace WebSocket.Interfaces
{
    public interface IAPIPrivate
    {
        AccountStatus GetAccountStatus();
        MobilePhoneNumber RegisterMobilePhoneNumber(string mobilePhoneNumber);
        MobilePhoneNumber VerifyMobilePhoneNumber(string verificationCode);
        Balance[] GetAccountBalance();
    }
}
