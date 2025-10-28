namespace WinFormsApp1.Controller.organizator
{
    partial class OrganizatorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            exit = new Button();
            chempionat = new Button();
            Protokol = new Button();
            expertUprav = new Button();
            chempionatOptions = new Button();
            label1 = new Label();
            wellcome = new Label();
            comboBox3 = new ComboBox();
            add = new Button();
            button1 = new Button();
            label2 = new Label();
            dateTimePicker1 = new DateTimePicker();
            dateTimePicker2 = new DateTimePicker();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            plus = new Button();
            groupBox1 = new GroupBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // exit
            // 
            exit.Location = new Point(10, 15);
            exit.Name = "exit";
            exit.Size = new Size(77, 29);
            exit.TabIndex = 0;
            exit.Text = "Выход";
            exit.UseVisualStyleBackColor = true;
            exit.Click += exit_Click;
            // 
            // chempionat
            // 
            chempionat.Location = new Point(12, 102);
            chempionat.Name = "chempionat";
            chempionat.Size = new Size(125, 43);
            chempionat.TabIndex = 2;
            chempionat.Text = "Чемпионат";
            chempionat.UseVisualStyleBackColor = true;
            chempionat.Click += chempionat_Click;
            // 
            // Protokol
            // 
            Protokol.Location = new Point(12, 280);
            Protokol.Name = "Protokol";
            Protokol.Size = new Size(125, 43);
            Protokol.TabIndex = 3;
            Protokol.Text = "Протоколы";
            Protokol.UseVisualStyleBackColor = true;
            Protokol.Click += Protokol_Click;
            // 
            // expertUprav
            // 
            expertUprav.Location = new Point(12, 221);
            expertUprav.Name = "expertUprav";
            expertUprav.Size = new Size(125, 43);
            expertUprav.TabIndex = 4;
            expertUprav.Text = "Управление экспертами";
            expertUprav.UseVisualStyleBackColor = true;
            expertUprav.Click += expertUprav_Click;
            // 
            // chempionatOptions
            // 
            chempionatOptions.Location = new Point(12, 161);
            chempionatOptions.Name = "chempionatOptions";
            chempionatOptions.Size = new Size(125, 43);
            chempionatOptions.TabIndex = 5;
            chempionatOptions.Text = "Настройка чемпионата";
            chempionatOptions.UseVisualStyleBackColor = true;
            chempionatOptions.Click += chempionatOptions_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(143, 9);
            label1.Name = "label1";
            label1.Size = new Size(219, 21);
            label1.TabIndex = 6;
            label1.Text = "ТЕХНИЧЕСКАЯ ДИРЕКЦИЯ";
            // 
            // wellcome
            // 
            wellcome.AutoSize = true;
            wellcome.Location = new Point(143, 30);
            wellcome.Name = "wellcome";
            wellcome.Size = new Size(60, 15);
            wellcome.TabIndex = 7;
            wellcome.Text = "Wellcome";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(12, 58);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(229, 23);
            comboBox3.TabIndex = 13;
            comboBox3.Text = "Чемпионат";
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // add
            // 
            add.Location = new Point(6, 15);
            add.Name = "add";
            add.Size = new Size(75, 23);
            add.TabIndex = 0;
            add.Text = "Добавить";
            add.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(87, 15);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "Изменить";
            button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 56);
            label2.Name = "label2";
            label2.Size = new Size(105, 15);
            label2.TabIndex = 2;
            label2.Text = "Даты чемпионата";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(117, 50);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(200, 23);
            dateTimePicker1.TabIndex = 3;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Location = new Point(323, 50);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(200, 23);
            dateTimePicker2.TabIndex = 4;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(11, 85);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(512, 23);
            textBox1.TabIndex = 5;
            textBox1.Text = "Название чемпионата";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(529, 85);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 6;
            textBox2.Text = "S чемпионата";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(11, 114);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(229, 23);
            comboBox1.TabIndex = 7;
            comboBox1.Text = "Компетенция";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(246, 114);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(171, 23);
            comboBox2.TabIndex = 8;
            comboBox2.Text = "Главный эксперт";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(423, 114);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 9;
            textBox3.Text = "Кол-во экс";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(529, 114);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(62, 23);
            textBox4.TabIndex = 10;
            textBox4.Text = "Кол-во уч";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(601, 114);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(28, 23);
            textBox5.TabIndex = 11;
            textBox5.Text = "S";
            // 
            // plus
            // 
            plus.Location = new Point(9, 141);
            plus.Name = "plus";
            plus.Size = new Size(23, 23);
            plus.TabIndex = 12;
            plus.Text = "+";
            plus.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(plus);
            groupBox1.Controls.Add(textBox5);
            groupBox1.Controls.Add(textBox4);
            groupBox1.Controls.Add(textBox3);
            groupBox1.Controls.Add(comboBox2);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(dateTimePicker2);
            groupBox1.Controls.Add(dateTimePicker1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(add);
            groupBox1.Location = new Point(143, 87);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(733, 361);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            // 
            // OrganizatorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(888, 463);
            Controls.Add(comboBox3);
            Controls.Add(wellcome);
            Controls.Add(label1);
            Controls.Add(chempionatOptions);
            Controls.Add(expertUprav);
            Controls.Add(Protokol);
            Controls.Add(chempionat);
            Controls.Add(groupBox1);
            Controls.Add(exit);
            Name = "OrganizatorForm";
            Text = "OrganizatorForm";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button exit;
        private Button chempionat;
        private Button Protokol;
        private Button expertUprav;
        private Button chempionatOptions;
        private Label label1;
        private Label wellcome;
        private ComboBox comboBox3;
        private Button add;
        private Button button1;
        private Label label2;
        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
        private TextBox textBox1;
        private TextBox textBox2;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private Button plus;
        private GroupBox groupBox1;
    }
}