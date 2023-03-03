using System.Text;
using Microsoft.Extensions.Configuration;

IConfigurationBuilder builder = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddUserSecrets(typeof(Program).Assembly, optional: true)
.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

IConfigurationRoot config = builder.Build();

if (args.Length > 0)
{
    HttpClient client = new HttpClient();

    client.DefaultRequestHeaders.Add("authorization", "Bearer " + config["GptApiKey"]);

    var content = new StringContent("{\"model\": \"text-davinci-001\", \"prompt\": \"" + args[0] + "\",\"temperature\": 1,\"max_tokens\": 100}",
        Encoding.UTF8, "application/json");

    HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content);

    string responseString = await response.Content.ReadAsStringAsync();

    Console.WriteLine(responseString);
}
else
{
    Console.WriteLine("----> You need to provide some input ");
}