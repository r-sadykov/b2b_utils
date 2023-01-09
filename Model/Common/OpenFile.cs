using Microsoft.Win32;

namespace B2B_Utils.Model
{
    internal static class OpenFile
    {
        public static OpenFileDialog GetFile()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = ".log",
                Filter =
                    "Log Files (*.log)|*.log|Logs Files (*.logs)|*.logs|Excel 2013-2016 Files (*.xlsx)|*.xlsx|Excel 2005-2010 Files (*.xls)|*.xls"
            };

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = ofd.ShowDialog();

            // Get the selected file name and display in a TextBox 
            return ofd;
        }
    }
}
