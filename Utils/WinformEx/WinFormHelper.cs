using System.Windows.Forms;

namespace Utils.WinformEx
{
   public class WinFormHelper
    {
        #region MessageBox
        public static void ShowWarning(string warningTxt)
        {
            MessageBox.Show(warningTxt, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError(string errorTxt)
        {
            MessageBox.Show(errorTxt, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarning(object ex)
        {
            MessageBox.Show(ex.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError(object ex)
        {
            MessageBox.Show(ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        } 
        #endregion
    }
}
