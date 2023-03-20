using System.Windows.Forms;

namespace Finding_files_and_folders;

public partial class Form1 : Form
{
    private readonly SearchEngine se = new();
    public SynchronizationContext UiContext;

    public Form1()
    {
        InitializeComponent();
        // Название главного окна
        Text = "Поиск файлов и папок";

        // Получим контекст синхронизации для текущего потока 
        UiContext = SynchronizationContext.Current;

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

        // глубина цвета изображений
        image_list1.ColorDepth = ColorDepth.Depth32Bit;
        // установим размер изображения
        image_list1.ImageSize = new Size(16, 16);
        // ассоциируем список маленьких изображений с ListView
        listView1.SmallImageList = image_list1;
    }
    // Список изображений для хранения малых значков
    ImageList image_list1 = new ImageList();

    public string Label1NumberOfFilesFound
    {
        get => label1_number_of_files_found.Text;
        set
        {
            var number = int.Parse(label1_number_of_files_found.Text);
            number++;
            label1_number_of_files_found.Text = number.ToString();
        }
    }

    int _indexIcon = 0;
    private void AddItemToListView()
    {
        Icon icon = Icon.ExtractAssociatedIcon(se._lvi.Tag.ToString());
        image_list1.Images.Add(icon);
        se._lvi.ImageIndex = _indexIcon;
        _indexIcon++;
        UiContext.Send(d => listView1.Items.Add(se._lvi), null);
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
        var thread1 = new Thread(se.Process);
        thread1.Name = "Поиск файлов и папок";
        thread1.IsBackground = true;
        // Сбросить состояние события.
        se.EventForFileFound.Reset();
        // Запускаем поток
        thread1.Start();

        while (true)
            // Если EventForFileFound сигналит, файл найден, увеличиваем счетчик найденных файлов
            if (se.EventForFileFound.WaitOne(0))
            {
                // Увеличиваем счетчик найденных файлов
                Label1NumberOfFilesFound = label1_number_of_files_found.Text;
                // Добавляем файл в ListView1
                AddItemToListView();
                // Сбросить состояние события.
                se.EventForFileFound.Reset();
                // Если событие EventForSearchComplete сигналит, поиск завершен, выходим из цикла и снимаем заблокированный интерфейс
                if (se.EventForSearchComplete.WaitOne(0))
                {
                    button1_find.Enabled = true;
                    button2_stop.Enabled = false;
                    checkBox1_subfolders.Enabled = true;
                    textBox1_file_extension.Enabled = true;
                    textBox2_words_in_file.Enabled = true;
                    comboBox1.Enabled = true;

                    se.EventForSearchComplete.Reset();
                    break;
                }
            }
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

    // Отрабатывает на изменение состояния чекбокса checkBox1_subfolders.
    private void checkBox1_subfolders_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox1_subfolders.Checked)
            se.SearchInSubdirectories = true;
        else
            se.SearchInSubdirectories = false;
    }
}