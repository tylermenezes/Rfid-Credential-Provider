using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Wizard.UI
{
	[DefaultEvent("SetActive")]
	public class WizardPage : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.Container components = null;

		public WizardPage()
		{
			InitializeComponent();
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		protected WizardSheet GetWizard()
		{
			WizardSheet wizard = (WizardSheet)this.ParentForm;
			return wizard;
		}

		protected void SetWizardButtons(WizardButtons buttons)
		{
			GetWizard().SetWizardButtons(buttons);
		}

		protected void EnableCancelButton(bool enableCancelButton)
		{
			GetWizard().EnableCancelButton(enableCancelButton);
		}

		protected void PressButton(WizardButtons buttons)
		{
			GetWizard().PressButton(buttons);
		}

		[Category("Wizard")]
		public event CancelEventHandler SetActive;

		public virtual void OnSetActive(CancelEventArgs e)
		{
			if (SetActive != null)
				SetActive(this, e);
		}

		[Category("Wizard")]
		public event WizardPageEventHandler WizardNext;

		public virtual void OnWizardNext(WizardPageEventArgs e)
		{
			if (WizardNext != null)
				WizardNext(this, e);
		}

		[Category("Wizard")]
		public event WizardPageEventHandler WizardBack;

		public virtual void OnWizardBack(WizardPageEventArgs e)
		{
			if (WizardBack != null)
				WizardBack(this, e);
		}

		[Category("Wizard")]
		public event CancelEventHandler WizardFinish;

		public virtual void OnWizardFinish(CancelEventArgs e)
		{
			if (WizardFinish != null)
				WizardFinish(this, e);
		}

		[Category("Wizard")]
		public event CancelEventHandler QueryCancel;

		public virtual void OnQueryCancel(CancelEventArgs e)
		{
			if (QueryCancel != null)
				QueryCancel(this, e);
		}
	}
}
