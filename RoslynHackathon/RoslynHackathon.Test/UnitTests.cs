using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;

namespace RoslynHackathon.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {
        [TestMethod]
        public void Should_not_show_diagnostic_when_empty()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void Should_show_warning_when_type_with_non_alphanumeric_character()
        {
            var test = Properties.Resources.Test;

            var expected = new DiagnosticResult
            {
                Id = RoslynHackathonAnalyzer.DiagnosticId,
                Message = String.Format("Type name '{0}' contains non alphanumeric characters", "TypeæName"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 3, 11)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = Properties.Resources.FixTest;

            VerifyCSharpFix(test, fixtest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new RoslynHackathonCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new RoslynHackathonAnalyzer();
        }
    }
}