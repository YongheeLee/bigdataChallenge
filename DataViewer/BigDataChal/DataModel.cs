using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDataChal
{
    public class BaseT : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    public class ItemCountT : BaseT
    {
        private string item = null;
        private int count = -1;

        public string Item { get { return item; } set { item = value; OnChanged("Item"); } }
        public int Count { get { return count; } set { count = value; OnChanged("Count"); } }
    }

        public class ServiceInfoT : BaseT 
    {
        private int svcid = -1;
        private int id = -1;
        private string korName = null;
        private string engName = null;
        private string shortIntro = null;
        private string fullIntro = null;
        private string tag = null;
        private string homeURL = null;
        private string plsturl = null;
        private string ggplurl = null;

        public int SerivceID { get { return svcid; } set { svcid = value; OnChanged("SerivceID"); } }
        public int ID { get { return id; } set { id = value; OnChanged("ID"); } }
        public string KorName { get { return korName; } set { korName = value; OnChanged("KorName"); } }
        public string EngName { get { return engName; } set { engName = value; OnChanged("EngName"); } }
        public string ShortIntro { get { return shortIntro; } set { shortIntro = value; OnChanged("ShortIntro"); } }
        public string FullIntro { get { return fullIntro; } set { fullIntro = value; OnChanged("FullIntro"); } }
        public string Tag { get { return tag; } set { tag = value; OnChanged("Tag"); } }

        public string Homepage { get { return homeURL; } set { homeURL = value; OnChanged("Homepage"); } }
        public string AppStore { get { return plsturl; } set { plsturl = value; OnChanged("AppStore"); } }
        public string PlayStore { get { return ggplurl; } set { ggplurl = value; OnChanged("PlayStore"); } }
    }

    public class JobInfoT : BaseT
    {
        private int jobid = -1;
        private int id = -1;
        private string role = null;
        private string career = null;
        private string type = null;
        private int minsalary = -1;
        private int maxsalary = -1;
        private double stc_optn_mnmm_amt = 0;
        private double stc_optn_mxmm_amt = 0;
        private string tech = null;
        private string lang = null;

        public int JobID { get { return jobid; } set { jobid = value; OnChanged("JobID"); } }
        public int ID { get { return id; } set { id = value; OnChanged("ID"); } }
        public string Role { get { return role; } set { role = value; OnChanged("Role"); } }
        public string Career { get { return career; } set { career = value; OnChanged("Career"); } }
        public string ContractType { get { return type; } set { type = value; OnChanged("ContractType"); } }

        public int MinSalary { get { return minsalary; } set { minsalary = value; OnChanged("MinSalary"); } }
        public int MaxSalary { get { return maxsalary; } set { maxsalary = value; OnChanged("MaxSalary"); } }

        public double OptnMin { get { return stc_optn_mnmm_amt; } set { stc_optn_mnmm_amt = value; OnChanged("OptnMin"); } }
        public double OptnMax { get { return stc_optn_mxmm_amt; } set { stc_optn_mxmm_amt = value; OnChanged("OptnMax"); } }

        public string Technique { get { return tech; } set { tech = value; OnChanged("Technique"); } }
        public string Language { get { return lang; } set { lang = value; OnChanged("Language"); } }

    }

    public class CompanyInfoT : BaseT
    {
        private int id = -1;
        private string korName = null;
        private string engName = null;
        private string shortIntro = null;
        private string fullIntro = null;
        private string foundDate = null;
        private string employeeCnt = null;
        private string accml_invsm_attrt_amt = null;
        private string homepage = null;
        private string sns = null;
        private string entrp_rcmmn_cnt = null;
        private string entrp_rcmmn_cont = null;
        private string businessCat = null;
        private string address = null;
        private string technology = null;
        private string newsCompany = null;
        private string newsDate = null;
        private string newsTitle = null;
        private string newsLink = null;

        private List<ServiceInfoT> services = new List<ServiceInfoT>();
        private List<JobInfoT> jobs = new List<JobInfoT>();
        private List<string> keyword = new List<string>();

        public int ID { get { return id; } set { id = value; OnChanged("ID"); } }
        public string KorName { get { return korName; } set { korName = value; OnChanged("KorName"); } }
        public string EngName { get { return engName; } set { engName = value; OnChanged("EngName"); } }
        public string ShortIntro { get { return shortIntro; } set { shortIntro = value; OnChanged("ShortIntro"); } }
        public string FullIntro { get { return fullIntro; } set { fullIntro = value; OnChanged("FullIntro"); } }
        public string FoundDate { get { return foundDate; } set { foundDate = value; OnChanged("FoundDate"); } }
        public string EmployeeCnt { get { return employeeCnt; } set { employeeCnt = value; OnChanged("EmployeeCnt"); } }
        public string Invsm { get { return accml_invsm_attrt_amt; } set { accml_invsm_attrt_amt = value; OnChanged("Invsm"); } }
        public string Homepage { get { return homepage; } set { homepage = value; OnChanged("Homepage"); } }
        public string SNS { get { return sns; } set { sns = value; OnChanged("SNS"); } }
        public string RcmmnCnt { get { return entrp_rcmmn_cnt; } set { entrp_rcmmn_cnt = value; OnChanged("RcmmnCnt"); } }
        public string RcmmnCont { get { return entrp_rcmmn_cont; } set { entrp_rcmmn_cont = value; OnChanged("RcmmnCont"); } }
        public string BusinessCat { get { return businessCat; } set { businessCat = value; OnChanged("BusinessCat"); } }
        public string Address { get { return address; } set { address = value; OnChanged("Address"); } }
        public string Technology { get { return technology; } set { technology = value; OnChanged("Technology"); } }
        public string NewsCompany { get { return newsCompany; } set { newsCompany = value; OnChanged("NewsCompany"); } }
        public string NewsDate { get { return newsDate; } set { newsDate = value; OnChanged("NewsDate"); } }
        public string NewsTitle { get { return newsTitle; } set { newsTitle = value; OnChanged("NewsTitle"); } }
        public string NewsLink { get { return newsLink; } set { newsLink = value; OnChanged("NewsLink"); } }

        public List<ServiceInfoT> Services { get { return services; } }
        public List<JobInfoT> Jobs { get { return jobs; } }
        public List<string> Keyword { get { return keyword; } }
    }


}
