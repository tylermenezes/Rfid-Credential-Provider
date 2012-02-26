using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Wizard.UI;

namespace TestWizard
{
    public class CompletePage : Wizard.UI.ExternalWizardPage
	{
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Label introductionLabel;
		private System.Windows.Forms.Label promptLabel;
		private System.ComponentModel.IContainer components = null;

		public CompletePage()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.titleLabel = new System.Windows.Forms.Label();
            this.introductionLabel = new System.Windows.Forms.Label();
            this.promptLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Sidebar
            // 
            this.Sidebar.Size = new System.Drawing.Size(165, 320);
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(176, 4);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(288, 28);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Done!";
            // 
            // introductionLabel
            // 
            this.introductionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.introductionLabel.Location = new System.Drawing.Point(176, 40);
            this.introductionLabel.Name = "introductionLabel";
            this.introductionLabel.Size = new System.Drawing.Size(288, 44);
            this.introductionLabel.TabIndex = 2;
            this.introductionLabel.Text = "Your credentials were modified.";
            // 
            // promptLabel
            // 
            this.promptLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.promptLabel.Location = new System.Drawing.Point(176, 296);
            this.promptLabel.Name = "promptLabel";
            this.promptLabel.Size = new System.Drawing.Size(288, 16);
            this.promptLabel.TabIndex = 3;
            this.promptLabel.Text = "Press Finish to exit.";
            // 
            // CompletePage
            // 
            this.Controls.Add(this.promptLabel);
            this.Controls.Add(this.introductionLabel);
            this.Controls.Add(this.titleLabel);
            this.Name = "CompletePage";
            this.Size = new System.Drawing.Size(465, 320);
            this.SetActive += new System.ComponentModel.CancelEventHandler(this.CompletePage_SetActive);
            this.Load += new System.EventHandler(this.CompletePage_Load);
            this.Controls.SetChildIndex(this.titleLabel, 0);
            this.Controls.SetChildIndex(this.introductionLabel, 0);
            this.Controls.SetChildIndex(this.promptLabel, 0);
            this.Controls.SetChildIndex(this.Sidebar, 0);
            this.ResumeLayout(false);

		}
		#endregion

        private void CompletePage_SetActive(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SetWizardButtons(WizardButtons.Finish);
		}

        private void CompletePage_Load(object sender, System.EventArgs e)
		{
			this.Sidebar.BackgroundImage = new Bitmap(this.GetType(), "Bitmaps.Sidebar.bmp");
		}
	}
}

