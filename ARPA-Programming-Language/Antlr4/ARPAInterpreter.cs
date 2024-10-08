using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using ARPA_Programming_Language.Exceptions;
using System.Globalization;
using System.Text;

namespace ARPA_Programming_Language.Antlr4
{
    public class ARPAInterpreter : IARPAVisitor<object>
    {
        private Dictionary<string, Func<object[], object>> _functions = new Dictionary<string, Func<object[], object>>();
        private readonly Dictionary<string, object> _variables = new Dictionary<string, object>();

        public StringBuilder _output { get; private set; } = new StringBuilder();

        public object Visit(IParseTree tree)
        {
            if (tree == null)
            {
                Console.WriteLine("Tree node is null.");
                return null;
            }

            return tree.Accept(this);
        }

        public void Execute(string userCode)
        {
            // Kullanıcı kodunu parse et
            var inputStream = new AntlrInputStream(userCode);
            var lexer = new ARPALexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new ARPAParser(commonTokenStream);
            var context = parser.program(); // Program bağlamını al

            // Programı çalıştır
            Visit(context);
        }

        public object VisitAssignment([NotNull] ARPAParser.AssignmentContext context)
        {
            var varName = context.ID().GetText();
            var value = Visit(context.expression());
            _variables[varName] = value; // Değeri kaydet
            return value;
        }

        public object VisitBlock([NotNull] ARPAParser.BlockContext context)
        {
            object result = null;
            foreach (var statement in context.statement())
            {
                try
                {
                    result = Visit(statement);
                }
                catch (ReturnException returnEx)
                {
                    return returnEx.ReturnValue; // Fonksiyon içinde return edildiğinde değeri geri döndür
                }
            }
            return result;
        }

        public object VisitChildren(IRuleNode node)
        {
            object result = null;
            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);
                if (child != null) // null kontrolü ekleyin
                {
                    result = child.Accept(this); // Her bir alt düğümü ziyaret et
                }
            }
            return result;
        }

        public object VisitDeclaration([NotNull] ARPAParser.DeclarationContext context)
        {
            return Visit(context.variableDeclaration()); // Değişken tanımını değerlendir
        }

        public object VisitErrorNode(IErrorNode node)
        {
            // node.Payload nesnesini IToken olarak cast ediyoruz.
            var token = node.Payload as IToken;
            if (token != null)
            {
                var line = token.Line;
                var column = token.Column;
                throw new Exception($"Hata (Satır: {line}, Sütun: {column}): {node.GetText()}");
            }

            // Eğer cast başarısız olursa uygun bir hata fırlatıyoruz.
            throw new Exception($"Hata: {node.GetText()} - Geçersiz token tipi.");
        }

        public object VisitExpression([NotNull] ARPAParser.ExpressionContext context)
        {
            // Sayı ifadelerini değerlendir
            if (context.NUMBER() != null)
            {
                var numberText = context.NUMBER().GetText();
                if (double.TryParse(numberText, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                {
                    return result; // Başarılıysa sonucu döndür
                }
                else
                {
                    throw new Exception($"Invalid number format: {numberText}");
                }
            }

            // String ifadelerini değerlendir
            if (context.STRING() != null)
            {
                return context.STRING().GetText().Trim('"'); // Metni değerlendir
            }

            // Değişken ifadelerini değerlendir
            if (context.ID() != null)
            {
                var varName = context.ID().GetText();
                if (!_variables.ContainsKey(varName))
                {
                    throw new InvalidOperationException($"Değişken tanımlı değil: {varName}");
                }
                return _variables[varName]; // Değişkeni değerlendir
            }

            // Fonksiyon çağrısını değerlendir
            if (context.functionCall() != null)
            {
                try
                {
                    return Visit(context.functionCall()); // Fonksiyon çağrısını değerlendir
                }
                catch (ReturnException returnEx)
                {
                    return returnEx.ReturnValue; // Dönüş değerini al
                }
            }

            if (context.expression().Length == 1)
            {
                return Visit(context.expression(0)); // Tek bir ifade varsa, onu değerlendir
            }

            // Koşullu ifadeleri değerlendir
            if (context.ChildCount == 3 && IsComparisonOperator(context.GetChild(1).GetText()))
            {
                var left = Visit(context.expression(0));
                var right = Visit(context.expression(1));

                // Null kontrolü
                if (left == null || right == null)
                {
                    throw new InvalidOperationException("Karşılaştırma işlemleri için değerler tanımlı olmalıdır.");
                }

                string op = context.GetChild(1).GetText(); // İşlem sembolü

                // Karşılaştırma işlemleri
                // Karşılaştırma işlemleri
                bool result = left is double leftValue && right is double rightValue
                    ? op switch
                    {
                        "==" => leftValue == rightValue,
                        "!=" => leftValue != rightValue,
                        ">" => leftValue > rightValue,
                        "<" => leftValue < rightValue,
                        ">=" => leftValue >= rightValue,
                        "<=" => leftValue <= rightValue,
                        _ => throw new InvalidOperationException($"Desteklenmeyen işlem: {op}")
                    }
                    : left is string leftStr && right is string rightStr
                    ? op switch
                    {
                        "==" => leftStr.Equals(rightStr),
                        "!=" => !leftStr.Equals(rightStr),
                        _ => throw new InvalidOperationException($"String türü için desteklenmeyen işlem: {op}")
                    }
                    : throw new InvalidOperationException("Karşılaştırma işlemleri sadece sayılar veya string türleri ile yapılabilir.");

                // Koşulun sonucunu döndür
                return result; // Burada sonucu döndürdük
            }

            // Aritmetik işlemleri değerlendir
            if (context.ChildCount == 3 && IsArithmeticOperator(context.GetChild(1).GetText()))
            {
                var left = Visit(context.expression(0));
                var right = Visit(context.expression(1));

                // Null kontrolü
                if (left == null || right == null)
                {
                    throw new InvalidOperationException("Değerler tanımlı olmalıdır.");
                }

                string op = context.GetChild(1).GetText(); // İşlem sembolü

                // Aritmetik işlemleri kontrol et
                if (left is double leftValue && right is double rightValue)
                {
                    return op switch
                    {
                        "+" => leftValue + rightValue,
                        "-" => leftValue - rightValue,
                        "*" => leftValue * rightValue,
                        "/" => leftValue / rightValue,
                        _ => throw new InvalidOperationException($"Desteklenmeyen işlem: {op}")
                    };
                }
                // String birleştirme kontrolü
                else if (left is string leftStr || right is string rightStr)
                {
                    leftStr = left as string;
                    rightStr = right as string;
                    string result = (left is string ? leftStr : left.ToString()) + (right is string ? rightStr : right.ToString());
                    return result; // String birleştirme
                }
                else
                {
                    throw new InvalidOperationException("Aritmetik işlemler veya string birleştirme için geçerli değerler tanımlı olmalıdır.");
                }
            }

            // "yazdır" komutunda string ve int değerleri yan yana yazdırma
            if (context.ChildCount == 3 && context.GetChild(0).GetText() == "yazdır")
            {
                var left = Visit(context.expression(1)); // İlk ifadeyi değerlendir
                var right = Visit(context.expression(2)); // İkinci ifadeyi değerlendir

                if (left == null || right == null)
                {
                    throw new InvalidOperationException("Yazdırma işlemi için değerler tanımlı olmalıdır.");
                }

                if (left is string str && right is double num)
                {
                    // string ve int'i birleştir
                    return str + num.ToString(); // "Toplam: 12"
                }

                if (left is double numLeft && right is string strRight)
                {
                    // int ve string'i birleştir
                    return numLeft.ToString() + strRight; // "12Toplam"
                }

                throw new InvalidOperationException("Aritmetik işlemler sadece sayılar ile yapılabilir.");
            }

            throw new NotImplementedException("İfade değerlendirme için tanımlı değil.");
        }

        // Aritmetik işlem olup olmadığını kontrol eden yardımcı metod
        private bool IsArithmeticOperator(string op)
        {
            return op switch
            {
                "+" => true,
                "-" => true,
                "*" => true,
                "/" => true,
                _ => false
            };
        }

        // Karşılaştırma işlemi olup olmadığını kontrol eden yardımcı metod
        private bool IsComparisonOperator(string op)
        {
            return op switch
            {
                "==" => true,
                "!=" => true,
                ">" => true,
                "<" => true,
                ">=" => true,
                "<=" => true,
                _ => false
            };
        }

        public object VisitExpressionStatement([NotNull] ARPAParser.ExpressionStatementContext context)
        {
            return Visit(context.expression()); // İfadeyi değerlendir
        }

        public object VisitIfStatement([NotNull] ARPAParser.IfStatementContext context)
        {
            // İlk koşulu değerlendir
            var condition = (bool)Visit(context.expression(0));

            if (condition)
            {
                // Eğer koşulu doğruysa ilk bloğu değerlendir
                Visit(context.block(0));
            }
            else
            {
                // "değilseeğer" ve "değilse" durumlarını kontrol et
                bool evaluated = false; // Bir blok değerlendirildi mi?

                // "değilseeğer" bloklarını işle
                for (int i = 1; i < context.expression().Length; i++)
                {
                    var elseIfCondition = (bool)Visit(context.expression(i));
                    if (elseIfCondition)
                    {
                        Visit(context.block(i));
                        evaluated = true; // Bir blok değerlendirildi, daha fazla kontrol yapma
                        break;
                    }
                }

                // Hiçbir "değilseeğer" koşulu sağlanmazsa ve "değilse" bloğu varsa
                if (!evaluated && context.DEĞİLSE() != null)
                {
                    Visit(context.block(context.block().Length - 1));
                }
            }
            return null;
        }

        public object VisitPrintStatement([NotNull] ARPAParser.PrintStatementContext context)
        {
            var value = Visit(context.expression());
            _output.AppendLine(value.ToString());
            return null;
        }

        public object VisitProgram(ARPAParser.ProgramContext context)
        {
            // 1. Aşama: Fonksiyon Tanımlamalarını Topla
            foreach (var statement in context.statement())
            {
                if (statement.declaration() != null && statement.declaration().functionDeclaration() != null)
                {
                    Visit(statement.declaration().functionDeclaration());
                }
            }

            // 2. Aşama: Diğer İfadeleri Çalıştır
            foreach (var statement in context.statement())
            {
                // Fonksiyon tanımlamaları zaten işlendiği için onları atlıyoruz
                if (statement.declaration() != null && statement.declaration().functionDeclaration() != null)
                {
                    continue;
                }
                Visit(statement);
            }

            return null;
        }

        public object VisitStatement([NotNull] ARPAParser.StatementContext context)
        {
            return VisitChildren(context); // Alt ifadeleri değerlendir
        }

        public object VisitTerminal(ITerminalNode node)
        {
            return node.GetText(); // Terminal düğüm değerini döndür
        }

        public object VisitVariableDeclaration([NotNull] ARPAParser.VariableDeclarationContext context)
        {
            var varType = context.GetChild(0).GetText(); // Değişken türünü al
            var varName = context.ID().GetText(); // Değişken adını al

            if (context.expression() != null) // Değer ataması varsa
            {
                var value = Visit(context.expression());
                _variables[varName] = value; // Değeri kaydet
            }
            else
            {
                _variables[varName] = null; // Değeri tanımla ama boş bırak
            }
            return null;
        }

        public object VisitParamList([NotNull] ARPAParser.ParamListContext context)
        {
            var paramList = new List<string>();

            // Parametrelerin hepsini gez
            foreach (var param in context.ID())
            {
                paramList.Add(param.GetText());
            }

            return paramList;
        }

        public object VisitFunctionCall([NotNull] ARPAParser.FunctionCallContext context)
        {
            string functionName = context.ID().GetText(); // Fonksiyon ismini al
            var args = Visit(context.argList()) as List<object>; // Argüman listesini değerlendir

            // Eğer argümanlar null ise boş bir dizi döndür
            if (args == null)
            {
                args = new List<object>();
            }

            // Fonksiyonun var olup olmadığını kontrol et
            if (_functions.TryGetValue(functionName, out var function))
            {
                try
                {
                    // Fonksiyonu argümanlarla çağır
                    return function(args.ToArray());
                }
                catch (ReturnException returnEx)
                {
                    // ReturnException yakalandığında dönen değeri geri döndür
                    return returnEx.ReturnValue;
                }
            }

            throw new Exception($"Fonksiyon '{functionName}' bulunamadı."); // Hata fırlat
        }

        public object VisitArgList([NotNull] ARPAParser.ArgListContext context)
        {
            var argList = new List<object>();
            foreach (var expr in context.expression())
            {
                var argValue = Visit(expr);
                argList.Add(argValue);
            }
            return argList;
        }

        public object VisitFunctionDeclaration([NotNull] ARPAParser.FunctionDeclarationContext context)
        {
            string functionName = context.ID().GetText();
            var paramList = Visit(context.paramList()) as List<string> ?? new List<string>();
            var block = context.block();

            _functions[functionName] = (args) =>
            {
                for (int i = 0; i < paramList.Count; i++)
                {
                    _variables[paramList[i]] = args[i];
                }

                try
                {
                    return Visit(block);
                }
                catch (ReturnException returnEx)
                {
                    return returnEx.ReturnValue; // Return değeri yakalandığında döndür
                }
            };

            return null;
        }

        public object VisitReturnStatement([NotNull] ARPAParser.ReturnStatementContext context)
        {
            object returnValue = context.expression() != null ? Visit(context.expression()) : null;
            throw new ReturnException(returnValue); // Return ifadesi için özel exception fırlat
        }
    }
}
