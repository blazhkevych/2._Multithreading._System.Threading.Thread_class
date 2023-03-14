using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Finding_files_and_folders;

public partial class Form1 : Form
{
    private readonly SearchEngine se = new();
    public SynchronizationContext uiContext;

    public Form1()
    {
        InitializeComponent();
        // Название главного окна
        Text = "Поиск файлов и папок";

        // Получим контекст синхронизации для текущего потока 
        uiContext = SynchronizationContext.Current;

        // Добавляем диски в комбобокс.
        foreach (var drive in se.Drives)
            comboBox1.Items.Add(drive);

        // Устанавливаем первый элемент комбобокса по умолчанию.
        comboBox1.SelectedIndex = 0;

        // Устанавливаем фокус на textBox.
        textBox2_words_in_file.Focus();

        // Установим табличный режим отображения
        listView1.View = View.Details;

        // При выборе элемента списка будет подсвечена вся строка
        listView1.FullRowSelect = true;

        // Установим сортировку элементов в порядке возрастания
        listView1.Sorting = SortOrder.Ascending;

        // Добавим колонки в список
        listView1.Columns.Add("Имя", 200, HorizontalAlignment.Left);
        listView1.Columns.Add("Папка", 150, HorizontalAlignment.Left);
        listView1.Columns.Add("Размер", 100, HorizontalAlignment.Left);
        listView1.Columns.Add("Дата модификации", 150, HorizontalAlignment.Left);

        // Установим первую колонку по умолчанию
        listView1.Columns[0].Width = 150;

        // Установим ширину второй колонки
        listView1.Columns[1].Width = 350;

        // Установим ширину третьей колонки
        listView1.Columns[2].Width = 90;

        // Установим ширину четвертой колонки
        listView1.Columns[3].Width = 150;

        // Установим табличный режим отображения
        listView1.View = View.Details;

        // Список изображений для хранения малых значков
        ImageList image_list1 = new ImageList();


        //se.AFileWasFoundWithTheGivenMask += HandleFileFoundEvent();

    }

    //private void HandleFileFoundEvent(int number)
    //{
    //    if (label1_number_of_files_found.InvokeRequired)
    //    {
    //        label1_number_of_files_found.Invoke(new Action<int>(HandleFileFoundEvent), number);
    //        return;
    //    }

    //    label1_number_of_files_found.Text = number.ToString();
    //}


    private void AddItemToListView(string filePath)
    {
        if (listView1.InvokeRequired)
        {
            listView1.Invoke(new Action<string>(AddItemToListView), filePath);
            return;
        }

        // Добавляем файл в ListView1
        var item = new ListViewItem(filePath);
        listView1.Items.Add(item);
    }

    // Кнопка Найти.
    private void button1_find_Click(object sender, EventArgs e)
    {
        // Отключаем кнопку Найти на время поиска
        button1_find.Enabled = false;
        // Активируем кнопку Остановить
        button2_stop.Enabled = true;
        // Блокируем чекбокс на время поиска
        checkBox1_subfolders.Enabled = false;
        // Блокируем textBox1_file_extension на время поиска
        textBox1_file_extension.Enabled = false;
        // Блокируем textBox2_words_in_file на время поиска
        textBox2_words_in_file.Enabled = false;
        // Блокируем comboBox1 на время поиска
        comboBox1.Enabled = false;

        // Устанавливаем путь поиска
        se.Path = comboBox1.Text;

        // Очищаем список
        listView1.Items.Clear();

        // Создаем фоновый новый поток для поиска
        var thread = new Thread(se.Process);

        // Запускаем поток
        thread.Start();
    }

    // Отрабатывает на изменение текста в textBox1_file_extension.
    private void textBox1_file_extension_TextChanged(object sender, EventArgs e)
    {
        se.Mask = textBox1_file_extension.Text;
    }
    // Отрабатывает на изменение текста в textBox2_words_in_file.
    private void textBox2_words_in_file_TextChanged(object sender, EventArgs e)
    {
        se.Text = textBox2_words_in_file.Text;
    }
}

// Нажали кнопку Найти и начался поиск в отдельном вторичном фоновом потоке !
// Обновление статистики в процессе поиска по кол-ву файлов, в том же потоке. Нашел файл и обновил.