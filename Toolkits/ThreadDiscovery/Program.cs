﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utility.MSDN;
using Utility.HttpCache;
using Newtonsoft.Json;

namespace ThreadDiscovery
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        private static void FroumCacheDemo()
        {
            var provider = DefaultLocalFileSystemHttpCacheProvider.Current;

            Regex urlRegex = new Regex(@"(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})");

            var resolver = new RegexPathResolver(urlRegex, "thread_id_{1}.xml");

            provider.Configure(@"D:\httpcache", resolver);

            var collection = new ThreadCollection(Community.MSDN, "wpdevelop");

            var task = collection.NavigateToPageAsync(1);

            task.Wait();

            var threads = task.Result;

            var client = new HttpClient();

            foreach (var thread in threads)
            {
                var urlString = thread + "&outputAs=xml";

                var uri = new Uri(urlString);

                if (provider.IsCached(uri))
                {
                    continue;
                }

                var httpTask = client.GetStreamAsync(uri);

                httpTask.Wait();

                provider.Cache(httpTask.Result, uri);
            }
        }

        private static void Analyze()
        {
            string[] keywords = new string[]
            {
                "winjs", "c#", "c++", ".net", "wpsl", "listview", "cortana", "streamsocket",
                "mediaelement", "sqlite", "cordova", "emulator", "gridview", "contactstore",
                "datepicker", "inkcanvas", "bug", "webauthenticationbroker", "10532", "blend",
                "xaml"
            };

            List<IThreadAnalyze> als = new List<IThreadAnalyze>();

            foreach (var keword in keywords)
            {
                als.Add(new KeyWordCountAnalyze(keword));
            }

            // als.Add(new AskerAnalyze());

            var snapshoot = new ForumSnapshot(Community.MSDN, "wpdevelop");

            snapshoot.Load(@"D:\Archive\uwp_page_0_35.json");

            var data = snapshoot.GetData();

            IDictionary<string, object> result = new Dictionary<string, object>();

            var filter = data.Where(m => !m.Answered);

            foreach (ThreadInfo info in filter)
            {
                foreach (var al in als)
                {
                    al.Analyze(result, info);
                }
            }

            foreach (KeyValuePair<string, object> kv in result)
            {
                Console.WriteLine("{0}, {1}", kv.Key, kv.Value);
                // File.WriteAllText(string.Format("{0}.txt", kv.Key), kv.Value.ToString());
            }
        }

        private static void SaveSanpshot()
        {
            var snapshoot = new ForumSnapshot(Community.MSDN, "wpdevelop");

            var task = snapshoot.TakeAsync(35);

            task.Wait();

            snapshoot.Save(@"D:\Archive\uwp_page_0_35.json");
        }

        static void ImportThreads()
        {
            for (int i = 1; i < 5; i++)
            {
                //Run(Community.MSDN, "wpdevelop", i).Wait();
                //Run(Community.TECHNET, "Office2016setupdeploy%2COffice2016ITPro", i).Wait();
                // Run(Community.MSDN, "WindowsIoT", i).Wait();
                Run(Community.MSDN, "appsforoffice", i).Wait();
            }
        }

        static async Task Run(Community community, string forum, int index)
        {
            var collection = new ThreadCollection(community, forum);

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
