namespace CodeGenerator.CSharp.VSExtension.UserControls
{
    partial class CodeGeneratorOptionsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NameSpaceGroupBox = new System.Windows.Forms.GroupBox();
            this.NameSpaceTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RedefineNameSpaceCheckBox = new System.Windows.Forms.CheckBox();
            this.IsSealedCheckBox = new System.Windows.Forms.CheckBox();
            this.InSingleFileCheckBox = new System.Windows.Forms.CheckBox();
            this.NameSpaceGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // NameSpaceGroupBox
            // 
            this.NameSpaceGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameSpaceGroupBox.Controls.Add(this.NameSpaceTextBox);
            this.NameSpaceGroupBox.Controls.Add(this.label1);
            this.NameSpaceGroupBox.Controls.Add(this.RedefineNameSpaceCheckBox);
            this.NameSpaceGroupBox.Location = new System.Drawing.Point(3, 3);
            this.NameSpaceGroupBox.Name = "NameSpaceGroupBox";
            this.NameSpaceGroupBox.Size = new System.Drawing.Size(392, 78);
            this.NameSpaceGroupBox.TabIndex = 0;
            this.NameSpaceGroupBox.TabStop = false;
            this.NameSpaceGroupBox.Text = "Name Space";
            // 
            // NameSpaceTextBox
            // 
            this.NameSpaceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameSpaceTextBox.Location = new System.Drawing.Point(81, 42);
            this.NameSpaceTextBox.Name = "NameSpaceTextBox";
            this.NameSpaceTextBox.Size = new System.Drawing.Size(305, 20);
            this.NameSpaceTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name Space";
            // 
            // RedefineNameSpaceCheckBox
            // 
            this.RedefineNameSpaceCheckBox.AutoSize = true;
            this.RedefineNameSpaceCheckBox.Location = new System.Drawing.Point(6, 19);
            this.RedefineNameSpaceCheckBox.Name = "RedefineNameSpaceCheckBox";
            this.RedefineNameSpaceCheckBox.Size = new System.Drawing.Size(134, 17);
            this.RedefineNameSpaceCheckBox.TabIndex = 0;
            this.RedefineNameSpaceCheckBox.Text = "Redefine Name Space";
            this.RedefineNameSpaceCheckBox.UseVisualStyleBackColor = true;
            // 
            // IsSealedCheckBox
            // 
            this.IsSealedCheckBox.AutoSize = true;
            this.IsSealedCheckBox.Location = new System.Drawing.Point(9, 87);
            this.IsSealedCheckBox.Name = "IsSealedCheckBox";
            this.IsSealedCheckBox.Size = new System.Drawing.Size(139, 17);
            this.IsSealedCheckBox.TabIndex = 1;
            this.IsSealedCheckBox.Text = "Make classes as sealed";
            this.IsSealedCheckBox.UseVisualStyleBackColor = true;
            // 
            // InSingleFileCheckBox
            // 
            this.InSingleFileCheckBox.AutoSize = true;
            this.InSingleFileCheckBox.Location = new System.Drawing.Point(9, 110);
            this.InSingleFileCheckBox.Name = "InSingleFileCheckBox";
            this.InSingleFileCheckBox.Size = new System.Drawing.Size(141, 17);
            this.InSingleFileCheckBox.TabIndex = 2;
            this.InSingleFileCheckBox.Text = "Put all classes in one file";
            this.InSingleFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // CodeGeneratorOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.InSingleFileCheckBox);
            this.Controls.Add(this.IsSealedCheckBox);
            this.Controls.Add(this.NameSpaceGroupBox);
            this.Name = "CodeGeneratorOptionsControl";
            this.Size = new System.Drawing.Size(398, 154);
            this.NameSpaceGroupBox.ResumeLayout(false);
            this.NameSpaceGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox NameSpaceGroupBox;
        private System.Windows.Forms.TextBox NameSpaceTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox RedefineNameSpaceCheckBox;
        private System.Windows.Forms.CheckBox IsSealedCheckBox;
        private System.Windows.Forms.CheckBox InSingleFileCheckBox;
    }
}
