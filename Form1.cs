using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // инициализируем необходимые для работы массивы
        List<Driver> driver = new List<Driver>();
        List<DriverToCar> driverToCar = new List<DriverToCar>();
        List<Car> car = new List<Car>();
        List<GIBDD> gibdd = new List<GIBDD>();

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateDriver()
        {
            // чистим таблицу "Водители"
            dataGridView1.Rows.Clear();
            // чистим выпадающий список водителей
            comboBox1.Items.Clear();
            // берём всех не удалённых водителей и преобразуем их в список
            foreach (Driver tmp in driver.Where(w => w.deleted == false).Select(w => w).ToList())
            { // идём по списку водителей
                // формируем массив данных водителя для таблицы
                object[] arr = { tmp.ID, tmp.FIO, tmp.Exp, tmp.Licence, tmp.Birthday.ToLongDateString() };
                // Добавляем водителя в таблицу
                dataGridView1.Rows.Add(arr);
                // Добавляем водителя в выпадающий список
                comboBox1.Items.Add(tmp.FIO + " ID: " + tmp.ID.ToString());
            }
        }

        private void UpdateCar()
        {
            // чистим таблицу "Машины"
            dataGridView2.Rows.Clear();
            // чистим выпадающий список машин
            comboBox2.Items.Clear();
            // берём все не удалённые машины и преобразуем их в список
            foreach (Car tmp in car.Where(w => w.deleted == false).Select(w => w).ToList())
            { // идём по списку машин
                // формируем массив данных машины для таблицы
                object[] arr = { tmp.ID, tmp.Brand, tmp.Model, tmp.BodyType, tmp.RegNum };
                // Добавляем машину в таблицу
                dataGridView2.Rows.Add(arr);
                // Добавляем машину в выпадающий список
                comboBox2.Items.Add(tmp.Brand + " " + tmp.Model + " " + tmp.RegNum + " ID: " + tmp.ID.ToString());
            }
        }

        private void UpdateGIBDD()
        {
            // чистим таблицу "ДТП"
            dataGridView4.Rows.Clear();
            // чистим выпадающий список мест из "Дополнительная информация"
            comboBox5.Items.Clear();
            // создаём пустой список для мест
            List<string> places = new List<string>();
            // идём по массиву всех не удалённых ДТП
            foreach (GIBDD tmp in gibdd.Where(w => w.deleted == false).Select(w => w).ToList())
            {
                // создаём переменную для списка участников ДТП
                string multineCars = "";
                // идём по списку машин-участников ДТП
                foreach (int carDriverId in tmp.CarID)
                {
                    // по ID участника получаем запись
                    DriverToCar record = driverToCar.Where(w => w.ID == carDriverId).Select(w => w).First();
                    // по ID машины и водителя получаем машину и водителя соответственно
                    Car tmp_car = car.Where(w => w.ID == record.CarID).Select(w => w).First();
                    Driver tmp_driver = driver.Where(w => w.ID == record.DriverID).Select(w => w).First();
                    // формируем строку
                    multineCars += tmp_driver.FIO + " на " + tmp_car.Brand + " " + tmp_car.Model + " " + tmp_car.RegNum + " ID: " + record.ID + "\n";
                }
                // создаём массив объектов для таблицы
                object[] arr = { tmp.ID, tmp.Name, tmp.DTPNum, tmp.Date.ToLongDateString(), tmp.Place, tmp.DTPType, tmp.DTPReason, tmp.VictimNum, multineCars };
                // добавляем место в отдельный список
                places.Add(tmp.Place);
                // добавляем массив в таблицу
                dataGridView4.Rows.Add(arr);
            }
            // идём по массиву уникальных мест
            foreach (string place in places.Distinct())
            {
                // добавляем их в список мест на вкладку "Дополнительная информация"
                comboBox5.Items.Add(place);
            }
        }

        private void UpdateDriver2Car()
        {
            // чистим таблицу "Машины водителей"
            dataGridView3.Rows.Clear();
            // чистим выпадающий список машин водителей
            comboBox3.Items.Clear();
            // берём все не удалённые записи и преобразуем их в список
            foreach (DriverToCar tmp in driverToCar.Where(w => w.deleted == false).Select(w => w).ToList())
            { // идём по списку записей
                // получаем водителя по ID
                Driver tmp_driver = driver.Where(w => w.ID == tmp.DriverID).Select(w => w).First();
                // получаем машину по ID
                Car tmp_car = car.Where(w => w.ID == tmp.CarID).Select(w => w).First();
                // формируем массив данных записи
                object[] arr = { tmp.ID, tmp_driver.FIO + " ID: " + tmp.DriverID.ToString(), tmp_car.Brand +" "+ tmp_car.Model +" "+ tmp_car.RegNum +" ID: "+ tmp.CarID.ToString() };
                // Добавляем машину в таблицу
                dataGridView3.Rows.Add(arr);
                // Добавляем машину в выпадающий список
                comboBox3.Items.Add(tmp_driver.FIO + " на " + tmp_car.Brand + " " + tmp_car.Model + " " + tmp_car.RegNum + " ID: " + tmp.ID.ToString());
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Объявляем необходимые переменные
            string fname;
            BinaryFormatter bf;
            FileStream fs;
            fname = "driver.dat"; // прописываем путь к файлу
            bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
            fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
            bf.Serialize(fs, driver); // сериализуем данные в файл
            fs.Close(); // закрываем файл

            fname = "car.dat"; // прописываем путь к файлу
            bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
            fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
            bf.Serialize(fs, car); // сериализуем данные в файл
            fs.Close(); // закрываем файл

            fname = "driverToCar.dat"; // прописываем путь к файлу
            bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
            fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
            bf.Serialize(fs, driverToCar); // сериализуем данные в файл
            fs.Close(); // закрываем файл

            fname = "gibdd.dat"; // прописываем путь к файлу
            bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
            fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
            bf.Serialize(fs, gibdd); // сериализуем данные в файл
            fs.Close(); // закрываем файл
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fname;
            BinaryFormatter bf;
            FileStream fs;
            fname = "driver.dat"; // прописываем путь к файлу
            // загружаем данные, только если файл существует
            if (File.Exists(fname))
            {
                bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
                fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
                driver = (List<Driver>)bf.Deserialize(fs); // десереализуем данные из файла
                fs.Close(); // закрываем файл
                if (driver.Count > 0) // если файл не пустой - необходимо восстановить индекс
                {
                    Driver.Index = driver.Last().ID + 1; // берём ID самое последней записи, делаем +1 и записываем как индекс
                }
            }
            UpdateDriver();
            fname = "car.dat"; // прописываем путь к файлу
            // загружаем данные, только если файл существует
            if (File.Exists(fname))
            {
                bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
                fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
                car = (List<Car>)bf.Deserialize(fs); // десереализуем данные из файла
                fs.Close(); // закрываем файл
                if (car.Count > 0) // если файл не пустой - необходимо восстановить индекс
                {
                    Car.Index = car.Last().ID + 1; // берём ID самое последней записи, делаем +1 и записываем как индекс
                }
            }
            UpdateCar();
            fname = "driverToCar.dat"; // прописываем путь к файлу
            // загружаем данные, только если файл существует
            if (File.Exists(fname))
            {
                bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
                fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
                driverToCar = (List<DriverToCar>)bf.Deserialize(fs); // десереализуем данные из файла
                fs.Close(); // закрываем файл
                if (driverToCar.Count > 0) // если файл не пустой - необходимо восстановить индекс
                {
                    DriverToCar.Index = driverToCar.Last().ID + 1; // берём ID самое последней записи, делаем +1 и записываем как индекс
                }
            }
            UpdateDriver2Car();
            fname = "gibdd.dat"; // прописываем путь к файлу
            // загружаем данные, только если файл существует
            if (File.Exists(fname))
            {
                bf = new BinaryFormatter(); // создаём объект класса BinaryFormatter
                fs = new FileStream(fname, FileMode.OpenOrCreate); // открываем файл
                gibdd = (List<GIBDD>)bf.Deserialize(fs); // десереализуем данные из файла
                fs.Close(); // закрываем файл
                if (gibdd.Count > 0) // если файл не пустой - необходимо восстановить индекс
                {
                    GIBDD.Index = gibdd.Last().ID + 1; // берём ID самое последней записи, делаем +1 и записываем как индекс
                }
            }
            UpdateGIBDD();
        }

        // функиця добавления водителя
        private void button1_Click(object sender, EventArgs e)
        {
            // берём данные из полей в форме
            string fio = textBox1.Text;
            int exp = (int)numericUpDown1.Value;
            string license = textBox2.Text;
            DateTime birthday = dateTimePicker1.Value;

            // создаём сущность водителя
            Driver tmp = new Driver(fio, exp, license, birthday);
            // добавляем водителя в массив
            driver.Add(tmp);
            // обновляем таблицу водителей
            UpdateDriver();
        }

        // функция выбора водителя по клику мышкой в таблице
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // если индекс больше-равен нуля
            if (e.RowIndex >= 0)
            {
                // получаем из ячейки ID водителя
                var driverId = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                // записываем ID в label 
                label13.Text = driverId;

                // получаем водителя по ID
                Driver tmp = driver.Where(w => w.ID == Int32.Parse(driverId)).Select(w => w).First();

                // Записываем данные водителя в поля
                textBox4.Text = tmp.FIO;
                numericUpDown2.Value = tmp.Exp;
                textBox3.Text = tmp.Licence;
                dateTimePicker2.Value = tmp.Birthday;
            }
        }

        // функция редактирования водителя
        private void button2_Click(object sender, EventArgs e)
        {
            // получаем сущность водителя по ID
            Driver tmp = driver.Where(w => w.ID == Int32.Parse(label13.Text)).Select(w => w).First();
            // редактируем водителя данными из полей
            tmp.FIO = textBox4.Text;
            tmp.Exp = (int)numericUpDown2.Value;
            tmp.Licence = textBox3.Text;
            tmp.Birthday = dateTimePicker2.Value;
            // обновляем таблицу водителей
            UpdateDriver();
        }

        // функция удаления водителя
        private void button3_Click(object sender, EventArgs e)
        {
            // помечаем водителя удалённым
            driver.Where(w => w.ID == Int32.Parse(label13.Text)).Select(w => w).First().deleted = true;
            // обновляем таблицу водителей
            UpdateDriver();
        }

        // функция создания машины
        private void button6_Click(object sender, EventArgs e)
        {
            // получаем данные машины из полей
            string brand = textBox8.Text;
            string model = textBox12.Text;
            string bodytype = textBox7.Text;
            string regnum = textBox11.Text;

            // создаём сущность машины
            Car tmp = new Car(brand, model, bodytype, regnum);
            // добавляем машину в массив
            car.Add(tmp);
            // обновляем таблицу машин
            UpdateCar();
        }

        // ффункция редактировангия машины
        private void button5_Click(object sender, EventArgs e)
        {
            // получаем сущность машины по ID
            Car tmp = car.Where(w => w.ID == Int32.Parse(label14.Text)).Select(w => w).First();
            // редактируем машину данными из полей
            tmp.Brand = textBox6.Text;
            tmp.Model = textBox10.Text;
            tmp.BodyType = textBox5.Text;
            tmp.RegNum = textBox9.Text;
            // обновляем таблицу машин
            UpdateCar();
        }

        // функция выбора автомобиля по клику мышкой в таблице
        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // проверяем, что индекс больше-равен нуля
            if (e.RowIndex >= 0)
            {
                // получаем ID из ячейки
                var carId = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                // записываем ID машины в Label
                label14.Text = carId;

                // по ID машины получаем экземпляр
                Car tmp = car.Where(w => w.ID == Int32.Parse(carId)).Select(w => w).First();

                // Заполняем поля для редактирования текущими данными машины
                textBox6.Text = tmp.Brand;
                textBox10.Text = tmp.Model;
                textBox5.Text = tmp.BodyType;
                textBox9.Text = tmp.RegNum;
            }
        }

        // функция удаления машины
        private void button4_Click(object sender, EventArgs e)
        {
            // помечаем автомобиль удалённым
            car.Where(w => w.ID == Int32.Parse(label14.Text)).Select(w => w).First().deleted = true;
            // обновляем таблицу машин
            UpdateCar();
        }

        // фукнкция связки водителя и автомобиля
        private void button7_Click(object sender, EventArgs e)
        {
            // полчаем ID водителя и автомобиля из полей comboBox
            int driverId = Int32.Parse(comboBox1.Text.Split(' ').Last());
            int carId = Int32.Parse(comboBox2.Text.Split(' ').Last());

            // создаём связь водителя и автомобиля
            DriverToCar tmp = new DriverToCar(driverId, carId);
            // Добавляем запись в массив
            driverToCar.Add(tmp);
            // обновляем таблицу "Машины водителей"
            UpdateDriver2Car();
        }

        // функция выбора записи по клику мышкой в таблицу
        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // проверям, что индекс не отрицательное число
            if (e.RowIndex >= 0)
            {
                // получаем индекс записи из ячейки таблицы
                var rId = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
                // записываем ID Записи в label
                label30.Text = rId;
            }
            
        }

        // функция удаления записи Водитель-Машины
        private void button8_Click(object sender, EventArgs e)
        {
            // почеаем запись удалённой
            driverToCar.Where(w => w.ID == Int32.Parse(label30.Text)).Select(w => w).First().deleted = true;
            // обновляем таблицу записей
            UpdateDriver2Car();
        }

        // функция создания ДТП
        private void button11_Click(object sender, EventArgs e)
        {
            // получаем данные о ДТП из полей
            string name = textBox20.Text;
            string actNum = textBox16.Text;
            DateTime dtpDate = dateTimePicker3.Value;
            string place = textBox15.Text;
            string dtptype = textBox22.Text;
            string dtpreason = textBox21.Text;
            int victimnum = (int)numericUpDown3.Value;

            // создаём объект ДТП с заданными данными
            GIBDD tmp = new GIBDD(name, actNum, dtpDate, place, dtptype, dtpreason, victimnum);
            // Добавляем ДТП в массив
            gibdd.Add(tmp);
            // обновляем табдицу ДТП
            UpdateGIBDD();
        }

        // функция выбора ДТП из таблицы по клику мышки
        private void dataGridView4_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // очищаем список участников выбранного ДТП
            comboBox4.Items.Clear();
            // проверяем что индекс строки больше-равен нуля
            if (e.RowIndex >= 0)
            {
                // получаем ID ДТП из ячейки таблицы
                var dtpId = dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString();
                // записываем ID в label
                label37.Text = dtpId;

                // получаем экземпляр ДТП по ID
                GIBDD tmp = gibdd.Where(w => w.ID == Int32.Parse(label37.Text)).Select(w => w).First();

                // заполняем поля для редактирования ДТП полученными данными
                textBox19.Text = tmp.Name;
                textBox18.Text = tmp.DTPNum;
                dateTimePicker4.Value = tmp.Date;
                textBox17.Text = tmp.Place;
                textBox14.Text = tmp.DTPType;
                textBox13.Text = tmp.DTPReason;
                numericUpDown4.Value = tmp.VictimNum;

                // формируем список участников ДТП
                // идём по массиву связей водитель-автомобиль
                foreach (int carDriverId in tmp.CarID)
                {
                    // по ID записи получаем запись
                    DriverToCar record = driverToCar.Where(w => w.ID == carDriverId).Select(w => w).First();
                    // по данным из записи получаем Машину и Водителя
                    Car tmp_car = car.Where(w => w.ID == record.CarID).Select(w => w).First();
                    Driver tmp_driver = driver.Where(w => w.ID == record.DriverID).Select(w => w).First();
                    // формируем и добавляем в выпадающий список участника в читаемом формате
                    comboBox4.Items.Add(tmp_driver.FIO + " на " + tmp_car.Brand + " " + tmp_car.Model + " " + tmp_car.RegNum + " ID: " + record.ID);
                }
            }
        }

        // функция редактирования ДТП
        private void button10_Click(object sender, EventArgs e)
        {
            // получаем экземпляр ДТП по ID
            GIBDD tmp = gibdd.Where(w => w.ID == Int32.Parse(label37.Text)).Select(w => w).First();

            // редактируем ДТП данными из полей
            tmp.Name = textBox19.Text;
            tmp.DTPNum = textBox18.Text;
            tmp.Date = dateTimePicker4.Value;
            tmp.Place = textBox17.Text;
            tmp.DTPType = textBox14.Text;
            tmp.DTPReason = textBox13.Text;
            tmp.VictimNum = (int)numericUpDown4.Value;
            // обновляем таблицу ДТП
            UpdateGIBDD();
        }

        // функция удаления ДТП
        private void button9_Click(object sender, EventArgs e)
        {
            // помечаем ДТП удалённым
            gibdd.Where(w => w.ID == Int32.Parse(label37.Text)).Select(w => w).First().deleted = true;
            // обновляем таблицу ДТП
            UpdateGIBDD();
        }

        // функция добавления участника в ДТП
        private void button12_Click(object sender, EventArgs e)
        {
            // получаем ID выбранной записи "Машины водителей"
            int carDriverId = Int32.Parse(comboBox3.Text.Split(' ').Last());
            // получаем ID ДТП
            int dtpId = Int32.Parse(label37.Text);

            // получаем экземпляр ДТП
            GIBDD tmp = gibdd.Where(w => w.ID == dtpId).Select(w => w).First();
            // Добавляем запись в массив участников
            tmp.CarID.Add(carDriverId);
            // обновляем таблицу ДТП
            UpdateGIBDD();
        }

        // функция удаления участника из ДТП
        private void button13_Click(object sender, EventArgs e)
        {
            // очищаем выпадающий список участников выбранного ДТП
            comboBox4.Items.Clear();
            // получаем ID записи которую надо удалить
            int tmp_carDriverId = Int32.Parse(comboBox4.Text.Split(' ').Last());
            // получаем экземпляр ДТП
            GIBDD tmp = gibdd.Where(w => w.ID == Int32.Parse(label37.Text)).Select(w => w).First();
            // удаляем участника из ДТП
            tmp.CarID.Remove(tmp_carDriverId);
            // обновляем таблицу ДТП
            UpdateGIBDD();
            // идём по массиву участников ДТП
            foreach (int carDriverId in tmp.CarID)
            {
                // получаем данные записи по ID участника
                DriverToCar record = driverToCar.Where(w => w.ID == carDriverId).Select(w => w).First();
                // по данным участника получаем данные машины и водителя
                Car tmp_car = car.Where(w => w.ID == record.CarID).Select(w => w).First();
                Driver tmp_driver = driver.Where(w => w.ID == record.DriverID).Select(w => w).First();
                // формируем строку и добавляем в выпадающий список в удобном формате
                comboBox4.Items.Add(tmp_driver.FIO + " на " + tmp_car.Brand + " " + tmp_car.Model + " " + tmp_car.RegNum + " ID: " + record.ID);
            }
        }

        // список водителей совершивших более 1 ДТП ( >=2 )
        private void button14_Click(object sender, EventArgs e)
        {
            // создаём пустой список ID водителей
            List<int> driverDTP = new List<int>();
            // заполняем его нулями
            for (int i = 0; i <= driver.Count(); i++)
            {
                driverDTP.Add(0);
            }
            // идём по массиву всех ДТП
            foreach (GIBDD dtp in gibdd)
            {
                // в каждом ДТП идём по списку Участников
                foreach (int carDriverId in dtp.CarID)
                {
                    // получаем ID водителя в ДТП
                    int driverId = driverToCar.Where(w => w.ID == carDriverId).Select(w => w).First().DriverID;
                    // Добавляем ему одно участние в ДТП
                    driverDTP[driverId] += 1;
                }
            }

            // создаём пустую строку для текста сообщения
            string driversList = "";
            // идём по списку водителей
            for (int i = 0; i < driverDTP.Count; i++)
            {
                // проверяем, что ДТП > 1
                if (driverDTP[i] >= 2)
                {
                    // получаем все данные водителя по ID
                    Driver dtpDriver = driver.Where(w => w.ID == i).Select(w => w).First();
                    // добавляем водителя в список
                    driversList += $"Водитель {dtpDriver.FIO} - {driverDTP[i]} ДТП\n";
                }
            }

            // формируем данные для отображения
            string caption = "Список водителей с кол-вом ДТП > 1";
            // вызываем небольшой MessageBoxs
            MessageBox.Show(driversList, caption, MessageBoxButtons.OK);
        }

        //Список водителей, участвующих в ДТП в заданном месте
        private void button15_Click(object sender, EventArgs e)
        {
            // получаем выбранное место из выпадающего списка
            string place = comboBox5.Text;
            // создаём пустой список водителей
            List<Driver> driversList = new List<Driver>();
            // создаём пустую строку для будущего сообщения
            string driversString = "";
            // идём по списку всех не удалённых ДТП произошелшиз в заданном месте
            foreach(GIBDD dtp in gibdd.Where(w => w.deleted == false).Where(w => w.Place == place).Select(w => w))
            {
                // в каждом ДТП идём по списку участников
                foreach (int carDriverId in dtp.CarID)
                {
                    // получаем ID водителя
                    int tmp_driverId = driverToCar.Where(w => w.ID == carDriverId).Select(w => w).First().DriverID;
                    // по ID водителя получаем все данные водителя
                    Driver tmp_driver = driver.Where(w => w.ID == tmp_driverId).Select(w => w).First();
                    // добавляем водителя в массив
                    driversList.Add(tmp_driver);
                }
            }

            // идём по массиву уникальных водителей
            foreach (Driver tmp_driver in driversList.Distinct())
            {
                // формируем и создаём многострочную строку
                driversString += $"{tmp_driver.FIO}\n";
            }

            // формируем данные для отображения
            string caption = "Список водителей, участвующих в ДТП в заданном месте";
            // вызываем небольшой MessageBox
            MessageBox.Show(driversString, caption, MessageBoxButtons.OK);
        }

        //Список водителей, участвующих в ДТП на заданную дату
        private void button16_Click(object sender, EventArgs e)
        {
            // получаем дату
            DateTime date = dateTimePicker5.Value;
            // создаём список водителей
            List<Driver> driversList = new List<Driver>();
            // создаём пустую строку для будущего сообщения
            string driversString = "";
            // идём по списку всех не удалённых ДТП случившихся в заданную дату
            foreach (GIBDD dtp in gibdd.Where(w => w.deleted == false).Where(w => w.Date.ToLongDateString() == date.ToLongDateString()).Select(w => w))
            {
                // идём по списку участников ДТП
                foreach (int carDriverId in dtp.CarID)
                {
                    // получаем ID водителя
                    int tmp_driverId = driverToCar.Where(w => w.ID == carDriverId).Select(w => w).First().DriverID;
                    // получаем водителя по ID
                    Driver tmp_driver = driver.Where(w => w.ID == tmp_driverId).Select(w => w).First();
                    // Добавляем водителя в список
                    driversList.Add(tmp_driver);
                }
            }
            
            // идём по списку уникальных водителей
            foreach (Driver tmp_driver in driversList.Distinct())
            {
                // формируем и добавляем строку
                driversString += $"{tmp_driver.FIO}\n";
            }

            // формируем данные для отображения
            string caption = "Список водителей, участвующих в ДТП на заданную дату";
            // вызываем небольшой MessageBox
            MessageBox.Show(driversString, caption, MessageBoxButtons.OK);
        }

        //ДТП с максимальным количеством потерпевших
        private void button17_Click(object sender, EventArgs e)
        {
            // получаем список всех не удалённых ДТП, сортируем их по Убыванию (от большего к меньшему) по полю "Кол-во потерпевших" и берём самый первый элемент
            GIBDD maxVictims = gibdd.Where(w => w.deleted == false).OrderByDescending(w => w.VictimNum).First();

            // формируем данные для отображения
            string caption = "ДТП с максимальным количеством потерпевших";
            string dtpString = $"ID: {maxVictims.ID}, Отдел ГИБДД: {maxVictims.Name}, Кол-во потерпевших: {maxVictims.VictimNum}, Номер акта: {maxVictims.DTPNum}, Дата: {maxVictims.Date.ToLongDateString()}, Место: {maxVictims.Place}";
            // вызываем небольшой MessageBox
            MessageBox.Show(dtpString, caption, MessageBoxButtons.OK);
        }

        //Список водителей, участвующих в ДТП с наездом на пешеходов
        private void button18_Click(object sender, EventArgs e)
        {
            // задаём тип нарушения
            string dtptype = "Наезд на пешехода";
            // создаём пустой массив водителей
            List<Driver> driversList = new List<Driver>();
            // создаём пустую строку для будущего сообщения
            string driversString = "";
            // идём по списку всех не удалённых ДТП, где типо ДТП совпадает с заданным
            foreach (GIBDD dtp in gibdd.Where(w => w.deleted == false).Where(w => w.DTPType == dtptype).Select(w => w))
            {
                // в каждом ДТП идём по списку участников
                foreach (int carDriverId in dtp.CarID)
                {
                    // получаем ID водителя
                    int tmp_driverId = driverToCar.Where(w => w.ID == carDriverId).Select(w => w).First().DriverID;
                    // по ID водителя получаем все данные водителя
                    Driver tmp_driver = driver.Where(w => w.ID == tmp_driverId).Select(w => w).First();
                    // Добавляем в массив
                    driversList.Add(tmp_driver);
                }
            }

            // идём по списку уникальных водителей
            foreach (Driver tmp_driver in driversList.Distinct())
            {
                // формируем и добавляем строку
                driversString += $"{tmp_driver.FIO}\n";
            }

            // формируем данные для отображения
            string caption = "Список водителей, участвующих в ДТП с наездом на пешеходов";
            // вызываем небольшой MessageBox
            MessageBox.Show(driversString, caption, MessageBoxButtons.OK);
        }

        //Причины ДТП в порядке убывания их количества
        private void button19_Click(object sender, EventArgs e)
        {
            // создаём пустой словарь, где ключ - причина ДТП, а значение - их количество
            Dictionary<string, int> reasonCountPairs = new Dictionary<string, int>();
            
            // получаем словарь из неудалённых ДТП, где все записи сгруппированные по причине и посчитаны (key - причина, value - кол-во ДТП)
            reasonCountPairs = gibdd.Where(w => w.deleted == false).GroupBy(w => w.DTPReason).ToDictionary(x => x.Key, x => x.Count());

            // создаём пустую строку для сообщения
            string reasonCountString = "";
            // идём по словарю
            foreach (KeyValuePair<string,int> reason in reasonCountPairs)
            {
                // формируем и добавляем строку
                reasonCountString += $"{reason.Key} - {reason.Value}\n";
            }

            // формируем данные для отображения
            string caption = "Причины ДТП в порядке убывания их количества";
            // вызываем небольшой MessageBoxs
            MessageBox.Show(reasonCountString, caption, MessageBoxButtons.OK);
        }
    }
}
