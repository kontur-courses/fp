namespace TagCloudContainer.Gui
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.wordsTextBox = new System.Windows.Forms.TextBox();
            this.chooseFontColorButton = new System.Windows.Forms.Button();
            this.resultWidthTextBox = new System.Windows.Forms.TextBox();
            this.resultHeightTextBox = new System.Windows.Forms.TextBox();
            this.resultSizeXLabel = new System.Windows.Forms.Label();
            this.saveResultButton = new System.Windows.Forms.Button();
            this.generateButton = new System.Windows.Forms.Button();
            this.openFileButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.resultPictureBox = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.statusLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultPictureBox)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // wordsTextBox
            // 
            this.wordsTextBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.wordsTextBox.Location = new System.Drawing.Point(0, 0);
            this.wordsTextBox.Multiline = true;
            this.wordsTextBox.Name = "wordsTextBox";
            this.wordsTextBox.Size = new System.Drawing.Size(281, 450);
            this.wordsTextBox.TabIndex = 1;
            // 
            // chooseFontColorButton
            // 
            this.chooseFontColorButton.Location = new System.Drawing.Point(135, 17);
            this.chooseFontColorButton.Name = "chooseFontColorButton";
            this.chooseFontColorButton.Size = new System.Drawing.Size(91, 23);
            this.chooseFontColorButton.TabIndex = 5;
            this.chooseFontColorButton.Text = "Цвет шрифта";
            this.chooseFontColorButton.UseVisualStyleBackColor = true;
            this.chooseFontColorButton.Click += new System.EventHandler(this.ChooseFontColor_Click);
            // 
            // resultWidthTextBox
            // 
            this.resultWidthTextBox.Location = new System.Drawing.Point(6, 17);
            this.resultWidthTextBox.Name = "resultWidthTextBox";
            this.resultWidthTextBox.Size = new System.Drawing.Size(47, 20);
            this.resultWidthTextBox.TabIndex = 6;
            this.resultWidthTextBox.Text = "600";
            // 
            // resultHeightTextBox
            // 
            this.resultHeightTextBox.Location = new System.Drawing.Point(79, 17);
            this.resultHeightTextBox.Name = "resultHeightTextBox";
            this.resultHeightTextBox.Size = new System.Drawing.Size(50, 20);
            this.resultHeightTextBox.TabIndex = 7;
            this.resultHeightTextBox.Text = "600";
            // 
            // resultSizeXLabel
            // 
            this.resultSizeXLabel.AutoSize = true;
            this.resultSizeXLabel.Location = new System.Drawing.Point(59, 20);
            this.resultSizeXLabel.Name = "resultSizeXLabel";
            this.resultSizeXLabel.Size = new System.Drawing.Size(14, 13);
            this.resultSizeXLabel.TabIndex = 8;
            this.resultSizeXLabel.Text = "X";
            // 
            // saveResultButton
            // 
            this.saveResultButton.Location = new System.Drawing.Point(432, 20);
            this.saveResultButton.Name = "saveResultButton";
            this.saveResultButton.Size = new System.Drawing.Size(75, 23);
            this.saveResultButton.TabIndex = 4;
            this.saveResultButton.Text = "Сохранить";
            this.saveResultButton.UseVisualStyleBackColor = true;
            this.saveResultButton.Click += new System.EventHandler(this.SaveResultButton_Click);
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(325, 20);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(101, 23);
            this.generateButton.TabIndex = 3;
            this.generateButton.Text = "Сгенерировать";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(244, 20);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(75, 23);
            this.openFileButton.TabIndex = 2;
            this.openFileButton.Text = "Открыть файл";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.resultPictureBox);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(281, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 450);
            this.panel1.TabIndex = 9;
            // 
            // resultPictureBox
            // 
            this.resultPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultPictureBox.Location = new System.Drawing.Point(0, 0);
            this.resultPictureBox.Name = "resultPictureBox";
            this.resultPictureBox.Size = new System.Drawing.Size(519, 350);
            this.resultPictureBox.TabIndex = 9;
            this.resultPictureBox.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.statusLabel);
            this.panel2.Controls.Add(this.resultWidthTextBox);
            this.panel2.Controls.Add(this.openFileButton);
            this.panel2.Controls.Add(this.resultHeightTextBox);
            this.panel2.Controls.Add(this.generateButton);
            this.panel2.Controls.Add(this.resultSizeXLabel);
            this.panel2.Controls.Add(this.saveResultButton);
            this.panel2.Controls.Add(this.chooseFontColorButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 350);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(519, 100);
            this.panel2.TabIndex = 0;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusLabel.Location = new System.Drawing.Point(0, 87);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(83, 13);
            this.statusLabel.TabIndex = 9;
            this.statusLabel.Text = "Готов к работе";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.wordsTextBox);
            this.Name = "MainForm";
            this.Text = "Облако слов";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultPictureBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox wordsTextBox;
        private System.Windows.Forms.Button chooseFontColorButton;
        private System.Windows.Forms.TextBox resultWidthTextBox;
        private System.Windows.Forms.TextBox resultHeightTextBox;
        private System.Windows.Forms.Label resultSizeXLabel;
        private System.Windows.Forms.Button saveResultButton;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox resultPictureBox;
        private System.Windows.Forms.Label statusLabel;
    }
}

