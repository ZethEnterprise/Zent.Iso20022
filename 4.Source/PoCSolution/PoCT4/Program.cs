using PoCT4.afolder;

namespace PoCT4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var t = new RuntimeTextTemplate1( )
            {
                FirstName = "David",
                LastName = "something"
            };

            var outputText1 = t.TransformText();
            Console.WriteLine(outputText1);

        }
    }
}
