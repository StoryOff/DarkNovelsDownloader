using RestSharp;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;
using Tesseract;
using Newtonsoft.Json;
using DarkNovelsDownloader.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DarkNovelsDownloader.Commands
{
    class MainFunctions
    {
        private readonly static string _api = "https://api.dark-novels.ru/v2/";

        private readonly static Regex NoSpecialCharacters = new Regex("(?i)[^A-Za-z0-9А-Яа-я +-.]");

        public static string ConvertImageToText(ConvertImageSettings settings)
        {
            using var engine = new TesseractEngine(settings.EngineFolderPath, settings.Lang, EngineMode.Default);
            using var img = Pix.LoadFromFile(settings.Path);
            using var page = engine.Process(img);
            return page.GetText();
        }

        public static async Task<byte[]> GetArchieve(GetArchieveSettings settings)
        {
            var client = new RestClient(_api+"chapter/")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("token", settings.Token);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("b", settings.B);
            request.AddParameter("f", settings.F);
            foreach (var chapter in settings.C)
            {
                request.AddParameter("c", chapter);
            }
            IRestResponse response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return response.RawBytes;
            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(5000);
                response = await client.ExecuteAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return response.RawBytes;
            }

            {
                MessageBox.Show(response.StatusCode.ToString());
                return null;
            }
        }

        public static async Task<ChaptersModel> GetChapters(string bookId, string token)
        {
            var client = new RestClient(_api + "toc/" + bookId)
            {
                Timeout = -1,
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("token", token);
            IRestResponse response = await client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ChaptersModel>(response.Content);
        }

        public static async Task ExtractZip(ExtractZipSettings settings)
        {
            if (!Directory.Exists(settings.ExtractPath))
                Directory.CreateDirectory(settings.ExtractPath);

            await Task.Factory.StartNew(()=> {
                ZipFile.ExtractToDirectory(settings.ZipPath, settings.ExtractPath);
            });
        }

        public static async void GetChaptersText(GetChaptersSettings settings, List<Chapter> selectedChapters)
        {
            var chapters = await GetArchieve(settings.GetArchieveSettings);
            await File.WriteAllBytesAsync(settings.ExtractZipSettings.ZipPath, chapters);
            await ExtractZip(settings.ExtractZipSettings);

            if (settings.GetArchieveSettings.F == "png")
            {
                string chaptersText = "";
                var files = Directory.GetFiles(settings.ExtractZipSettings.ExtractPath, "*.png");

                string fileName = selectedChapters.FirstOrDefault().Title;

                if (selectedChapters.Count > 1)
                {
                    fileName += " - " + selectedChapters.LastOrDefault().Title;
                }

                fileName = NoSpecialCharacters.Replace(fileName, String.Empty);

                fileName += ".docx";

                foreach (var file in files)
                {
                    settings.ConvertImageSettings.Path = file.ToString();
                    chaptersText += ConvertImageToText(settings.ConvertImageSettings);
                    chaptersText += "\r\n";
                    File.WriteAllText(settings.ExtractZipSettings.ExtractPath + "\\" + fileName, chaptersText);
                    File.Delete(settings.ConvertImageSettings.Path);
                }

            }
        }
    }
}