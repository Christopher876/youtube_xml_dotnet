using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CommandLine;

namespace youtube_xml_dotnet
{
    class Program
    {
        public class Options{
            [Option('l',"load-channels",Required=false,HelpText = "Load the \"channel-list.txt\" channels into the server")]
            public bool LoadChannelList{get;set;}

            [Option('f',"force-intial",Required=false,HelpText = "Force server to regenerate the initial manager")]
            public bool ForceInitialGeneration{get;set;}

            [Option('c',"check",Required=false,HelpText = "Check for any new videos from the channels")]
            public bool CheckVideos{get;set;}
        }

        static void Main(string[] args)
        {
            //Ctrl+C Event - Stop and clean up things
            Console.CancelKeyPress += delegate{
                //TODO When Hangfire is added, clean up all hangfire background processes
                Console.WriteLine("Exitting... No Hangfire Background Events to clean up");
            };

            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>( o => {
                    ChannelManager manager = new ChannelManager();
                    //Should we add any channels to our server?
                    if(o.LoadChannelList){
                        manager.AddChannels();
                    }
                    //Generate the initial files that are needed
                    else if(!File.Exists(@"youtube-channels.json") || o.ForceInitialGeneration){
                        manager.GenerateInitialManager();
                    }
                    if(o.CheckVideos){
                        manager.LoadChannelList();
                        manager.CheckForVideoUpdates();
                    }
                });
                //manager.GenerateInitialManager();
                //manager.LoadChannelList();
                //manager.CheckForVideoUpdates();
            
        }
    }
}
