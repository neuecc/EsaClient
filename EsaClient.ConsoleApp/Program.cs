using EsaClient.Toolkit;
using System;
using System.Threading.Tasks;

namespace EsaClient.ConsoleApp
{
    class Program
    {
        async static Task Run()
        {
            var esa = new EsaClient("");
            esa.Logger = new Microsoft.Extensions.Logging.Console.ConsoleLogger("Console", (_, __) => true, true);

            //await esa.PostExportAsync(new[]
            //{

            //}, "", userAsEsaBot: true);



        }

        static void Main(string[] args)
        {
            try
            {
                Run().GetAwaiter().GetResult();
                Console.WriteLine("End");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
