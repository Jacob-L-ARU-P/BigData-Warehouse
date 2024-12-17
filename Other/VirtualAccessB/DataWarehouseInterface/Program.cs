// See https://aka.ms/new-console-template for more information
// Using Directives
using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Text.Json;
using MongoDB.Driver.Core.Configuration;
using System.Threading;

namespace MyProgram
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Greet User
            Console.WriteLine("Hello, World!\n");

            // ################## Mongo Client Connection ####################
            /*
             host refers to the VM instance running the MongoServer and it's ip
             Future revisions of main program would change host to be a user
             input / semi-auto querying of Hyper-V/VM directly?
             Currently beyond scope of project.
            */
            const string host = "172.31.177.71";
            // Connection URI
            const string connectionUri = "mongodb://172.31.177.71:27017";
            
            // Mongo Client Settings
            /* 
             Configures connection settings in code, rather than in the URI
             Allows easier changes to settings at runtime
             "helps you catch errors during compilation"
             provides more configuration options than the connection URI
            */
            var settings = new MongoClientSettings()
            {
                Scheme = ConnectionStringScheme.MongoDB,
                Server = new MongoServerAddress(host, 27017),
                ApplicationName = "DataWarehouseInterfaceTesting",
            };

            // Create a new connection client and connect to server
            var client = new MongoClient(settings);

            // Ping Database
            var pingDatabase = client.GetDatabase("admin");
            var ping = new BsonDocument("ping", 1);
            try
            {
                pingDatabase.RunCommand<BsonDocument>(ping);
                Console.WriteLine("Pinged Database Successfully!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //
            Beta(client);
            
            // Get Database names list
            //var cursor = await client.ListDatabasesAsync();
            //await cursor.ForEachAsync(db => Console.WriteLine(db["name"]));

            // 
            // ################## ----------------------- #################### 

            Console.WriteLine("");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        public static async void Beta(MongoClient args) 
        {
            
            // Get Database names list
            var cursor = await args.ListDatabasesAsync();
            await cursor.ForEachAsync(db => Console.WriteLine(db["name"]));

            
        }
        // Method Gamma incomplete at time of save
        public static void Gamma(MongoClient args) 
        {
            // Accesses/Creates "theWarehouse"
            args.GetDatabase("theWarehouse");

        }
    }
}