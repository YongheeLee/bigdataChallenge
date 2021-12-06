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

        List<string> esse = new List<string> { "서비스", "플랫폼", "개발", "자동차", "의료", " 커머스", "쇼핑", "헬스케어", "농업", "데이터", "인공지능",
                "공장", "자동화", "분석", "교육", "투자", "콘텐츠", "앱", "마케팅", "패션", "금융", "게임", "여행", "클라우드", "컨설팅", "채용", "부동산", "소통", "미디어", "예술", "디자인", "B2B", "B2C", "결제", "자산", "핀테크", "블록체인", "네트워크", "병원", "자율주행", "홈페이지", "스포츠", "언어", "무역","SNS", "스타트업", "음악"};


        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            string comPath = ConfigurationManager.AppSettings["company"];
            string svcPath = ConfigurationManager.AppSettings["service"];
            string jobPath = ConfigurationManager.AppSettings["job"];

            DataManager.Instance.LoadCSV(comPath, svcPath, jobPath);
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
                if (item.Jobs.Count() == 0)
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

        private void jobAraBtn_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> tech = new Dictionary<string, int>();
            List<ItemCountT> techList = new List<ItemCountT>();

            foreach (var item in DataManager.Instance.JobInfos)
            {
                foreach(var nt in item.Techs)
                {
                    if (tech.ContainsKey(nt) == false)
                        tech.Add(nt, 0);

                    ++tech[nt];
                }
            }

            foreach (var item in tech)
            {
                var jobList = DataManager.Instance.JobInfos.Where(s => s.Technique != null && s.Technique.Contains(item.Key));
                double low = 0, high = 0;
                double lowCnt = 0, highCnt = 0;

                foreach (var sub in jobList)
                {
                    if (sub.MinSalary != -1)
                    {
                        low += sub.MinSalary;
                        lowCnt += 1;
                    }

                    if (sub.MaxSalary != -1)
                    {
                        high += sub.MaxSalary;
                        highCnt += 1;
                    }
                }

                if(low< 0 || high<0)
                {

                }

                techList.Add(new ItemCountT { Item = item.Key, Count = item.Value, Min = lowCnt == 0 ? 0 : (int)(low / lowCnt), Max = highCnt == 0 ? 0 : (int)(high / highCnt) });


                if (techList.Last().Max<0)
                {

                }
            }

            jobTechLV.ItemsSource = null;
            jobTechLV.ItemsSource = techList.OrderByDescending(s => s.Count).ToList();
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
                    if (esse.FirstOrDefault(s => s == word) == null)
                        continue;

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
            techRelateLV.ItemsSource = keyList.OrderByDescending(s => s.Count).ToList();



        }

        private SortListView selList = null;

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                var sb = new StringBuilder();
                var selectedItems = selList.SelectedItems;

                foreach (var item in selectedItems)
                {
                    ItemCountT a = item as ItemCountT;
                    sb.Append(string.Format("{0},{1}\n", a.Item, a.Count));
                }

                Clipboard.SetDataObject(sb.ToString());
            }
        }

        private void ctrAraBtn_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> cont = new Dictionary<string, int>();
            List<ItemCountT> contList = new List<ItemCountT>();

            foreach (var item in DataManager.Instance.JobInfos)
            {
                if (item.Career != null)
                {
                    string str = item.Career;
                    str = str.Replace("\"", "");

                    string[] fields = str.Split(',');

                    for (int i = 0; i < fields.Count(); ++i)
                    {
                        if (cont.ContainsKey(fields[i]) == false)
                            cont.Add(fields[i], 0);

                        ++cont[fields[i]];
                    }
                }
            }

            foreach (var item in cont)
            {
                contList.Add(new ItemCountT { Item = item.Key, Count = item.Value });
            }
            cntTypeLV.ItemsSource = null;
            cntTypeLV.ItemsSource = contList.OrderByDescending(s => s.Count);
        }

        private void ctrrelateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (cntTypeLV.SelectedItem == null)
                return;

            ItemCountT item = cntTypeLV.SelectedItem as ItemCountT;

            var jobList = DataManager.Instance.JobInfos.Where(s => s.Career != null && s.Career.Contains(item.Item));
            var comList = DataManager.Instance.ComInfos.Where(s => s.Jobs.Where(j => j.Career != null && j.Career.Contains(item.Item)).Count() > 0);

            Dictionary<string, int> temp1 = new Dictionary<string, int>();
            Dictionary<string, int> temp2 = new Dictionary<string, int>();
            List<ItemCountT> keyList1 = new List<ItemCountT>();
            List<ItemCountT> keyList2 = new List<ItemCountT>();

            foreach (var sub in comList)
            {
                foreach (var word in sub.Keyword)
                {
                    if (esse.FirstOrDefault(s => s == word) == null)
                        continue;

                    if (temp2.ContainsKey(word) == false)
                        temp2.Add(word, 0);

                    ++temp2[word];
                }

            }

            foreach (var sub in jobList)
            {
                foreach (var nt in sub.Techs)
                {
                    if (temp1.ContainsKey(nt) == false)
                        temp1.Add(nt, 0);

                    ++temp1[nt];
                }
            }


            foreach (var sub in temp1)
            {
                keyList1.Add(new ItemCountT { Item = sub.Key, Count = sub.Value });
            }

            foreach (var sub in temp2)
            {
                keyList2.Add(new ItemCountT { Item = sub.Key, Count = sub.Value });
            }
            cntTechLV.ItemsSource = null;
            cntTechLV.ItemsSource = keyList1.OrderByDescending(s => s.Count).ToList();

            cntKeyLV.ItemsSource = null;
            cntKeyLV.ItemsSource = keyList2.OrderByDescending(s => s.Count).ToList();
        }

        private void dateAraBtn_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> date = new Dictionary<string, int>();
            List<ItemCountT> dateList = new List<ItemCountT>();

            foreach (var item in DataManager.Instance.ComInfos)
            {
                if (item.Jobs.Count() == 0)
                    continue;

                if (item.FoundDate != null)
                {
                    string str = item.FoundDate.Substring(0, 4);

                    if (date.ContainsKey(str) == false)
                        date.Add(str, 0);

                    ++date[str];

                }
            }

            foreach (var item in date)
            {
                dateList.Add(new ItemCountT { Item = item.Key, Count = item.Value });
            }
            dateLV.ItemsSource = null;
            dateLV.ItemsSource = dateList.OrderByDescending(s => s.Count).ToList();
        }

        private void daterelateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dateLV.SelectedItem == null)
                return;

            ItemCountT item = dateLV.SelectedItem as ItemCountT;

            var comList = DataManager.Instance.ComInfos.Where(s => s.FoundDate != null && s.FoundDate.Contains(item.Item));

            Dictionary<string, int> temp1 = new Dictionary<string, int>();
            Dictionary<string, int> temp2 = new Dictionary<string, int>();
            List<ItemCountT> keyList1 = new List<ItemCountT>();
            List<ItemCountT> keyList2 = new List<ItemCountT>();

            foreach (var sub in comList)
            {
                foreach (var word in sub.Keyword)
                {
                    if (esse.FirstOrDefault(s => s == word) == null)
                        continue;

                    if (temp2.ContainsKey(word) == false)
                        temp2.Add(word, 0);

                    ++temp2[word];
                }

            }

            foreach (var sub in comList)
            {
                if (sub.Jobs.Count() == 0)
                    continue;

                foreach (var job in sub.Jobs)
                {
                    foreach (var nt in job.Techs)
                    {
                        if (temp1.ContainsKey(nt) == false)
                            temp1.Add(nt, 0);

                        ++temp1[nt];
                    }
                }

            }


            foreach (var sub in temp1)
            {
                keyList1.Add(new ItemCountT { Item = sub.Key, Count = sub.Value });
            }

            foreach (var sub in temp2)
            {
                keyList2.Add(new ItemCountT { Item = sub.Key, Count = sub.Value });
            }
            dateTechLV.ItemsSource = null;
            dateTechLV.ItemsSource = keyList1.OrderByDescending(s => s.Count).ToList();

            dateKeyLV.ItemsSource = null;
            dateKeyLV.ItemsSource = keyList2.OrderByDescending(s => s.Count).ToList();
        }

        private void LV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selList = sender as SortListView;
        }

        private void keyAraBtn_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> temp1 = new Dictionary<string, int>();
            List<ItemCountT> keyList1 = new List<ItemCountT>();

            Dictionary<string, int> min = new Dictionary<string, int>();
            Dictionary<string, int> max = new Dictionary<string, int>();
            Dictionary<string, int> mincnt = new Dictionary<string, int>();
            Dictionary<string, int> maxcnt = new Dictionary<string, int>();


            foreach (var sub in DataManager.Instance.ComInfos)
            {
                if (sub.Jobs.Count() == 0)
                    continue;

                foreach (var word in sub.Keyword)
                {
                    if (esse.FirstOrDefault(s => s == word) == null)
                        continue;

                    if (temp1.ContainsKey(word) == false)
                    {
                        temp1.Add(word, 0);
                        min.Add(word, 0);
                        max.Add(word, 0);
                        mincnt.Add(word, 0);
                        maxcnt.Add(word, 0);
                    }
                    ++temp1[word];

                    foreach (var job in sub.Jobs)
                    {
                        min[word] += job.MinSalary != -1 ? job.MinSalary : 0;
                        mincnt[word] += job.MinSalary != -1 ? 1 : 0;
                        max[word] += job.MaxSalary != -1 ? job.MaxSalary : 0;
                        maxcnt[word] += job.MaxSalary != -1 ? 1 : 0;
                    }
                }


            }

            foreach (var sub in temp1)
            {
                double minavs = mincnt[sub.Key] == 0 ? 0 : min[sub.Key] / mincnt[sub.Key];
                double maxavs = maxcnt[sub.Key] == 0 ? 0 : max[sub.Key] / maxcnt[sub.Key];

                keyList1.Add(new ItemCountT { Item = sub.Key, Count = sub.Value, Min = (int)minavs, Max = (int)maxavs });
            }
            keyLV.ItemsSource = null;
            keyLV.ItemsSource = keyList1.OrderByDescending(s => s.Count).ToList();

            keyComLV.ItemsSource = null;
            keyComLV.ItemsSource = keyList1.OrderByDescending(s => s.Count).ToList();

        }

        private void keyrelateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (keyLV.SelectedItem == null)
                return;

            ItemCountT item = keyLV.SelectedItem as ItemCountT;

            Dictionary<string, int> temp1 = new Dictionary<string, int>();
            List<ItemCountT> keyList1 = new List<ItemCountT>();

            
            foreach (var sub in DataManager.Instance.ComInfos)
            {
                if (sub.Keyword.FirstOrDefault(s => s.Contains(item.Item)) == null)
                    continue;

                foreach (var job in sub.Jobs)
                {
                    foreach (var nt in job.Techs)
                    {
                        if (temp1.ContainsKey(nt) == false)
                        {
                            temp1.Add(nt, 0);
                           
                        }

                       

                        ++temp1[nt];
                    }
                }
            }

            foreach (var sub in temp1)
            {
                keyList1.Add(new ItemCountT { Item = sub.Key, Count = sub.Value });
            }
            keyTechLV.ItemsSource = null;
            keyTechLV.ItemsSource = keyList1.OrderByDescending(s => s.Count).ToList();
        }

        private void comDetailBtn_Click(object sender, RoutedEventArgs e)
        {
            if (keyComLV.SelectedItem == null)
                return;

            ItemCountT cnt = keyComLV.SelectedItem as ItemCountT;

            List<CompanyInfoT> newInfo = new List<CompanyInfoT>();
            foreach(var com in DataManager.Instance.ComInfos)
            {
                if (com.Keyword.FirstOrDefault(s => s.Contains(cnt.Item)) == null)
                    continue;

                newInfo.Add(com);
            }

            keyComInfoLV.ItemsSource = null;
            keyComInfoLV.ItemsSource = newInfo;

        }
    }
}

