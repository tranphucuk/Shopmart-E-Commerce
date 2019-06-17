using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetCoreOnlineShop.Extensions
{
    public static class ImgurExtension
    {
        public static async Task<string> UploadToImgur(string path)
        {
            var imgLink = string.Empty;
            try
            {
                var client = new ImgurClient("6cbb22757cdc4bc", "108b6a91467b0cb15ba36bbbd1d083c5f5e7d9e2");
                var endpoint = new ImageEndpoint(client);
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    imgLink = (await endpoint.UploadImageStreamAsync(fs)).Link;
                }
            }
            catch (ImgurException imgurEx)
            {
                Debug.Write("An error occurred uploading an image to Imgur.");
                Debug.Write(imgurEx.Message);
            }
            catch (Exception ex)
            {

            }
            return imgLink;
        }
    }
}
