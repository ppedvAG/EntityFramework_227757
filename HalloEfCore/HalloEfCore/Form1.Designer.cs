namespace HalloEfCore
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
            flowLayoutPanel1 = new FlowLayoutPanel();
            button1 = new Button();
            button5 = new Button();
            button7 = new Button();
            button2 = new Button();
            button8 = new Button();
            button3 = new Button();
            button4 = new Button();
            button6 = new Button();
            button9 = new Button();
            button10 = new Button();
            dataGridView1 = new DataGridView();
            flowLayoutPanel2 = new FlowLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            textBox1 = new TextBox();
            button11 = new Button();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.BackColor = Color.LightSalmon;
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(button5);
            flowLayoutPanel1.Controls.Add(button7);
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button8);
            flowLayoutPanel1.Controls.Add(button3);
            flowLayoutPanel1.Controls.Add(button4);
            flowLayoutPanel1.Controls.Add(button6);
            flowLayoutPanel1.Controls.Add(button9);
            flowLayoutPanel1.Controls.Add(button10);
            flowLayoutPanel1.Controls.Add(groupBox1);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1070, 188);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // button1
            // 
            button1.AutoSize = true;
            button1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(196, 42);
            button1.TabIndex = 0;
            button1.Text = "&Kill && Create DB";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button5
            // 
            button5.AutoSize = true;
            button5.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button5.Location = new Point(205, 3);
            button5.Name = "button5";
            button5.Size = new Size(105, 42);
            button5.TabIndex = 4;
            button5.Text = "Save all";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button7
            // 
            button7.AutoSize = true;
            button7.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button7.Location = new Point(316, 3);
            button7.Name = "button7";
            button7.Size = new Size(204, 42);
            button7.TabIndex = 6;
            button7.Text = "Load Employees ";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button2
            // 
            button2.AutoSize = true;
            button2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button2.Location = new Point(526, 3);
            button2.Name = "button2";
            button2.Size = new Size(369, 42);
            button2.TabIndex = 1;
            button2.Text = "Load Employees (Eager Loading)";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button8
            // 
            button8.AutoSize = true;
            button8.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button8.Location = new Point(3, 51);
            button8.Name = "button8";
            button8.Size = new Size(494, 42);
            button8.TabIndex = 7;
            button8.Text = "Load Employees (Eager Loading) SPlit Query";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button3
            // 
            button3.AutoSize = true;
            button3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button3.Location = new Point(503, 51);
            button3.Name = "button3";
            button3.Size = new Size(211, 42);
            button3.TabIndex = 2;
            button3.Text = "Query Employees";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.AutoSize = true;
            button4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button4.Location = new Point(720, 51);
            button4.Name = "button4";
            button4.Size = new Size(269, 42);
            button4.TabIndex = 3;
            button4.Text = "Den besten Employees";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button6
            // 
            button6.AutoSize = true;
            button6.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button6.Location = new Point(3, 99);
            button6.Name = "button6";
            button6.Size = new Size(178, 42);
            button6.TabIndex = 5;
            button6.Text = "Attach manual";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button9
            // 
            button9.AutoSize = true;
            button9.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button9.Location = new Point(187, 99);
            button9.Name = "button9";
            button9.Size = new Size(163, 42);
            button9.TabIndex = 8;
            button9.Text = "Query paged";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.AutoSize = true;
            button10.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button10.Location = new Point(356, 99);
            button10.Name = "button10";
            button10.Size = new Size(167, 42);
            button10.TabIndex = 9;
            button10.Text = "EXECUTE SQL";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 188);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 82;
            dataGridView1.RowTemplate.Height = 41;
            dataGridView1.Size = new Size(1070, 408);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel2.BackColor = Color.PaleTurquoise;
            flowLayoutPanel2.Dock = DockStyle.Top;
            flowLayoutPanel2.Location = new Point(0, 188);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(1070, 0);
            flowLayoutPanel2.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.AutoSize = true;
            groupBox1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Location = new Point(529, 99);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(307, 86);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Levenshtein ";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(textBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(button11, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 35);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(301, 48);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(3, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(200, 39);
            textBox1.TabIndex = 0;
            // 
            // button11
            // 
            button11.AutoSize = true;
            button11.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button11.Location = new Point(209, 3);
            button11.Name = "button11";
            button11.Size = new Size(89, 42);
            button11.TabIndex = 1;
            button11.Text = "Suche";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1070, 596);
            Controls.Add(dataGridView1);
            Controls.Add(flowLayoutPanel2);
            Controls.Add(flowLayoutPanel1);
            Name = "Form1";
            Text = "Form1";
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Button button1;
        private DataGridView dataGridView1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button button10;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox textBox1;
        private Button button11;
    }
}