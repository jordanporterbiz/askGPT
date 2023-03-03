using Microsoft.Extensions.Configuration;

IConfigurationBuilder builder = new ConfigurationBuilder();



if (args.Length > 0)
{
    HttpClient client = new HttpClient();

    client.DefaultRequestHeaders.Add("authorization", "Bearer ");
}
else
{
    Console.WriteLine("----> You need to provide some input");
}