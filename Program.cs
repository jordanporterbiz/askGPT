using Microsoft.Extensions.Configuration;

IConfigurationBuilder builder = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
.AddUserSecrets(typeof(Program).Assembly, optional: true);

IConfigurationRoot config = builder.Build();

if (args.Length > 0)
{
    HttpClient client = new HttpClient();

    client.DefaultRequestHeaders.Add("authorization", "Bearer " + config["GptApiKey"]);
}
else
{
    Console.WriteLine("----> You need to provide some input ");
}