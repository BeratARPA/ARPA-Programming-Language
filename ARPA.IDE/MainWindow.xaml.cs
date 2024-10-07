using ARPA_Programming_Language.Antlr4;
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
            string text = TextEditor.Text; // Kullanıcı kodunu al
            var interpreter = new ARPAInterpreter();

            // Kodu çalıştır
            interpreter.Execute(text);

            StringBuilder output = interpreter.Output;

            // Çıktıyı göster
            TextBoxOutput.Text = output.ToString();
        }
    }
}