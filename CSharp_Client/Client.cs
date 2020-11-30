using System;
using System.IO;
using System.Net.Sockets;
using System.Xml.Linq;


namespace CSharp_Client
{
    public class Client
    {
        private string ID;
        private readonly StreamReader reader;
        private StreamWriter writer;
        private TcpClient socket;
        
        public Client(){
           
            
            try{
                connect();
            }catch(IOException e){
               Console.WriteLine("Error establishing connection to the server");
                Environment.Exit(606);
            } 
            NetworkStream stream = socket.GetStream();
            this.reader = new StreamReader(stream);
            this.writer = new StreamWriter(stream);
            sendCommand("new connection");
            String idStr = recieveMessage();
            Console.WriteLine("here");
            idStr = idStr.Replace("[ID]", "");
            this.ID = idStr;
        }

        public void connect(){

            try{
                // Connecting to the server and creating objects for communications
                this.socket = new TcpClient("localhost", 8888);

            }
            catch (SocketException e){
                Console.WriteLine("Unable to connect to the server");
                Environment.Exit(606);
            }
        }

        
        //Returns null if no message in inputstream
        public string recieveMessage(){
            Console.WriteLine("read mess");
            string message = "";
            try{
                message = reader.ReadLine();
            }
            catch (IOException e){
                return null;
            }
            return message;
        }
        
        public void sendCommand(String command){
            try{
                writer.WriteLine(command);
                writer.Flush();
            }
            catch (System.IO.IOException e){
                reconnect();
            }
            
        }
        public string getID(){
            return this.ID;
        }

        public TcpClient getSocket(){
            return this.socket;
        }

        public StreamWriter getWriter(){
            return this.writer;
        }


        public void reconnect(){
            this.connect();
            NetworkStream stream = socket.GetStream();
            this.writer = new StreamWriter(stream);
        }
    
    }
    
   
   
    

}