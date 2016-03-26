using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace BingWallpaper
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isConnected = false;
            while (!isConnected)
            {
                try
                {
                    Thread.Sleep(5 * 1000);
                    BingImage bingResponse;
                    string imageURL;
                    using (WebClient bingClient = new WebClient())
                    {
                        bingResponse = JsonConvert.DeserializeObject<BingImage>(bingClient.DownloadString("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US"));
                        isConnected = true;
                        imageURL = $"http://www.bing.com{bingResponse.images.FirstOrDefault().url}";
                    }
                    string wallpapersPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\Bing Wallpapers\";
                    string picturePath = $"{wallpapersPath}{bingResponse.images.FirstOrDefault().hsh}.jpg";
                    if (!File.Exists(picturePath))
                    {
                        if (!Directory.Exists(wallpapersPath))
                            Directory.CreateDirectory(wallpapersPath);
                        using (WebClient imageClient = new WebClient())
                        {
                            imageClient.DownloadFile(imageURL, picturePath);
                        }
                        Wallpaper.Set(picturePath);
                    }
                }
                catch (WebException)
                {
                    Thread.Sleep(5 * 1000);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
