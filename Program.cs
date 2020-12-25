using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using HtmlAgilityPack;

namespace YTFramework
{
    class Program

    /***
     *     __     __         _         _            _____                      _                 _           
     *     \ \   / /        | |       | |          |  __ \                    | |               | |          
     *      \ \_/ /__  _   _| |_ _   _| |__   ___  | |  | | _____      ___ __ | | ___   __ _  __| | ___ _ __ 
     *       \   / _ \| | | | __| | | | '_ \ / _ \ | |  | |/ _ \ \ /\ / / '_ \| |/ _ \ / _` |/ _` |/ _ \ '__|
     *        | | (_) | |_| | |_| |_| | |_) |  __/ | |__| | (_) \ V  V /| | | | | (_) | (_| | (_| |  __/ |   
     *        |_|\___/ \__,_|\__|\__,_|_.__/ \___| |_____/ \___/ \_/\_/ |_| |_|_|\___/ \__,_|\__,_|\___|_|   
     *                                                                                                       
     * 
     *   12/23-24/2020
     *   @TODO add progress bar
     *   @TODO detect clipboard changes
     *   @TODO Strip or maybe add data
     *   
     */

    {
        [STAThread]

        static void Main(string[] args)
        {

            bool finished = false;
            List<string> list = new List<string>();

            Console.WriteLine("When you are finished adding songs press enter");

            while (true)
            {
                if (!finished)
                {
                    Console.WriteLine("Please enter a youtube link: ");
                    string input = Console.ReadLine();
                    if (!String.IsNullOrEmpty(input))
                    {
                        list.Add(input);
                    }
                    else
                    {
                        finished = true;
                    }
                    continue;
                    
                }
                foreach(var x in list)
                {
                    DownloadSong(MakeLink(x));
                }
                Console.WriteLine("Resetting Program!");
                finished = false;
            }

            /*while (true)
            {
                Console.WriteLine("Stealing Clipboard Data and Turning It Into A Song!");
                string song = String.Format(Clipboard.GetText());
                song = MakeLink(song);
                //DownloadSong(song);
                Console.WriteLine("Press spacebar to download another song or esc to exit\n");
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                if (keyPressed.Key != ConsoleKey.Spacebar)
                {
                    break;
                }
            }*/
        }
        static string MakeLink(string link)
        {
            link = link.Substring(24);
            link = link.Insert(0, "https://www.320youtube.com/v17/");
            return link;
        }

        static void DownloadSong(string link)
        {
            /*var http = WebRequest.CreateHttp(link);
            var response = http.GetResponse();
            var stream = response.GetResponseStream();
            var content = new System.IO.StreamReader(stream).ReadToEnd();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content.Replace("doctype","DOCTYPE"));
            Console.WriteLine(doc);*/

            var html = new HtmlWeb().Load(link);
            var doc = html.DocumentNode;
            var downloaddiv = doc.SelectSingleNode("//div[@id='download']/div/div/a");

            var downloadlocation = @"D:\Documents\Testi\";

            var download = String.Format(downloaddiv.GetAttributeValue("href", "Failed"));

            Console.WriteLine("Generated Download Link: {0}",download);
            using(var wc = new WebClient())
            {
                try
                {
                    wc.DownloadData(download);
                    string filename = "";
                    if (!String.IsNullOrEmpty(wc.ResponseHeaders["Content-Disposition"]))
                    {
                        filename = wc.ResponseHeaders["Content-Disposition"].Substring(wc.ResponseHeaders["Content-Disposition"].IndexOf("filename=") + 9).Replace("\"", "");
                    }
                    wc.DownloadFile(download, downloadlocation + filename);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
    }
}
