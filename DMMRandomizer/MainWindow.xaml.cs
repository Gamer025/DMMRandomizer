using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace DMMRandomizer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Randomize_Button_Click(object sender, RoutedEventArgs e)
        {
            if (DMMFile_Box.Text != "DMI File")
            {
                string line;
                int counter = 1; //Counter for current line number
                int startingLine = 0; //The line number at which the map data starts (?,?,z-level)
                int rowAmount = 0; //How much tiles a row contains
                List<string> mapData = new List<string>(); //All the data that defines which tile to use at which position

                using (StreamReader sr = new StreamReader(DMMFile_Box.Text))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("(1,1,1)"))
                        {
                            startingLine = counter;
                        }
                        if (line.StartsWith("\"}") && startingLine != 0)
                        {
                            rowAmount = counter - startingLine - 1;
                            break;
                        }

                        counter++;
                    }
                }
                Debug.WriteLine(startingLine);
                Debug.WriteLine(rowAmount);

                counter = 0;

                using (StreamReader sr = new StreamReader(DMMFile_Box.Text))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (counter >= startingLine + 1)
                        {
                            if (!line.StartsWith("(") && !line.StartsWith("\"") && line != "aaa")
                            {
                                mapData.Add(line);
                            }
                        }

                        counter++;
                    }
                }

                counter = 0;

                using (StreamReader sr = new StreamReader(DMMFile_Box.Text))
                {
                    using (var writer = new StreamWriter(DMMFile_Box.Text + ".new"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (counter >= startingLine + 1)
                            {
                                if (!line.StartsWith("(") && !line.StartsWith("\"") && line != "aaa")
                                {
                                    int random = rnd.Next(0, mapData.Count);
                                    writer.WriteLine(mapData[random]);
                                    mapData.RemoveAt(random);
                                }
                                else
                                    writer.WriteLine(line);

                            }
                            else
                            {
                                writer.WriteLine(line);
                            }
                            counter++;
                        }
                    }
                }

            }

        }

        private void SelectDMMFIle_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Dream Maker Map|*.dmm";
            if (openFileDialog.ShowDialog() == true)
            {
                DMMFile_Box.Text = openFileDialog.FileName;
            }
        }
    }
}
