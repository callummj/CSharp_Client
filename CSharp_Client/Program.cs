using System;
using System.Collections;
using System.IO;
using System.Threading;
using CSharp_Client;


namespace CSharp_Client{
    
    class Program{
        public static ArrayList onlineTraders = new ArrayList();

        static void Main(string[] args){

            Client client = null;
            
        try { 
            client = new Client();
            //(new Thread(new ConnectionChecker(client.socket)) + start());
        }
        catch (IOException e) {
            Console.WriteLine("Error connecting to the server.");
            Environment.Exit(1);
        }
        
        ConnectionChecker connCheker = new ConnectionChecker(client.getSocket(), client);
        Thread connCheckerThread = new Thread(new ThreadStart(connCheker.run));
        connCheckerThread.Start();
        
        
        InputThread inputThreadObj = new InputThread(client);
        Thread inputThread = new Thread(new ThreadStart(inputThreadObj.run));
        inputThread.Start();
        //(new Thread(new GUI(client)) + start());
        
        bool running = true;
        Console.WriteLine(("Successfully connected to market with ID: " + (client.getID() + ". Type \'help\' for list of commands.")));
        
        while (running) {
            Console.Write("> ");
            String input = Console.ReadLine();
                switch (input) {
                    case "balance":
                        client.sendCommand("balance");
              
                        break;
                    case "buy":
                        client.sendCommand("buy");
                     
                        break;
                    case "sell":
                        Console.WriteLine("sell");
                        bool gettingRecip = true;
                        int recipient = 0;
                        while (gettingRecip){
                             Console.WriteLine("Enter an id who you would like to sell to: ");
                             try{
                                 recipient = Convert.ToInt32(Console.ReadLine());
                                 if (recipient != null){
                                     gettingRecip = false;
                                 }
                                 else{
                                     Console.WriteLine("Please enter a valid user ID");
                                 }
                             }
                             catch (System.FormatException e){
                                 Console.WriteLine("Please enter a valid user ID");
                             }
                             
                        }
                        //Send server commands.
                        client.sendCommand("sell");
                        client.sendCommand(recipient.ToString());
                        break;
                    case "quit":
                        client.sendCommand("quit");
                        Console.WriteLine("Disconnecting from server...");
                            Environment.Exit(1);
                        break;
                    case "help":
                        Console.WriteLine("Available commands:\n> balance\n> sell\n> status\n> connections");
                        break;
                    case "status":
                        client.sendCommand("status");
                        break;
                    case "connections":
                        client.sendCommand("connections");
                        break;
                    default:
                        Console.WriteLine("Invalid command, type \'help\' for a list of all commands.");
                        break;
                }
            }
        
        }
        
        public static void restartInputThread(Client client){
            InputThread inputThreadObj = new InputThread(client);
            Thread inputThread = new Thread(new ThreadStart(inputThreadObj.run));
            inputThread.Start();
        }

        
    }
}
