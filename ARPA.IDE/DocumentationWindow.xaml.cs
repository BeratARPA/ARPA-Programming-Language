using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ARPA.IDE
{
    /// <summary>
    /// Interaction logic for DocumentationWindow.xaml
    /// </summary>
    public partial class DocumentationWindow : Window
    {
        public DocumentationWindow()
        {
            InitializeComponent();

            string htmlContent = @"
        <html>
        <head>
            <meta charset='UTF-8'>
            <title>ARPA Programlama Dili Dökümantasyonu</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 20px; }
                h1 { color: #2E8B57; }
                h2 { color: #4682B4; }
                ul { list-style-type: square; margin-left: 20px; }
                code { background-color: #f4f4f4; padding: 2px 4px; border-radius: 4px; }
            </style>
        </head>
        <body>
            <h1>ARPA Programlama Dili Dökümantasyonu</h1>
            <p>
                ARPA, C# tabanlı ve sadece Türkçe sözdizimi kullanan bir programlama dilidir. Temel veri tiplerinden kontrol yapılarında, fonksiyon tanımlamalarına kadar C# ile benzerlik gösterir, ancak Türkçe anahtar kelimelerle çalışır.
            </p>

            <h2>1. Veri Tipleri</h2>
            <p>ARPA dilinde dört temel veri tipi bulunmaktadır:</p>
            <ul>
                <li><strong>sayı</strong>: Tam sayıları ifade eder. Örnek: <code>sayı x = 10;</code></li>
                <li><strong>ondalık</strong>: Ondalıklı sayıları ifade eder. Örnek: <code>ondalık y = 3.14;</code></li>
                <li><strong>metin</strong>: Metin değerlerini ifade eder. Örnek: <code>metin ad = ""Berat"";</code></li>
                <li><strong>mantık</strong>: Mantıksal değerleri ifade eder. <code>doğru</code> ve <code>yanlış</code> değerlerini alabilir. Örnek: <code>mantık sonuc = doğru;</code></li>
            </ul>

            <h2>2. Değişken Tanımlama</h2>
            <p>Değişkenler, bir veri tipi belirlenerek tanımlanır ve isteğe bağlı olarak bir başlangıç değeri atanabilir.</p>
            <pre><code><veri tipi> <değişken adı> = <değer>;</code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>
sayı yaş = 21;
metin isim = ""Arpa"";
mantık öğrenciMi = doğru;
            </code></pre>

            <h2>3. Atama İşlemleri</h2>
            <p>Bir değişkene değer atamak için <code>=</code> işareti kullanılır.</p>
            <pre><code><değişken adı> = <ifade>;</code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>
yaş = 22;
isim = ""Yeni Ad"";
            </code></pre>

            <h2>4. Fonksiyonlar</h2>
            <p>ARPA'da fonksiyonlar, veri tipi belirlenerek tanımlanır ve isteğe bağlı parametreler alabilir. Fonksiyonun dönüş tipi belirlenmelidir. Eğer dönüş değeri yoksa <code>boş</code> kullanılır.</p>
            <pre><code><veri tipi> <fonksiyon adı>(<parametre listesi>) {
    // Fonksiyon gövdesi
}
            </code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>
sayı topla(sayı a, sayı b) {
    döndür a + b;
}
            </code></pre>

            <h2>5. Koşul Yapıları</h2>
            <p>ARPA'da koşul yapıları Türkçe anahtar kelimelerle tanımlanır.</p>
            <ul>
                <li><strong>eğer</strong>: Bir koşulun doğru olup olmadığını kontrol eder.</li>
                <li><strong>değilseeğer</strong>: Eğer önceki <code>eğer</code> bloğu başarısız olursa, ek bir koşulu kontrol eder.</li>
                <li><strong>değilse</strong>: Tüm diğer koşullar başarısız olursa çalışır.</li>
            </ul>
            <pre><code>
eğer (koşul) {
    // Koşul doğruysa çalışacak kod
} değilseeğer (koşul) {
    // Alternatif koşul doğruysa çalışacak kod
} değilse {
    // Hiçbir koşul doğru değilse çalışacak kod
}
            </code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>
eğer (yaş > 18) {
    yazdır(""Reşit"");
} değilseeğer (yaş == 18) {
    yazdır(""Yeni reşit"");
} değilse {
    yazdır(""Reşit değil"");
}
            </code></pre>

            <h2>6. Girdi/Çıktı İşlemleri</h2>
            <p>Program boyunca kullanıcıya bilgi vermek veya ekrana yazdırmak için <code>yazdır</code> komutu kullanılır.</p>
            <pre><code>yazdır(<ifade>);</code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>yazdır(""Merhaba Dünya!"");</code></pre>

            <h2>7. Fonksiyon Çağırma</h2>
            <p>Fonksiyonlar isimleri ile çağrılır ve parametreler parantez içinde verilir.</p>
            <pre><code><fonksiyon adı>(<parametreler>);</code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>
sayı sonuc = topla(5, 10);
yazdır(sonuc);
            </code></pre>

            <h2>8. Geri Dönüş Değeri</h2>
            <p>Bir fonksiyonun sonucunu döndürmek için <code>döndür</code> anahtar kelimesi kullanılır.</p>
            <pre><code>döndür <ifade>;</code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>
sayı kare(sayı x) {
    döndür x * x;
}
            </code></pre>

        </body>
        </html>";

            WebBrowserDocumentation.NavigateToString(htmlContent);
        }
    }
}
