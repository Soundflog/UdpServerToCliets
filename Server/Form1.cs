using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        private UdpClient _udpServer;
        private const int ServerPort = 8000;
        private List<RadioButton> radioButtons = new List<RadioButton>();
        
        public Form1()
        {
            InitializeComponent();
            
            
            radioButtons.Add(radioButton1);
            radioButtons.Add(radioButton2);
            radioButtons.Add(radioButton3);
            radioButtons.Add(radioButton4);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _udpServer = new UdpClient(ServerPort);
            label1.Text = @"Server started on port " + ServerPort;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
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

        static byte[] GenerateGraphics(string text)
        {
            using (Bitmap image = new Bitmap(100, 100))
            {
                // Создаем новый объект Graphics из Bitmap
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    // Настраиваем параметры рисования
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.Clear(Color.White); // Заливаем изображение белым цветом
                    using (Font font = new Font("Arial", 8))
                    {
                        using (Brush brush = new SolidBrush(Color.Black))
                        {
                            // Рисуем текст на изображении
                            graphics.DrawString(text, font, brush, new PointF(10, 10));
                        }
                    }

                    string filename = "image.jpg";
                    // Сохраняем изображение в формате JPEG
                    image.Save(filename, ImageFormat.Jpeg);
                }
            }
            byte[] data = File.ReadAllBytes("image.jpg");
            return data;
        }


        private void button2_Click(object sender, EventArgs e)
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
                
                string name = null;
                foreach (var radioButton in radioButtons)
                {
                    if (radioButton.Checked)
                    {
                         name = radioButton.Text;
                         MessageBox.Show(radioButton.Name);
                    }
                }

                /*Random random = new Random();
                int objectType = random.Next(1, 5);*/
                // int objectType = 4;
                switch (name)
                {
                    case "jpg":
                        fileData = GenerateGraphics("FROM JPG");
                        extension = "jpg";
                        break;
                    case "txt":
                        fileData = GenerateTextFile();
                        extension = "txt";
                        break;
                    case "png":
                        fileData = GenerateGraphics("FROM PNG");
                        extension = "png";
                        break;
                    case "html":
                        fileData = GenerateHtmlFile();
                        extension = "html";
                        break;
                    default:
                        fileData = GenerateTextFile();
                        extension = "txt";
                        break;
                }

                // Send file extension
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
    }
}