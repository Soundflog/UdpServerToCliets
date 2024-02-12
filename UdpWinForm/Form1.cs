using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace UdpWinForm
{
    public partial class Form1 : Form
    {
        
        private UdpClient _udpServer;
        private const int ServerPort = 8000;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            _udpServer = new UdpClient(ServerPort);
            label1.Text = @"Server started on port " + ServerPort;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_udpServer != null)
            {
                _udpServer.Close();
            }
        }

        private void startButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Receive request
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] request = _udpServer.Receive(ref clientEndPoint);
                string requestString = Encoding.ASCII.GetString(request);

                label2.Text = @"Received request from client " + clientEndPoint + @": " + requestString + Environment.NewLine;
                
                // Generate file based on request
                byte[] fileData;
                string extension;
                Random random = new Random();
                int objectType = random.Next(1, 5);
                // int objectType = 4;
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
                    case 4:
                        pictureBox1.Image.Save("picture.png", ImageFormat.Png);
                        byte[] data = File.ReadAllBytes("picture.png");
                        Process.Start("picture.png");
                        fileData = data;
                        extension = "png";
                        break;
                    default:
                        fileData = GenerateTextFile();
                        extension = "txt";
                        break;
                }

                // Send file extension
                // var threadSend = new Thread(SendFile);
                // threadSend.Start();
                byte[] extensionData = Encoding.ASCII.GetBytes(extension);
                _udpServer.Send(extensionData, extensionData.Length, clientEndPoint);

                // Send file data
                _udpServer.Send(fileData, fileData.Length, clientEndPoint);
                
                label2.Text = @"Sent file with extension " + extension + @" to client " + clientEndPoint + Environment.NewLine;
            }
            catch (Exception ex)
            {
                label2.Text = @"Error: " + ex.Message + Environment.NewLine;
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
            
            Random random = new Random();
            int objectType = random.Next(1, 4);
            switch (objectType)
            {
                case 1:
                    GenerateGraphics("Hello, world!");
                    break;
                case 2:
                    GenerateGraphics("HTML file");
                    break;
                case 3:
                    GenerateGraphics("Image file");
                    break;
            }

            Process.Start("image.jpg");
            byte[] data = File.ReadAllBytes("image.jpg");
            return data;
        }

        static void GenerateGraphics(string text)
        {
            using (Bitmap image = new Bitmap(100, 100))
            {
                // Создаем новый объект Graphics из Bitmap
                using (Graphics graphics = Graphics.FromImage(image))
                {
<<<<<<< Updated upstream
                    // Настраиваем параметры рисования
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.Clear(Color.White); // Заливаем изображение белым цветом
                    using (Font font = new Font("Arial", 8))
=======
                    // Receive request
                    IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    IPEndPoint remoteEP = null;
                    byte[] request = server.Receive(ref remoteEP);
                    string requestString = System.Text.Encoding.ASCII.GetString(request);

                    Invoke((MethodInvoker)delegate
>>>>>>> Stashed changes
                    {
                        using (Brush brush = new SolidBrush(Color.Black))
                        {
                            // Рисуем текст на изображении
                            graphics.DrawString(text, font, brush, new PointF(10, 10));
                        }
                    }

                    string filename = "image.jpg";
                    // Сохраняем изображение в формате JPEG
                    // var pictureBox1 = new PictureBox();
                    // pictureBox1.Image = image;
                    
                    image.Save(filename, ImageFormat.Jpeg);
                    
                    // pictureBox.Image = image;
                }
            }
        }
        

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            // string fileText = System.IO.File.ReadAllText(filename);
            // textBox1.Text = fileText;
            // MessageBox.Show("Файл открыт");
            pictureBox1.Image = Image.FromFile(filename);
        }

        
    }
    
}