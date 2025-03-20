using System.Diagnostics.CodeAnalysis;

namespace Electrolux.BFF.ComparePage
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new App(args);
            app.StartApplication();
        }
    }
}