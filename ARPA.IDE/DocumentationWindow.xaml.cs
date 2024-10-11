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
                ARPA, C# tabanlı ve yalnızca Türkçe sözdizimi kullanan bir programlama dilidir. Temel veri tiplerinden kontrol yapıları, fonksiyon tanımlamalarına kadar C# ile benzerlik gösterir, ancak Türkçe anahtar kelimelerle çalışır. ARPA, kullanıcıların Türkçe ile daha doğal bir programlama deneyimi yaşamalarını sağlamak amacıyla geliştirilmiştir.
            </p>

            <h2>1. Veri Tipleri</h2>
            <p>ARPA dilinde dört temel veri tipi bulunmaktadır:</p>
            <ul>
                <li><strong>sayi</strong>: Tam sayıları ifade eder. Örnek: <code>sayi x = 10;</code></li>
                <li><strong>ondalik</strong>: Ondalıklı sayıları ifade eder. Örnek: <code>ondalik y = 3.14;</code></li>
                <li><strong>metin</strong>: Metin değerlerini ifade eder. Örnek: <code>metin ad = ""Berat"";</code></li>
                <li><strong>mantik</strong>: Mantıksal değerleri ifade eder. <code>dogru</code> ve <code>yanlis</code> değerlerini alabilir. Örnek: <code>mantik sonuc = dogru;</code></li>
            </ul>

            <h2>2. Değişken Tanımlama</h2>
            <p>Değişkenler, bir veri tipi belirlenerek tanımlanır ve isteğe bağlı olarak bir başlangıç değeri atanabilir.</p>
            <pre><code><veri tipi> <değişken adı> = <değer>;</code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>
sayi yaş = 21;
metin isim = ""Arpa"";
mantik öğrenciMi = dogru;
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
            <p>ARPA'da fonksiyonlar, veri tipi belirlenerek tanımlanır ve isteğe bağlı parametreler alabilir. Fonksiyonun dönüş tipi belirlenmelidir. Eğer dönüş değeri yoksa <code>bos</code> kullanılır.</p>
            <pre><code><veri tipi> <fonksiyon adı>(<parametre listesi>) {
    // Fonksiyon gövdesi
}
            </code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>
sayi topla(sayi a, sayi b) {
    dondur a + b;
}
            </code></pre>

            <h2>5. Koşul Yapıları</h2>
            <p>ARPA'da koşul yapıları Türkçe anahtar kelimelerle tanımlanır.</p>
            <ul>
                <li><strong>eger</strong>: Bir koşulun doğru olup olmadığını kontrol eder.</li>
                <li><strong>degilseeger</strong>: Eğer önceki <code>eger</code> bloğu başarısız olursa, ek bir koşulu kontrol eder.</li>
                <li><strong>degilse</strong>: Tüm diğer koşullar başarısız olursa çalışır.</li>
            </ul>
            <pre><code>
eger (koşul) {
    // Koşul doğruysa çalışacak kod
} degilseeger (koşul) {
    // Alternatif koşul doğruysa çalışacak kod
} degilse {
    // Hiçbir koşul doğru değilse çalışacak kod
}
            </code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>
eger (yaş > 18) {
    yazdir(""Reşit"");
} degilseeger (yaş == 18) {
    yazdir(""Yeni reşit"");
} degilse {
    yazdir(""Reşit değil"");
}
            </code></pre>

            <h2>6. Girdi/Çıktı İşlemleri</h2>
            <p>Program boyunca kullanıcıya bilgi vermek veya ekrana yazdırmak için <code>yazdir</code> komutu kullanılır.</p>
            <pre><code>yazdir(<ifade>);</code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>yazdir(""Merhaba Dünya!"");</code></pre>

            <h2>7. Fonksiyon Çağırma</h2>
            <p>Fonksiyonlar isimleri ile çağrılır ve parametreler parantez içinde verilir.</p>
            <pre><code><fonksiyon adı>(<parametreler>);</code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>
sayi sonuc = topla(5, 10);
yazdir(sonuc);
            </code></pre>

            <h2>8. Geri Dönüş Değeri</h2>
            <p>Bir fonksiyonun sonucunu döndürmek için <code>dondur</code> anahtar kelimesi kullanılır.</p>
            <pre><code>dondur <ifade>;</code></pre>
            <p><strong>Örnek:</strong></p>
            <pre><code>
sayi kare(sayi x) {
    dondur x * x;
}
            </code></pre>

            <h2>9. Döngüler</h2>
            <p>ARPA dilinde döngü yapıları, tekrarlayan işlemleri kolayca gerçekleştirmek için kullanılır. Temel döngü yapıları arasında <code>while</code> ve <code>for</code> döngüleri bulunmaktadır.</p>
            <ul>
                <li><strong>while döngüsü</strong>: Koşul doğru olduğu sürece çalışır.</li>
                <li><strong>for döngüsü</strong>: Belirli bir sayıda tekrarlama yapar.</li>
            </ul>
            <pre><code>
sayi i = 0;
while (i < 10) {
    yazdir(i);
    i = i + 1;
}

for (sayi j = 0; j < 10; j = j + 1) {
    yazdir(j);
}
            </code></pre>          

        </body>
        </html>";

            WebBrowserDocumentation.NavigateToString(htmlContent);
        }
    }
}
