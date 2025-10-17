namespace AngleSharp.Css.Tests.Styling
{
    using System.Threading.Tasks;
    using AngleSharp;
    using NUnit.Framework;

    [TestFixture]
    public class FirstLetterPseudoElementTests
    {
        [Test]
        public async Task HostElementDeclarationsExcludeFirstLetter()
        {
            const string source =
                @"<html>
                    <head>
                        <style>
                            p::first-letter { float: left; color: red; }
                        </style>
                    </head>
                    <body>
                        <p>Example</p>
                    </body>
                </html>";

            var configuration = new Configuration().WithCss();
            var context = BrowsingContext.New(configuration);
            var document = await context.OpenAsync(request => request.Content(source));
            var window = document.DefaultView;
            Assert.That(window, Is.Not.Null);
            var paragraph = document.QuerySelector("p");
            Assert.That(paragraph, Is.Not.Null);

            var styles = window!.GetStyleCollection();
            var hostDeclarations = styles.ComputeDeclarations(paragraph!);
            Assert.That(hostDeclarations.GetPropertyValue("float"), Is.EqualTo(string.Empty));

            var firstLetterDeclarations = styles.ComputeDeclarations(paragraph!, "::first-letter");
            Assert.That(firstLetterDeclarations.GetPropertyValue("float"), Is.EqualTo("left"));
        }
    }
}
