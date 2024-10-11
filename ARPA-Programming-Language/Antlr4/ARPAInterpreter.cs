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
        private readonly Dictionary<string, Func<object[], object>> _functions = new();
        private readonly Dictionary<string, object> _variables = new();
        public List<string> _errors;
        public StringBuilder _output = new();

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
            var context = ParseUserCode(userCode);
            _errors = new ARPAErrorListener().Errors;
            Visit(context);
        }

        private IParseTree ParseUserCode(string userCode)
        {
            var inputStream = new AntlrInputStream(userCode);
            var lexer = new ARPALexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ARPAParser(tokenStream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ARPAErrorListener());

            return parser.program();
        }

        public object VisitAssignment([NotNull] ARPAParser.AssignmentContext context)
        {
            var varName = context.ID().GetText();
            var value = Visit(context.expression());
            _variables[varName] = value; // Store the value
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
            var token = node.Payload as IToken;
            if (token != null)
            {
                var line = token.Line;
                var column = token.Column;
                throw new Exception($"Error (Line: {line}, Column: {column}): {node.GetText()}");
            }

            throw new Exception($"Error: {node.GetText()} - Invalid token type.");
        }

        public object VisitExpression([NotNull] ARPAParser.ExpressionContext context)
        {
            // Switch case context'in çeşitli child'larını kontrol eder.
            return context switch
            {
                _ when context.TRUE() != null => true,
                _ when context.FALSE() != null => false,
                _ when context.NUMBER() != null => ParseNumber(context.NUMBER().GetText()),
                _ when context.STRING() != null => context.STRING().GetText().Trim('"'),
                _ when context.ID() != null => GetVariableValue(context.ID().GetText()),
                _ when context.functionCall() != null => VisitFunctionCall(context.functionCall()),
                _ => HandleComplexExpression(context)
            };
        }

        private object ParseNumber(string numberText)
        {
            if (double.TryParse(numberText, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result; // Return the parsed number
            }
            throw new Exception($"Invalid number format: {numberText}");
        }

        private object GetVariableValue(string varName)
        {
            if (!_variables.TryGetValue(varName, out var value))
            {
                throw new InvalidOperationException($"Variable not defined: {varName}");
            }
            return value; // Return variable value
        }

        private object HandleComplexExpression(ARPAParser.ExpressionContext context)
        {
            if (context.ChildCount == 3)
            {
                var operatorSymbol = context.GetChild(1).GetText();
                if (IsComparisonOperator(operatorSymbol))
                {
                    return EvaluateComparison(context);
                }

                if (IsArithmeticOperator(operatorSymbol))
                {
                    return EvaluateArithmetic(context);
                }

                if (IsLogicalOperator(operatorSymbol))
                {
                    var left = Visit(context.expression(0));
                    var right = Visit(context.expression(1));
                    return EvaluateLogical(left, right, operatorSymbol);
                }

                if (context.GetChild(0).GetText() == "yazdır")
                {
                    return HandlePrintStatement(context);
                }
            }

            throw new NotImplementedException("Expression evaluation not defined.");
        }

        // Mantıksal operatör olup olmadığını kontrol eden metod
        private bool IsLogicalOperator(string op) => op switch
        {
            "ve" => true,
            "veya" => true,
            _ => false
        };

        // Mantıksal operatörlerin değerlendirilmesi
        private bool EvaluateLogical(object left, object right, string op)
        {
            if (left is bool leftBool && right is bool rightBool)
            {
                return op switch
                {
                    "ve" => leftBool && rightBool,
                    "veya" => leftBool || rightBool,
                    _ => throw new InvalidOperationException($"Unsupported logical operation: {op}")
                };
            }

            throw new InvalidOperationException("Logical operations require boolean values.");
        }

        private object EvaluateComparison(ARPAParser.ExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));

            if (left == null || right == null)
            {
                throw new InvalidOperationException("Values must be defined for comparison operations.");
            }

            return (left, right) switch
            {
                (double lValue, double rValue) => EvaluateDoubleComparison(lValue, rValue, context.GetChild(1).GetText()),
                (string lStr, string rStr) => EvaluateStringComparison(lStr, rStr, context.GetChild(1).GetText()),
                _ => throw new InvalidOperationException("Comparison operations can only be performed on numbers or strings.")
            };
        }

        private bool EvaluateDoubleComparison(double left, double right, string op) => op switch
        {
            "==" => left == right,
            "!=" => left != right,
            ">" => left > right,
            "<" => left < right,
            ">=" => left >= right,
            "<=" => left <= right,
            _ => throw new InvalidOperationException($"Unsupported operation: {op}")
        };

        private bool EvaluateStringComparison(string left, string right, string op) => op switch
        {
            "==" => left.Equals(right),
            "!=" => !left.Equals(right),
            _ => throw new InvalidOperationException($"Unsupported operation for strings: {op}")
        };

        private object EvaluateArithmetic(ARPAParser.ExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));

            if (left == null || right == null)
            {
                throw new InvalidOperationException("Values must be defined for arithmetic operations.");
            }

            if (left is double leftValue && right is double rightValue)
            {
                return PerformArithmeticOperation(leftValue, rightValue, context.GetChild(1).GetText());
            }

            if (left is string leftStr || right is string rightStr)
            {
                leftStr = left as string ?? left.ToString();
                rightStr = right as string ?? right.ToString();
                return leftStr + rightStr; // String concatenation
            }

            throw new InvalidOperationException("Arithmetic operations require valid values.");
        }

        private double PerformArithmeticOperation(double left, double right, string op) => op switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            _ => throw new InvalidOperationException($"Unsupported operation: {op}")
        };

        private object HandlePrintStatement(ARPAParser.ExpressionContext context)
        {
            var left = Visit(context.expression(1));
            var right = Visit(context.expression(2));

            if (left == null || right == null)
            {
                throw new InvalidOperationException("Values must be defined for print operations.");
            }

            return left.ToString() + right.ToString(); // Print concatenation
        }

        // Aritmetik işlem olup olmadığını kontrol eden yardımcı metod
        private bool IsArithmeticOperator(string op) => op switch
        {
            "+" => true,
            "-" => true,
            "*" => true,
            "/" => true,
            _ => false
        };

        // Karşılaştırma işlemi olup olmadığını kontrol eden yardımcı metod
        private bool IsComparisonOperator(string op) => op switch
        {
            "==" => true,
            "!=" => true,
            ">" => true,
            "<" => true,
            ">=" => true,
            "<=" => true,
            _ => false
        };

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
                if (!evaluated && context.DEGILSE() != null)
                {
                    Visit(context.block(context.block().Length - 1));
                }
            }
            return null;
        }

        public object VisitPrintStatement([NotNull] ARPAParser.PrintStatementContext context)
        {
            var value = Visit(context.expression());
            if (value != null)
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
            var funcName = context.ID().GetText();
            if (_functions.TryGetValue(funcName, out var func))
            {
                var args = Visit(context.argList()) as List<object>; // Argüman listesini değerlendir
                if (args == null)
                    args = new List<object>();

                try
                {
                    return func(args.ToArray()); // Call the function
                }
                catch (ReturnException returnEx)
                {
                    // ReturnException yakalandığında dönen değeri geri döndür
                    return returnEx.ReturnValue;
                }
            }
            throw new InvalidOperationException($"Function not defined: {funcName}");
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

        public object VisitWhileLoopStatement([NotNull] ARPAParser.WhileLoopStatementContext context)
        {
            while ((bool)Visit(context.expression()))
            {
                Visit(context.block()); // Koşul sağlandığı sürece bloğu çalıştır
            }
            return null;
        }

        public object VisitForLoopStatement([NotNull] ARPAParser.ForLoopStatementContext context)
        {
            // For döngüsünün başlangıç durumu çalıştırılır (initializer)
            if (context.variableDeclaration() != null)
            {
                Visit(context.variableDeclaration());
            }

            // Döngü koşul kontrolü
            while (context.expression() == null || (bool)Visit(context.expression()))
            {
                // Döngü gövdesi
                Visit(context.block());

                // İterasyon kısmı (örneğin i++)
                if (context.assignment() != null)
                {
                    Visit(context.assignment());
                }
            }

            return null;
        }
    }
}
