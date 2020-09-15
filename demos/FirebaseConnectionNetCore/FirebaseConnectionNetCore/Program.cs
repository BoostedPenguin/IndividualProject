using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;


namespace FirebaseConnectionNetCore
{
    class Program
    {
        static FirestoreDb db;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            db = FirestoreDb.Create("individualprojects3");

            AddData();


            Console.ReadLine();
        }

        public static async void AddData()
        {
            DocumentReference docRef = db.Collection("users").Document();
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "first_name", "Ada" },
                { "age", 5 },
            };
            await docRef.SetAsync(user);

        }
    }
}
