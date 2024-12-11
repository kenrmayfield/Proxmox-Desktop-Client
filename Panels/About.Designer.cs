using System.ComponentModel;

namespace Proxmox_Desktop_Client.Panels;

partial class About
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
        this.pictureBox_github = new System.Windows.Forms.PictureBox();
        this.label_programName = new System.Windows.Forms.Label();
        this.label_author = new System.Windows.Forms.Label();
        this.label_current = new System.Windows.Forms.Label();
        this.label_release = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_github)).BeginInit();
        this.SuspendLayout();
        // 
        // pictureBox_github
        // 
        this.pictureBox_github.ErrorImage = null;
        this.pictureBox_github.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_github.Image")));
        this.pictureBox_github.InitialImage = null;
        this.pictureBox_github.Location = new System.Drawing.Point(12, 9);
        this.pictureBox_github.Name = "pictureBox_github";
        this.pictureBox_github.Size = new System.Drawing.Size(100, 100);
        this.pictureBox_github.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pictureBox_github.TabIndex = 0;
        this.pictureBox_github.TabStop = false;
        this.pictureBox_github.Click += new System.EventHandler(this.PictureBox_Click);
        // 
        // label_programName
        // 
        this.label_programName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_programName.Location = new System.Drawing.Point(118, 9);
        this.label_programName.Name = "label_programName";
        this.label_programName.Size = new System.Drawing.Size(254, 23);
        this.label_programName.TabIndex = 1;
        this.label_programName.Text = "Proxmox Desktop Client";
        this.label_programName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // label_author
        // 
        this.label_author.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label_author.Location = new System.Drawing.Point(118, 32);
        this.label_author.Name = "label_author";
        this.label_author.Size = new System.Drawing.Size(254, 23);
        this.label_author.TabIndex = 2;
        this.label_author.Text = "Matthew Bate";
        this.label_author.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // label_current
        // 
        this.label_current.Location = new System.Drawing.Point(118, 55);
        this.label_current.Name = "label_current";
        this.label_current.Size = new System.Drawing.Size(254, 23);
        this.label_current.TabIndex = 3;
        this.label_current.Text = "Version:";
        this.label_current.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // label_release
        // 
        this.label_release.Location = new System.Drawing.Point(118, 78);
        this.label_release.Name = "label_release";
        this.label_release.Size = new System.Drawing.Size(254, 23);
        this.label_release.TabIndex = 4;
        this.label_release.Text = "Latest:";
        this.label_release.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // About
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(384, 121);
        this.Controls.Add(this.label_release);
        this.Controls.Add(this.label_current);
        this.Controls.Add(this.label_author);
        this.Controls.Add(this.label_programName);
        this.Controls.Add(this.pictureBox_github);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "About";
        this.Text = "About";
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox_github)).EndInit();
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Label label_release;

    private System.Windows.Forms.Label label_current;

    private System.Windows.Forms.Label label_author;

    private System.Windows.Forms.Label label_programName;

    private System.Windows.Forms.PictureBox pictureBox_github;

    #endregion
}