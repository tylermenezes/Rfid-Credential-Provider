using System;
using System.Windows.Forms;

using Wizard.UI;

namespace TestWizard
{
	public class Program
	{
		[STAThread]
		static void Main() 
		{
			TestWizardSheet wizard = new TestWizardSheet();

			Application.EnableVisualStyles();
			Application.DoEvents();
			Application.Run(wizard);
		}
	}
}
