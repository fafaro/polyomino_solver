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
            this.solveButton = new System.Windows.Forms.Button();
            this.boardViewer1 = new ui.BoardViewer();
            this.pieceBuilder1 = new ui.PieceBuilder();
            this.piecesViewer1 = new ui.PiecesViewer();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Controls.Add(this.boardViewer1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.solveButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pieceBuilder1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.piecesViewer1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(702, 464);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // solveButton
            // 
            this.solveButton.BackColor = System.Drawing.Color.YellowGreen;
            this.solveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solveButton.Location = new System.Drawing.Point(505, 3);
            this.solveButton.Name = "solveButton";
            this.solveButton.Size = new System.Drawing.Size(194, 34);
            this.solveButton.TabIndex = 1;
            this.solveButton.Text = "Solve";
            this.solveButton.UseVisualStyleBackColor = false;
            // 
            // boardViewer1
            // 
            this.boardViewer1.BackColor = System.Drawing.Color.Blue;
            this.boardViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boardViewer1.Location = new System.Drawing.Point(3, 3);
            this.boardViewer1.Name = "boardViewer1";
            this.tableLayoutPanel1.SetRowSpan(this.boardViewer1, 3);
            this.boardViewer1.Size = new System.Drawing.Size(496, 458);
            this.boardViewer1.TabIndex = 0;
            // 
            // pieceBuilder1
            // 
            this.pieceBuilder1.BackColor = System.Drawing.Color.Goldenrod;
            this.pieceBuilder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pieceBuilder1.Location = new System.Drawing.Point(505, 217);
            this.pieceBuilder1.Name = "pieceBuilder1";
            this.pieceBuilder1.Size = new System.Drawing.Size(194, 244);
            this.pieceBuilder1.TabIndex = 2;
            // 
            // piecesViewer1
            // 
            this.piecesViewer1.BackColor = System.Drawing.Color.SteelBlue;
            this.piecesViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.piecesViewer1.Location = new System.Drawing.Point(505, 43);
            this.piecesViewer1.Name = "piecesViewer1";
            this.piecesViewer1.Size = new System.Drawing.Size(194, 168);
            this.piecesViewer1.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 464);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "Polyomino Solver";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BoardViewer boardViewer1;
        private System.Windows.Forms.Button solveButton;
        private PieceBuilder pieceBuilder1;
        private PiecesViewer piecesViewer1;
    }
}

