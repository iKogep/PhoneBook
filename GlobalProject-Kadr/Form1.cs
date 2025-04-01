#region Пространства имен
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO; //Работа с файловой системой. Папки, файлы.
/*  
    Чтобы пространство имен стало доступно,
    надо добавить ссылку на файл сборки, в котором
    оно определено. Для этого надо в меню Project
    выбрать команду Add Reference и указать файл сборки.
*/
using System.Data.SqlServerCe; //Проект-Добавить ссылку-.NET-System.Data.SqlServerCe
using System.Collections; //Работа с коллекциями, в том числе ListView
#endregion


namespace GlobalProject_Kadr
{

    public partial class MainForm : Form
    {

        #region Переменные для работы с потомками основного окна!!!
        string FPath = ""; //Переменная для хранения целевого каталога. В нем будут находиться файлы баз данных.
        internal string GlobalCommand = "SELECT * FROM people ORDER BY"; //Переменная для хранения команды SQL-запроса на вывод списка абонентов. С доступом из других форм-потомков.
        bool sort_order = false; //Если истина, то сортировка по возрасанию, если ложь - по убыванию. Абонент.
        int sort_column = 0; //Номер столбца по которому осуществляется сортировка. Абонент.
        bool sort_order_tlf = false; //Если истина, то сортировка по возрасанию, если ложь - по убыванию. Телефон.
        int sort_column_tlf = 0; //Номер столбца по которому осуществляется сортировка. Телефон.
        internal int current_position = 0; //Текущая позиция в листбоксе.
        //internal string current_element = ""; //Текущий элемент в листбоксе.
        ListViewItem current_element = new ListViewItem();
        #endregion

        #region !!! Основная процедура формы MainForm !!!
        public MainForm()
        {
            InitializeComponent();
            dateTimePicker1.Value = Convert.ToDateTime("01.01.1900"); //Полю дата рождения абонента присваиваем стартовое значение.
            dateTimePicker2.Value = DateTime.Today; //Полю дата актуализации номера телефона присваиваем текущий день.
            listView1.Enabled = listView2.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = button5.Enabled = button6.Enabled = button7.Enabled = button8.Enabled = button9.Enabled = button10.Enabled = button11.Enabled = false; //Блокируем все поля для ввода даных, пока пользователь не подключится к БД.
        }
        #endregion

        #region Сканирование файла настроек. Этот блок пока не используется. Данный проект не сразу стал телефонным справочником.
        public void ScanIniFile()
        {

            #region Распознаем свое положение
            string MPath = ""; //Полное имя папки программы для чтения ini файла.
            string MName = ""; //Имя файла текущей программы. Без расширения. Потом из него будет получен ini-файл.
            MPath = Directory.GetParent(Environment.GetCommandLineArgs()[0]).ToString(); //Получаем полный путь, откуда запущена текущая программа.
            for (int i = MPath.Length + 1; i <= Environment.GetCommandLineArgs()[0].Length - 1 - 4; i++) //От длины пути папки +1 (учитываем "\"), до полного имени файла -1 (счет с нуля) или -4 (без расширения exe).
                MName = MName + Environment.GetCommandLineArgs()[0][i]; //Получаем имя файла исполняемой программы без расширения.
            #endregion

            #region Открываем файл настроек
            if (File.Exists(MPath + "\\" + MName + ".ini")) //Проверяем открывается ли файл настроек.
            {
                string[] tempList = File.ReadAllLines(MPath + "\\" + MName + ".ini"); //Считываем файл построчно в строковый массив.
                bool key = true; //Ключ, отслеживает, содержит ли файл настроек целевую папку.
                foreach (string str in tempList) //Перечисляем весь строковый массив.
                {
                    if (str.Length > 9) //Если строка меньше 10 символов, то она просто не может содержать путь.
                        if ((str[0].ToString() + str[1].ToString() + str[2].ToString() + str[3].ToString() + str[4].ToString() + str[5].ToString() + str[6].ToString()) == "folder=") //Проверяем, соответствует ли начало строки нашему ключу.
                        {
                            FPath = ""; //Очищаем строку папки.
                            for (int i = 7; i < str.Length; i++)
                                FPath = FPath + str[i].ToString(); //Посимвольно считываем значение параметра.
                            key = false; //Перекючаем ключ.
                            Console.WriteLine("Параметр найден. Целевой каталог получен.");
                        }
                }
                if (key) //Проверяем был ли переключен ключ.
                {
                    MessageBox.Show("Ошибка! (x02)\r\nФайл настроек не содержит параметра:\r\nfolder=D:\\temp"); //Если ключ не переключился выводим сообщение об ошибке.
                    toolStripStatusLabel1.Text = "Файл настроек поврежден";
                    //CloseMainForm(); //Прерываем приложение.
                }
            }
            else //Если не открывается файл настроек (нет файла или недостаточно прав) выводим сообщение об ошибке.
            {
                MessageBox.Show("Ошибка! (x01)\r\nНе удалось открыть файл настроек:\r\n" + MPath + "\\" + MName + ".ini" + "\r\nФайл настроек должен располагаться в одной папке с программой.\r\nФайл настроек должен содержать параметр вида:\r\nfolder=D:\\temp"); //Сообщение об ошибке.
                toolStripStatusLabel1.Text = "Файл настроек не найден";
                //CloseMainForm(); //Прерываем приложение.
            }
            #endregion

        }
        #endregion

        #region Меню создать БД
        private void создатьБДToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Проверяем существование файла БД
            SqlCeEngine engine;
            engine = new SqlCeEngine("Data Source='contacts.sdf';");
            if (!(File.Exists("contacts.sdf"))) //Если файл БД не существует.
            {
                создатьБДToolStripMenuItem.Enabled = false;
                engine.CreateDatabase(); //Создаем БД, соответственно вместе с файлом. В случае не компакт едишн создалась бы именно БД, без файла.
                SqlCeConnection connection = new SqlCeConnection(engine.LocalConnectionString);
                connection.Open();
                SqlCeCommand command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE people (pid INT IDENTITY(1,1) NOT NULL, nickname NVARCHAR(50) NULL, surname NVARCHAR(50) NULL, name NVARCHAR(50) NULL, patronimic NVARCHAR(50) NULL, birthday TINYINT NULL, birthmonth TINYINT NULL, birthyear SMALLINT NULL, comment NVARCHAR(4000) NULL, PRIMARY KEY (pid))"; //Команда создания таблицы с указанием уникального поля (идентификатора) и первичного ключа, в нашем случае одно и тоже поле. Не думаю, что это могут быть разные поля.
                command.ExecuteScalar(); //Выполнение команды.
                command.CommandText = "CREATE TABLE numbers (nid INT IDENTITY(1,1) NOT NULL, peopleid INT NOT NULL, number NVARCHAR(10) NOT NULL, actual DATETIME NULL, comment NVARCHAR(4000) NULL, PRIMARY KEY (nid))";
                command.ExecuteScalar();
                command.CommandText = "ALTER TABLE numbers ADD CONSTRAINT FK_numbers_people FOREIGN KEY (peopleid) REFERENCES people (pid)"; //Команда, устанавливающая ограничение по связи для созданных таблиц.
                command.ExecuteScalar();
                connection.Close();
                MessageBox.Show("Процедура создания БД завершена.\n\rДля начала работы необходимо подключиться к БД.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Файл базы данных уже существует.\n\rДля начала работы необходимо подключиться к БД.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Меню подключения к БД
        private void подключитьсяКБДToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Блок проверки существования и содержания файла настроек
            toolStripStatusLabel1.Text = "Открытие файла настроек";
            ScanIniFile(); //Ищем и считываем файл настроек.
            if (FPath.Length == 0) //Проверям, дал ли результат поиск и анализ файла настроек.
            {
                toolStripStatusLabel1.Text = "Завершение работы";
                CloseMainForm(); //Если целевая строка нулевой длины, то либо нет файла настроек, либо он не содержит нужный параметр.
            }
            */
            подключитьсяКБДToolStripMenuItem.Enabled = false; //Отключить пункт меню подключения к БД.
            создатьБДToolStripMenuItem.Enabled = false; //Отключить пункт меню создания БД.
            toolStripStatusLabel1.Text = "Поиск БД";
            //Проверяем существование файла БД
            SqlCeEngine engine;
            engine = new SqlCeEngine("Data Source='contacts.sdf';");
            if (!(File.Exists("contacts.sdf")))
            {
                toolStripStatusLabel1.Text = "БД не найдена";
                MessageBox.Show("Файл базы данных не найден.\n\rОбратитесь к администратору.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseMainForm();
            }
            else
            {
                //Загрузка БД
                GlobalCommand = "SELECT * FROM people ORDER BY";
                ShowDB(GlobalCommand);
                listView1.Enabled = listView2.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = button5.Enabled = button6.Enabled = button7.Enabled = button8.Enabled = button9.Enabled = button10.Enabled = button11.Enabled = true;
            }
            //Проверяем существование файла БД
            toolStripStatusLabel1.Text = "Авторизация пользователя";
            //Выводим окно для ввода логина и пароля
            //Получаем логин и пароль
            //Сверяем его с содержимым БД
            toolStripStatusLabel1.Text = "Подключение выполнено";
            //Не нужно проверять логин при каждом подключении (транзакции) это ведь нужно для программы, а не для базы
        }
        #endregion

        #region Меню О программе
        private void словариToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mes = "В текстовых поисковых полях допускается использование:";
            mes += "\n\r% - Любая строка длиной от нуля и более символов.";
            mes += "\n\r_ - Любой одиночный символ.";
            mes += "\n\r\n\rВ режиме \"Поиск по всем реквизитам\" есть две особенности.";
            mes += "\n\r1. Если Вы поставили галочку и не заполнили поисковое поле, будут отобраны записи в которых это поле является пустым.";
            mes += "\n\r2. Если Вы не поставили вообще ни одну галочку и нажали \"Найти\", то будут отобраны все абоненты, у которых есть хотябы один привязанный телефон. Таким образом будут отсеяны абоненты без единого телефона.";
            mes += "";
            mes += "";
            mes += "\n\r\n\rПрограмма написана в учебных целях.";
            mes += "\n\rАвтор не несет никакой ответственности, вообще ни за что!";
            //mes += "\n\r\n\rТестовая копия для моего друга - \n\r                                                             Шабаева Семена Альбертовича ;)";
            //mes += "\n\r\n\r                                        Тестовая копия для моей любимой жены ;)";
            MessageBox.Show(mes, "О программе", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }
        #endregion

        #region Меню выход
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseMainForm();
        }
        #endregion

        #region Отобразить БД
        private void ShowDB(string str) //Получаем строку запроса, точнее ее начальную часть, без указания порядка сортировки.
        {
            SqlCeEngine engine = new SqlCeEngine("Data Source='contacts.sdf';"); //Указываем имя файла БД.
            SqlCeConnection connection = new SqlCeConnection(engine.LocalConnectionString); //Создаем подключение.
            //Вставляем блок формирования порядка сортировки.
            switch (sort_column) //Узнаем по какому столбцу сортировка и добавляем данные об этом в запрос.
            {
                case 0:  // поле pid
                    str += " pid";
                    break;
                case 1:  // поле nickname
                    str += " nickname";
                    break;
                case 2:  // поле surname
                    str += " surname";
                    break;
                case 3:  // поле name
                    str += " name";
                    break;
                case 4:  // поле patronimic
                    str += " patronimic";
                    break;
                case 5:  // поле birthday
                    str += " birthday";
                    break;
                case 6:  // поле birthmonth
                    str += " birthmonth";
                    break;
                case 7:  // поле birthyear
                    str += " birthyear";
                    break;
                case 8:  // поле full birthday
                    if (!sort_order) str += " birthyear DESC, birthmonth DESC, birthday";
                    else str += " birthyear, birthmonth, birthday";
                    break;
                case 9:  // поле comment
                    str += " comment";
                    break;
            };
            if (!sort_order) str += " DESC"; //Если выбран обратный порядок сортировки, добавляем соответствующий параметр.
            toolStripStatusLabel1.Text = "" + str; //Выводим итоговую строку запроса в статус-бар. Украшательство.
            //К этому моменту строка запроса полностью сформирована и готова к выполнению.
            connection.Open(); //Открываем подключение.
            SqlCeCommand command = connection.CreateCommand(); //Создаем команду для открытого подключения.
            command.CommandText = str; //Передаем текст команды.
            SqlCeDataReader dataReader = command.ExecuteReader(); //Выполняем команду.
            string st;  //Будет использоваться для передачи значений полей БД в поля листбокса.
            int itemIndex = 0; //Счетчик для определения номера элемента в листбоксе.
            listView1.Items.Clear(); //Очищаем список листбокса.
            while (dataReader.Read()) //Перечисляем строки с результатами запроса.
            {
                for (int i = 0; i < dataReader.FieldCount; i++) //Перечисляем поля внутри одной строки данных, полученной в результате запроса.
                {
                    st = dataReader.GetValue(i).ToString(); //Каждое поле в текстовую переменную.
                    switch (i) //В зависимости от номера поля значения, добавляем в соответствующее поле листбокса.
                    {
                        case 0:  // поле pid
                            listView1.Items.Add(st);
                            break;
                        case 1:  // поле nickname
                            listView1.Items[itemIndex].SubItems.Add(st);
                            //listView1.Items.Add(st);
                            break;
                        case 2:  // поле surname
                            listView1.Items[itemIndex].SubItems.Add(st);
                            break;
                        case 3:  // поле name
                            listView1.Items[itemIndex].SubItems.Add(st);
                            break;
                        case 4:  // поле patronimic
                            listView1.Items[itemIndex].SubItems.Add(st);
                            break;
                        case 5:  // поле birthday
                            if (st.Length >= 10)
                            {
                                listView1.Items[itemIndex].SubItems.Add(st.Remove(10));
                            }
                            else
                            {
                                listView1.Items[itemIndex].SubItems.Add(st);
                            }
                            break;
                        case 6:  // поле comment
                            listView1.Items[itemIndex].SubItems.Add(st);
                            break;
                    };
                }
                itemIndex++; //Прибевляем значение, для перехода к следующему полю.
            }
            connection.Close(); //Закрываем соединение.
        }
        #endregion

        #region Отобразить ТЛФ
        private void ShowTLF()
        {
            listView2.Items.Clear(); //Очищаем список листбокса2.
            if (!(listView1.SelectedItems == null | listView1.SelectedItems.Count == 0)) //Если НЕ(элемент не выбран и количество выбранных элементов равно нулю). То есть, если в листбоксе1 выбран абонент.
            {
                // Обновляем список привязанных телефонов
                SqlCeEngine engine = new SqlCeEngine("Data Source='contacts.sdf';");
                SqlCeConnection connection = new SqlCeConnection(engine.LocalConnectionString);
                // Вставляем блок сортировки
                string str = "SELECT * FROM numbers WHERE (peopleid LIKE ?) ORDER BY"; //Конструируем запрос сначала в текстовой строке.
                switch (sort_column_tlf) //Устанавливаем поле для сортировки.
                {
                    case 0:  // поле nid
                        str += " nid";
                        break;
                    case 1:  // поле peopleid
                        str += " peopleid";
                        break;
                    case 2:  // поле number
                        str += " number";
                        break;
                    case 3:  // поле actual
                        str += " actual";
                        break;
                    case 4:  // поле comment
                        str += " comment";
                        break;
                };
                if (!sort_order_tlf) str += " DESC"; //Если выбрана обратная сортировка.
                //
                connection.Open(); //Открываем соединение.
                SqlCeCommand command = connection.CreateCommand(); //Создаем экземпляр команды.
                command.CommandText = str; //Задаем команду.
                command.Parameters.Add("peopleid", listView1.SelectedItems[0].Text); //Указываем значение вместо знака вопроса.
                SqlCeDataReader dataReader = command.ExecuteReader(); //Выполняем запрос.
                string st;
                int itemIndex = 0;
                while (dataReader.Read())
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        st = dataReader.GetValue(i).ToString();
                        switch (i)
                        {
                            case 0:  // поле nid
                                listView2.Items.Add(st);
                                break;
                            case 1:  // поле peopleid
                                listView2.Items[itemIndex].SubItems.Add(st);
                                //listView1.Items.Add(st);
                                break;
                            case 2:  // поле number
                                listView2.Items[itemIndex].SubItems.Add(st);
                                break;
                            case 3:  // поле actual
                                listView2.Items[itemIndex].SubItems.Add(st.Remove(10));
                                break;
                            case 4:  // поле comment
                                listView2.Items[itemIndex].SubItems.Add(st);
                                break;
                        };
                    }
                    itemIndex++;
                }
                connection.Close(); //Закрываем соединение.
            }
        }
        #endregion

        #region Завершение работы программы. Использовать только эту процедуру!
        private void CloseMainForm()
        {
            toolStripStatusLabel1.Text = "Завершение работы";
            Close();
        }
        #endregion

        #region Кнопка добавить абонента
        private void button1_Click(object sender, EventArgs e)
        {
            shielding();
            SqlCeConnection conn = new SqlCeConnection("Data Source ='contacts.sdf'");
            conn.Open();
            SqlCeCommand command = conn.CreateCommand();
            if (dateTimePicker1.Value.Date.Year.ToString("0000") + "-" + dateTimePicker1.Value.Date.Month.ToString("00") + "-" + dateTimePicker1.Value.Date.Day.ToString("00") == "1900-01-01")
            {
                command.CommandText = "INSERT INTO people(nickname, surname, name, patronimic, comment) VALUES(?,?,?,?,?)";
                command.Parameters.Add("nickname", textBox1.Text);
                command.Parameters.Add("surname", textBox2.Text);
                command.Parameters.Add("name", textBox3.Text);
                command.Parameters.Add("patronimic", textBox4.Text);
                command.Parameters.Add("comment", textBox5.Text);
                command.ExecuteScalar();
            }
            else
            {
                command.CommandText = "INSERT INTO people(nickname, surname, name, patronimic, birthday, comment) VALUES(?,?,?,?,?,?)";
                command.Parameters.Add("nickname", textBox1.Text);
                command.Parameters.Add("surname", textBox2.Text);
                command.Parameters.Add("name", textBox3.Text);
                command.Parameters.Add("patronimic", textBox4.Text);
                command.Parameters.Add("birthday", dateTimePicker1.Value.Date.Year.ToString("0000") + "-" + dateTimePicker1.Value.Date.Month.ToString("00") + "-" + dateTimePicker1.Value.Date.Day.ToString("00"));
                command.Parameters.Add("comment", textBox5.Text);
                command.ExecuteScalar();
            }
            conn.Close();
            // очистить поля ввода
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            dateTimePicker1.Value = Convert.ToDateTime("01.01.1900");
            textBox5.Clear();
            textBox6.Clear();
            dateTimePicker2.Value = DateTime.Today;
            textBox7.Clear();
            //Обновить поле с БД
            GlobalCommand = "SELECT * FROM people ORDER BY";
            ShowDB(GlobalCommand);
            ShowTLF();
            // установить курсор в поле textBox1
            textBox1.Focus();
            //listView1.FocusedItem.;
        }
        #endregion

        #region Кнопка изменить данные абонента
        private void button2_Click(object sender, EventArgs e)
        {
            shielding();
            if (listView1.SelectedItems.Count != 0)
            {
                SqlCeEngine engine = new SqlCeEngine("Data Source='contacts.sdf';");
                SqlCeConnection connection = new SqlCeConnection(engine.LocalConnectionString);
                connection.Open();
                SqlCeCommand command = connection.CreateCommand();
                if (dateTimePicker1.Checked)
                {
                    command.CommandText = "UPDATE people SET nickname = ?, surname = ?, name = ?, patronimic = ?, birthday = ?, comment = ? WHERE pid = ?";
                    command.Parameters.Add("nickname", textBox1.Text);
                    command.Parameters.Add("surname", textBox2.Text);
                    command.Parameters.Add("name", textBox3.Text);
                    command.Parameters.Add("patronimic", textBox4.Text);
                    command.Parameters.Add("birthday", dateTimePicker1.Value.Date.Year.ToString("0000") + "-" + dateTimePicker1.Value.Date.Month.ToString("00") + "-" + dateTimePicker1.Value.Date.Day.ToString("00"));
                    command.Parameters.Add("comment", textBox5.Text);
                    command.Parameters.Add("pid", listView1.SelectedItems[0].Text);
                }
                else
                {
                    command.CommandText = "UPDATE people SET nickname = ?, surname = ?, name = ?, patronimic = ?, birthday = NULL, comment = ? WHERE pid = ?";
                    command.Parameters.Add("nickname", textBox1.Text);
                    command.Parameters.Add("surname", textBox2.Text);
                    command.Parameters.Add("name", textBox3.Text);
                    command.Parameters.Add("patronimic", textBox4.Text);
                    command.Parameters.Add("comment", textBox5.Text);
                    command.Parameters.Add("pid", listView1.SelectedItems[0].Text);
                }
                command.ExecuteScalar(); // выполнить команду
                connection.Close();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                dateTimePicker1.Value = Convert.ToDateTime("01.01.1900");
                textBox5.Clear();
                textBox6.Clear();
                dateTimePicker2.Value = DateTime.Today;
                textBox7.Clear();
                ShowDB(GlobalCommand);
                listView1.Focus();
                current_position = listView1.Items.IndexOf(listView1.FindItemWithText(current_element.Text));
                listView1.Items[current_position].Selected = true;
                if ((listView1.Items.Count - 1 - current_position) > 0)
                {
                    listView1.EnsureVisible(current_position + 1);
                }
                else
                {
                    listView1.EnsureVisible(current_position);
                }
                ShowTLF();
            }
        }
        #endregion

        #region Кнопка удалить абонента
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems == null | listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Не выбран абонент для удаления.");
            }
            else
            {
                SqlCeEngine engine = new SqlCeEngine("Data Source='contacts.sdf';");
                SqlCeConnection connection = new SqlCeConnection(engine.LocalConnectionString);
                connection.Open();
                SqlCeCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM numbers WHERE (peopleid = ?)";
                command.Parameters.Add("peopleid", listView1.SelectedItems[0].Text);
                command.ExecuteScalar(); // выполнить команду
                command.CommandText = "DELETE FROM people WHERE (pid = ?)";
                command.Parameters.Add("pid", listView1.SelectedItems[0].Text);
                command.ExecuteScalar(); // выполнить команду
                connection.Close();
                //Обновить БД
                ShowDB(GlobalCommand);
                if ((current_position >= 0) & (current_position < listView1.Items.Count))
                {
                    listView1.Focus();
                    listView1.Items[current_position].Selected = true;
                    listView1.EnsureVisible(current_position);
                }
                if ((current_position > 0) & (listView1.Items.Count > 0) & (current_position >= listView1.Items.Count))
                {
                    listView1.Focus();
                    current_position = listView1.Items.Count - 1;
                    listView1.Items[current_position].Selected = true;
                    listView1.EnsureVisible(current_position);
                }
                ShowTLF();
                //Очистить поля
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                dateTimePicker1.Value = Convert.ToDateTime("01.01.1900");
                textBox5.Clear();
                textBox6.Clear();
                dateTimePicker2.Value = DateTime.Today;
                textBox7.Clear();
            }
        }
        #endregion

        #region Кнопка добавить ТЛФ
        private void button5_Click(object sender, EventArgs e)
        {
            shielding();
            if (listView1.SelectedItems.Count != 0)
            {
                SqlCeEngine engine = new SqlCeEngine("Data Source='contacts.sdf';");
                SqlCeConnection connection = new SqlCeConnection(engine.LocalConnectionString);
                connection.Open();
                SqlCeCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO numbers(peopleid, number, actual, comment) VALUES(?,?,?,?)";
                command.Parameters.Add("peopleid", listView1.SelectedItems[0].Text);
                command.Parameters.Add("number", textBox6.Text);
                command.Parameters.Add("actual", dateTimePicker2.Value.Date.Year.ToString("0000") + "-" + dateTimePicker2.Value.Date.Month.ToString("00") + "-" + dateTimePicker2.Value.Date.Day.ToString("00"));
                command.Parameters.Add("comment", textBox7.Text);
                command.ExecuteScalar(); // выполнить команду
                connection.Close();
                ShowTLF();
                textBox6.Clear();
                dateTimePicker2.Value = DateTime.Today;
                textBox7.Clear();
            }
        }
        #endregion

        #region Кнопка изменить данные ТЛФ
        private void button6_Click(object sender, EventArgs e)
        {
            shielding();
            if (listView2.SelectedItems.Count != 0)
            {
                SqlCeEngine engine = new SqlCeEngine("Data Source='contacts.sdf';");
                SqlCeConnection connection = new SqlCeConnection(engine.LocalConnectionString);
                connection.Open();
                SqlCeCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE numbers SET number = ?, actual = ?, comment = ? WHERE nid = ?";
                command.Parameters.Add("number", textBox6.Text);
                command.Parameters.Add("actual", dateTimePicker2.Value.Date.Year.ToString("0000") + "-" + dateTimePicker2.Value.Date.Month.ToString("00") + "-" + dateTimePicker2.Value.Date.Day.ToString("00"));
                command.Parameters.Add("comment", textBox7.Text);
                command.Parameters.Add("nid", listView2.SelectedItems[0].Text);
                command.ExecuteScalar(); // выполнить команду
                connection.Close();
                ShowTLF();
                textBox6.Clear();
                dateTimePicker2.Value = DateTime.Today;
                textBox7.Clear();
            }
        }
        #endregion

        #region Кнопка удалить ТЛФ
        private void button7_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems == null | listView2.SelectedItems.Count == 0)
            {
                MessageBox.Show("Не выбран номер телефона для удаления.");
            }
            else
            {
                SqlCeEngine engine = new SqlCeEngine("Data Source='contacts.sdf';");
                SqlCeConnection connection = new SqlCeConnection(engine.LocalConnectionString);
                connection.Open();
                SqlCeCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM numbers WHERE (nid = ?)";
                command.Parameters.Add("nid", listView2.SelectedItems[0].Text);
                command.ExecuteScalar(); // выполнить команду
                connection.Close();
                //Обновить БД
                ShowTLF();
                //Очистить поля
                textBox6.Clear();
                dateTimePicker2.Value = DateTime.Today;
                textBox7.Clear();
            }
        }
        #endregion

        #region Кнопка очистить поля
        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            dateTimePicker1.Value = Convert.ToDateTime("01.01.1900");
            dateTimePicker1.Checked = false;
            textBox5.Clear();
            textBox6.Clear();
            dateTimePicker2.Value = DateTime.Today;
            textBox7.Clear();
            textBox8.Clear();
        }
        #endregion

        #region Кнопка обновить
        private void button9_Click(object sender, EventArgs e)
        {
            GlobalCommand = "SELECT * FROM people ORDER BY";
            ShowDB(GlobalCommand);
        }
        #endregion

        #region Кнопка поиск
        private void button4_Click(object sender, EventArgs e)
        {
            Form2 FindForm = new Form2();
            FindForm.Owner = this;                        //владелец новой формы - текущая форма
            FindForm.ShowDialog();                        //запускаем новую форму в режиме блокировки основной
            ShowDB(GlobalCommand); //Не знаю, нужна ли здесь эта команда!!! Как-то завязано с обновлением? Отображение не всей, а полной базы?
            ShowTLF();
        }
        #endregion

        #region Кнопка быстрый поиск по номеру
        private void button10_Click(object sender, EventArgs e)
        {
            shielding();
            GlobalCommand = "SELECT DISTINCT people.pid, people.nickname, people.surname, people.name, people.patronimic, people.birthday, people.comment FROM people JOIN numbers ON people.pid = numbers.peopleid AND (numbers.number LIKE '" + textBox8.Text + "') ORDER BY";
            ShowDB(GlobalCommand);
            ShowTLF();
        }
        #endregion

        #region Кнопка быстрый поиск по ПФИО
        private void button11_Click(object sender, EventArgs e)
        {
            shielding();
            GlobalCommand = "SELECT DISTINCT * FROM people WHERE (people.nickname LIKE '" + textBox8.Text + "') OR (people.surname LIKE '" + textBox8.Text + "') OR (people.name LIKE '" + textBox8.Text + "') OR (people.patronimic LIKE '" + textBox8.Text + "') ORDER BY";
            ShowDB(GlobalCommand);
            ShowTLF();
        }
        #endregion

        #region При выборе абонента в списке - его данные заполняют форму, производится выборка привязанных номеров ТЛФ
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // При выборе строки событие ItemSelectionChanged возникает два раза:
            // первый раз, когда выделенная в данный момент строка теряут фокус,
            // второй - когда строка, в которой сделан щелчок, получает фокус.
            // Нас интересует строка, которая получает фокус.
            current_position = e.ItemIndex;
            current_element = e.Item;
            if (e.IsSelected)
            {
                // строка выбрана, т.е. она получила фокус
                //MessageBox.Show(listView1.Items[e.ItemIndex].Text);
                for (int i = 1; i < listView1.Items[e.ItemIndex].SubItems.Count; i++)
                {
                    switch (i)
                    {
                        case 1:
                            textBox1.Text = listView1.Items[e.ItemIndex].SubItems[i].Text;
                            break;
                        case 2:
                            textBox2.Text = listView1.Items[e.ItemIndex].SubItems[i].Text;
                            break;
                        case 3:
                            textBox3.Text = listView1.Items[e.ItemIndex].SubItems[i].Text;
                            break;
                        case 4:
                            textBox4.Text = listView1.Items[e.ItemIndex].SubItems[i].Text;
                            break;
                        case 5:
                            if (listView1.Items[e.ItemIndex].SubItems[i].Text.Length == 10)
                            {
                                dateTimePicker1.Value = Convert.ToDateTime(listView1.Items[e.ItemIndex].SubItems[i].Text);
                                dateTimePicker1.Checked = true;
                            }
                            else
                            {
                                dateTimePicker1.Value = Convert.ToDateTime("01.01.1900");
                                dateTimePicker1.Checked = false;
                            }
                            break;
                        case 6:
                            textBox5.Text = listView1.Items[e.ItemIndex].SubItems[i].Text;
                            break;
                    }
                }
                ShowTLF();
                textBox6.Clear();
                dateTimePicker2.Value = DateTime.Today;
                textBox7.Clear();
            }
        }
        #endregion

        #region При выборе ТЛФ в списке - его данные заполняют форму
        private void listView2_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // При выборе строки событие ItemSelectionChanged возникает два раза:
            // первый раз, когда выделенная в данный момент строка теряут фокус,
            // второй - когда строка, в которой сделан щелчок, получает фокус.
            // Нас интересует строка, которая получает фокус.
            if (e.IsSelected)
            {
                // строка выбрана, т.е. она получила фокус
                //MessageBox.Show(listView1.Items[e.ItemIndex].Text);
                for (int i = 1; i < listView2.Items[e.ItemIndex].SubItems.Count; i++)
                {
                    switch (i)
                    {
                        case 2:
                            textBox6.Text = listView2.Items[e.ItemIndex].SubItems[i].Text;
                            break;
                        case 3:
                            dateTimePicker2.Value = Convert.ToDateTime(listView2.Items[e.ItemIndex].SubItems[i].Text);
                            break;
                        case 4:
                            textBox7.Text = listView2.Items[e.ItemIndex].SubItems[i].Text;
                            break;
                    }
                }
            }
        }
        #endregion

        #region Клик по листбоксу (в том числе по пустому месту)
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowTLF();
        }
        #endregion

        #region Сортировка окна абонентов
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (sort_column == e.Column)
            {
                if (sort_order) { sort_order = false; }
                else { sort_order = true; }
            }
            else
            {
                sort_order = true;
                switch (e.Column)
                {
                    case 0:  // поле pid
                        sort_column = 0;
                        break;
                    case 1:  // поле nickname
                        sort_column = 1;
                        break;
                    case 2:  // поле surname
                        sort_column = 2;
                        break;
                    case 3:  // поле name
                        sort_column = 3;
                        break;
                    case 4:  // поле patronimic
                        sort_column = 4;
                        break;
                    case 5:  // поле birthday
                        sort_column = 5;
                        break;
                    case 6:  // поле birthmonth
                        sort_column = 6;
                        break;
                    case 7:  // поле birthyear
                        sort_column = 7;
                        break;
                    case 8:  // поле full birthday
                        sort_column = 8;
                        break;
                    case 9:  // поле comment
                        sort_column = 9;
                        break;
                }
            }
            ShowDB(GlobalCommand);
            if ((current_position >= 0) & (current_position < listView1.Items.Count))
            {
                listView1.Focus();
                current_position = listView1.Items.IndexOf(listView1.FindItemWithText(current_element.Text));
                listView1.Items[current_position].Selected = true;
                if ((listView1.Items.Count - 1 - current_position) > 0)
                {
                    listView1.EnsureVisible(current_position + 1);
                }
                else
                {
                    listView1.EnsureVisible(current_position);
                }
            }
            ShowTLF();
        }
        #endregion

        #region Сортировка окна ТЛФ
        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (sort_column_tlf == e.Column)
            {
                if (sort_order_tlf) { sort_order_tlf = false; }
                else { sort_order_tlf = true; }
            }
            else
            {
                sort_order_tlf = true;
                switch (e.Column)
                {
                    case 0:  // поле nid
                        sort_column_tlf = 0;
                        break;
                    case 1:  // поле peopleid
                        sort_column_tlf = 1;
                        break;
                    case 2:  // поле number
                        sort_column_tlf = 2;
                        break;
                    case 3:  // поле actual
                        sort_column_tlf = 3;
                        break;
                    case 4:  // поле comment
                        sort_column_tlf = 4;
                        break;
                };
            }
            ShowTLF();
        }
        #endregion

        #region Экранирование всех полей от одиночной кавычки
        private void shielding()
        {
            textBox1.Text = textBox1.Text.Replace("'", " ");
            textBox2.Text = textBox2.Text.Replace("'", " ");
            textBox3.Text = textBox3.Text.Replace("'", " ");
            textBox4.Text = textBox4.Text.Replace("'", " ");
            textBox5.Text = textBox5.Text.Replace("'", " ");
            textBox6.Text = textBox6.Text.Replace("'", " ");
            textBox7.Text = textBox7.Text.Replace("'", " ");
            textBox8.Text = textBox8.Text.Replace("'", "_");

            textBox1.Text = textBox1.Text.Trim();
            textBox2.Text = textBox2.Text.Trim();
            textBox3.Text = textBox3.Text.Trim();
            textBox4.Text = textBox4.Text.Trim();
            textBox5.Text = textBox5.Text.Trim();
            textBox6.Text = textBox6.Text.Trim();
            textBox7.Text = textBox7.Text.Trim();
        }
        #endregion

    }
}
