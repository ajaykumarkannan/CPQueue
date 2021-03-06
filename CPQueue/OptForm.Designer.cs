﻿namespace CPQueue
{
    partial class OptForm
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
            this.stackBox = new System.Windows.Forms.CheckBox();
            this.splitCopyBox = new System.Windows.Forms.CheckBox();
            this.showOptionsBox = new System.Windows.Forms.CheckBox();
            this.spaceBox = new System.Windows.Forms.CheckBox();
            this.newlineBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.miniBox = new System.Windows.Forms.CheckBox();
            this.staytopbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // stackBox
            // 
            this.stackBox.AutoSize = true;
            this.stackBox.Location = new System.Drawing.Point(12, 12);
            this.stackBox.Name = "stackBox";
            this.stackBox.Size = new System.Drawing.Size(160, 17);
            this.stackBox.TabIndex = 0;
            this.stackBox.Text = "Use Stack instead of Queue";
            this.stackBox.UseVisualStyleBackColor = true;
            // 
            // splitCopyBox
            // 
            this.splitCopyBox.AutoSize = true;
            this.splitCopyBox.Location = new System.Drawing.Point(12, 35);
            this.splitCopyBox.Name = "splitCopyBox";
            this.splitCopyBox.Size = new System.Drawing.Size(73, 17);
            this.splitCopyBox.TabIndex = 2;
            this.splitCopyBox.Text = "Split Copy";
            this.splitCopyBox.UseVisualStyleBackColor = true;
            // 
            // showOptionsBox
            // 
            this.showOptionsBox.AutoSize = true;
            this.showOptionsBox.Location = new System.Drawing.Point(12, 58);
            this.showOptionsBox.Name = "showOptionsBox";
            this.showOptionsBox.Size = new System.Drawing.Size(170, 17);
            this.showOptionsBox.TabIndex = 3;
            this.showOptionsBox.Text = "Show Options on Main Screen";
            this.showOptionsBox.UseVisualStyleBackColor = true;
            // 
            // spaceBox
            // 
            this.spaceBox.AutoSize = true;
            this.spaceBox.Location = new System.Drawing.Point(12, 81);
            this.spaceBox.Name = "spaceBox";
            this.spaceBox.Size = new System.Drawing.Size(106, 17);
            this.spaceBox.TabIndex = 4;
            this.spaceBox.Text = "Space Separator";
            this.spaceBox.UseVisualStyleBackColor = true;
            this.spaceBox.CheckedChanged += new System.EventHandler(this.spaceBox_CheckedChanged);
            // 
            // newlineBox
            // 
            this.newlineBox.AutoSize = true;
            this.newlineBox.Location = new System.Drawing.Point(150, 81);
            this.newlineBox.Name = "newlineBox";
            this.newlineBox.Size = new System.Drawing.Size(113, 17);
            this.newlineBox.TabIndex = 5;
            this.newlineBox.Text = "Newline Separator";
            this.newlineBox.UseVisualStyleBackColor = true;
            this.newlineBox.CheckedChanged += new System.EventHandler(this.newlineBox_CheckedChanged);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(43, 127);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(124, 127);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // miniBox
            // 
            this.miniBox.AutoSize = true;
            this.miniBox.Location = new System.Drawing.Point(119, 35);
            this.miniBox.Name = "miniBox";
            this.miniBox.Size = new System.Drawing.Size(75, 17);
            this.miniBox.TabIndex = 8;
            this.miniBox.Text = "Mini Mode";
            this.miniBox.UseVisualStyleBackColor = true;
            // 
            // staytopbox
            // 
            this.staytopbox.AutoSize = true;
            this.staytopbox.Location = new System.Drawing.Point(12, 104);
            this.staytopbox.Name = "staytopbox";
            this.staytopbox.Size = new System.Drawing.Size(84, 17);
            this.staytopbox.TabIndex = 9;
            this.staytopbox.Text = "Stay on Top";
            this.staytopbox.UseVisualStyleBackColor = true;
            // 
            // OptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 161);
            this.Controls.Add(this.staytopbox);
            this.Controls.Add(this.miniBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.newlineBox);
            this.Controls.Add(this.spaceBox);
            this.Controls.Add(this.showOptionsBox);
            this.Controls.Add(this.splitCopyBox);
            this.Controls.Add(this.stackBox);
            this.Name = "OptForm";
            this.Text = "Options";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox stackBox;
        private System.Windows.Forms.CheckBox splitCopyBox;
        private System.Windows.Forms.CheckBox showOptionsBox;
        private System.Windows.Forms.CheckBox spaceBox;
        private System.Windows.Forms.CheckBox newlineBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox miniBox;
        private System.Windows.Forms.CheckBox staytopbox;
    }
}