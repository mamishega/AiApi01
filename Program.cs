using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

partial class Program
{
    static async Task Main(string[] args)
    {
        // Get the OpenAI API secrets from the GetSecrets method in the OpenaiSecrets class
        var secrets = OpenaiSecrets.GetSecrets();
        
        // Create an AuthenticationHeaderValue with the secrets
        var headers = new AuthenticationHeaderValue("Bearer", secrets.ApiKey);
        
        // Set the URL to the OpenAI API endpoint for the Codex model completions
        var url = "https://api.openai.com/v1/completions";
        


        // Set the prompt for the code generation, as well as the temperature and max_tokens values
        var prompt = "Generate 10 random multiple choice question with correct answer in json.";
        var data = new {prompt, model = "text-davinci-003", max_tokens = 1000, temperature = 0.5 };


        
        
        // Serialize the data object to JSON
        var json = JsonConvert.SerializeObject(data);
        System.Console.WriteLine(json);

        // Create a new HttpClient and set the Authorization header to the AuthenticationHeaderValue we created earlier
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = headers;
            
            // Send a POST request to the OpenAI API with the JSON data
            var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            // If the response was successful, parse the JSON response and print the generated code
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseContent);
                System.Console.WriteLine("-------------------------------");
                var text = result.choices[0].text;
                Console.WriteLine(text);
            }
            // If the response was not successful, print an error message with the status code
            else
            {
                Console.WriteLine($"Failed to generate code. Status code: {response.StatusCode}");
            }
        }
    }
}



public static class OpenaiSecrets
{
    // Define a static method for getting the OpenAI API secrets
    public static dynamic GetSecrets()
    {
        // Load and return your OpenAI API secrets as needed
        return new { ApiKey ="YOUR_API_KEY_HERE"};
    }
}

    

