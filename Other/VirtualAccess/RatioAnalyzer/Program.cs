// 
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Step 1: Initialize MongoDB Client
        // var client = new MongoClient("mongodb://localhost:27017");
        var client = new MongoClient("mongodb://172.25.98.31:27017");
        var database = client.GetDatabase("Densities"); // Replace with your database name

        // Step 2: Get Ratio collection as queryable 
        var ratioCollection = database.GetCollection<BsonDocument>("male_female_proportion_ratios").AsQueryable();

        // Step 3: Add all the ratios to a list
        var ratios_19_25 = ratioCollection.Where(p => p["Proportion_19_25"] > 0).OrderBy(b => b["Proportion_19_25"]).ToList();

        Console.WriteLine("Testing\n");
        foreach (var e in ratios_19_25) 
        {
            Console.WriteLine(e);
        }

        

    }
    // 439 Unique Locations, Wide Areas (All Caps) included
}