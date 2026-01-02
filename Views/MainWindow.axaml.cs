using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TQM_project.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void RunEngine(object? sender, RoutedEventArgs e)
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName =
                    Path.Combine(AppContext.BaseDirectory, "engine");

                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                // Gửi input từ UI
                await process.StandardInput.WriteAsync(InputBox.Text);
                process.StandardInput.Close();

                // Nhận output
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                process.WaitForExit();

                OutputBox.Text = string.IsNullOrWhiteSpace(error)
                    ? output
                    : "ERROR:\n" + error;
            }
            catch (Exception ex)
            {
                OutputBox.Text = ex.Message;
            }
        }
    }
}
