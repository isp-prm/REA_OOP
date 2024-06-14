using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    [Serializable]
    internal class DriverToCar: IComparable<DriverToCar>
    {
        public int ID;
        public int DriverID;
        public int CarID;
        public bool deleted;
        private static int size = 0;

        public DriverToCar(int driverid, int carid)
        {
            ID = size;
            DriverID = driverid;
            CarID = carid;
            deleted = false;
            size++;
        }

        public static int Index
        {
            set => size = value;
        }

        public int CompareTo(DriverToCar obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }
    }
}
