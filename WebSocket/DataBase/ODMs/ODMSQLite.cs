using System;
using SQLite;

namespace WebSocket.DataBase.ODMs
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Indexed]
        public string Book { get; set; }

        [Indexed]
        public DateTime Date { get;  set; }

        public decimal AmountMXN { get; set; }

        public decimal AmountCrypto { get; set; }

        public decimal Price { get; set; }
    }

    public class BitsoBuy : Transaction
    {
        public decimal MaxPrice { get; set; }
    }


    public class BitsoSell : Transaction
    {
        [Indexed]
        public DateTime SoldDate { get; set; }

        public decimal PriceSold { get; set; }

        public bool Completed { get; set; }

        public BitsoSell() { }
    }
}
