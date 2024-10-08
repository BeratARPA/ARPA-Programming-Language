//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ARPA.g4 by ANTLR 4.13.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="ARPAParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface IARPAListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProgram([NotNull] ARPAParser.ProgramContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProgram([NotNull] ARPAParser.ProgramContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] ARPAParser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] ARPAParser.StatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDeclaration([NotNull] ARPAParser.DeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDeclaration([NotNull] ARPAParser.DeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.variableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableDeclaration([NotNull] ARPAParser.VariableDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.variableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableDeclaration([NotNull] ARPAParser.VariableDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.functionDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionDeclaration([NotNull] ARPAParser.FunctionDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.functionDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionDeclaration([NotNull] ARPAParser.FunctionDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.paramList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParamList([NotNull] ARPAParser.ParamListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.paramList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParamList([NotNull] ARPAParser.ParamListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignment([NotNull] ARPAParser.AssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignment([NotNull] ARPAParser.AssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.expressionStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpressionStatement([NotNull] ARPAParser.ExpressionStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.expressionStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpressionStatement([NotNull] ARPAParser.ExpressionStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfStatement([NotNull] ARPAParser.IfStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfStatement([NotNull] ARPAParser.IfStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.printStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrintStatement([NotNull] ARPAParser.PrintStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.printStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrintStatement([NotNull] ARPAParser.PrintStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlock([NotNull] ARPAParser.BlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlock([NotNull] ARPAParser.BlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.returnStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterReturnStatement([NotNull] ARPAParser.ReturnStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.returnStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitReturnStatement([NotNull] ARPAParser.ReturnStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] ARPAParser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] ARPAParser.ExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.functionCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionCall([NotNull] ARPAParser.FunctionCallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.functionCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionCall([NotNull] ARPAParser.FunctionCallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ARPAParser.argList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArgList([NotNull] ARPAParser.ArgListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ARPAParser.argList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArgList([NotNull] ARPAParser.ArgListContext context);
}
