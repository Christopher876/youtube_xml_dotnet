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
                ChannelManager manager = new ChannelManager();
                // manager.GenerateInitialManager(filename:"channels.txt",isFile:true);
                //manager.GenerateInitialManager();
                manager.LoadChannelList();
                manager.CheckForVideoUpdates();
            }
            
        }
    }
}
