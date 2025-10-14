namespace zad5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello from ASP.NET Core!");
            string text = System.IO.File.ReadAllText(@"C:\temp\test.txt");

            app.MapGet("/zad6", () => text);

            app.Run();
        }
    }
}
