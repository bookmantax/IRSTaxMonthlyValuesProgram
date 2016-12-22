using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*
- remove all comma's, ctrl-f then delete
- Steps: Save file as a .csv.
- copy output statements into sql
*/

namespace CityMonthDatabaseGenerator
{
    public partial class Form1 : Form
    {
        PerDiemAmountsEntities db = new PerDiemAmountsEntities();
        List<string> months = new List<string>() { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        List<string> files;
        List<string> listA;
        string state, city, basepath, seasonBegin, seasonEnd;
        int lodging, meals, row, total, year;

        public Form1()
        {
            InitializeComponent();
            yearComboBox.SelectedIndex = 1;
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            Initialize();
            DirectoryInfo d = new DirectoryInfo(basepath);
            FileInfo[] Files = d.GetFiles("*.csv"); //Getting csv files
            foreach (FileInfo file in Files)
            {
                files.Add(basepath + "\\" + file.Name);
            }
            if(files.Count > 0)
            {
                foreach (string file in files)
                {
                    try
                    {
                        StreamReader reader = new StreamReader(File.OpenRead(file), Encoding.UTF8);
                        listA = new List<string>();
                        while (!reader.EndOfStream)
                        {
                            #region starting lines
                            if (row == 1)
                            {
                                reader.ReadLine();
                                reader.ReadLine();
                                row++;
                            }
                            #endregion
                            ReadLine(reader);
                        }
                        reader.Close();
                        errorLabel.Text = "Complete";
                    }
                    catch (Exception ex)
                    { Console.Write(ex); }
                }
                
            }
        }

        private void ReadLine(StreamReader reader)
        {
            bool start = false, monthsSwitched = false; ;
            var line = reader.ReadLine();
            var values = line.Split(';');
            listA = ((string)values[0]).Split(',').ToList();
            if (listA.ElementAt(0) != "")
            {
                try
                {
                    state = listA.ElementAt(1);
                    if (city != listA.ElementAt(2))
                        city = listA.ElementAt(2);
                    seasonBegin = listA.ElementAt(4);
                    seasonEnd = listA.ElementAt(5);
                    lodging = Int32.Parse(listA.ElementAt(listA.Count - 2).Substring(2));
                    meals = Int32.Parse(listA.ElementAt(listA.Count - 1).Substring(2));
                    total = lodging + meals;
                    var cityPerDiem = (from p in db.CityPerDiems where p.City == city && p.State == state select p).FirstOrDefault();
                    if (cityPerDiem != null)
                    {
                        if (seasonEnd == "")
                        {
                            foreach (string s in months)
                            {
                                Console.WriteLine("insert into " + s + "PerDiem(Year, Meals, Lodging, Total, CityId) values(" + year +
                                    ", " + meals + ", " + lodging + ", " + total + ", " + cityPerDiem.Id + ")");
                            }
                        }
                        else
                        {
                            foreach (string s in months)
                            {
                                if (s == seasonBegin.Substring(0, 3).ToUpper())
                                {
                                    start = true;
                                }
                                if (start)
                                {
                                    Console.WriteLine("insert into " + s + "PerDiem(Year, Meals, Lodging, Total, CityId) values(" + year +
                                    ", " + meals + ", " + lodging + ", " + total + ", " + cityPerDiem.Id + ")");
                                }
                                if (s == seasonEnd.Substring(0, 3).ToUpper() && start)
                                {
                                    break;
                                }
                                else if (s == "DEC")
                                {
                                    monthsSwitched = true;
                                }
                            }
                            if (monthsSwitched)
                            {
                                foreach (string s in months)
                                {
                                    Console.WriteLine("insert into " + s + "PerDiem(Year, Meals, Lodging, Total, CityId) values(" + year +
                                    ", " + meals + ", " + lodging + ", " + total + ", " + cityPerDiem.Id + ")");
                                    if (s == seasonEnd.Substring(0, 3).ToUpper())
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            }
        }

        private void Initialize()
        {
            basepath = filePathTextBox.Text;
            state = "";
            city = "";
            lodging = 0;
            meals = 0;
            files = new List<string>();
            row = 1;
            year = Int32.Parse(yearComboBox.SelectedItem.ToString());
        }
    }
}
