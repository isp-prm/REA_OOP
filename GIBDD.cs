using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WindowsFormsApp1
{
    [Serializable]
    internal class GIBDD: IComparable<GIBDD>
    {
        public int ID;
        public string Name;
        public string DTPNum;
        public List<int> CarID;
        public DateTime Date;
        public string Place;
        public string DTPType;
        public string DTPReason;
        public int VictimNum;
        public bool deleted;
        private static int size = 0;

        public GIBDD(string name, string dtpnum, DateTime date, string place, string dtptype, string dtpreason, int victimNum)
        {
            ID = size;
            Name = name;
            Date = date;
            DTPNum = dtpnum;
            Place = place;
            DTPType = dtptype;
            DTPReason = dtpreason;
            VictimNum = victimNum;
            CarID = new List<int>();
            deleted = false;
            size++;
        }

        public static int Index
        {
            set => size = value;
        }

        public int CompareTo(GIBDD obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }

    }
}
