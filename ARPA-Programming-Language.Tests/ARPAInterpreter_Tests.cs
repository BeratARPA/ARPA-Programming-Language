using ARPA_Programming_Language.Antlr4;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace ARPA_Programming_Language.Tests
{
    public class ARPAInterpreter_Tests
    {
        private ARPAInterpreter _interpreter;

        [SetUp]
        public void SetUp()
        {
            // ARPA yorumlayıcısını her testten önce başlatıyoruz.
            _interpreter = new ARPAInterpreter();
        }

        // Yardımcı fonksiyon: ARPA kodunu çalıştır ve beklenen çıktı ile karşılaştır
        private void ExecuteAndAssertOutput(string code, string expectedOutput)
        {
            _interpreter.Execute(code); // Kodunuzu çalıştıran metot
            string actualOutput = _interpreter._output.ToString().Trim();
            // Çıktıları normalize et
            var expectedLines = expectedOutput.Trim().Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var actualLines = actualOutput.Trim().Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            // Her bir satırı karşılaştır
            for (int i = 0; i < expectedLines.Length; i++)
            {
                string expectedLine = expectedLines[i].Trim();
                string actualLine = actualLines.Length > i ? actualLines[i].Trim() : string.Empty;

                // Beklenen ve elde edilen satırları karşılaştır
                ClassicAssert.AreEqual(expectedLine, actualLine);
            }
        }

        // Yardımcı fonksiyon: Hata fırlatılıp fırlatılmadığını kontrol et
        private void ExecuteWithException(string code, Type expectedException)
        {
            Assert.Throws(expectedException, () => _interpreter.Execute(code));
        }

        [Test]
        public void VariableDeclarationAndAssignment_ShouldWorkCorrectly()
        {
            string code = @"
    sayi x = 10;
    ondalik y = 20.5;
    metin z = ""Merhaba"";
    yazdir(x);
    yazdir(y);
    yazdir(z);
    ";
            string expectedOutput = @"
    10
    20,5
    Merhaba";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void ArithmeticOperators_ShouldWorkCorrectly()
        {
            string code = @"
    sayi x = 10 + 5;
    ondalik y = 20.0 / 2.0;
    yazdir(x);
    yazdir(y);
    ";
            string expectedOutput = @"
    15
    10";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void IfElseStatement_ShouldWorkCorrectly()
        {
            string code = @"
    sayi x = 10;
    eger (x > 5) {
        yazdir(""Büyük"");
    } degilse {
        yazdir(""Küçük"");
    }
    ";
            string expectedOutput = "Büyük";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void ComparisonOperators_ShouldWorkCorrectly()
        {
            string code = @"
    mantik result1 = 10 > 5;
    mantik result2 = 20 == 20;
    mantik result3 = 30 < 25;
    yazdir(result1);
    yazdir(result2);
    yazdir(result3);
    ";
            string expectedOutput = @"
    true
    true
    false";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void FunctionDeclarationAndCall_ShouldWorkCorrectly()
        {
            string code = @"
    sayi topla(sayi a, sayi b) {
        dondur a + b;
    }

    sayi result = topla(5, 10);
    yazdir(result);
    ";
            string expectedOutput = "15";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void StringConcatenation_ShouldWorkCorrectly()
        {
            string code = @"
    metin greeting = ""Merhaba, "";
    metin name = ""Dünya"";
    yazdir(greeting + name);
    ";
            string expectedOutput = "Merhaba, Dünya";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void UndefinedVariable_ShouldThrowException()
        {
            string code = @"
    yazdir(x);
    ";

            ExecuteWithException(code, typeof(InvalidOperationException));
        }

        [Test]
        public void ReturnStatement_ShouldWorkCorrectly()
        {
            string code = @"
    sayi topla(sayi a, sayi b) {
        dondur a + b;
    }
    sayi result = topla(10, 20);
    yazdir(result);
    ";
            string expectedOutput = "30";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void WhileLoop_ShouldWorkCorrectly()
        {
            string code = @"
    sayi x = 0;
    eger (x < 5) {
        yazdir(x);
        x = x + 1;
    }
    ";
            string expectedOutput = @"
    0
    1
    2
    3
    4";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void ForLoop_ShouldWorkCorrectly()
        {
            string code = @"
    sayi toplam = 0;
    for (sayi i = 0; i < 5; i = i + 1) {
        toplam = toplam + i;
    }
    yazdir(toplam);
    ";
            string expectedOutput = "10"; // 0 + 1 + 2 + 3 + 4 = 10

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void NestedIfStatements_ShouldWorkCorrectly()
        {
            string code = @"
    sayi x = 10;
    eger (x > 5) {
        eger (x > 8) {
            yazdir(""Büyük 8"");
        } degilse {
            yazdir(""Küçük veya eşit 8"");
        }
    } degilse {
        yazdir(""Küçük veya eşit 5"");
    }
    ";
            string expectedOutput = "Büyük 8";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void FunctionWithNoReturn_ShouldWorkCorrectly()
        {
            string code = @"
    sayi printSum(sayi a, sayi b) {
        yazdir(a + b);
    }

    printSum(3, 7);
    ";
            string expectedOutput = "10";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void FunctionWithMultipleParameters_ShouldWorkCorrectly()
        {
            string code = @"
    sayi multiply(sayi a, sayi b) {
        dondur a * b;
    }

    sayi result = multiply(4, 5);
    yazdir(result);
    ";
            string expectedOutput = "20";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void MultipleVariableDeclarations_ShouldWorkCorrectly()
        {
            string code = @"
    sayi a = 1, b = 2, c = 3;
    yazdir(a + b + c);
    ";
            string expectedOutput = "6";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void LogicalOperators_ShouldWorkCorrectly()
        {
            string code = @"
    mantik result1 = true ve false;
    mantik result2 = true veya false;
    yazdir(result1);
    yazdir(result2);
    ";
            string expectedOutput = @"
    false
    true";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void EmptyBlock_ShouldNotThrowError()
        {
            string code = @"
    sayi x = 10;
    {
        // empty block
    }
    yazdir(x);
    ";
            string expectedOutput = "10";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

        [Test]
        public void ExceptionOnInvalidFunctionCall_ShouldThrowException()
        {
            string code = @"
    sayi result = invalidFunction(1, 2);
    ";

            ExecuteWithException(code, typeof(InvalidOperationException));
        }

        [Test]
        public void MultipleReturnStatements_ShouldReturnFirstValidOne()
        {
            string code = @"
    sayi test() {
        dondur 10;
        dondur 20;
    }

    sayi result = test();
    yazdir(result);
    ";
            string expectedOutput = "10";

            ExecuteAndAssertOutput(code, expectedOutput);
        }

    }
}
