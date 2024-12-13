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
        //var client = new MongoClient("mongodb://localhost:27017");
        var client = new MongoClient("mongodb://172.25.98.31:27017");
        var database = client.GetDatabase("Densities"); // Replace with your database name

        // Step 2: Reference the male and female collections
        var malesCollection = database.GetCollection<BsonDocument>("maleDensities");
        var femalesCollection = database.GetCollection<BsonDocument>("femaleDensities");

        // Step 3: Reference the new collection where ratios will be saved
        var newCollection = database.GetCollection<BsonDocument>("male_female_proportion_ratios");

        // Step 4: Get all documents from the males and females collections
        var males = await malesCollection.Find(new BsonDocument()).ToListAsync();
        var females = await femalesCollection.Find(new BsonDocument()).ToListAsync();

        // Step 5: Use LINQ to join the two collections based on CODE and calculate the ratios
        var ratios = males.Join(
            females, // Inner collection (females)
            male => male["CODE"], // Outer key selector (from males)
            female => female["CODE"], // Inner key selector (from females)
            (male, female) => new BsonDocument
            {
                { "CODE", male["CODE"] },
                { "Location", male["Location"] },
                { "Proportion_AllAges", SafeDivide(male["AllAges"].AsInt32, female["AllAges"].AsInt32) },
                { "Proportion_Under18", SafeDivide(male["<18"].AsInt32, female["<18"].AsInt32) },
                { "Proportion_19_25", SafeDivide(male["19-25"].AsInt32, female["19-25"].AsInt32) },
                { "Proportion_26_30", SafeDivide(male["26-30"].AsInt32, female["26-30"].AsInt32) },
                { "Proportion_31_35", SafeDivide(male["31-35"].AsInt32, female["31-35"].AsInt32) },
                { "Proportion_35_45", SafeDivide(male["35-45"].AsInt32, female["35-45"].AsInt32) },
                { "Proportion_45_60", SafeDivide(male["45-60"].AsInt32, female["45-60"].AsInt32) },
                { "Proportion_61Plus", SafeDivide(male[">61"].AsInt32, female[">61"].AsInt32) }
            }
        ).ToList();

        // Step 6: Insert the calculated ratios into the new collection
        if (ratios.Any())
        {
            await newCollection.InsertManyAsync(ratios); // Insert the list into the new collection
            Console.WriteLine("Ratios successfully inserted into the new collection!");
        }
        else
        {
            Console.WriteLine("No ratios were found or calculated.");
        }
    }

    // Helper method to safely divide and handle divide by zero
    static double SafeDivide(int numerator, int denominator)
    {
        // Ternary Statement with 3 parts
        // If (condition) do (Thing) else return (Thing 2)
        return denominator != 0 ? (double)numerator / denominator : 0;
        // "Thing" is returned, and as such partially dictated by the method it is used in,
        // in this case, it must return a double.
    }
}
