using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RoslynHackathon
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RoslynHackathonAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RoslynHackathon.AlphanumericTypeNames";
        internal const string Title = "Type name contains non alphanumeric characters";
        internal const string MessageFormat = "Type name '{0}' contains non alphanumeric characters";
        internal const string Category = "Naming";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var symbol = context.Symbol;

            if (!IsValidName(symbol.Name))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule, symbol.Locations[0], symbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static bool IsValidName(string name)
        {
            return !String.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, "^[a-zA-Z0-9]*$");
        }
    }
}
