using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using System.Text.Json;
//using MongoDB.Driver.Core;
//using MongoDB.Driver.Linq;
namespace MongoTest
{
    // This is the Program!
    class MainClass
    {
        public static void Main(string[] args)
        {
            TestMongo tm = new TestMongo();
            Console.WriteLine("Before Tests");
            Console.WriteLine("Using Mongo syntax - press enter");
            Console.ReadLine();
            tm.testResteraunts();
            TestMongoLinq tml = new TestMongoLinq();
            Console.WriteLine("Using Linq syntax - press enter");
            Console.ReadLine();
            tml.testResteraunts();
            Console.WriteLine("Done Test");
            Console.WriteLine("\nBeep Boop");
            Console.ReadLine();
        }
    }

    // These are the Programs in-main functions
    // In a larger project they would be in their own files.
    public class TestMongo
    {
        public async void testResteraunts()
        {
            Console.WriteLine("Using Mongo syntax");
            var client = new MongoClient("mongodb://172.25.98.31:27017");
            //var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("test");
            var collection = database.GetCollection<BsonDocument>("restaurants");
            var filter = Builders<BsonDocument>.Filter.Eq("borough", "Brooklyn");
            await collection.Find(filter).ForEachAsync(d => Console.WriteLine(d));

        }
    }
    public class TestMongoLinq
    {
        public void testResteraunts()
        {
            Console.WriteLine("Using LINQ syntax");

            // Initialize MongoDB client and get database/collection
            //                                    V Default: localhost V
            var client = new MongoClient("mongodb://172.25.98.31:27017");
            var database = client.GetDatabase("test");
            var collection = database.GetCollection<BsonDocument>("restaurants");

            // LINQ query to filter restaurants in "Brooklyn"
            var queryableCollection = collection.AsQueryable();
            //SQL Syntax
            var brooklynRestaurants = from restaurant in queryableCollection
                                      where restaurant["borough"] == "Brooklyn"
                                      select restaurant;
            // Asynchronously iterate through the results and print each restaurant document
            foreach (var restaurant in brooklynRestaurants) Console.WriteLine(restaurant);

            //Lambda syntax
            var queensRestaurants = queryableCollection
                .Where(r => r["borough"] == "Queens")
                .Select(x => x["name"])
                .OrderBy(r => r["name"]);

            var bostonMarkets = queryableCollection
                .Where(n => n["name"] == "Boston Market")
                .Select(x => new { restName = x["name"], restId = x["restaurant_id"], restCuisine = x["cuisine"] });

            foreach (var restaurant in queensRestaurants) Console.WriteLine(restaurant);
            foreach (var restaurant in bostonMarkets) Console.WriteLine(restaurant);
        }
    }
}
