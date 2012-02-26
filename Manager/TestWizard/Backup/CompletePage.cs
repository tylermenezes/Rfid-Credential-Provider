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
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.IContainer components = null;

		public CompletePage()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
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
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Sidebar
			// 
			this.Sidebar.Name = "Sidebar";
			this.Sidebar.Size = new System.Drawing.Size(165, 320);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 64);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "CompletePage";
			// 
			// CompletePage
			// 
			this.Controls.Add(this.label1);
			this.Name = "CompletePage";
			this.Size = new System.Drawing.Size(424, 320);
			this.Load += new System.EventHandler(this.CompletePage_Load);
			this.SetActive += new System.ComponentModel.CancelEventHandler(this.CompletePage_SetActive);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.Sidebar, 0);
			this.ResumeLayout(false);

		}
		#endregion

		private void CompletePage_SetActive(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SetWizardButtons(WizardButtons.Back | WizardButtons.Finish);
		}

		private void CompletePage_Load(object sender, System.EventArgs e)
		{
			this.Sidebar.BackgroundImage = new Bitmap(this.GetType(), "Bitmaps.Sidebar.bmp");
		}
	}
}

