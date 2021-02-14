using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace K8sGracefulShutdownTester.Tester
{
    class Program
    {
        const string serviceUrl = "http://localhost:5000/slow";

        static void Main(string[] args)
        {
            Test().Wait();
            Console.WriteLine("Hello World!");
        }

        static async Task Test()
        {
            while (true)
            {
                using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(15) })
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        var resp = await httpClient.GetAsync(serviceUrl);

                        if (resp.IsSuccessStatusCode)
                        {
                            Console.WriteLine("{0}: Successful request, headers:{1}", DateTime.UtcNow, resp.Headers);
                        }
                        else
                        {
                            Console.WriteLine("{0}: Error! StatusCode: {1}, Response: {2}, headers:{3}", DateTime.UtcNow, resp.StatusCode, await resp.Content.ReadAsStringAsync(), resp.Headers);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0}: Error! Exception: {1}", DateTime.UtcNow, ex.Message);
                    }
                }
            }
        }
    }
}
