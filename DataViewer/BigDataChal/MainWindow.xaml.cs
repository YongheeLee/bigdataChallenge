using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
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

namespace BigDataChal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            string comPath = ConfigurationManager.AppSettings["company"];
            string svcPath = ConfigurationManager.AppSettings["service"];
            string jobPath = ConfigurationManager.AppSettings["job"];

            DataManager.Instance.LoadCSV(comPath, svcPath, jobPath);
        }

        private void jobAraBtn_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> tech = new Dictionary<string, int>();
            List<ItemCountT> techList = new List<ItemCountT>();

            foreach (var item in DataManager.Instance.JobInfos)
            {
                if (swonlyCK.IsChecked.Value && item.Role.Contains("SW") == false)
                    continue;

                if (item.Technique != null)
                {
                    string str = item.Technique;
                    str = str.Replace("\"", "");

                    string[] fields = str.Split(',');

                    for (int i = 0; i < fields.Count(); ++i)
                    {
                        if (tech.ContainsKey(fields[i]) == false)
                            tech.Add(fields[i], 0);

                        ++tech[fields[i]];
                    }
                }
            }

            foreach (var item in tech)
            {
                techList.Add(new ItemCountT { Item = item.Key, Count = item.Value });
            }
            jobTechLV.ItemsSource = null;
            jobTechLV.ItemsSource = techList;
        }

        private void comTxtBtn_Click(object sender, RoutedEventArgs e)
        {
            string dir = System.IO.Path.Combine(ConfigurationManager.AppSettings["workingDir"], "Text");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            foreach (var item in DataManager.Instance.ComInfos)
            {
                string file = System.IO.Path.Combine(dir, string.Format("{0}.txt", item.ID));
                System.IO.File.Delete(file);
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(new System.IO.FileStream(file, System.IO.FileMode.Create), Encoding.UTF8))
                {
                    if (item.ShortIntro != null)
                        sw.Write(item.ShortIntro);

                    if (item.FullIntro != null)
                        sw.Write(item.FullIntro);
                }
            }
        }


        private void comTxtLimitBtn_Click(object sender, RoutedEventArgs e)
        {
            string dir = System.IO.Path.Combine(ConfigurationManager.AppSettings["workingDir"], "Text");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);


            foreach (var item in DataManager.Instance.ComInfos)
            {
                string file = System.IO.Path.Combine(dir, string.Format("{0}_lmt.txt", item.ID));
                System.IO.File.Delete(file);
                if (item.Jobs.Where(s => s.Role.Contains("SW")).Count() == 0)
                    continue;

                System.IO.File.Delete(file);
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(new System.IO.FileStream(file, System.IO.FileMode.Create), Encoding.UTF8))
                {
                    if (item.ShortIntro != null)
                        sw.Write(item.ShortIntro);

                    if (item.FullIntro != null)
                        sw.Write(item.FullIntro);
                }
            }
        }

        private void relateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (jobTechLV.SelectedItem == null)
                return;

            ItemCountT item = jobTechLV.SelectedItem as ItemCountT;

            var list = DataManager.Instance.ComInfos.Where(s => s.Jobs.Where(j => j.Technique != null && j.Technique.Contains(item.Item)).Count() > 0);

            Dictionary<string, int> temp = new Dictionary<string, int>();
            List<ItemCountT> keyList = new List<ItemCountT>();

            foreach (var sub in list)
            {
                foreach (var word in sub.Keyword)
                {
                    if (temp.ContainsKey(word) == false)
                        temp.Add(word, 0);

                    ++temp[word];
                }

            }

            foreach (var sub in temp)
            {
                keyList.Add(new ItemCountT { Item = sub.Key, Count = sub.Value });
            }

            techRelateLV.ItemsSource = null;
            techRelateLV.ItemsSource = keyList;
        }
    }
}

