using System.Text.Json;
using System.Net.Http.Json;
using System.Text;

var API_KEY = "Your API key";
var client = new HttpClient() { BaseAddress = new("https://api.openai.com") };
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");

Console.WriteLine("Чат с AI начался! Для выхода введите \"exit\"");
string? input;
Console.Write("Введите ваш вопрос: ");
while ((input = Console.ReadLine()) is not null && input.ToLower() != "exit")
{
    try
    {
        var response = await client.PostAsJsonAsync("/v1/chat/completions", new
        {
            model = "gpt-4o-mini-2024-07-18",
            messages = new[] { new { role = "user", content = input } },
            temperature = 0.7
        });

        response.EnsureSuccessStatusCode();
        using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var content = json.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content").GetString();

        Console.WriteLine($"\nОтвет:\n{content}\n{new string('-', 50)}\n");
    }
    catch (Exception ex) 
    {
        Console.WriteLine($"\nОшибка: {ex.Message}\n{new string('-', 50)}\n");
    }
    Console.Write("Введите ваш вопрос: ");
}
