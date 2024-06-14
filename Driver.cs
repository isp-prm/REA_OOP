using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WindowsFormsApp1
{
    [Serializable]
    internal class Driver: IComparable<Driver>
    {
        public int ID;
        public string FIO;
        public int Exp; // Стаж вождения
        public string Licence;
        public DateTime Birthday;
        private static int size = 0;
        public bool deleted; // Сущестувет - false, удалено - true

        public Driver(string fio, int exp, string license, DateTime birthday)
        {
            ID = size;
            FIO = fio;
            Exp = exp;
            Licence = license;
            Birthday = birthday;
            size++;
            deleted = false;
        }

        public static int Index
        {
            set => size = value;
        }

        public int CompareTo(Driver obj)
        {
            if (this.ID > obj.ID) { return 1; }
            if (this.ID < obj.ID) { return -1; }
            return 0;
        }
    }
}
