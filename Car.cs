using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    [Serializable]
    internal class Car: IComparable<Car>
    {
        public int ID;
        public string Brand;
        public string Model;
        public string BodyType;
        public string RegNum;
        public bool deleted;
        private static int size = 0;

        public Car(string brand, string model, string bodyType, string regNum)
        {
            ID = size;
            Brand = brand;
            Model = model;
            BodyType = bodyType;
            RegNum = regNum;
            deleted = false;
            size++;
        }

        public static int Index
        {
            set => size = value;
        }

        public int CompareTo(Car obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }
    }
}
