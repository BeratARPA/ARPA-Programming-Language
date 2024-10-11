using Antlr4.Runtime;

namespace ARPA_Programming_Language.Antlr4
{
    public class ARPAErrorListener : BaseErrorListener
    {
        public List<string> Errors { get; } = new List<string>();

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add($"Line {line}:{charPositionInLine} {msg}");
        }
    }
}
