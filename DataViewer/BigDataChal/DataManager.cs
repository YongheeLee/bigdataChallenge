using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BigDataChal
{
    public class DataManager
    {


        private static DataManager mgr = null;
        public static DataManager Instance
        {
            get
            {
                if (mgr == null)
                    mgr = new DataManager();
                return mgr;
            }
        }

        private Dictionary<TypeM, List<int>> typeEncode = new Dictionary<TypeM, List<int>>();

        private ObservableCollection<CompanyInfoT> comInfos = new ObservableCollection<CompanyInfoT>();
        private ObservableCollection<CompanyInfoT> comInfosSW = new ObservableCollection<CompanyInfoT>();

        private ObservableCollection<ServiceInfoT> svcInfos = new ObservableCollection<ServiceInfoT>();
        private ObservableCollection<JobInfoT> jobInfos = new ObservableCollection<JobInfoT>();
        private ObservableCollection<string> techInfos = new ObservableCollection<string>();

        private Dictionary<string, string> techRepName = new Dictionary<string, string>(); //스텍별로 대표할 이름을 담은 맵!
        private Dictionary<string, int> techIndexInfos = new Dictionary<string, int>(); //스텍별 대표이름을 인덱싱한 맵!

        public ObservableCollection<CompanyInfoT> ComInfos {
            get { return comInfosSW; }
            set { this.comInfosSW = value; }
        }
        public ObservableCollection<ServiceInfoT> SvcInfos { get { return svcInfos; } }
        public ObservableCollection<JobInfoT> JobInfos { get { return jobInfos; } }
        public ObservableCollection<string> TechGroupInfos {
            set { this.techInfos = value; }
            get { return this.techInfos; }
        }

        public void SetTechInfos(string techCategoryFile, string techNameFile) {

            if (!string.IsNullOrEmpty(techNameFile))
            {
                using (System.IO.StreamReader sr = System.IO.File.OpenText(techNameFile))
                {
                    string line = sr.ReadLine();
                    int index = 0;
                    while (line != null)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            string data = Regex.Replace(line, @"\s", "");
                            techIndexInfos.Add(data, index);
                            index++;
                        }
                        line = sr.ReadLine();
                    }
                }
            }

            if (!string.IsNullOrEmpty(techCategoryFile))
            {
                using (System.IO.StreamReader sr = System.IO.File.OpenText(techCategoryFile))
                {
                    string line = sr.ReadLine();

                        while (line != null)
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                string[] data = Regex.Replace(line, @"\s", "").Split(',');
                                if(data.Length > 1)
                                {
                                    if (data[1] == "" || data[1].Equals("xxxx")) //데이터가 확실히 분류되지 않거나 없는건 빈값처리!
                                    {
                                        addTechRepName(data[0], "");

                                    }
                                    else if (data[0] == "Column1" || data[1] == "Column2") { } //컬럼은 스킵
                                    else
                                    {
                                        addTechRepName(data[0], data[1]);
                                    }
                                }
                            } 
                            line = sr.ReadLine();
                         }
                  }
             }
        }

        private void addTechRepName(string key, string value) {
            if (!techRepName.ContainsKey(key)) techRepName.Add(key, value);
            Console.WriteLine("key = " + key + " / value = " + techRepName[key]);
        }

        public void LoadCSV(string companyFile, string serviceFile, string jobFile)
        {
            string dir = System.IO.Path.Combine(ConfigurationManager.AppSettings["workingDir"], "Text");

            Dictionary<int, int> comIdToIdx = new Dictionary<int, int>();
            if (!string.IsNullOrEmpty(companyFile))
            {
                using (System.IO.StreamReader sr = System.IO.File.OpenText(companyFile))
                {
                    string line = sr.ReadLine();

                    bool inContent = false;
                    string sub = null;
                    List<string> data = new List<string>();

                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null)
                            break;

                        for (int i = 0; i < line.Length; ++i)
                        {
                            if (!inContent && line[i] == ',')
                            {
                                data.Add(sub);
                                sub = null;
                            }
                            else if (line[i] == '\"')
                            {
                                inContent = !inContent;
                                sub += line[i];
                            }
                            else
                            {
                                sub += line[i];
                            }
                        }

                        if (data.Count == 18)
                        {
                            data.Add(sub);

                            if(data[11]!=null)
                            {

                            }
                            CompanyInfoT info = new CompanyInfoT
                            {
                                ID = int.Parse(data[0]),
                                KorName = data[1],
                                EngName = data[2],
                                ShortIntro = data[3],
                                FullIntro = data[4],
                                FoundDate = data[5],
                                EmployeeCnt = data[6],
                                Invsm = data[7],
                                Homepage = data[8],
                                SNS = data[9],
                                RcmmnCnt = data[10],
                                RcmmnCont = data[11],
                                BusinessCat = data[12],
                                Address = data[13],
                                Technology = data[14],
                                NewsCompany = data[15],
                                NewsDate = data[16],
                                NewsTitle = data[17],
                                NewsLink = data[18],
                            };

                            comIdToIdx.Add(info.ID, comInfos.Count);

                            string keyword = System.IO.Path.Combine(dir, string.Format("{0}_lmt.txt.txt", info.ID));

                            if (System.IO.File.Exists(keyword))
                            {
                                using (System.IO.StreamReader srSub = System.IO.File.OpenText(keyword))
                                {
                                    while(true)
                                    {
                                        string lineSub = srSub.ReadLine();
                                        if (lineSub == null)
                                            break;

                                        lineSub = lineSub.Replace("\'", "");
                                        lineSub = lineSub.Replace("\"", "");
                                        lineSub = lineSub.Replace(",", "");
                                        lineSub = lineSub.Replace(" ", "");

                                        info.Keyword.Add(lineSub);
                                    }
                                }
                            }

                            comInfos.Add(info);
                            inContent = false;
                            sub = null;
                            data.Clear();
                        }
                                           }

                }
            }

            if (!string.IsNullOrEmpty(serviceFile))
            {
                using (System.IO.StreamReader sr = System.IO.File.OpenText(serviceFile))
                {
                    string line = sr.ReadLine();

                    bool inContent = false;
                    string sub = null;
                    List<string> data = new List<string>();

                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null)
                            break;

                        for (int i = 0; i < line.Length; ++i)
                        {
                            if (!inContent && line[i] == ',')
                            {
                                data.Add(sub);
                                sub = null;
                            }
                            else if (line[i] == '\"')
                            {
                                inContent = !inContent;
                                sub += line[i];
                            }
                            else
                            {
                                sub += line[i];
                            }
                        }

                        if (data.Count == 9)
                        {
                            data.Add(sub);

                            ServiceInfoT info = new ServiceInfoT
                            {
                                SerivceID = int.Parse(data[0]),
                                ID = int.Parse(data[1]),
                                KorName = data[2],
                                EngName = data[3],
                                ShortIntro = data[4],
                                FullIntro = data[5],
                                Tag = data[6],
                                Homepage = data[7],
                                AppStore = data[8],
                                PlayStore = data[9],
                            };

                            svcInfos.Add(info);
                            inContent = false;
                            sub = null;
                            data.Clear();

                            
                            if (comIdToIdx.ContainsKey(info.ID))
                            {
                                comInfos[comIdToIdx[info.ID]].Services.Add(info);
                            }

                            
                                                   
                        }

                    }
                }
            }

            if (!string.IsNullOrEmpty(jobFile))
            {
                using (System.IO.StreamReader sr = System.IO.File.OpenText(jobFile))
                {
                    string line = sr.ReadLine();

                    bool inContent = false;
                    string sub = null;
                    List<string> data = new List<string>();

                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null)
                            break;

                        for (int i = 0; i < line.Length; ++i)
                        {
                            if (!inContent && line[i] == ',')
                            {
                                data.Add(sub);
                                sub = null;
                            }
                            else if (line[i] == '\"')
                            {
                                inContent = !inContent;
                                sub += line[i];
                            }
                            else
                            {
                                sub += line[i];
                            }
                        }

                        if (data.Count == 10)
                        {
                            data.Add(sub);

                            JobInfoT info = new JobInfoT
                            {
                                JobID = int.Parse(data[0]),
                                ID = int.Parse(data[1]),
                                Role = data[2],
                                Career = data[3],
                                ContractType = data[4],
                                MinSalary = data[5] == null ? -1 : int.Parse(data[5]),
                                MaxSalary = data[6] == null ? -1 : int.Parse(data[6]),
                                OptnMin = data[7] == null ? -1 : double.Parse(data[7]),
                                OptnMax = data[8] == null ? -1 : double.Parse(data[8]),
                                Technique = data[9],
                                Language = data[10],
                            };

                            if (info.Technique != null)
                            {
                                info.Technique = info.Technique.Replace("\"", "");
                                info.Technique = info.Technique.Replace("#", ",");
                            }

                            processingRawData(info);

                            if (info.MinSalary >= 10000000)
                                info.MinSalary /= 10000;

                            if (info.MaxSalary >= 10000000)
                                info.MaxSalary /= 10000;

                            if (info.MinSalary > 30000)
                                info.MinSalary = -1;

                            if (info.MaxSalary > 30000)
                                info.MaxSalary = -1;

                           // if (info.Technique != null)
                           // {
                           //     info.Technique = info.Technique.Replace("\"", "");
                           //     info.Technique = info.Technique.Replace("#", ",");
                           //
                           //     string[] tfields = info.Technique.Split(',');
                           //     foreach (var techname in tfields)
                           //     {
                           //         info.Techs.Add(techname);
                           //     }
                           // }

                            if (comIdToIdx.ContainsKey(info.ID) && info.Role.Equals("SW 개발"))
                            {
                                comInfos[comIdToIdx[info.ID]].Jobs.Add(info);
                                info.KorName = comInfos[comIdToIdx[info.ID]].KorName;
                            }

                            if (info.Role.Equals("SW 개발"))
                                jobInfos.Add(info);

                            inContent = false;
                            sub = null;
                            data.Clear();


                        }
                    }
                }
            }

            if(!string.IsNullOrEmpty(ConfigurationManager.AppSettings["techNameS"]))
            {
                typeEncode.Add(TypeM.BackEnd, new List<int>());
                typeEncode.Add(TypeM.FrontEnd, new List<int>());
                typeEncode.Add(TypeM.FullStack, new List<int>());
                typeEncode.Add(TypeM.Mobile, new List<int>());
                typeEncode.Add(TypeM.DeskTop, new List<int>());
                typeEncode.Add(TypeM.Algorithm, new List<int>());
                typeEncode.Add(TypeM.Enterprise, new List<int>());
                typeEncode.Add(TypeM.Firmware, new List<int>());
                typeEncode.Add(TypeM.Infra, new List<int>());

                using(System.IO.StreamReader sr = System.IO.File.OpenText(ConfigurationManager.AppSettings["techNameS"]))
                {
                    while(true)
                    {
                        string line = sr.ReadLine();
                        if (line == null)
                            break;

                        string[] fields = line.Split(',');
                        typeEncode[TypeM.BackEnd].Add(int.Parse(fields[1]));
                        typeEncode[TypeM.FrontEnd].Add(int.Parse(fields[2]));
                        typeEncode[TypeM.FullStack].Add(int.Parse(fields[3]));
                        typeEncode[TypeM.Mobile].Add(int.Parse(fields[4]));
                        typeEncode[TypeM.DeskTop].Add(int.Parse(fields[5]));
                        typeEncode[TypeM.Algorithm].Add(int.Parse(fields[6]));
                        typeEncode[TypeM.Enterprise].Add(int.Parse(fields[7]));
                        typeEncode[TypeM.Firmware].Add(int.Parse(fields[8]));
                        typeEncode[TypeM.Infra].Add(int.Parse(fields[9]));
                    }
                }

            }

            foreach (var item in comInfos)
            {
                if (item.Jobs.Count == 0)
                    continue;

                double min = 0, max = 0, minCnt = 0, maxCnt = 0;
                foreach(var job in item.Jobs)
                {
                    min += job.MinSalary != -1 ? job.MinSalary : 0;
                    max += job.MaxSalary != -1 ? job.MaxSalary : 0;
                    minCnt += job.MinSalary != -1 ? 1 : 0;
                    maxCnt += job.MaxSalary != -1 ? 1 : 0;
                }

                min = minCnt != 0 ? min / minCnt : 0;
                max = maxCnt != 0 ? max / maxCnt : 0;

                item.Min = (int)min;
                item.Max = (int)max;

                if(item.Jobs.Count > 0)
                {
                    comInfosSW.Add(item);
                }
            }

            string csvPath = System.IO.Path.Combine(ConfigurationManager.AppSettings["workingDir"], "jobOnehot.csv"); //csv 파일 경로
            string columns = "JobID," + String.Join(",", techIndexInfos.Keys.Select(n => n).ToArray()); //csv 파일 컬럼 값들(id + 스텍 대표이름들..)

            if (System.IO.File.Exists(csvPath))
                System.IO.File.Delete(csvPath);
      
            //item.JobID, item.OneHot[0], item.OneHot[1].......... item.OneHot[item.OneHot.Count()-1] 형식으로 csv 파일에 데이터 입력!
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(csvPath, false, System.Text.Encoding.GetEncoding("utf-8")))
            {
                file.WriteLine(columns);
                foreach (var item in jobInfos)
                {
                    string rows = item.JobID.ToString() + "," + String.Join(",", item.OneHot.Select(p => p.ToString()).ToArray());
                    file.WriteLine(rows);
                }
            }  

        }

        private void processingRawData(JobInfoT info)
        {
            if (info.Technique == null)
                return;

            info.OneHot = Enumerable.Repeat(0, techIndexInfos.Count).ToArray();

            StringBuilder result = new StringBuilder();
            result.Append(info.Technique + "\r\n");

            string[] words = Regex.Replace(info.Technique, @"\s", "").Split(',');
            SortedSet<string> keys = new SortedSet<string>();

            for (int i = 0; i < words.Length; i ++)
            {
                
                Console.WriteLine("word = " + words[i]);
                if(techRepName.ContainsKey(words[i]))
                {
                    string repName = techRepName[words[i]];
                    if(!string.IsNullOrEmpty(repName))
                    {
                        if (!keys.Contains(repName))
                        {
                            keys.Add(repName);
                            info.Techs.Add(repName);
                        }

                        int index = techIndexInfos[repName];
                        info.OneHot[index] = 1;
                    }
                }
            }

            info.TechniqueSum = String.Join("/", info.Techs.Select(p => p).ToArray());


        }

    }
}
