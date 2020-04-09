using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace ConsoleHTTPRequest
{
    class Program
    {
        const string URL = "http://localhost:5000/somedata";
        const string GET = "get";
        const string POST = "post";
        const string DELETE = "delete";
        const string PUT = "put";
        const string FLAG_SORTED = "--sorted";
        const string FLAG_ID = "--id";

        static async Task Main(string[] args)
        {
            string inputStr = "";
            string[] commandsStr;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("appId", "campus-task");

            while (true)
            {

                inputStr = Console.ReadLine();
                commandsStr = inputStr.Split(' ');

                if(commandsStr.Length == 0)
                {
                    continue;
                }

                switch (commandsStr[0])
                {
                    case GET:
                        {
                            string flagValue = GetFlagValue(commandsStr);

                            HttpResponseMessage response = await client.GetAsync($"{URL}{flagValue}");
                            string result = response.Content.ReadAsStringAsync().Result;

                            PrintResponseMessage(response, result);
                            break;
                        }
                    case POST:
                        {
                            if(commandsStr.Length < 2)
                            {
                                continue;
                            }

                            string dataStr = inputStr.Substring(POST.Length + 1, inputStr.Length - POST.Length - 1);
                            var data = new StringContent(dataStr, Encoding.UTF8, "application/json");

                            var response = await client.PostAsync(URL, data);

                            string result = await response.Content.ReadAsStringAsync();

                            PrintResponseMessage(response, result);
                            break;
                        }
                    case DELETE:
                        {
                            if (commandsStr.Length < 2)
                            {
                                continue;
                            }

                            var response = await client.DeleteAsync($"{URL}/{commandsStr[1]}");

                            PrintResponseMessage(response, "Item deleted!");
                            break;
                        }
                    case PUT:
                        {
                            if(commandsStr.Length < 3)
                            {
                                continue;
                            }

                            var data = new StringContent(commandsStr[2], Encoding.UTF8, "application/json");

                            var response = await client.PutAsync($"{URL}/{commandsStr[1]}", data);

                            PrintResponseMessage(response, "Item changed!");
                            break;
                        }
                }

                Console.WriteLine();
            }
        }

        static string GetFlagValue(string[] str)
        {
            if(str.Length < 3)
            {
                return "";
            }

            if(str[1].Equals(FLAG_ID))
            {
                return $"/{str[2]}";
            }
            else if(str[1].Equals(FLAG_SORTED))
            {
                return $"/?sorted={str[2]}";
            }

            return "";
        }

        static void PrintResponseMessage(HttpResponseMessage response, string result)
        {
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"{(int)response.StatusCode}({response.StatusCode})");
            }
        }
    }
}
