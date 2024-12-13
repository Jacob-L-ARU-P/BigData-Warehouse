using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;

class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        // MongoDB connection setup
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("populationData");
        var collection =
        database.GetCollection<BsonDocument>("lifeExpectancy");
        // Fetch data from MongoDB
        var documents = await collection.Find(new
        BsonDocument()).ToListAsync();
        // Process data: Calculate average MatB (life expectancy) per Band
        var bandData = new Dictionary<int, List<double>>();
        int loopCounter = 0;
        double matB;
        try
        {
            foreach (var doc in documents)
            {
                loopCounter++;
                int band = doc["Band"].AsInt32;
                BsonValue bsonValue = doc["MatB"];
                if (bsonValue.IsInt32)
                {
                    matB = bsonValue.AsInt32; // Convert int to double
                }
                else if (bsonValue.IsInt64)
                {
                    matB = bsonValue.AsInt64; // Convert long to double
                }
                else
                {
                    matB = bsonValue.AsDouble;
                }
                if (bandData.Count == 0)
                {
                    bandData[band] = new List<double>();
                }
                else if (!bandData.ContainsKey(band))
                {
                    bandData[band] = new List<double>();
                }
                bandData[band].Add(matB);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(loopCounter);
            Console.WriteLine(e.Message);
        }
        var bands = bandData.Keys.OrderBy(b => b).ToArray();
        var averageLifeExpectancy = bands.Select(b => bandData[b].Average()).ToArray();
        // Plotting with ScottPlot
        var plt = new ScottPlot.Plot();
        plt.Add.Bars(averageLifeExpectancy);
        //plt.Axes(averageLifeExpectancy, bands.Select(b => b.ToString()).ToArray());
        plt.Title("Average Life Expectancy (MatB) by Band");
        plt.XLabel("Band");
        plt.YLabel("Average Life Expectancy (MatB)");
        plt.SavePng("./../AverageLifeExpectancyByBand.png", 400, 300);
        Console.WriteLine("Graph saved as 'AverageLifeExpectancyByBand.png'");
    }
}