using System;
using System.Threading.Tasks;

namespace EsaClient.ConsoleApp
{
    class Program
    {
        async static Task Run()
        {
            var esa = new EsaClient("");

            var huga = await esa.GetPostsAsync("grani", "");
        }

        static void Main(string[] args)
        {
            try
            {
                Run().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
