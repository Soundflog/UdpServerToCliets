using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpServerToCliets
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            // int port = 8000;
            // IPEndPoint endPoint = null;
            // UdpClient listener = new UdpClient("localhost", port);
            // // listener.Start();
            // Console.WriteLine("Server started");
            //
            // while (true)
            // {
            //     Console.WriteLine("Client connected");
            //
            //     // Generate random object type
            //     Random random = new Random();
            //     int objectType = random.Next(1, 4);
            //     // int objectType = 3;
            //     // Generate file
            //     string extension = "";
            //     byte[] fileData = null;
            //     switch (objectType)
            //     {
            //         case 1:
            //             extension = "txt";
            //             fileData = GenerateTextFile();
            //             break;
            //         case 2:
            //             extension = "html";
            //             fileData = GenerateHtmlFile();
            //             break;
            //         case 3:
            //             extension = "jpg";
            //             fileData = GenerateImageFile();
            //             break;
            //     }
            //
            //     var receivedBytes = listener.Receive(ref endPoint);
            //     // Send file to client
            //     using (FileStream stream = new FileStream())
            //     {
            //         // Send file extension
            //         byte[] extensionData = Encoding.UTF8.GetBytes(extension);
            //         stream.Write(extensionData, 0, extensionData.Length);
            //         Console.WriteLine($"file.{extension} sent");
            //         // Send file data
            //         stream.Write(fileData, 0, fileData.Length);
            //         // Console.WriteLine($"File: {Encoding.UTF8.GetString(fileData)} sent");
            //     }
            //
            //     client.Close();
            //     Console.WriteLine("Client disconnected");
            // }

            int serverPort = 8123;
            UdpClient server = new UdpClient(serverPort);

            Console.WriteLine("Server started on port {0}", serverPort);

            while (true)
            {
                // Receive request
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] request = server.Receive(ref clientEndPoint);
                string requestString = System.Text.Encoding.ASCII.GetString(request);

                Console.WriteLine("Received request from client {0}: {1}", clientEndPoint, requestString);

                // Generate file based on request
                byte[] fileData;
                string extension;
                Random rnd = new Random();
                int objectType = rnd.Next(1, 4);
                switch (objectType)
                {
                    case 1:
                        fileData = GenerateTextFile();
                        extension = "txt";
                        break;
                    case 2:
                        fileData = GenerateHtmlFile();
                        extension = "html";
                        break;
                    case 3:
                        fileData = GenerateImageFile();
                        extension = "jpg";
                        break;
                    default:
                        fileData = new byte[0];
                        extension = "";
                        break;
                }

                // Send file extension
                byte[] extensionData = System.Text.Encoding.ASCII.GetBytes(extension);
                server.Send(extensionData, extensionData.Length, clientEndPoint);

                // Send file data
                server.Send(fileData, fileData.Length, clientEndPoint);

                Console.WriteLine("Sent file with extension {0} to client {1}", extension, clientEndPoint);
                
            }
        }

        static byte[] GenerateTextFile()
        {
            string text = "Hello, world!";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
            return data;
        }

        static byte[] GenerateHtmlFile()
        {
            string html = "<html><body><h1>Hello, world!</h1></body></html>";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(html);
            return data;
        }

        static byte[] GenerateImageFile()
        {
            
            byte[] data = File.ReadAllBytes("image.jpg");
            return data;
        }
    }
}
