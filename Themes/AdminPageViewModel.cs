using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Input;
using Customers_support_chat_bot.Core;
using Customers_support_chat_bot.MVVM.Model;
using Microsoft.Win32;


namespace Customers_support_chat_bot.Themes
{
    public class AdminPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<LogFile> _logFiles;

        public ObservableCollection<LogFile> LogFiles
        {
            get { return _logFiles; }
            set
            {
                _logFiles = value;
                OnPropertyChanged("LogFiles");
            }
        }

        public ICommand DownloadLogFileCommand { get; set; }

        public AdminPageViewModel()
        {
            var logFilePaths = Directory.GetFiles("../../../logs");
            var logFiles = new ObservableCollection<LogFile>();
            foreach (var filePath in logFilePaths)
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var dateString = fileName.Substring(3); // Remove the "log" prefix
                DateTime date;
                if (DateTime.TryParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    logFiles.Add(new LogFile { DisplayName = date.ToString("dd-MM-yyyy"), FileName = fileName });
                }
            }

            LogFiles = logFiles;
            DownloadLogFileCommand = new RelayCommand(DownloadLogFile);
        }

        private void DownloadLogFile(object logFile)
        {
            if (logFile is LogFile file)
            {
                string sourcePath = Path.GetFullPath(Path.Combine("..", "..", "..", "logs", file.FileName + ".txt"));
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = file.DisplayName,
                    DefaultExt = ".txt",
                    Filter = "Text documents (.txt)|*.txt"
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.Copy(sourcePath, saveFileDialog.FileName, true);
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}