using System.Text;
using System.Text.RegularExpressions;

namespace Finding_files_and_folders;

//delegate void MyDelegate();
internal class SearchEngine
{
    // Конструктор по умолчанию.
    public SearchEngine()
    {
        Path = "C:\\";
        Mask = "*.txt";
        Text = "";
        Di = new DirectoryInfo(Path);
        RegMask = null;
        CountOfMatchFiles = 0;
        Drives = GetDrives();
    }

    // Путь к файлу.
    public string Path { get; set; }

    // Маска поиска для файлов.
    public string Mask { get; set; }

    // Текст для поиска текста в файлах. 
    public string Text { get; set; }

    // Создание объекта DirectoryInfo на основе введенного пути.
    private DirectoryInfo Di { get; set; }

    // Объект регулярного выражения на основе маски для поиска файлов.
    private Regex RegMask { get; set; }

    // Объект регулярного выражения на основе текста для поиска текста в файлах.
    private Regex RegText { get; set; }

    // Количество найденных файлов в данный момент.
    private ulong CountOfMatchFiles { get; set; }

    // Диски на компьютере.
    public string[] Drives { get; set; }

    // Получаем диски на компьютере.
    private string[] GetDrives()
    {
        return Drives = Directory.GetLogicalDrives();
    }

    // Метод добавляет \ в случае его отсутствия.
    private string AddSlash(string path)
    {
        if (path[path.Length - 1] != '\\')
            path += '\\';
        return path;
    }

    // Преобразуем введенную маску для файлов в регулярное выражение.
    private void ConvertMaskToRegEx(string mask)
    {
        // Заменяем . на \.
        mask = mask.Replace(".", @"\.");
        // Заменяем ? на .
        mask = mask.Replace("?", ".");
        // Заменяем * на .*
        mask = mask.Replace("*", ".*");
        // Указываем, что требуется найти точное соответствие маске
        mask = "^" + mask + "$";
        Mask = mask;
        // Создание объекта регулярного выражения на основе маски.
        RegMask = new Regex(Mask, RegexOptions.IgnoreCase);


    }

    //private void CreateRegEx()
    //{
    //}

    // Экранируем спецсимволы во введенном тексте.
    private void EscapeSpecialCharacters(string text)
    {
        Text = Regex.Escape(text);
    }

    // Создание объекта регулярного выражения на основе текста.
    private void CreateRegEx(string text)
    {
        RegText = Text.Length == 0 ? null : new Regex(Text, RegexOptions.IgnoreCase);
    }





    // Был найден файл по заданной маске
    //public event Action AFileWasFoundWithTheGivenMask;

    // Метод увеличивает найденное количество файлов на 1
    private void OnAFileWasFoundWithTheGivenMask()
    {
        CountOfMatchFiles++;
    }

    // Метод поиска файлов.
    private  ulong FindTextInFiles(Regex regText, DirectoryInfo di, Regex regMask)
    {
        // Поток для чтения из файла
        StreamReader sr = null;
        // Список найденных совпадений
        MatchCollection mc = null;

        // Количество обработанных файлов
        ulong CountOfMatchFiles = 0;

        FileInfo[] fi = null;
        try
        {
            // Получаем список файлов
            fi = di.GetFiles();
        }
        catch
        {
            return CountOfMatchFiles;
        }

        // Перебираем список файлов
        foreach (var f in fi) // поискать desktop.ini
            // Если файл соответствует маске
            if (regMask.IsMatch(f.Name))
            {
                // Увеличиваем счетчик
                ++CountOfMatchFiles; // сделать событие на изменение кол-ва найденных файлов (скорее использовать триггер или что-то из того что тогда изучали)
                OnAFileWasFoundWithTheGivenMask();
                // ???????


                //Console.WriteLine("File " + f.Name);

                if (regText != null)
                {
                    // Открываем файл
                    sr = new StreamReader(di.FullName + @"\" + f.Name,
                        Encoding.Default);
                    // Считываем целиком
                    var Content = sr.ReadToEnd();
                    // Закрываем файл
                    sr.Close();
                    // Ищем заданный текст
                    mc = regText.Matches(Content);
                    // Перебираем список вхождений
                    foreach (Match m in mc) Console.WriteLine("Текст найден в позиции {0}.", m.Index);
                }
            }

        // Получаем список подкаталогов
        var diSub = di.GetDirectories();
        // Для каждого из них вызываем (рекурсивно) эту же функцию поиска
        foreach (var diSubDir in diSub)
            CountOfMatchFiles += FindTextInFiles(regText, diSubDir, regMask);

        // Возврат количества обработанных файлов
        return CountOfMatchFiles;
    }

    // Процесс поиска файлов.
    public void Process()
    {
        // Добавляем \ в конец пути, если его нет.
        Path = AddSlash(Path);
        // Создаем объект DirectoryInfo на основе введенного пути.
        Di = new DirectoryInfo(Path);
        // Преобразуем введенную маску для файлов в регулярное выражение.
        ConvertMaskToRegEx(Mask);
        // Создаем объект регулярного выражения на основе маски.
        //CreateRegEx();
        // Экранируем спецсимволы во введенном тексте.
        EscapeSpecialCharacters(Text);
        // Создаем объект регулярного выражения на основе текста.
        CreateRegEx(Text);
        // Поиск файлов.
        CountOfMatchFiles = FindTextInFiles(RegText, Di, RegMask);
    }

    // Метод добавления файла в список
    private void AddFileToListView(FileInfo f)
    {
        // Создаем новый элемент списка
        ListViewItem lvi = new ListViewItem();

        // Устанавливаем имя файла
        lvi.Text = f.Name;

        // Устанавливаем путь к файлу
        lvi.SubItems.Add(f.DirectoryName);

        // Устанавливаем размер файла
        lvi.SubItems.Add(f.Length.ToString());

        // Устанавливаем дату создания файла
        lvi.SubItems.Add(f.LastWriteTime.ToString());

        // Добавляем элемент в список
        //listView1.Items.Add(lvi);


    }
}