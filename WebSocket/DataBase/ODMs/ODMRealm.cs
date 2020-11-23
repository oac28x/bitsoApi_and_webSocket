using System;
using System.Collections.Generic;
using Realms;

namespace WebSocket.DataBase.ODMs
{

    // Define your models like regular C# classes
    public class Dog : RealmObject
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Person Owner { get; set; }
    }

    public class Person : RealmObject
    {
        public string Name { get; set; }
        public IList<Dog> Dogs { get; }
    }
}
