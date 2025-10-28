namespace WorldSkillsRussia
{
    partial class FormUchastnikiForOrganizator
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBCompetition = new System.Windows.Forms.ComboBox();
            this.comboBRole = new System.Windows.Forms.ComboBox();
            this.dataGVUchastniki = new System.Windows.Forms.DataGridView();
            this.pictureBLogo = new System.Windows.Forms.PictureBox();
            this.checkBFilter = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGVUchastniki)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(342, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Компетенция";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(385, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Роль";
            // 
            // comboBCompetition
            // 
            this.comboBCompetition.FormattingEnabled = true;
            this.comboBCompetition.Location = new System.Drawing.Point(423, 16);
            this.comboBCompetition.Name = "comboBCompetition";
            this.comboBCompetition.Size = new System.Drawing.Size(234, 21);
            this.comboBCompetition.TabIndex = 2;
            this.comboBCompetition.Text = "Компетенция";
            this.comboBCompetition.SelectedIndexChanged += new System.EventHandler(this.comboBCompetition_SelectedIndexChanged);
            // 
            // comboBRole
            // 
            this.comboBRole.FormattingEnabled = true;
            this.comboBRole.Location = new System.Drawing.Point(423, 41);
            this.comboBRole.Name = "comboBRole";
            this.comboBRole.Size = new System.Drawing.Size(234, 21);
            this.comboBRole.TabIndex = 3;
            this.comboBRole.Text = "Эксперт";
            this.comboBRole.SelectedIndexChanged += new System.EventHandler(this.comboBRole_SelectedIndexChanged);
            // 
            // dataGVUchastniki
            // 
            this.dataGVUchastniki.AllowUserToAddRows = false;
            this.dataGVUchastniki.AllowUserToDeleteRows = false;
            this.dataGVUchastniki.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGVUchastniki.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGVUchastniki.Location = new System.Drawing.Point(12, 89);
            this.dataGVUchastniki.Name = "dataGVUchastniki";
            this.dataGVUchastniki.Size = new System.Drawing.Size(776, 367);
            this.dataGVUchastniki.TabIndex = 4;
            // 
            // pictureBLogo
            // 
            this.pictureBLogo.BackgroundImage = global::WorldSkillsRussia.Properties.Resources.wsrlogo_Red;
            this.pictureBLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBLogo.Location = new System.Drawing.Point(663, 6);
            this.pictureBLogo.Name = "pictureBLogo";
            this.pictureBLogo.Size = new System.Drawing.Size(125, 70);
            this.pictureBLogo.TabIndex = 21;
            this.pictureBLogo.TabStop = false;
            // 
            // checkBFilter
            // 
            this.checkBFilter.AutoSize = true;
            this.checkBFilter.Location = new System.Drawing.Point(423, 68);
            this.checkBFilter.Name = "checkBFilter";
            this.checkBFilter.Size = new System.Drawing.Size(113, 17);
            this.checkBFilter.TabIndex = 22;
            this.checkBFilter.Text = "Включить вильтр";
            this.checkBFilter.UseVisualStyleBackColor = true;
            this.checkBFilter.CheckedChanged += new System.EventHandler(this.checkBFilter_CheckedChanged);
            // 
            // FormUchastnikiForOrganizator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 468);
            this.Controls.Add(this.checkBFilter);
            this.Controls.Add(this.pictureBLogo);
            this.Controls.Add(this.dataGVUchastniki);
            this.Controls.Add(this.comboBRole);
            this.Controls.Add(this.comboBCompetition);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormUchastnikiForOrganizator";
            this.Text = "Участники чемпионата";
            this.Load += new System.EventHandler(this.FormUchastniki_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGVUchastniki)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBCompetition;
        private System.Windows.Forms.ComboBox comboBRole;
        private System.Windows.Forms.DataGridView dataGVUchastniki;
        private System.Windows.Forms.PictureBox pictureBLogo;
        private System.Windows.Forms.CheckBox checkBFilter;
    }
}