using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<CompanyInfoT> ComInfos { get => comInfos; }

        public void LoadCSV(string companyFile, string serviceFile, string jobFile)
        {
            if (!string.IsNullOrEmpty(companyFile))
            {
                using (System.IO.StreamReader sr = System.IO.File.OpenText(companyFile))
                {
                    string line = sr.ReadLine();

                    string fullLine = null;
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
                            data.Add(sub);

                        if (data.Count == 19)
                        {
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

                            comInfos.Add(info);
                            inContent = false;
                            sub = null;
                            data.Clear();

                        }

                    }

                }
            }
        }


    }
}
