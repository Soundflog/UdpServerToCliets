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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WInFormUDP
{
    public partial class Form1 : Form
    {
        private const string BasePath = @"E:\Github repository\UdpServerToCliets\UdpServerToCliets\bin\Debug";
        private static List<string> _paths = new() { BasePath + "\\html.html", 
            BasePath + "\\image.jpg",
            BasePath + "\\text.txt"};
        
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromHwnd(this.Handle);

            List<string> addresses = new List<string>();

            // CheckIpValue(textBox1.Text, ref addresses);
            //CheckIpValue(secondIpTBox.Text, ref addresses);
            //CheckIpValue(thirdIpTBox.Text, ref addresses);
            SendTextFiles(textBox1.Text);
        }
        
        private static void SendTextFiles(string addresses)
        {
            Random rnd = new Random();
            Task[] tasks = new Task[1];
            int fileIndex = rnd.Next(_paths.Count);
            tasks[addresses.IndexOf(addresses, StringComparison.Ordinal)] = Task.Run(() => { 
                SendFileByIp(addresses, _paths.ElementAt(3));
            });

            Task.WaitAll(tasks);
            MessageBox.Show("Все файлы успешно отправлены!");
        }

        private static void SendFileByIp(string ip, string fileAddress)
        {
            try
            {

                IPAddress remoteIPAddress = IPAddress.Parse(ip);
                const int remotePort = 5007;
                UdpClient sender = new UdpClient();
                IPEndPoint endPoint = new IPEndPoint(remoteIPAddress, remotePort);

                SendFileInfo(fileAddress, sender, endPoint);
                Thread.Sleep(2000);
                SendFile(fileAddress, sender, endPoint);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка!");
            }
        }

        public static void SendFileInfo(string fileAddress, UdpClient sender, IPEndPoint endPoint)
        {
            FileStream fs = new FileStream(@fileAddress, FileMode.Open, FileAccess.Read);
            FileDetails fileDet = new FileDetails();

            fileDet.FILETYPE = fs.Name.Substring(fs.Name.LastIndexOf("."));
            fileDet.FILESIZE = fs.Length;
            XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDetails));
            MemoryStream stream = new MemoryStream();

            fileSerializer.Serialize(stream, fileDet);
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, Convert.ToInt32(stream.Length));

            sender.Send(bytes, bytes.Length, endPoint);
            stream.Close();
        }

        private static void SendFile(string fileAddress, UdpClient sender, IPEndPoint endPoint)
        {
            FileStream fs = new FileStream(@fileAddress, FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[fs.Length];

            fs.Read(bytes, 0, bytes.Length);

            try
            {
                sender.Send(bytes, bytes.Length, endPoint);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Ошибка!");
            }
            finally
            {
                fs.Close();
                sender.Close();
            }
        }

        private static void CheckIpValue(string ip, ref List<string> adresses)
        {
            if (ip.Equals("-") || string.IsNullOrEmpty(ip) || string.IsNullOrWhiteSpace(ip))
            {
                return;
            }

            adresses.Add(ip);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
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
            _paths.Add(filename);
        }
    }
}