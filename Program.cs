using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace youtube_xml_dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var db = new VideoContext()){
                // db.Add(new Video{
                //     name = "Batty Gone Wild",
                //     id = "irferjuverwu3367673e6u733",
                //     thumbnail = "dummynail.com/efouih",
                //     uploadDate = new DateTime(),
                //     views = 0,
                // });
                // db.SaveChanges();
                // var o = db.videos.Where(b => b.name.Contains("Batty Gone Wild")).ToArray();
                // Console.WriteLine("Name:" + o[0].name + " url: " + o[0].id + " upload date: " + o[0].uploadDate);
                // db.Remove(o);
                ChannelManager manager = new ChannelManager(channels:new List<YoutubeChannel>{new YoutubeChannel("Chris","oeuowuir"),new YoutubeChannel("Chris2","oytuowuir"),});
                manager.LoadChannelList();
            }
            
        }
    }
}
