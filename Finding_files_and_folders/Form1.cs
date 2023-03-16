using static System.Runtime.InteropServices.JavaScript.JSType;

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

        // ������ ����������� ��� �������� ����� �������
        ImageList image_list1 = new ImageList();


        se.CountChanged += CountChanged;




    }

    private void CountChanged(object sender, EventArgs e)
    {
        UiContext.Send(d => label1_number_of_files_found.Text = se.CountOfMatchFiles.ToString(), null);
        //label1.Text = se.CountOfMatchFiles.ToString();
    }

    private void AddItemToListView(string filePath)
    {
        //if (listView1.InvokeRequired)
        //{
        //    listView1.Invoke(new Action<string>(AddItemToListView), filePath);
        //    return;
        //}

        // ��������� ���� � ListView1
        //var item = new ListViewItem(filePath);
        //listView1.Items.Add(item);
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
        var thread = new Thread(se.Process);
        thread.IsBackground = true;
        // ��������� �����
        thread.Start();
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

// ������ ������ ����� � ������� ����� � ��������� ��������� ������� ������ !
// ���������� ���������� � �������� ������ �� ���-�� ������, � ��� �� ������. ����� ���� � �������.