using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private ObservableCollection<CompanyInfoT> comInfos = new ObservableCollection<CompanyInfoT>();
        private ObservableCollection<ServiceInfoT> svcInfos = new ObservableCollection<ServiceInfoT>();
        private ObservableCollection<JobInfoT> jobInfos = new ObservableCollection<JobInfoT>();

        public ObservableCollection<CompanyInfoT> ComInfos { get => comInfos; set => comInfos = value; }
        public ObservableCollection<ServiceInfoT> SvcInfos { get => svcInfos; }
        public ObservableCollection<JobInfoT> JobInfos { get => jobInfos; }

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

                            if (info.MinSalary >= 10000000)
                                info.MinSalary /= 10000;

                            if (info.MaxSalary >= 10000000)
                                info.MaxSalary /= 10000;

                            if (info.MinSalary > 30000)
                                info.MinSalary = -1;

                            if (info.MaxSalary > 30000)
                                info.MaxSalary = -1;

                            if (info.Technique != null)
                            {
                                info.Technique = info.Technique.Replace("\"", "");
                                info.Technique = info.Technique.Replace("#", ",");

                                string[] tfields = info.Technique.Split(',');
                                foreach (var techname in tfields)
                                {
                                    info.Techs.Add(techname);
                                }
                            }

                            if (comIdToIdx.ContainsKey(info.ID))
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
            }
            
        }

        //TODO Cherry
        private void ArrangeJobTech()
        {
            //1. Fill below dictionary key = Raw text, value = refined text
            Dictionary<string, string> rawToRefine = new Dictionary<string, string>()
            {
                { "C++", "C/C++" },
                {"C++ / QT Creator", "C/C++" },
                {"Visual C++ MFC", "C/C++" }
            };

            //2. Fill below dictionary key = refined text, value = index
            Dictionary<string, int> techToIndex = new Dictionary<string, int>();

            //3. Convert tech name in Job information to refined name
            foreach (var job in jobInfos)
            {

                
            }

            //4. Make one hot encoding information each job info
            foreach (var job in jobInfos)
            {
                job.OneHot = new int[techToIndex.Count];

            }


            //5. Make CSV File
        }

    }
}
