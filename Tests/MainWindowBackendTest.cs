using NUnit.Framework;
using Findin;
using System.Text.RegularExpressions;

namespace Tests
{
    public class Tests
    {
        [TestCase("um dois três")]
        [TestCase("a")]
        [TestCase("b")]
        [TestCase("Lorem ipsum dolor sit amet")]
        [TestCase("rs")]
        public void TextBoxHasValueMustBeTrue(string x)
        {
            Assert.True(MainWindowBackend.TextBoxHasValue(new System.Windows.Forms.TextBox { Text = x }));
        }

        [TestCase("")]
        [TestCase("    ")]
        [TestCase(null)]
        public void TextBoxHasValueMustBeFalse(string x)
        {
            Assert.False(MainWindowBackend.TextBoxHasValue(new System.Windows.Forms.TextBox { Text = x }));
        }

        [TestCase("[A-Za-z0-9]")]
        [TestCase("png|gif")]
        [TestCase(".")]
        [TestCase("\\(\\)")]
        public void RegexPatternIsValidMustBeTrue(string regex)
        {
            Assert.True(MainWindowBackend.RegexPatternIsValid(regex));
        }

        [TestCase("(")]
        [TestCase(")")]
        [TestCase("[")]
        public void RegexPatternIsValidMustBeFalse(string regex)
        {
            Assert.False(MainWindowBackend.RegexPatternIsValid(regex));
        }

        [TestCase(";;;;;")]
        [TestCase(";;")]
        [TestCase(";")]
        public void CleanSemiColonStringMustReturnEmpty(string x)
        {
            Assert.AreEqual("", MainWindowBackend.CleanSemiColonString(x));
        }

        [TestCase("*.cs;*.py;*.go;;;;;")]
        [TestCase("*.cs;*.py;*.go;;;;")]
        [TestCase("*.cs;*.py;*.go;;")]
        [TestCase(";*.cs;*.py;*.go")]
        public void CleanSemiColonStringMustReturnAValidString(string x)
        {
            Assert.AreEqual("*.cs;*.py;*.go", MainWindowBackend.CleanSemiColonString(x));
        }

        [TestCase("public class MainWindowBackend\r\n\t{")]
        [TestCase("var int = 3;\r\n\t{")]
        public void TryReadWholeLineShouldReadTheString(string x)
        {
            int matchIndex = Regex.Match(x, "{").Index;
            MainWindowBackend.TryReadWholeLine(x, matchIndex, out int lineNumber, out string lineContent);

            Assert.AreEqual(2, lineNumber);
            Assert.AreEqual("{", lineContent);
        }

        [TestCase("C:\\Users\\something\\file.cs", "something")]
        [TestCase("C:\\Users\\something\\repos\\js\\index.js", "repos|things")]
        [TestCase("C:\\Users\\something\\repos\\bin\\my_project.csproj", "bin|things|js")]
        [TestCase("D:\\Users\\something\\file.go", "Users")]
        public void ContainsIgnoredDirectoriesShouldReturnTrue(string filePath, string ignoredDirectories)
        {
            Assert.True(MainWindowBackend.ContainsIgnoredDirectories(filePath, ignoredDirectories));
        }
    }
}