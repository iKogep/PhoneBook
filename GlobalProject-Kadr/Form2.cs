#region Пространства имен
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
#endregion

namespace GlobalProject_Kadr
{
    public partial class Form2 : Form
    {
        #region !!! Основная процедура формы !!!
        public Form2()
        {
            InitializeComponent();
            ClearAll();
        }
        #endregion

        #region Очистка полей и оключение чекбоксов
        private void ClearAll()
        {
            checkBox1.CheckState = CheckState.Unchecked;
            textBox1.Clear();
            textBox1.Enabled = false;
            checkBox2.CheckState = CheckState.Unchecked;
            textBox2.Clear();
            textBox2.Enabled = false;
            checkBox3.CheckState = CheckState.Unchecked;
            textBox3.Clear();
            textBox3.Enabled = false;
            checkBox4.CheckState = CheckState.Unchecked;
            textBox4.Clear();
            textBox4.Enabled = false;
            checkBox5.CheckState = CheckState.Unchecked;
            dateTimePicker1.Value = Convert.ToDateTime("01.01.1900");
            dateTimePicker1.Enabled = false;
            checkBox6.CheckState = CheckState.Unchecked;
            dateTimePicker2.Value = DateTime.Today;
            dateTimePicker2.Enabled = false;
            checkBox7.CheckState = CheckState.Unchecked;
            textBox5.Clear();
            textBox5.Enabled = false;
            checkBox8.CheckState = CheckState.Unchecked;
            textBox6.Clear();
            textBox6.Enabled = false;
            checkBox9.CheckState = CheckState.Unchecked;
            dateTimePicker3.Value = Convert.ToDateTime("01.01.1900");
            dateTimePicker3.Enabled = false;
            checkBox10.CheckState = CheckState.Unchecked;
            dateTimePicker4.Value = DateTime.Today;
            dateTimePicker4.Enabled = false;
            checkBox11.CheckState = CheckState.Unchecked;
            textBox7.Clear();
            textBox7.Enabled = false;
        }
        #endregion

        #region Кнопка очистить
        private void button1_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
        #endregion

        #region Чекбоксы
        private void checkBox1_CheckStateChanged(object sender, EventArgs e) //Происходит при изменении статуса чекбокса.
        {
            if (checkBox1.CheckState == CheckState.Unchecked | checkBox1.CheckState == CheckState.Indeterminate) //Если значение не определено или чекбокс снят.
            {
                numericUpDown1.Value--; //Уменьшаем значение контролной переменной.
                textBox1.Clear(); //Очищаем текстовое поле.
                textBox1.Enabled = false; //Деактивируем текстовое поле.
            }
            else //Иначе. То есть если чекбокс установлен.
            {
                numericUpDown1.Value++; //Увеличиваем значение контрольной переменной.
                textBox1.Enabled = true; //Активируем текстовое поле.
            }
        } //txt

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox2.CheckState == CheckState.Unchecked | checkBox2.CheckState == CheckState.Indeterminate)
            {
                numericUpDown1.Value--;
                textBox2.Clear();
                textBox2.Enabled = false;
            }
            else
            {
                numericUpDown1.Value++;
                textBox2.Enabled = true;
            }
        } //txt

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox3.CheckState == CheckState.Unchecked | checkBox3.CheckState == CheckState.Indeterminate)
            {
                numericUpDown1.Value--;
                textBox3.Clear();
                textBox3.Enabled = false;
            }
            else
            {
                numericUpDown1.Value++;
                textBox3.Enabled = true;
            }
        } //txt

        private void checkBox4_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox4.CheckState == CheckState.Unchecked | checkBox4.CheckState == CheckState.Indeterminate)
            {
                numericUpDown1.Value--;
                textBox4.Clear();
                textBox4.Enabled = false;
            }
            else
            {
                numericUpDown1.Value++;
                textBox4.Enabled = true;
            }
        } //txt

        private void checkBox5_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox5.CheckState == CheckState.Unchecked | checkBox5.CheckState == CheckState.Indeterminate)
            {
                numericUpDown1.Value--;
                dateTimePicker1.Value = Convert.ToDateTime("01.01.1900"); //Выставляем дату по умолчанию.
                dateTimePicker1.Enabled = false; //Деактивируем поле.
                checkBox6.CheckState = CheckState.Unchecked; //Меняем статус чекбокса для связанной даты. Чекбоксы полей с датами работают парами, то есть, если выбрать вторую дату, то автоматом включается первая, а если выключить первую, то автоматом выключится и вторая.
            }
            else
            {
                numericUpDown1.Value++;
                dateTimePicker1.Enabled = true;
            }
        } //date

        private void checkBox6_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox6.CheckState == CheckState.Unchecked | checkBox6.CheckState == CheckState.Indeterminate)
            {
                dateTimePicker2.Value = DateTime.Today;
                dateTimePicker2.Enabled = false;
            }
            else
            {
                dateTimePicker2.Enabled = true;
                checkBox5.CheckState = CheckState.Checked;
            }
        } //date

        private void checkBox7_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox7.CheckState == CheckState.Unchecked | checkBox7.CheckState == CheckState.Indeterminate)
            {
                numericUpDown1.Value--;
                textBox5.Clear();
                textBox5.Enabled = false;
            }
            else
            {
                numericUpDown1.Value++;
                textBox5.Enabled = true;
            }
        } //txt

        private void checkBox8_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox8.CheckState == CheckState.Unchecked | checkBox8.CheckState == CheckState.Indeterminate)
            {
                numericUpDown2.Value--;
                textBox6.Clear();
                textBox6.Enabled = false;
            }
            else
            {
                numericUpDown2.Value++;
                textBox6.Enabled = true;
            }
        } //txt

        private void checkBox9_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox9.CheckState == CheckState.Unchecked | checkBox9.CheckState == CheckState.Indeterminate)
            {
                numericUpDown2.Value--;
                dateTimePicker3.Value = Convert.ToDateTime("01.01.1900");
                dateTimePicker3.Enabled = false;
                checkBox10.CheckState = CheckState.Unchecked;
            }
            else
            {
                numericUpDown2.Value++;
                dateTimePicker3.Enabled = true;
            }
        } //date

        private void checkBox10_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox10.CheckState == CheckState.Unchecked | checkBox10.CheckState == CheckState.Indeterminate)
            {
                dateTimePicker4.Value = DateTime.Today;
                dateTimePicker4.Enabled = false;
            }
            else
            {
                dateTimePicker4.Enabled = true;
                checkBox9.CheckState = CheckState.Checked;
            }
        } //date

        private void checkBox11_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox11.CheckState == CheckState.Unchecked | checkBox11.CheckState == CheckState.Indeterminate)
            {
                numericUpDown2.Value--;
                textBox7.Clear();
                textBox7.Enabled = false;
            }
            else
            {
                numericUpDown2.Value++;
                textBox7.Enabled = true;
            }
        } //txt
        #endregion

        #region Кнопка отмена
        private void button3_Click(object sender, EventArgs e)
        {
            Dispose(); //Закрываем форму так потому, что открывали командой ShowDialog
        }
        #endregion

        #region Кнопка НАЙТИ
        private void button2_Click(object sender, EventArgs e)
        {
            shielding();
            MainForm tempForm = (MainForm)this.Owner; //Создаем экземпляр основной формы, родительский для текущего. Через этот экземпляр можно будет обращаться к расшаренным переменным основного (родительского) окна.
            if (numericUpDown1.Value > 0 & numericUpDown2.Value == 0) //Выбран только абонент. Это мы определили по значениям контрольных переменных (полей).
            {
                tempForm.GlobalCommand = "SELECT DISTINCT * FROM people WHERE"; //Получаем доступ к глобальной переменной - команде запроса. В запросе ставим звездочку потому, что поиск осуществляем только по одной таблице и хотим получить все ее поля. Зато есть заморочки с использованием AND.
                bool andFlag = false; //Флаг, отслеживающий с какого момента необходимо начинать ставить ключевое слово AND.
                if (checkBox1.CheckState == CheckState.Checked) //Если выбран чекбокс.
                {
                    if (andFlag) { tempForm.GlobalCommand += " AND"; } //Если флаг уже включен, то добавляем AND. Хотя именно в этом месте он не может быть включен, но всеже блок имеет общую структуру.
                    andFlag = true; //Если попали в ветку, включаем флаг.
                    tempForm.GlobalCommand += " (people.nickname LIKE '" + textBox1.Text + "')"; //Добавляем условие поиска.
                }
                if (checkBox2.CheckState == CheckState.Checked) 
                {
                    if (andFlag) { tempForm.GlobalCommand += " AND"; }
                    andFlag = true;
                    tempForm.GlobalCommand += " (people.surname LIKE '" + textBox2.Text + "')"; 
                }
                if (checkBox3.CheckState == CheckState.Checked) 
                {
                    if (andFlag) { tempForm.GlobalCommand += " AND"; }
                    andFlag = true;
                    tempForm.GlobalCommand += " (people.name LIKE '" + textBox3.Text + "')"; 
                }
                if (checkBox4.CheckState == CheckState.Checked) 
                {
                    if (andFlag) { tempForm.GlobalCommand += " AND"; }
                    andFlag = true;
                    tempForm.GlobalCommand += " (people.patronimic LIKE '" + textBox4.Text + "')"; 
                }
                if (checkBox5.CheckState == CheckState.Checked & checkBox6.CheckState == CheckState.Checked) 
                {
                    if (andFlag) { tempForm.GlobalCommand += " AND"; }
                    andFlag = true;
                    tempForm.GlobalCommand += " (people.birthday >= '" + dateTimePicker1.Value.Date.Year.ToString("0000") + "-" + dateTimePicker1.Value.Date.Month.ToString("00") + "-" + dateTimePicker1.Value.Date.Day.ToString("00") + "') AND (people.birthday <= '" + dateTimePicker2.Value.Date.Year.ToString("0000") + "-" + dateTimePicker2.Value.Date.Month.ToString("00") + "-" + dateTimePicker2.Value.Date.Day.ToString("00") + "')"; 
                }
                if (checkBox5.CheckState == CheckState.Checked & checkBox6.CheckState != CheckState.Checked) 
                {
                    if (andFlag) { tempForm.GlobalCommand += " AND"; }
                    andFlag = true;
                    tempForm.GlobalCommand += " (people.birthday = '" + dateTimePicker1.Value.Date.Year.ToString("0000") + "-" + dateTimePicker1.Value.Date.Month.ToString("00") + "-" + dateTimePicker1.Value.Date.Day.ToString("00") + "')"; 
                }
                if (checkBox7.CheckState == CheckState.Checked) 
                {
                    if (andFlag) { tempForm.GlobalCommand += " AND"; }
                    andFlag = true;
                    tempForm.GlobalCommand += " (people.comment LIKE '" + textBox5.Text + "')"; 
                }
                tempForm.GlobalCommand += " ORDER BY"; //В конце дописываем команду сортировки, а ключ сортировки будет добавлен непосредственно в процедуре вывода списка абонентов. За счет этого мы добились того, что порядок сортировки не сбрасывается при применении "фильтров".
            }
            else //Все другие варианты. А их два. 1 - Выбран телефон (независимо от того выбран абонент или нет). 2 - Не выбрано вообще ничего, при этом запрос не считается пустым (Будут выведены абоненты, имеющие хотябы один телефон. Абоненты без телефонов будут отсеяны.)!!! Вот такие особенности поиска.
            {
                tempForm.GlobalCommand = "SELECT DISTINCT people.pid, people.nickname, people.surname, people.name, people.patronimic, people.birthday, people.comment FROM people JOIN numbers ON people.pid = numbers.peopleid"; // Здесь ставить звездочку нельзя, иначе получим данные из обеих таблиц, причем сколько у абонента номеров, столько раз он повторится в списке. А выбрав только данные абонента, у нас корректно срабатывает ключ длявывода только уникальных значений. Зато при поиске по двум таблицам нет заморочки с AND.
                if (checkBox1.CheckState == CheckState.Checked) tempForm.GlobalCommand += " AND (people.nickname LIKE '" + textBox1.Text + "')";
                if (checkBox2.CheckState == CheckState.Checked) tempForm.GlobalCommand += " AND (people.surname LIKE '" + textBox2.Text + "')";
                if (checkBox3.CheckState == CheckState.Checked) tempForm.GlobalCommand += " AND (people.name LIKE '" + textBox3.Text + "')";
                if (checkBox4.CheckState == CheckState.Checked) tempForm.GlobalCommand += " AND (people.patronimic LIKE '" + textBox4.Text + "')";
                if (checkBox5.CheckState == CheckState.Checked & checkBox6.CheckState == CheckState.Checked) tempForm.GlobalCommand += " AND (people.birthday >= '" + dateTimePicker1.Value.Date.Year.ToString("0000") + "-" + dateTimePicker1.Value.Date.Month.ToString("00") + "-" + dateTimePicker1.Value.Date.Day.ToString("00") + "') AND (people.birthday <= '" + dateTimePicker2.Value.Date.Year.ToString("0000") + "-" + dateTimePicker2.Value.Date.Month.ToString("00") + "-" + dateTimePicker2.Value.Date.Day.ToString("00") + "')";
                if (checkBox5.CheckState == CheckState.Checked & checkBox6.CheckState != CheckState.Checked) tempForm.GlobalCommand += " AND (people.birthday = '" + dateTimePicker1.Value.Date.Year.ToString("0000") + "-" + dateTimePicker1.Value.Date.Month.ToString("00") + "-" + dateTimePicker1.Value.Date.Day.ToString("00") + "')";
                if (checkBox7.CheckState == CheckState.Checked) tempForm.GlobalCommand += " AND (people.comment LIKE '" + textBox5.Text + "')";
                if (checkBox8.CheckState == CheckState.Checked) tempForm.GlobalCommand += " AND (numbers.number LIKE '" + textBox6.Text + "')";
                if (checkBox9.CheckState == CheckState.Checked & checkBox10.CheckState == CheckState.Checked) tempForm.GlobalCommand += " AND (numbers.actual >= '" + dateTimePicker3.Value.Date.Year.ToString("0000") + "-" + dateTimePicker3.Value.Date.Month.ToString("00") + "-" + dateTimePicker3.Value.Date.Day.ToString("00") + "') AND (numbers.actual <= '" + dateTimePicker4.Value.Date.Year.ToString("0000") + "-" + dateTimePicker4.Value.Date.Month.ToString("00") + "-" + dateTimePicker4.Value.Date.Day.ToString("00") + "')";
                if (checkBox9.CheckState == CheckState.Checked & checkBox10.CheckState != CheckState.Checked) tempForm.GlobalCommand += " AND (numbers.actual = '" + dateTimePicker3.Value.Date.Year.ToString("0000") + "-" + dateTimePicker3.Value.Date.Month.ToString("00") + "-" + dateTimePicker3.Value.Date.Day.ToString("00") + "')";
                if (checkBox11.CheckState == CheckState.Checked) tempForm.GlobalCommand += " AND (numbers.comment LIKE '" + textBox7.Text + "')";
                tempForm.GlobalCommand += " ORDER BY";
            }
            Dispose(); //Закрываем форму так потому, что открывали командой ShowDialog. Если что-то не передаст - меняем на Close.
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
        }
        #endregion
    }
}
