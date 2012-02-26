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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiddlePage));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Banner
            // 
            this.Banner.Size = new System.Drawing.Size(465, 64);
            this.Banner.Subtitle = "Scan your credential to register or update it.";
            this.Banner.Title = "Scan A Credential";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(59, 70);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(368, 236);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // MiddlePage
            // 
            this.Controls.Add(this.pictureBox1);
            this.Name = "MiddlePage";
            this.Size = new System.Drawing.Size(465, 320);
            this.SetActive += new System.ComponentModel.CancelEventHandler(this.MiddlePage_SetActive);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.Banner, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        private PictureBox pictureBox1;

        RFID r;

		private void MiddlePage_SetActive(object sender, System.ComponentModel.CancelEventArgs e)
		{
            SetWizardButtons(WizardButtons.Back);
            string port = (string)RegistryAccess.Primary.GetValue("Port");
            r = new RFID(port);
            r.IncomingRfid += new RfidRead(r_IncomingRfid);
            this.Disposed += new EventHandler(MiddlePage_Disposed);
            this.WizardBack += new WizardPageEventHandler(MiddlePage_WizardBack);
            this.WizardFinish += new CancelEventHandler(MiddlePage_WizardFinish);
            this.WizardNext += new WizardPageEventHandler(MiddlePage_WizardNext);
		}

        void MiddlePage_WizardNext(object sender, WizardPageEventArgs e)
        {
            r.Close();
        }

        void MiddlePage_WizardFinish(object sender, CancelEventArgs e)
        {
            r.Close(); 
        }

        void MiddlePage_WizardBack(object sender, WizardPageEventArgs e)
        {
            r.Close();
        }

        void MiddlePage_Disposed(object sender, EventArgs e)
        {
            r.Close();
        }


        void r_IncomingRfid(string id)
        {
            this.Invoke(new SetActivePageCallback(delegate() { r.Close(); this.GetWizard().Pages.Add(new EditPage(id)); this.GetWizard().SetActivePage(this.GetWizard().Pages.Count - 1); }));
        }
	}

    public delegate void SetActivePageCallback();
}

