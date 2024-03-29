﻿using System.Text;
using System.Text.RegularExpressions;

namespace Finding_files_and_folders;

//delegate void MyDelegate();
internal class SearchEngine
{
    // Конструктор по умолчанию.
    public SearchEngine()
    {
        Path = "C:\\";
        Mask = "";
        Text = "";
        Di = new DirectoryInfo(Path);
        RegMask = null;
        //CountOfMatchFiles = 0;
        Drives = GetDrives();
        _lvi = new ListViewItem();
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

    // Поиск в подкаталогах ? да - true, нет - false
    public bool SearchInSubdirectories { get; set; }

    // Метод поиска файлов.
    private void FindTextInFiles(Regex regText/*текст в файле - "А крила має"*/, DirectoryInfo di, Regex regMask/*файл - 1.txt*/)
    {
        // Поток для чтения из файла
        StreamReader sr = null;
        // Список найденных совпадений
        MatchCollection mc = null;

        FileInfo[] fi = null;

        // Получаем список файлов
        fi = di.GetFiles();

        // Перебираем список файлов
        foreach (var f in fi)
        {
            // Если файл соответствует маске
            if (regMask.IsMatch(f.Name))
            {
                CreateListViewItem(f);
                //NewFileFound.Set();
            }
            // Поиск текста в файле.
            if (regText != null)
            {
                // Открываем файл
                try
                {
                    sr = new StreamReader(di.FullName + @"\" + f.Name,
                        Encoding.Default);
                    // Считываем целиком
                    var Content = sr.ReadToEnd();
                    // Закрываем файл
                    sr.Close();
                    // Ищем заданный текст
                    mc = regText.Matches(Content);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (mc.Count > 0)
                {
                    CreateListViewItem(f);
                    NewFileFound.Set();
                    ContinueSearching.WaitOne();
                    //while (true)
                    //{
                    //    if (ContinueSearching.WaitOne(0))
                    //    {
                    //        break;
                    //    }
                    //}
                }
            }
        }

        if (SearchInSubdirectories == true)
        {
            // Получаем список подкаталогов
            var diSub = di.GetDirectories();
            // Для каждого из них вызываем (рекурсивно) эту же функцию поиска
            foreach (var diSubDir in diSub)
                FindTextInFiles(regText, diSubDir, regMask);
        }
    }

    public ManualResetEvent ContinueSearching = new(false);
    public ManualResetEvent NewFileFound = new(false);
    public ManualResetEvent EventForSearchComplete = new(false);
    //public ManualResetEvent event_for_suspend = new(false);

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
        FindTextInFiles(RegText, Di, RegMask);
        // Поиск завершен.
        EventForSearchComplete.Set();
    }

    public ListViewItem _lvi;
    // Создание элемента ListViewItem.
    private void CreateListViewItem(FileInfo f)
    {
        // Создаем новый элемент списка
        _lvi = new ListViewItem();

        // Устанавливаем имя файла
        _lvi.Text = f.Name;

        // Устанавливаем путь к файлу
        _lvi.SubItems.Add(f.DirectoryName);

        // Устанавливаем размер файла
        _lvi.SubItems.Add(f.Length.ToString());

        // Устанавливаем дату создания файла
        _lvi.SubItems.Add(f.LastWriteTime.ToString());

        // Полный путь к файлу
        _lvi.Tag = f.FullName;
    }
}