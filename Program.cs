using System;
using System.IO;
using CommandLine;

namespace youtube_xml_dotnet
{
    class Program
    {
        private static int testNumber = 0;
        public class Options{
            [Option('l',"load-channels",Required = false,HelpText = "Load the \"channel-list.txt\" channels into the server")]
            public bool LoadChannelList{get;set;}

            [Option('f',"force-intial",Required = false,HelpText = "Force server to regenerate the initial manager")]
            public bool ForceInitialGeneration{get;set;}

            [Option('c',"check",Required = false,HelpText = "Check for any new videos from the channels")]
            public bool CheckVideos{get;set;}

            [Option('t',"time",Required = false,HelpText = "Test Hangfire Background")]
            public int Interval{get;set;}
        }

        public static void ColorPrint(ConsoleColor color, string output){
            Console.ForegroundColor = color;
            Console.WriteLine(output);
            Console.ResetColor();
        }

        public static void TestBackground(){
            ColorPrint(ConsoleColor.Green,"Checking for new videos");
            ChannelManager.Instance.CheckForVideoUpdates();
            ColorPrint(ConsoleColor.Blue,"Finished new video check");
        }

        static void Main(string[] args)
        {
            //Ctrl+C Event - Stop and clean up things
            Console.CancelKeyPress += delegate{
                Console.WriteLine("Exitting... No Quartz Background Events to clean up");
            };

            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>( o => {
                    //Should we add any channels to our server?
                    if(o.LoadChannelList){
                        ChannelManager.Instance.AddChannels();
                    }
                    //Generate the initial files that are needed
                    else if(!File.Exists(@"youtube-channels.json") || o.ForceInitialGeneration){
                        ChannelManager.Instance.GenerateInitialManager();
                    }
                    if(o.CheckVideos){
                        ChannelManager.Instance.LoadChannelList();
                        ChannelManager.Instance.CheckForVideoUpdates();
                    }

                    if(!ChannelManager.Instance.isChannelsLoaded)
                        ChannelManager.Instance.LoadChannelList();
                    //Quartz.net implementation of a background scheduler
                    JobScheduler.Start(o.Interval);
                    Console.WriteLine("Press Ctrl+C to exit");
                    while(true){
                        Console.Write("> ");
                        string input = Console.ReadLine();
                        switch (input){
                            case "gen":
                                Console.WriteLine("Generating Initial Channel Manager!");
                                ChannelManager.Instance.GenerateInitialManager();
                                break;
                            case "check":
                            case "c":
                                Console.WriteLine("Checking for any video updates!");
                                ChannelManager.Instance.CheckForVideoUpdates();
                                Console.WriteLine("Finished checking for any new videos!");
                                break;
                            case "load":
                            case "l":
                                ColorPrint(ConsoleColor.Green,"Loading new channels from channel-list.txt...");
                                ChannelManager.Instance.AddChannels();
                                ColorPrint(ConsoleColor.Blue,"Finished loading new channels");
                                break;
                            default:
                                Console.WriteLine("Not a valid command");
                                break;
                        }
                    }
                });
        }
    }
}
