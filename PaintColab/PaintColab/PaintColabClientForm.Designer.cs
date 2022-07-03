
namespace PaintColab
{
    partial class PaintColabClientForm
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
            this.components = new System.ComponentModel.Container();
            this.serverPortLabel = new System.Windows.Forms.Label();
            this.serverPortTextBox = new System.Windows.Forms.TextBox();
            this.connectServerButton = new System.Windows.Forms.Button();
            this.canvasPanel = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serverIPLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.thicknessLabel = new System.Windows.Forms.Label();
            this.redLabel = new System.Windows.Forms.Label();
            this.greenLabel = new System.Windows.Forms.Label();
            this.blueLabel = new System.Windows.Forms.Label();
            this.alphaLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serverPortLabel
            // 
            this.serverPortLabel.AutoSize = true;
            this.serverPortLabel.Location = new System.Drawing.Point(234, 9);
            this.serverPortLabel.Name = "serverPortLabel";
            this.serverPortLabel.Size = new System.Drawing.Size(62, 13);
            this.serverPortLabel.TabIndex = 0;
            this.serverPortLabel.Text = "Server port:";
            // 
            // serverPortTextBox
            // 
            this.serverPortTextBox.Location = new System.Drawing.Point(302, 6);
            this.serverPortTextBox.Name = "serverPortTextBox";
            this.serverPortTextBox.Size = new System.Drawing.Size(82, 20);
            this.serverPortTextBox.TabIndex = 1;
            this.serverPortTextBox.Text = "11000";
            // 
            // connectServerButton
            // 
            this.connectServerButton.Location = new System.Drawing.Point(390, 5);
            this.connectServerButton.Name = "connectServerButton";
            this.connectServerButton.Size = new System.Drawing.Size(96, 20);
            this.connectServerButton.TabIndex = 2;
            this.connectServerButton.Text = "Connect Server";
            this.connectServerButton.UseVisualStyleBackColor = true;
            this.connectServerButton.Click += new System.EventHandler(this.connectServerButton_Click);
            // 
            // canvasPanel
            // 
            this.canvasPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canvasPanel.AutoSize = true;
            this.canvasPanel.Location = new System.Drawing.Point(12, 32);
            this.canvasPanel.Name = "canvasPanel";
            this.canvasPanel.Size = new System.Drawing.Size(776, 406);
            this.canvasPanel.TabIndex = 3;
            this.canvasPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvasPanel_MouseDown);
            this.canvasPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvasPanel_MouseMove);
            this.canvasPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvasPanel_MouseUp);
            this.canvasPanel.Resize += new System.EventHandler(this.canvasPanel_Resize);
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // serverIPLabel
            // 
            this.serverIPLabel.AutoSize = true;
            this.serverIPLabel.Location = new System.Drawing.Point(12, 9);
            this.serverIPLabel.Name = "serverIPLabel";
            this.serverIPLabel.Size = new System.Drawing.Size(54, 13);
            this.serverIPLabel.TabIndex = 4;
            this.serverIPLabel.Text = "Server IP:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(72, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(156, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "localhost";
            // 
            // thicknessLabel
            // 
            this.thicknessLabel.AutoSize = true;
            this.thicknessLabel.Location = new System.Drawing.Point(502, 11);
            this.thicknessLabel.Name = "thicknessLabel";
            this.thicknessLabel.Size = new System.Drawing.Size(20, 13);
            this.thicknessLabel.TabIndex = 6;
            this.thicknessLabel.Text = "T: ";
            this.thicknessLabel.MouseLeave += new System.EventHandler(this.thicknessLabel_MouseLeave);
            this.thicknessLabel.MouseHover += new System.EventHandler(this.thicknessLabel_MouseHover);
            // 
            // redLabel
            // 
            this.redLabel.AutoSize = true;
            this.redLabel.Location = new System.Drawing.Point(539, 11);
            this.redLabel.Name = "redLabel";
            this.redLabel.Size = new System.Drawing.Size(21, 13);
            this.redLabel.TabIndex = 7;
            this.redLabel.Text = "R: ";
            this.redLabel.MouseLeave += new System.EventHandler(this.redLabel_MouseLeave);
            this.redLabel.MouseHover += new System.EventHandler(this.redLabel_MouseHover);
            // 
            // greenLabel
            // 
            this.greenLabel.AutoSize = true;
            this.greenLabel.Location = new System.Drawing.Point(577, 11);
            this.greenLabel.Name = "greenLabel";
            this.greenLabel.Size = new System.Drawing.Size(18, 13);
            this.greenLabel.TabIndex = 8;
            this.greenLabel.Text = "G:";
            this.greenLabel.MouseLeave += new System.EventHandler(this.greenLabel_MouseLeave);
            this.greenLabel.MouseHover += new System.EventHandler(this.greenLabel_MouseHover);
            // 
            // blueLabel
            // 
            this.blueLabel.AutoSize = true;
            this.blueLabel.Location = new System.Drawing.Point(617, 11);
            this.blueLabel.Name = "blueLabel";
            this.blueLabel.Size = new System.Drawing.Size(20, 13);
            this.blueLabel.TabIndex = 9;
            this.blueLabel.Text = "B: ";
            this.blueLabel.MouseLeave += new System.EventHandler(this.blueLabel_MouseLeave);
            this.blueLabel.MouseHover += new System.EventHandler(this.blueLabel_MouseHover);
            // 
            // alphaLabel
            // 
            this.alphaLabel.AutoSize = true;
            this.alphaLabel.Location = new System.Drawing.Point(653, 11);
            this.alphaLabel.Name = "alphaLabel";
            this.alphaLabel.Size = new System.Drawing.Size(20, 13);
            this.alphaLabel.TabIndex = 10;
            this.alphaLabel.Text = "A: ";
            this.alphaLabel.MouseLeave += new System.EventHandler(this.alphaLabel_MouseLeave);
            this.alphaLabel.MouseHover += new System.EventHandler(this.alphaLabel_MouseHover);
            // 
            // PaintColabClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.alphaLabel);
            this.Controls.Add(this.blueLabel);
            this.Controls.Add(this.greenLabel);
            this.Controls.Add(this.redLabel);
            this.Controls.Add(this.thicknessLabel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.serverIPLabel);
            this.Controls.Add(this.canvasPanel);
            this.Controls.Add(this.connectServerButton);
            this.Controls.Add(this.serverPortTextBox);
            this.Controls.Add(this.serverPortLabel);
            this.Name = "PaintColabClientForm";
            this.Text = "PaintColabClient";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PaintColabClientForm_FormClosing);
            this.Load += new System.EventHandler(this.PaintColabClientForm_Load);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.form_MouseWheel);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverPortLabel;
        private System.Windows.Forms.TextBox serverPortTextBox;
        private System.Windows.Forms.Button connectServerButton;
        private System.Windows.Forms.Panel canvasPanel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label serverIPLabel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label thicknessLabel;
        private System.Windows.Forms.Label redLabel;
        private System.Windows.Forms.Label greenLabel;
        private System.Windows.Forms.Label blueLabel;
        private System.Windows.Forms.Label alphaLabel;
    }
}

