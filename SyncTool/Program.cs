using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ExifLib;
using FlickrNet;

namespace SyncTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Flickr flickr = new Flickr(FlickrKey.Key, FlickrKey.Secret);
            Auth auth = null;
            if (File.Exists("key.txt"))
            {
                using (var sr = new StreamReader("key.txt"))
                {
                    flickr.OAuthAccessToken = sr.ReadLine();
                    flickr.OAuthAccessTokenSecret = sr.ReadLine();
                }

                     
            }

            if (!string.IsNullOrEmpty(flickr.OAuthAccessToken) &&
                 !string.IsNullOrEmpty(flickr.OAuthAccessTokenSecret))
            {
                auth = flickr.AuthOAuthCheckToken();
                int g = 56;
            }
            else
            {
                var requestToken = flickr.OAuthGetRequestToken("oob");
                var url = flickr.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Delete);
                Process.Start(url);

                var verifier = Console.ReadLine();

                OAuthAccessToken accessToken = flickr.OAuthGetAccessToken(requestToken, verifier);
                flickr.OAuthAccessToken = accessToken.Token;
                flickr.OAuthAccessTokenSecret = accessToken.TokenSecret;

                using (var sw = new StreamWriter("key.txt", false))
                {
                    sw.WriteLine(flickr.OAuthAccessToken);
                    sw.WriteLine(flickr.OAuthAccessTokenSecret);
                }

                auth = flickr.AuthOAuthCheckToken();
                int y = 56;
            }

            var baseFolder = @"D:\MyData\Camera Dumps\";

            var ex = new PhotoSearchExtras();
            var p = flickr.PhotosSearch(new PhotoSearchOptions(auth.User.UserId){Extras = PhotoSearchExtras.DateTaken | PhotoSearchExtras.PathAlias, });
            var sets = flickr.PhotosetsGetList();
            foreach (var set in sets)
            {
                var setDir = Path.Combine(baseFolder, set.Title);
                if ( Directory.Exists(setDir) )
                {
                    var setPhotos = flickr.PhotosetsGetPhotos(set.PhotosetId, PhotoSearchExtras.DateTaken | PhotoSearchExtras.MachineTags | PhotoSearchExtras.OriginalFormat | PhotoSearchExtras.DateUploaded);
                    foreach(var setPhoto in setPhotos)
                    {
                        if (Math.Abs((setPhoto.DateUploaded - setPhoto.DateTaken).TotalSeconds) < 60)
                        {
                            // Suspicious
                            int s = 56;
                        }

                        string ext = ".jpg";
                        if (setPhoto.OriginalFormat != "jpg")
                        {
                            int xxx = 56;
                        }
                        var filePath = Path.Combine(setDir, setPhoto.Title + ext);

                        if (!File.Exists(filePath))
                        {
                            // try mov
                            filePath = Path.Combine(setDir, setPhoto.Title + ".mov");

                            if (!File.Exists(filePath))
                            {
                                Console.WriteLine("not found " + filePath);
                            }
                        }

                        Console.WriteLine(filePath);
                        if ( File.Exists(filePath))
                        {
                            DateTime dateTaken;
                            if (!GetExifDateTaken(filePath, out dateTaken))
                            {
                                var fi = new FileInfo(filePath);
                                dateTaken = fi.LastWriteTime; 
                            }

                            if (Math.Abs((dateTaken - setPhoto.DateTaken).TotalSeconds) > 10)
                            {
                                int hmmm = 56;
                            }
                        }
                    }
                }
            }
            //@"D:\MyData\Camera Dumps\"
            
            int z =  56;


        }

        static bool GetExifDateTaken(string filePath, out DateTime dateTaken)
        {
            dateTaken = DateTime.MinValue;

            try
            {
                ExifReader rr = new ExifReader(filePath);
                return rr.GetTagValue(ExifTags.DateTime, out dateTaken);
            }
            catch (ExifLibException)
            {
                return false;
            }
            
        }
    }
}
