﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utility.MSDN;

namespace ThreadDiscovery
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i < 10; i++)
            {
                Run(i).Wait();
            }
        }

        static async Task Run(int index)
        {
            var collection = new ThreadCollection("wpdevelop");

            var threads = await collection.NavigateToPageAsync(index);

            var client = new HttpClient();

            foreach (var thread in threads)
            {
                var body = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                                { new KeyValuePair<string, string>("", thread)});

                var message = await client.PostAsync("http://analyzeit.azurewebsites.net/api/thread", body);

                var result = await message.Content.ReadAsStringAsync();

                Console.WriteLine(result);
            }
        }
    }
}
