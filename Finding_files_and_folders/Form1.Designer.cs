namespace Finding_files_and_folders
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listView1 = new ListView();
            label1_file = new Label();
            textBox1_file_extension = new TextBox();
            label2_word_or_phrase = new Label();
            label3_discs = new Label();
            textBox2_words_in_file = new TextBox();
            comboBox1 = new ComboBox();
            button1_find = new Button();
            button2_stop = new Button();
            checkBox1_subfolders = new CheckBox();
            label4_search_result = new Label();
            label1 = new Label();
            label1_number_of_files_found = new Label();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Location = new Point(12, 104);
            listView1.Name = "listView1";
            listView1.Size = new Size(760, 445);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // label1_file
            // 
            label1_file.AutoSize = true;
            label1_file.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1_file.Location = new Point(26, 9);
            label1_file.Name = "label1_file";
            label1_file.Size = new Size(41, 19);
            label1_file.TabIndex = 1;
            label1_file.Text = "Файл";
            // 
            // textBox1_file_extension
            // 
            textBox1_file_extension.Location = new Point(12, 31);
            textBox1_file_extension.Name = "textBox1_file_extension";
            textBox1_file_extension.Size = new Size(69, 23);
            textBox1_file_extension.TabIndex = 2;
            textBox1_file_extension.Text = "*.txt";
            textBox1_file_extension.TextChanged += textBox1_file_extension_TextChanged;
            // 
            // label2_word_or_phrase
            // 
            label2_word_or_phrase.AutoSize = true;
            label2_word_or_phrase.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2_word_or_phrase.Location = new Point(180, 9);
            label2_word_or_phrase.Name = "label2_word_or_phrase";
            label2_word_or_phrase.Size = new Size(171, 19);
            label2_word_or_phrase.TabIndex = 3;
            label2_word_or_phrase.Text = "Слово или фраза в файле";
            // 
            // label3_discs
            // 
            label3_discs.AutoSize = true;
            label3_discs.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3_discs.Location = new Point(444, 9);
            label3_discs.Name = "label3_discs";
            label3_discs.Size = new Size(48, 19);
            label3_discs.TabIndex = 4;
            label3_discs.Text = "Диски";
            // 
            // textBox2_words_in_file
            // 
            textBox2_words_in_file.Location = new Point(87, 31);
            textBox2_words_in_file.Name = "textBox2_words_in_file";
            textBox2_words_in_file.Size = new Size(342, 23);
            textBox2_words_in_file.TabIndex = 5;
            textBox2_words_in_file.TextChanged += textBox2_words_in_file_TextChanged;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(435, 31);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(69, 23);
            comboBox1.TabIndex = 6;
            // 
            // button1_find
            // 
            button1_find.Location = new Point(510, 30);
            button1_find.Name = "button1_find";
            button1_find.Size = new Size(65, 23);
            button1_find.TabIndex = 7;
            button1_find.Text = "Найти";
            button1_find.UseVisualStyleBackColor = true;
            button1_find.Click += button1_find_Click;
            // 
            // button2_stop
            // 
            button2_stop.Enabled = false;
            button2_stop.Location = new Point(581, 30);
            button2_stop.Name = "button2_stop";
            button2_stop.Size = new Size(88, 23);
            button2_stop.TabIndex = 8;
            button2_stop.Text = "Остановить";
            button2_stop.UseVisualStyleBackColor = true;
            // 
            // checkBox1_subfolders
            // 
            checkBox1_subfolders.AutoSize = true;
            checkBox1_subfolders.Location = new Point(675, 33);
            checkBox1_subfolders.Name = "checkBox1_subfolders";
            checkBox1_subfolders.Size = new Size(97, 19);
            checkBox1_subfolders.TabIndex = 9;
            checkBox1_subfolders.Text = "Подкаталоги";
            checkBox1_subfolders.UseVisualStyleBackColor = true;
            checkBox1_subfolders.CheckedChanged += checkBox1_subfolders_CheckedChanged;
            // 
            // label4_search_result
            // 
            label4_search_result.AutoSize = true;
            label4_search_result.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label4_search_result.Location = new Point(200, 71);
            label4_search_result.Name = "label4_search_result";
            label4_search_result.Size = new Size(139, 19);
            label4_search_result.TabIndex = 10;
            label4_search_result.Text = "Результаты поиска : ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(334, 71);
            label1.Name = "label1";
            label1.Size = new Size(220, 19);
            label1.TabIndex = 11;
            label1.Text = "количество найденных файлов - ";
            // 
            // label1_number_of_files_found
            // 
            label1_number_of_files_found.AutoSize = true;
            label1_number_of_files_found.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1_number_of_files_found.Location = new Point(549, 71);
            label1_number_of_files_found.Name = "label1_number_of_files_found";
            label1_number_of_files_found.Size = new Size(17, 19);
            label1_number_of_files_found.TabIndex = 12;
            label1_number_of_files_found.Text = "0";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(label1_number_of_files_found);
            Controls.Add(label1);
            Controls.Add(label4_search_result);
            Controls.Add(checkBox1_subfolders);
            Controls.Add(button2_stop);
            Controls.Add(button1_find);
            Controls.Add(comboBox1);
            Controls.Add(textBox2_words_in_file);
            Controls.Add(label3_discs);
            Controls.Add(label2_word_or_phrase);
            Controls.Add(textBox1_file_extension);
            Controls.Add(label1_file);
            Controls.Add(listView1);
            MaximizeBox = false;
            MaximumSize = new Size(800, 600);
            MinimumSize = new Size(800, 600);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listView1;
        private Label label1_file;
        private Label label2_word_or_phrase;
        private Label label3_discs;
        private ComboBox comboBox1;
        private Button button1_find;
        private Button button2_stop;
        private CheckBox checkBox1_subfolders;
        private Label label4_search_result;
        private Label label1;
        private Label label1_number_of_files_found;
        private TextBox textBox1_file_extension;
        private TextBox textBox2_words_in_file;
    }
}