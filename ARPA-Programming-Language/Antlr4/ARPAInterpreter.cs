using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Text;

namespace ARPA_Programming_Language.Antlr4
{
    public class ARPAInterpreter : IARPAVisitor<object>
    {
        private readonly Dictionary<string, object> _variables = new Dictionary<string, object>();
        public StringBuilder Output { get; private set; } = new StringBuilder();

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
            foreach (var statement in context.statement())
            {
                Visit(statement); // Her bir ifadeyi değerlendir
            }
            return null;
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
            throw new Exception($"Hata: {node.GetText()}");
        }

        public object VisitExpression([NotNull] ARPAParser.ExpressionContext context)
        {
            // Sayı ifadelerini değerlendir
            if (context.NUMBER() != null)
            {
                return double.Parse(context.NUMBER().GetText()); // Sayıyı değerlendir
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
                if (left is double leftValue && right is double rightValue)
                {
                    return op switch
                    {
                        "==" => leftValue == rightValue,
                        "!=" => leftValue != rightValue,
                        ">" => leftValue > rightValue,
                        "<" => leftValue < rightValue,
                        ">=" => leftValue >= rightValue,
                        "<=" => leftValue <= rightValue,
                        _ => throw new InvalidOperationException($"Desteklenmeyen işlem: {op}")
                    };
                }
                else
                {
                    throw new InvalidOperationException("Karşılaştırma işlemleri sadece sayılar ile yapılabilir.");
                }
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

        public object VisitFunctionDeclaration([NotNull] ARPAParser.FunctionDeclarationContext context)
        {
            throw new NotImplementedException("Fonksiyon tanımı henüz desteklenmiyor.");
        }

        public object VisitIfStatement([NotNull] ARPAParser.IfStatementContext context)
        {
            var condition = (bool)Visit(context.expression(0)); // İlk koşulu değerlendir
            if (condition)
            {
                Visit(context.block(0)); // Eğer koşulu doğruysa ilk bloğu değerlendir
            }
            else if (context.DEĞİLSEEĞER() != null) // Eğer "değilse eğer" varsa
            {
                var elseIfCondition = (bool)Visit(context.expression(1)); // Alternatif koşulu değerlendir
                if (elseIfCondition)
                {
                    Visit(context.block(1)); // Eğer alternatif koşul doğruysa, ilgili bloğu değerlendir
                }
                else if (context.DEĞİLSE() != null) // Eğer "değilse" varsa
                {
                    Visit(context.block(2)); // Hiçbiri sağlanmazsa son bloğu değerlendir
                }
            }
            else if (context.DEĞİLSE() != null) // Eğer "değilse" varsa
            {
                Visit(context.block(1)); // Son bloğu değerlendir
            }
            return null;
        }

        public object VisitPrintStatement([NotNull] ARPAParser.PrintStatementContext context)
        {
            var value = Visit(context.expression());
            Output.AppendLine(value.ToString());
            return null;
        }

        public object VisitProgram([NotNull] ARPAParser.ProgramContext context)
        {
            foreach (var statement in context.statement())
            {
                Visit(statement); // Programdaki her bir ifadeyi değerlendir
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
    }
}
