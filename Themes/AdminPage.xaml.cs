using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Customers_support_chat_bot.Core;

namespace Customers_support_chat_bot.Themes;

public partial class AdminPage : Window
{
    public AdminPage()
    {
        InitializeComponent();
        DataContext = new AdminPageViewModel();
    }
    private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is TextBlock textBlock)
        {
            if (textBlock.DataContext is LogFile logFile)
            {
                if (DataContext is AdminPageViewModel viewModel)
                {
                    viewModel.DownloadLogFileCommand.Execute(logFile);
                }
            }
        }
    }
}

