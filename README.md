# Türkçe

# ARPA Programlama Dili

**ARPA**, **C# syntax** yapısıyla aynı olup, tamamen **Türkçe** yazılabilen bir programlama dilidir. Bu dil, **C# ve ANTLR4** teknolojileri kullanılarak geliştirilmiştir ve **ARPA Studio** adındaki **IDE** ile birlikte çalıştırılabilmektedir.

## Özellikler
- **Türkçe** yazım desteği
- **C# syntax** yapısı ile aynı
- Değişken tanımlama, atama, if/else yapıları, döngüler ve fonksiyonlar gibi temel programlama kavramlarını destekler
- **WPF Core 8.0** ile geliştirilen **ARPA Studio IDE'si**
- **Monaco Editor** bileşeni ile kod yazma ve düzenleme
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
sayi x = 10;
sayi y = 20;
eger (x < y)
{
    yazdir("x, y'den küçüktür.");
}
değilse
{
    yazdir("x, y'den büyük veya eşittir.");
}
```

### Desteklenen Veri Türleri
- `sayi`: Tam sayı değerleri için
- `ondalik`: Ondalıklı sayı değerleri için
- `metin`: String değerler için
- `mantik`: Mantıksal değerler (dogru/yanlis) için

### Desteklenen Operatörler
- Aritmetik Operatörler: `+`, `-`, `*`, `/`, `%`
- Karşılaştırma Operatörleri: `==`, `!=`, `>`, `<`, `>=`, `<=`
- Mantıksal Operatörler: `ve`, `veya`

## ARPA Studio
**ARPA Studio**, **ARPA** dilinde yazılan kodların düzenlenip çalıştırılabileceği bir IDE'dir. Kod editörü olarak **Monaco Editor** bileşeni kullanılmıştır. **IDE** üzerinde yazdığınız **ARPA** kodlarını anında çalıştırabilir ve çıktı penceresinde sonuçları görebilirsiniz.

## Katkıda Bulunma
Projeye katkıda bulunmak isterseniz, lütfen pull request gönderin veya bir issue açın. Katkılarınız bizim için değerlidir!

## Lisans
Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakabilirsiniz.

---

# English

# ARPA Programming Language

**ARPA** is a programming language that uses the same **C# syntax** but can be written entirely in **Turkish**. It has been developed using **C# and ANTLR4** technologies and can be run using the **ARPA Studio** IDE.

## Features
- Support for **Turkish** syntax
- Same structure as **C# syntax**
- Supports basic programming concepts such as variable declarations, assignments, if/else statements, loops, and functions
- **ARPA Studio IDE** developed with **WPF Core 8.0**
- Code writing and editing with the **Monaco Editor** component
- Instant execution of ARPA code with immediate result display
- Compilation of ARPA code using **ANTLR4**

## Installation
After cloning the project, follow these steps to run **ARPA Studio**:

1. Make sure **.NET Core 8.0** is installed.
2. Clone the project:
    ```bash
    git clone https://github.com/BeratARPA/ARPA-Programming-Language.git
    ```
3. Open the project with any .NET Core-supported IDE and compile it.
4. Launch **ARPA Studio** to run your code written in the **ARPA** language.

## Usage
**ARPA** language is written using **Turkish** commands and operators. Here is a simple example:

```arpa
sayi x = 10;
sayi y = 20;
eger (x < y)
{
    yazdir("x, y'den küçüktür.");
}
değilse
{
    yazdir("x, y'den büyük veya eşittir.");
}
```

### Supported Data Types
- `sayi`: For integer values
- `ondalik`: For decimal values
- `metin`: For string values
- `mantik`: For logical values (true/false)

### Supported Operators
- Arithmetic Operators: `+`, `-`, `*`, `/`, `%`
- Comparison Operators: `==`, `!=`, `>`, `<`, `>=`, `<=`
- Logical Operators: `ve`, `veya`

## ARPA Studio
**ARPA Studio** is an IDE where you can edit and run code written in the **ARPA** language. It uses the **Monaco Editor** component as the code editor. You can instantly execute your **ARPA** code and see the results in the output window.

## Contributing
If you would like to contribute to the project, please submit a pull request or open an issue. Your contributions are valuable to us!

## License
This project is licensed under the MIT License. For details, see the `LICENSE` file.
