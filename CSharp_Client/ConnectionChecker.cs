using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace CSharp_Client{
    public class ConnectionChecker{
        private TcpClient socket;
        private StreamWriter writer;
        private bool running;
        private Client client;
        
        public ConnectionChecker(TcpClient socket, Client client){
            this.socket = socket;
            NetworkStream stream = socket.GetStream();
            this.writer = new StreamWriter(stream);
            this.client = client;
        }

        //TODO Client does not re establish connection to the server if lost: System.IO.IOException:
        public void run(){
            this.running = true;
            while (this.running){
                Thread.Sleep(5000);
                try{
                    writer.WriteLine("@");
                    writer.Flush();
                }
                catch (IOException e){
                    Thread.Sleep(5000);
                    Boolean reconnected = false;
                    for (int i = 0; i < 5; i++){
                        try{
                            client.reconnect();
                            reconnected = true;
                            this.socket = client.getSocket();
                            try{
                                NetworkStream stream = socket.GetStream();
                                this.writer = new StreamWriter(stream);
                            }
                            catch (IOException error){
                                Console.WriteLine(e.StackTrace);
                            }
                        }
                        catch (IOException reconnectionError){
                            reconnected = false;
                        }

                        if (reconnected){
                            Console.WriteLine("reconnecting");
                            i = 10;
                            client.sendCommand("reconnection");
                            client.sendCommand(client.getID());
                        }
                    }

                    if (!reconnected){
                        Console.WriteLine("Connection with the server has been lost.");
                        Environment.Exit(606);
                    }
                    Program.restartInputThread(client);
                    

                }
            }
        }
       // Console.WriteLine("Connection to the server has been lost.");
                            //Environment.Exit(606);
    }
}