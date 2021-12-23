using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.IO;
using System.Xml;

namespace BotwInstaller.Wizard.ViewThemes.App
{
    public class TextEditorActions
    {
        public static void ChangeSyntaxHighlighting(ICSharpCode.AvalonEdit.TextEditor textEditor, string name)
        {
            using (FileStream stream = File.OpenRead($"ViewThemes\\Styles\\TextEditor\\SyntaxHighlighting\\{name}{ShellViewTheme.ThemeStr}"))
            using (XmlReader reader = XmlReader.Create(stream))
                textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }
    }
}
