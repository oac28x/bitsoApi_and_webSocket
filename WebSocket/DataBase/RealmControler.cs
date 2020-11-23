using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Realms;
using WebSocket.DataBase.ODMs;
using WebSocket.Interfaces;

namespace WebSocket.DataBase
{
    public class RealmControler : IDataBase
    {
        private readonly Realm context;

        public RealmControler()
        {
            string DBDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BitsoRealm");
            if (!Directory.Exists(DBDir)) Directory.CreateDirectory(DBDir);

            RealmConfiguration config = new RealmConfiguration(Path.Combine(DBDir, "AppDB.realm"));
            byte[] answerKey = new byte[64]; // key MUST be exactly this size to avoid a FormatException
            answerKey[0] = 0x2a;             // first byte is 42
            answerKey[1] = 0x70;             // second byte 112 followed by 62 zeroes
            config.EncryptionKey = answerKey;

            context = Realm.GetInstance(config);
        }


        public void test()
        {

            IQueryable<Dog> all = GetAll<Dog>();

            IQueryable<Dog> puppies = GetAll<Dog>().Where(d => d.Age < 2);

            Console.WriteLine($"Count {puppies.Count()}");

            Update(new Dog() { Name = "Rex", Age = 1 });

            Console.WriteLine($"Count {puppies.Count()}");


            var dog = puppies.First();

            UpdateElement(() => dog.Age = 3);

            Console.WriteLine($"Count {puppies.Count()}");
        }



        public IQueryable<T> GetAll<T>() where T : RealmObject
        {
            return context.All<T>();
        }

        public RealmObject GetElement<T>(string primaryKey) where T : RealmObject
        {
            return context.Find<T>(primaryKey);
        }

        public RealmObject GetElement<T>(long primaryKey) where T : RealmObject
        {
            return context.Find<T>(primaryKey);
        }

        public void Insert<T>(T element) where T : RealmObject
        {
            context.WriteAsync((r) =>
            {
                r.Add(element);
            }).ConfigureAwait(false);
        }


        public void Update<T>(T element) where T : RealmObject
        {
            context.WriteAsync((r) =>
            {
                r.Add(element, true);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ac"></param>
        /// UpdateElement(() => dog.Age = 3);
        public void UpdateElement(Action ac)
        {
            context.Write(() => ac.Invoke());
        }

        public void Remove<T>(T element) where T : RealmObject
        {
            context.WriteAsync((r) =>
            {
                r.Remove(element);
            }).ConfigureAwait(false);
        }

        public void RemoveRange<T>(IQueryable<T> query) where T : RealmObject
        {
            context.WriteAsync((r) =>
            {
                r.RemoveRange(query);
            }).ConfigureAwait(false);
        }
    }
}
