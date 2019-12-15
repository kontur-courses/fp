using System;
using System.Windows.Forms;
using TagsCloud.Interfaces;

namespace TagsCloud
{
	public class GUIExceptionsHandler: IExceptionHandler
	{
		public void Handle(Exception exception)
		{
			var caption = exception.TargetSite?.Name;
			MessageBox.Show(exception.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}