using ARPA_Programming_Language.Antlr4;
using System.IO;
using System.Text;
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
        }

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string text = TextEditor.Text; // Kullanıcı kodunu al
                var interpreter = new ARPAInterpreter();

                // Kodu çalıştır
                interpreter.Execute(text);

                StringBuilder output = interpreter._output;

                // Çıktıyı göster
                TextBoxOutput.Text = output.ToString();
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

        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            TextEditor.Clear();
            TextBoxOutput.Text = string.Empty;
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "ARPA Files (*.arp)|*.arp", // Sadece .arp uzantılı dosyaları açar
                DefaultExt = ".arp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Seçilen dosyanın içeriğini yükleyelim
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);
                TextEditor.Text = fileContent;
            }
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            // SaveFileDialog kullanarak dosya kaydetme işlemi
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "ARPA Files (*.arp)|*.arp", // Sadece .arp uzantılı dosyaları kaydeder
                DefaultExt = ".arp"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                // Metin düzenleyici içeriğini seçilen dosyaya kaydedelim
                string filePath = saveFileDialog.FileName;
                File.WriteAllText(filePath, TextEditor.Text);
            }
        }
    }
}