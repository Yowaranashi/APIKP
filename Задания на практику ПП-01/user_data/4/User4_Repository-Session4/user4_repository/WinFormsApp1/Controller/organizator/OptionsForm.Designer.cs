namespace WinFormsApp1.Controller.organizator
{
    partial class OptionsForm
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
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            dataGridView1 = new DataGridView();
            comboBox1 = new ComboBox();
            label6 = new Label();
            groupBox4 = new GroupBox();
            label5 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            label1 = new Label();
            checkBox2 = new CheckBox();
            checkBox1 = new CheckBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            groupBox3 = new GroupBox();
            button7 = new Button();
            dataGridView2 = new DataGridView();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(checkBox2);
            groupBox1.Controls.Add(checkBox1);
            groupBox1.Location = new Point(12, 55);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(776, 296);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button6);
            groupBox2.Controls.Add(button5);
            groupBox2.Controls.Add(button4);
            groupBox2.Controls.Add(dataGridView1);
            groupBox2.Controls.Add(comboBox1);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(groupBox4);
            groupBox2.Location = new Point(0, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(776, 296);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            // 
            // button6
            // 
            button6.Location = new Point(664, 256);
            button6.Name = "button6";
            button6.Size = new Size(96, 34);
            button6.TabIndex = 13;
            button6.Text = "Удалить";
            button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(108, 256);
            button5.Name = "button5";
            button5.Size = new Size(96, 34);
            button5.TabIndex = 12;
            button5.Text = "Изменить";
            button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(6, 256);
            button4.Name = "button4";
            button4.Size = new Size(96, 34);
            button4.TabIndex = 11;
            button4.Text = "Добавить";
            button4.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(6, 123);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(754, 127);
            dataGridView1.TabIndex = 10;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(558, 80);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(202, 23);
            comboBox1.TabIndex = 9;
            comboBox1.Text = "C-1";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(448, 88);
            label6.Name = "label6";
            label6.Size = new Size(104, 15);
            label6.TabIndex = 8;
            label6.Text = "День чемпионата";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(label2);
            groupBox4.Controls.Add(label3);
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(radioButton3);
            groupBox4.Controls.Add(radioButton2);
            groupBox4.Controls.Add(radioButton1);
            groupBox4.Location = new Point(443, 10);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(317, 57);
            groupBox4.TabIndex = 0;
            groupBox4.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(269, 19);
            label5.Name = "label5";
            label5.Size = new Size(32, 15);
            label5.TabIndex = 7;
            label5.Text = "Всех";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(194, 19);
            label2.Name = "label2";
            label2.Size = new Size(64, 15);
            label2.TabIndex = 4;
            label2.Text = "Экспертов";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(117, 19);
            label3.Name = "label3";
            label3.Size = new Size(71, 15);
            label3.TabIndex = 5;
            label3.Text = "Участников";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(5, 25);
            label4.Name = "label4";
            label4.Size = new Size(93, 15);
            label4.TabIndex = 6;
            label4.Text = "Протоколы для";
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(279, 38);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(14, 13);
            radioButton3.TabIndex = 2;
            radioButton3.TabStop = true;
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(219, 38);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(14, 13);
            radioButton2.TabIndex = 1;
            radioButton2.TabStop = true;
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(140, 38);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(14, 13);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(488, 108);
            label1.Name = "label1";
            label1.Size = new Size(53, 15);
            label1.TabIndex = 2;
            label1.Text = "Логотип";
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(51, 225);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(353, 19);
            checkBox2.TabIndex = 1;
            checkBox2.Text = "Зам главного эксперта может принимать участие в оценке";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(51, 200);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(320, 19);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Главный эксперт может принимать участие в оценке";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(120, 12);
            button1.Name = "button1";
            button1.Size = new Size(170, 37);
            button1.TabIndex = 1;
            button1.Text = "Основные настройки";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(296, 12);
            button2.Name = "button2";
            button2.Size = new Size(170, 38);
            button2.TabIndex = 2;
            button2.Text = "Список протоколов";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(473, 12);
            button3.Name = "button3";
            button3.Size = new Size(170, 38);
            button3.TabIndex = 3;
            button3.Text = "Список особых полномочий экспертов";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(button7);
            groupBox3.Controls.Add(dataGridView2);
            groupBox3.Controls.Add(button8);
            groupBox3.Controls.Add(button9);
            groupBox3.Location = new Point(12, 56);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(776, 296);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            // 
            // button7
            // 
            button7.Location = new Point(672, 190);
            button7.Name = "button7";
            button7.Size = new Size(96, 34);
            button7.TabIndex = 17;
            button7.Text = "Удалить";
            button7.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(16, 57);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowTemplate.Height = 25;
            dataGridView2.Size = new Size(754, 127);
            dataGridView2.TabIndex = 14;
            // 
            // button8
            // 
            button8.Location = new Point(129, 190);
            button8.Name = "button8";
            button8.Size = new Size(96, 34);
            button8.TabIndex = 16;
            button8.Text = "Изменить";
            button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            button9.Location = new Point(16, 190);
            button9.Name = "button9";
            button9.Size = new Size(96, 34);
            button9.TabIndex = 15;
            button9.Text = "Добавить";
            button9.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            button10.Location = new Point(296, 357);
            button10.Name = "button10";
            button10.Size = new Size(170, 38);
            button10.TabIndex = 4;
            button10.Text = "Сохранить";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // OptionsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(808, 406);
            Controls.Add(button10);
            Controls.Add(groupBox3);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(groupBox1);
            Name = "OptionsForm";
            Text = "Options";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label2;
        private Label label1;
        private CheckBox checkBox2;
        private CheckBox checkBox1;
        private Button button1;
        private Button button2;
        private Button button3;
        private GroupBox groupBox2;
        private GroupBox groupBox4;
        private Label label3;
        private Label label4;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private GroupBox groupBox3;
        private Button button6;
        private Button button5;
        private Button button4;
        private DataGridView dataGridView1;
        private ComboBox comboBox1;
        private Label label6;
        private Label label5;
        private Button button7;
        private DataGridView dataGridView2;
        private Button button8;
        private Button button9;
        private Button button10;
    }
}