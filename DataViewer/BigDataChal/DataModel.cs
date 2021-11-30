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

        public int ID { get { return id; } set { id = value; OnChanged("ID"); } }
        public string KorName { get { return korName; } set { korName = value; OnChanged("ID"); } }
        public string EngName { get { return engName; } set { engName = value; OnChanged("ID"); } }
        public string ShortIntro { get { return shortIntro; } set { shortIntro = value; OnChanged("ID"); } }
        public string FullIntro { get { return fullIntro; } set { fullIntro = value; OnChanged("ID"); } }
        public string FoundDate { get { return foundDate; } set { foundDate = value; OnChanged("ID"); } }
        public string EmployeeCnt { get { return employeeCnt; } set { employeeCnt = value; OnChanged("ID"); } }
        public string Invsm { get { return accml_invsm_attrt_amt; } set { accml_invsm_attrt_amt = value; OnChanged("ID"); } }
        public string Homepage { get { return homepage; } set { homepage = value; OnChanged("ID"); } }
        public string SNS { get { return sns; } set { sns = value; OnChanged("ID"); } }
        public string RcmmnCnt { get { return entrp_rcmmn_cnt; } set { entrp_rcmmn_cnt = value; OnChanged("ID"); } }
        public string RcmmnCont { get { return entrp_rcmmn_cont; } set { entrp_rcmmn_cont = value; OnChanged("ID"); } }
        public string BusinessCat { get { return businessCat; } set { businessCat = value; OnChanged("ID"); } }
        public string Address { get { return address; } set { address = value; OnChanged("ID"); } }
        public string Technology { get { return technology; } set { technology = value; OnChanged("ID"); } }
        public string NewsCompany { get { return newsCompany; } set { newsCompany = value; OnChanged("ID"); } }
        public string NewsDate { get { return newsDate; } set { newsDate = value; OnChanged("ID"); } }
        public string NewsTitle { get { return newsTitle; } set { newsTitle = value; OnChanged("ID"); } }
        public string NewsLink { get { return newsLink; } set { newsLink = value; OnChanged("ID"); } }
    }
}
