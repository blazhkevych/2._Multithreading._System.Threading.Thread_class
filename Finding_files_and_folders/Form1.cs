using System.Windows.Forms;

namespace Finding_files_and_folders;

public partial class Form1 : Form
{
    private readonly SearchEngine se = new();
    public SynchronizationContext UiContext;

    public Form1()
    {
        InitializeComponent();
        // �������� �������� ����
        Text = "����� ������ � �����";

        // ������� �������� ������������� ��� �������� ������ 
        UiContext = SynchronizationContext.Current;

        // ��������� ����� � ���������.
        foreach (var drive in se.Drives)
            comboBox1.Items.Add(drive);

        // ������������� ������ ������� ���������� �� ���������.
        comboBox1.SelectedIndex = 0;

        // ������������� ����� �� textBox.
        textBox2_words_in_file.Focus();

        // ��������� ��������� ����� �����������
        listView1.View = View.Details;

        // ��� ������ �������� ������ ����� ���������� ��� ������
        listView1.FullRowSelect = true;

        // ��������� ���������� ��������� � ������� �����������
        listView1.Sorting = SortOrder.Ascending;

        // ������� ������� � ������
        listView1.Columns.Add("���", 200, HorizontalAlignment.Left);
        listView1.Columns.Add("�����", 150, HorizontalAlignment.Left);
        listView1.Columns.Add("������", 100, HorizontalAlignment.Left);
        listView1.Columns.Add("���� �����������", 150, HorizontalAlignment.Left);

        // ��������� ������ ������� �� ���������
        listView1.Columns[0].Width = 150;

        // ��������� ������ ������ �������
        listView1.Columns[1].Width = 350;

        // ��������� ������ ������� �������
        listView1.Columns[2].Width = 90;

        // ��������� ������ ��������� �������
        listView1.Columns[3].Width = 150;

        // ��������� ��������� ����� �����������
        listView1.View = View.Details;

        // ������� ����� �����������
        image_list1.ColorDepth = ColorDepth.Depth32Bit;
        // ��������� ������ �����������
        image_list1.ImageSize = new Size(16, 16);
        // ����������� ������ ��������� ����������� � ListView
        listView1.SmallImageList = image_list1;
    }
    // ������ ����������� ��� �������� ����� �������
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

    // ������ �����.
    private void button1_find_Click(object sender, EventArgs e)
    {
        // ��������� ������ ����� �� ����� ������
        button1_find.Enabled = false;
        // ���������� ������ ����������
        button2_stop.Enabled = true;
        // ��������� ������� �� ����� ������
        checkBox1_subfolders.Enabled = false;
        // ��������� textBox1_file_extension �� ����� ������
        textBox1_file_extension.Enabled = false;
        // ��������� textBox2_words_in_file �� ����� ������
        textBox2_words_in_file.Enabled = false;
        // ��������� comboBox1 �� ����� ������
        comboBox1.Enabled = false;

        // ������������� ���� ������
        se.Path = comboBox1.Text;

        // ������� ������
        listView1.Items.Clear();

        // ������� ������� ����� ����� ��� ������
        var thread1 = new Thread(se.Process);
        thread1.Name = "����� ������ � �����";
        thread1.IsBackground = true;
        // �������� ��������� �������.
        se.EventForFileFound.Reset();
        // ��������� �����
        thread1.Start();

        while (true)
            // ���� EventForFileFound ��������, ���� ������, ����������� ������� ��������� ������
            if (se.EventForFileFound.WaitOne(0))
            {
                // ����������� ������� ��������� ������
                Label1NumberOfFilesFound = label1_number_of_files_found.Text;
                // ��������� ���� � ListView1
                AddItemToListView();
                // �������� ��������� �������.
                se.EventForFileFound.Reset();
                // ���� ������� EventForSearchComplete ��������, ����� ��������, ������� �� ����� � ������� ��������������� ���������
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

    // ������������ �� ��������� ������ � textBox1_file_extension.
    private void textBox1_file_extension_TextChanged(object sender, EventArgs e)
    {
        se.Mask = textBox1_file_extension.Text;
    }

    // ������������ �� ��������� ������ � textBox2_words_in_file.
    private void textBox2_words_in_file_TextChanged(object sender, EventArgs e)
    {
        se.Text = textBox2_words_in_file.Text;
    }

    // ������������ �� ��������� ��������� �������� checkBox1_subfolders.
    private void checkBox1_subfolders_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox1_subfolders.Checked)
            se.SearchInSubdirectories = true;
        else
            se.SearchInSubdirectories = false;
    }
}