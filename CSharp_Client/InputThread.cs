using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace CSharp_Client{
    
    
    
    public class InputThread{
        private TcpClient socket;
        private Client client;
        private StreamReader reader;
       

        public InputThread(Client client){
            
            
            this.client = client;
            this.socket = client.getSocket();
            NetworkStream stream = socket.GetStream();
            
            StreamReader input = null;
            try { 
                this.reader = new StreamReader(stream);
            } catch (IOException e) {
                
                Console.WriteLine(e.ToString());

            }
        }
        
        //Removes the command part from message which is used by the program to know what to do with the command.
        private String trimString(string str, String toRemove){
            return str.Replace(toRemove, "");
        }
        
        private void handleResponse(String message){
            if (message.StartsWith("[UPDATE]")) {
                //updateClientMethod()
                message = trimString(message, "[UPDATE]");
                Console.WriteLine(message);
                Console.Write("> ");

            }else if (message.StartsWith("[CONN]")){ //Connection/Disconnecitons
                message = trimString(message, "[CONN]");
                message = message.Replace("[CONN]", "");
                
                string[] connctionsTokens = message.Split(); //https://stackoverflow.com/questions/70405/does-c-sharp-have-a-string-tokenizer-like-javas
                Console.WriteLine("Connected Clients:");
                foreach (string word in connctionsTokens)
                {
                    if (word != ""){
                        Console.WriteLine("ID: " + word);
                    }
                    
                }
                Console.Write("> ");
            }else if (message.StartsWith("[NEW_CONN]")){ //
                message = trimString(message, "[NEW_CONN]");
                Console.WriteLine(message);

            }else if (message.StartsWith("[MARKET]")){ //
                message = trimString(message, "[MARKET]");
                Console.WriteLine("Market: " + message);

            }else if (message.StartsWith("[WARNING]")){
                message = trimString(message, "[WARNING]");
                Console.WriteLine("Warning: " + message);
                Console.Write("> ");
            }
        }

        public void run(){
            while (true){
                Console.WriteLine("Read input: ");
                try{
                    string response = reader.ReadLine();
                    handleResponse(response);
                }
                catch (IOException e){
                    Thread.CurrentThread.Interrupt();
                    break;
                }
                
            }
        }
    }
}