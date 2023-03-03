using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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

    try
    {
        var dyData = JsonConvert.DeserializeObject<dynamic>(responseString);

        string guess = GeussCommand(dyData.choices[0].text);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"----> My guess at the command prompt is: {guess}");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"----> Could not deserialise the JSON: {ex.Message}");
    }
    // Console.WriteLine(responseString);
}
else
{
    Console.WriteLine("----> You need to provide some input ");
}

static string GeussCommand(string raw)
{
    Console.WriteLine("----> GPT-3 API Returned Text: ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(raw);

    var lastIndex = raw.LastIndexOf("\n");

    string guess = raw.Substring(lastIndex + 1);

    Console.ResetColor();

    TextCopy.ClipboardService.SetText(guess);

    return guess;
}