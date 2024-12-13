//
using MongoConnectorLibrary;

//
Console.WriteLine("Hello, World!");
Console.ReadKey();

// Database Interfacer 300 Main Program
// ############################################################################################ 
// FIRST:  Load Dependencies/Libraries/Modules/etc
// SECOND: Greet User with Connect/Exit Prompt
// --- On Exit, Exit.
// --- On Connect, Instantiate a Mongo client and TRY to connect to local MongoDB hosted on VM
// ----- On FAIL, Suggest user checks VM is running properly, return to SECOND
// ----- On SUCCESS, inform user of good connect, go to MAIN MENU.
// ############################################################################################
// MAIN MENU:
// --- EXPLORE DB
// --- EDIT DB
// --- Run a DEMO
// --- Run a QUERY
// --- Exit
// ############################################################################################
// EXPLORE DB:
// - Browse db's
// - Browse collections
// - Browse documents in collections
// - Generate new collections
// - Browse documents in new collections
// - SAVE new collections
// ############################################################################################
// EDIT DB:
// - DELETE collections
// - ADD collections
// ############################################################################################
// Run a DEMO:
// - Run a pre-defined query, satisfying MUST and SHOULD criteria
// ############################################################################################
// Run a QUERY:
// - Run a user-defined query, satisfying SHOULD and MAY criteria
// ############################################################################################
// EXIT
// Exit program.
// ############################################################################################