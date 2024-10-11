using ARPA_Programming_Language.Antlr4;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace ARPA.IDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeMonacoEditor();
        }

        private async void InitializeMonacoEditor()
        {
            // WebView2'nin başlatılmasını bekleyin
            await MonacoEditorWebView.EnsureCoreWebView2Async(null);

            // Monaco Editor HTML dosyasını yükleyin
            var indexPath = Path.Combine(Directory.GetCurrentDirectory(), "index.html");
            MonacoEditorWebView.CoreWebView2.Navigate(indexPath);
        }

        private async void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Monaco Editor'den yazılan kodu al
                string text = await MonacoEditorWebView.ExecuteScriptAsync("window.editor.getValue();");

                // Gelen string JSON formatında olabilir, bunu düzenle:
                text = text.Trim('"').Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t");

                // Unicode kaçış karakterlerini çözümlemek için bir Regex kullan
                text = Regex.Unescape(text);

                // Kaçış karakterlerini kaldır (örn. \\u003C yerine < koy)
                text = Regex.Replace(text, @"\\u([0-9A-Fa-f]{4})", m => ((char)int.Parse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber)).ToString());

                var interpreter = new ARPAInterpreter();

                // Kodu çalıştır
                interpreter.Execute(text);

                StringBuilder output = interpreter._output;

                // Çıktıyı temizle ve ekrana yazdır
                string cleanedOutput = output.ToString().Replace("\\", "");
                TextBoxOutput.Text = cleanedOutput;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void MenuItemDocumentation_Click(object sender, RoutedEventArgs e)
        {
            DocumentationWindow documentationWindow = new DocumentationWindow();
            documentationWindow.ShowDialog();
        }

        private async void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            if (MonacoEditorWebView.CoreWebView2 != null)
            {
                // Yeni bir dosya için editörü temizler.
                await MonacoEditorWebView.ExecuteScriptAsync("window.editor.setValue('');");
            }
        }

        private async void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "ARPA Files (*.arp)|*.arp", // Sadece .arp uzantılı dosyaları açar
                DefaultExt = ".arp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);

                if (MonacoEditorWebView.CoreWebView2 != null)
                {
                    // Seçilen dosyanın içeriğini Monaco Editör'e yükle.
                    await MonacoEditorWebView.ExecuteScriptAsync($"window.editor.setValue(`{fileContent}`);");
                }
            }
        }

        private async void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            // SaveFileDialog kullanarak dosya kaydetme işlemi
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "ARPA Files (*.arp)|*.arp", // Sadece .arp uzantılı dosyaları kaydeder
                DefaultExt = ".arp"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                if (MonacoEditorWebView.CoreWebView2 != null)
                {
                    // Monaco Editör'den mevcut içeriği al
                    string editorContent = await MonacoEditorWebView.ExecuteScriptAsync("window.editor.getValue();");
                   

                    // İçeriği dosyaya kaydet
                    File.WriteAllText(filePath, editorContent);
                }
            }
        }

        private async void ThemeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //if (MonacoEditorWebView.CoreWebView2 != null && ThemeComboBox.SelectedItem is ComboBoxItem selectedItem)
            //{
            //    string themeName = selectedItem.Content.ToString();

            //    switch (themeName)
            //    {
            //        case "Light":
            //            await MonacoEditorWebView.ExecuteScriptAsync("monaco.editor.setTheme('vs');");
            //            break;
            //        case "Dark":
            //            await MonacoEditorWebView.ExecuteScriptAsync("monaco.editor.setTheme('vs-dark');");
            //            break;
            //        case "High Contrast":
            //            await MonacoEditorWebView.ExecuteScriptAsync("monaco.editor.setTheme('hc-black');");
            //            break;
            //    }
            //}
        }

        private void ThemeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeComboBox.SelectedIndex = 0;
        }
    }
}