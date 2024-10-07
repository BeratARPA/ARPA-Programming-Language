
# ARPA Programlama Dili

**ARPA**, **C# syntax** yapısıyla aynı olup, tamamen **Türkçe** yazılabilen bir programlama dilidir. Bu dil, **C# ve ANTLR4** teknolojileri kullanılarak geliştirilmiştir ve **ARPA Studio** adındaki **IDE** ile birlikte çalıştırılabilmektedir.

## Özellikler
- **Türkçe** yazım desteği
- **C# syntax** yapısı ile aynı
- Değişken tanımlama, atama, if/else yapıları, döngüler ve fonksiyonlar gibi temel programlama kavramlarını destekler
- **WPF Core 8.0** ile geliştirilen **ARPA Studio IDE'si**
- **AvalonEdit** bileşeni ile kod yazma ve düzenleme
- Yazılan ARPA kodlarının anında çalıştırılıp sonuçların görüntülenmesi
- ARPA dilinin derleme işlemleri **ANTLR4** kullanılarak gerçekleştirme

## Kurulum
Projeyi klonladıktan sonra, **ARPA Studio'yu** çalıştırabilmek için aşağıdaki adımları izleyin:

1. **.NET Core 8.0** yüklü olduğundan emin olun.
2. Projeyi klonlayın:
    ```bash
    git clone https://github.com/BeratARPA/ARPA-Programming-Language.git
    ```
3. Projeyi herhangi bir .NET Core destekleyen IDE ile açın ve derleyin.
4. **ARPA Studio'yu** başlatarak **ARPA** dilinde yazdığınız kodları çalıştırabilirsiniz.

## Kullanım
**ARPA** dili, **Türkçe** komutlar ve operatörler ile yazılmaktadır. İşte basit bir örnek:

```arpa
sayı x = 10;
sayı y = 20;
eğer (x < y)
{
    yazdır("x, y'den küçüktür.");
}
değilse
{
    yazdır("x, y'den büyük veya eşittir.");
}
```

### Desteklenen Veri Türleri
- `sayı`: Tam sayı değerleri için
- `ondalık`: Ondalıklı sayı değerleri için
- `metin`: String değerler için
- `mantık`: Mantıksal değerler (true/false) için

### Desteklenen Operatörler
- Aritmetik Operatörler: `+`, `-`, `*`, `/`, `%`
- Karşılaştırma Operatörleri: `==`, `!=`, `>`, `<`, `>=`, `<=`
- Mantıksal Operatörler: `ve`, `veya`

## ARPA Studio
**ARPA Studio**, **ARPA** dilinde yazılan kodların düzenlenip çalıştırılabileceği bir IDE'dir. Kod editörü olarak **AvalonEdit** bileşeni kullanılmıştır. **IDE** üzerinde yazdığınız **ARPA** kodlarını anında çalıştırabilir ve çıktı penceresinde sonuçları görebilirsiniz.

## Katkıda Bulunma
Projeye katkıda bulunmak isterseniz, lütfen pull request gönderin veya bir issue açın. Katkılarınız bizim için değerlidir!

## Lisans
Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakabilirsiniz.
