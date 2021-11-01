//Amir Hossein Zamani  9812151018
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ThreadingProject
{
    public partial class MainForm : Form
    {
        private string[] filePaths;
        private int fileCount = 10;
        string inputPath;

        public MainForm()
        {
            string[] tempPath;
            tempPath = (System.Reflection.Assembly.GetExecutingAssembly().Location).Split((@"\".ToCharArray())[0]); //Get application path (Path ends with a .exe file)
            for (int i = 0; i < tempPath.Length - 1; i++) //remove .exe file name from path
            {
                inputPath += tempPath[i];
                inputPath += @"\";
            }
            inputPath = inputPath + "Input Files";

            InitializeComponent();

            openFileDialog1.InitialDirectory = inputPath;
        }

        private void buttonInput_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(inputPath))
                Directory.CreateDirectory(inputPath);

            Thread t1 = new Thread(() =>
            {
                for (int i = 0; i < 4; i++)
                {
                    using (FileStream fstream = File.Create(inputPath + @"\input" + i + ".txt"))
                    {
                        Random randomNumber = new Random();
                        int fileNumberCount = randomNumber.Next(0, 100000);

                        for (int j = 0; j < fileNumberCount; j++)
                        {
                            int offset = 0;
                            string temp = Convert.ToString(randomNumber.Next(0, 100000)) + "\n";
                            fstream.Write(new UTF8Encoding(true).GetBytes(temp), offset, temp.Length);
                            offset += temp.Length;
                        }
                    }
                }
            });

            Thread t2 = new Thread(() =>
            {
                for (int i = 4; i < 7; i++)
                {
                    using (FileStream fstream = File.Create(inputPath + @"\input" + i + ".txt"))
                    {
                        Random randomNumber = new Random();
                        int fileNumberCount = randomNumber.Next(0, 100000);

                        for (int j = 0; j < fileNumberCount; j++)
                        {
                            int offset = 0;
                            string temp = Convert.ToString(randomNumber.Next(0, 100000)) + "\n";
                            fstream.Write(new UTF8Encoding(true).GetBytes(temp), offset, temp.Length);
                            offset += temp.Length;
                        }
                    }
                }
            });

            Thread t3 = new Thread(() =>
            {
                for (int i = 7; i < 10; i++)
                {
                    using (FileStream fstream = File.Create(inputPath + @"\input" + i + ".txt"))
                    {
                        Random randomNumber = new Random();
                        int fileNumberCount = randomNumber.Next(0, 1000000);  //Create random number

                        for (int j = 0; j < fileNumberCount; j++)
                        {
                            int offset = 0;
                            string temp = Convert.ToString(randomNumber.Next(0, 1000000)) + "\n";  //Create random number
                            fstream.Write(new UTF8Encoding(true).GetBytes(temp), offset, temp.Length);
                            offset += temp.Length;
                        }
                    }
                }
            });


            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();

            checkBox1.Checked = true;
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileNames.Length != fileCount)
                    MessageBox.Show("You should import at least " + fileCount + " files");

                else
                {
                    filePaths = openFileDialog1.FileNames;
                    checkBox2.Checked = true;
                    ButtonGo.Enabled = true;
                }
            }
        }

        private void ButtonGo_Click(object sender, EventArgs e)
        {
            double[] result = new double[2];

            //1 Thread
            result = Thread1(filePaths);
            textBoxResult1.Text = result[0].ToString();
            textBox1.Text = result[1].ToString() + " ms";

            //3 Thread
            result = Thread3(filePaths);
            textBoxResult3.Text = result[0].ToString();
            textBox3.Text = result[1].ToString() + " ms";

            //5 Threads
            result = Thread5(filePaths);
            textBoxResult5.Text = result[0].ToString();
            textBox5.Text = result[1].ToString() + " ms";

            //10 Threads
            result = Thread10(filePaths);
            textBoxResult10.Text = result[0].ToString();
            textBox10.Text = result[1].ToString() + " ms";
        }

        private double[] Thread1(string[] fileNames)
        {
            DateTime startTime = DateTime.Now;

            double sum = 0;
            for (int i = 0; i < fileCount; i++)
            {
                using (StreamReader reader = new StreamReader(fileNames[i]))
                {
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                        sum += Convert.ToDouble(temp);
                }
            }


            return new double[2] { sum, (DateTime.Now - startTime).TotalMilliseconds };
        }

        private double[] Thread3(string[] fileNames)
        {
            DateTime startTime = DateTime.Now;

            double sum1 = 0;
            Thread t1 = new Thread(() =>
              {
                  for (int i = 0; i < 4; i++)
                  {
                      using (StreamReader reader = new StreamReader(fileNames[i]))
                      {
                          Console.WriteLine("File " + i + " Readed.");
                          string temp;
                          while ((temp = reader.ReadLine()) != null)
                              sum1 += Convert.ToDouble(temp);
                      }
                  }
              });

            double sum2 = 0;
            Thread t2 = new Thread(() =>
                {
                    for (int i = 4; i < 7; i++)
                    {
                        using (StreamReader reader = new StreamReader(fileNames[i]))
                        {
                            Console.WriteLine("File " + i + " Readed.");
                            string temp;
                            while ((temp = reader.ReadLine()) != null)
                                sum2 += Convert.ToDouble(temp);
                        }
                    }
                });

            double sum3 = 0;
            Thread t3 = new Thread(() =>
            {
                for (int i = 7; i < 10; i++)
                {
                    using (StreamReader reader = new StreamReader(fileNames[i]))
                    {
                        Console.WriteLine("File " + i + " Readed.");
                        string temp;
                        while ((temp = reader.ReadLine()) != null)
                            sum3 += Convert.ToDouble(temp);
                    }
                }
            });

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();


            return new double[2] { sum1 + sum2 + sum3, (DateTime.Now - startTime).TotalMilliseconds };
        }

        private double[] Thread5(string[] fileNames)
        {
            DateTime startTime = DateTime.Now;

            double sum1 = 0;
            Thread t1 = new Thread(() =>
            {
                for (int i = 0; i < 2; i++)
                {
                    using (StreamReader reader = new StreamReader(fileNames[i]))
                    {
                        Console.WriteLine("File " + i + " Readed.");
                        string temp;
                        while ((temp = reader.ReadLine()) != null)
                            sum1 += Convert.ToDouble(temp);
                    }
                }
            });

            double sum2 = 0;
            Thread t2 = new Thread(() =>
            {
                for (int i = 2; i < 4; i++)
                {
                    using (StreamReader reader = new StreamReader(fileNames[i]))
                    {
                        Console.WriteLine("File " + i + " Readed.");
                        string temp;
                        while ((temp = reader.ReadLine()) != null)
                            sum2 += Convert.ToDouble(temp);
                    }
                }
            });

            double sum3 = 0;
            Thread t3 = new Thread(() =>
            {
                for (int i = 4; i < 6; i++)
                {
                    using (StreamReader reader = new StreamReader(fileNames[i]))
                    {
                        Console.WriteLine("File " + i + " Readed.");
                        string temp;
                        while ((temp = reader.ReadLine()) != null)
                            sum3 += Convert.ToDouble(temp);
                    }
                }
            });

            double sum4 = 0;
            Thread t4 = new Thread(() =>
            {
                for (int i = 6; i < 8; i++)
                {
                    using (StreamReader reader = new StreamReader(fileNames[i]))
                    {
                        Console.WriteLine("File " + i + " Readed.");
                        string temp;
                        while ((temp = reader.ReadLine()) != null)
                            sum4 += Convert.ToDouble(temp);
                    }
                }
            });

            double sum5 = 0;
            Thread t5 = new Thread(() =>
            {
                for (int i = 8; i < 10; i++)
                {
                    using (StreamReader reader = new StreamReader(fileNames[i]))
                    {
                        Console.WriteLine("File " + i + " Readed.");
                        string temp;
                        while ((temp = reader.ReadLine()) != null)
                            sum5 += Convert.ToDouble(temp);
                    }
                }
            });

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
            t5.Join();


            return new double[2] { sum1 + sum2 + sum3 + sum4 + sum5, (DateTime.Now - startTime).TotalMilliseconds };
        }

        private double[] Thread10(string[] fileNames)
        {
            DateTime startTime = DateTime.Now;

            double sum1 = 0;
            Thread t1 = new Thread(() =>
            {
                using (StreamReader reader = new StreamReader(fileNames[0]))
                {
                    Console.WriteLine("File " + 0 + " Readed.");
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                        sum1 += Convert.ToDouble(temp);
                }
            });

            double sum2 = 0;
            Thread t2 = new Thread(() =>
            {
                using (StreamReader reader = new StreamReader(fileNames[1]))
                {
                    Console.WriteLine("File " + 1 + " Readed.");
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                        sum2 += Convert.ToDouble(temp);
                }
            });

            double sum3 = 0;
            Thread t3 = new Thread(() =>
            {
                using (StreamReader reader = new StreamReader(fileNames[2]))
                {
                    Console.WriteLine("File " + 2 + " Readed.");
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                        sum3 += Convert.ToDouble(temp);
                }
            });

            double sum4 = 0;
            Thread t4 = new Thread(() =>
            {
                using (StreamReader reader = new StreamReader(fileNames[3]))
                {
                    Console.WriteLine("File " + 3 + " Readed.");
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                        sum4 += Convert.ToDouble(temp);
                }

            });

            double sum5 = 0;
            Thread t5 = new Thread(() =>
            {
                using (StreamReader reader = new StreamReader(fileNames[4]))
                {
                    Console.WriteLine("File " + 4 + " Readed.");
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                        sum5 += Convert.ToDouble(temp);
                }
            });

            double sum6 = 0;
            Thread t6 = new Thread(() =>
            {
                using (StreamReader reader = new StreamReader(fileNames[5]))
                {
                    Console.WriteLine("File " + 0 + " Readed.");
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                        sum6 += Convert.ToDouble(temp);
                }
            });

            double sum7 = 0;
            Thread t7 = new Thread(() =>
            {
                using (StreamReader reader = new StreamReader(fileNames[6]))
                {
                    Console.WriteLine("File " + 1 + " Readed.");
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                        sum7 += Convert.ToDouble(temp);
                }
            });

            double sum8 = 0;
            Thread t8 = new Thread(() =>
            {
                using (StreamReader reader = new StreamReader(fileNames[7]))
                {
                    Console.WriteLine("File " + 2 + " Readed.");
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                        sum8 += Convert.ToDouble(temp);
                }
            });

            double sum9 = 0;
            Thread t9 = new Thread(() =>
            {
                using (StreamReader reader = new StreamReader(fileNames[8]))
                {
                    Console.WriteLine("File " + 3 + " Readed.");
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                        sum9 += Convert.ToDouble(temp);
                }

            });

            double sum10 = 0;
            Thread t10 = new Thread(() =>
            {
                using (StreamReader reader = new StreamReader(fileNames[9]))
                {
                    Console.WriteLine("File " + 4 + " Readed.");
                    string temp;
                    while ((temp = reader.ReadLine()) != null)
                        sum10 += Convert.ToDouble(temp);
                }
            });


            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();
            t9.Start();
            t10.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
            t5.Join();
            t6.Join();
            t7.Join();
            t8.Join();
            t9.Join();
            t10.Join();

            return new double[2] { sum1 + sum2 + sum3 + sum4 + sum5 + sum6 + sum7 + sum8 + sum9 + sum10, (DateTime.Now - startTime).TotalMilliseconds };
        }
    }


}

