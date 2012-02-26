using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Wizard.UI;

namespace TestWizard
{
	public class MiddlePage : Wizard.UI.InternalWizardPage
	{
		private System.ComponentModel.IContainer components = null;

		public MiddlePage()
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
			this.SuspendLayout();
			// 
			// Banner
			// 
			this.Banner.Name = "Banner";
			this.Banner.Size = new System.Drawing.Size(336, 64);
			this.Banner.Subtitle = "This is the middle page of the wizard.";
			this.Banner.Title = "Middle Page";
			// 
			// MiddlePage
			// 
			this.Name = "MiddlePage";
			this.Size = new System.Drawing.Size(336, 150);
			this.SetActive += new System.ComponentModel.CancelEventHandler(this.MiddlePage_SetActive);
			this.ResumeLayout(false);

		}
		#endregion

		private void MiddlePage_SetActive(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SetWizardButtons(WizardButtons.Back | WizardButtons.Next);
		}
	}
}

