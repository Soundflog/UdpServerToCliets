using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UdpWinForm
{
    public partial class Form1 : Form
    {
        private UdpClient server;
        private bool isRunning;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            textBox1.Text = "8000";
            isRunning = false;
        }

        private void startButton_Click_1(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                // Start server
                int serverPort = int.Parse(textBox1.Text);
                server = new UdpClient(serverPort);
                isRunning = true;
                startButton.Text = "Stop";
                label1.Text = "Running on port " + serverPort;

                // Start listening for requests
                Task.Run(() => ListenForRequests());
            }
            else
            {
                // Stop server
                server.Close();
                isRunning = false;
                startButton.Text = "Start";
                label1.Text = "Stopped";
            }
        }

        private async Task ListenForRequests()
        {
            while (isRunning)
            {
                try
                {
                    // Receive request
                    IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] request = server.Receive();
                    string requestString = System.Text.Encoding.ASCII.GetString(request);

                    Invoke((MethodInvoker)delegate
                    {
                        LogMessage("Received request from client " + clientEndPoint + ": " + requestString);
                    });

                    // Generate file based on request
                    byte[] fileData;
                    string extension;
                    switch (requestString)
                    {
                        case "object1":
                            fileData = File.ReadAllBytes("file1.txt");
                            extension = "txt";
                            break;
                        case "object2":
                            fileData = File.ReadAllBytes("file2.html");
                            extension = "html";
                            break;
                        case "object3":
                            fileData = File.ReadAllBytes("file3.jpg");
                            extension = "jpg";
                            break;
                        default:
                            fileData = new byte[0];
                            extension = "";
                            break;
                    }

                    // Send file extension
                    byte[] extensionData = System.Text.Encoding.ASCII.GetBytes(extension);
                    await server.SendAsync(extensionData, extensionData.Length, clientEndPoint);

                    // Send file data
                    await server.SendAsync(fileData, fileData.Length, clientEndPoint);

                    Invoke((MethodInvoker)delegate
                    {
                        LogMessage("Sent file with extension " + extension + " to client " + clientEndPoint);
                    });
                }
                catch (SocketException ex)
                {
                    if (isRunning)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            LogMessage("Error receiving or sending data: " + ex.Message);
                        });
                    }
                }
            }
        }

        private void LogMessage(string message)
        {
            logTextBox.AppendText(message + "\n");
        }


        
    }
    
}