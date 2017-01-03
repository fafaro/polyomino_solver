namespace ui
{
    partial class MainForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.boardViewer1 = new ui.BoardViewer();
            this.pieceEditor1 = new ui.PieceEditor();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 249F));
            this.tableLayoutPanel1.Controls.Add(this.boardViewer1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 464F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(667, 451);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pieceEditor1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(421, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(243, 458);
            this.panel1.TabIndex = 2;
            // 
            // boardViewer1
            // 
            this.boardViewer1.BackColor = System.Drawing.Color.MidnightBlue;
            this.boardViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boardViewer1.Location = new System.Drawing.Point(3, 3);
            this.boardViewer1.Model = null;
            this.boardViewer1.Name = "boardViewer1";
            this.boardViewer1.Size = new System.Drawing.Size(412, 458);
            this.boardViewer1.TabIndex = 1;
            // 
            // pieceEditor1
            // 
            this.pieceEditor1.BackColor = System.Drawing.Color.Gray;
            this.pieceEditor1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pieceEditor1.Location = new System.Drawing.Point(0, 0);
            this.pieceEditor1.MinimumSize = new System.Drawing.Size(200, 1200);
            this.pieceEditor1.Name = "pieceEditor1";
            this.pieceEditor1.Size = new System.Drawing.Size(226, 1200);
            this.pieceEditor1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 451);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "Polyomino Solver";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BoardViewer boardViewer1;
        private System.Windows.Forms.Panel panel1;
        private PieceEditor pieceEditor1;
    }
}

