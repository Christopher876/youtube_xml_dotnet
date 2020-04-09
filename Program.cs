using System;
using System.IO;
using CommandLine;
using Email;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace youtube_xml_dotnet
{
    class Program
    {
        public class Options{
            [Option('l',"load-channels",Required = false,HelpText = "Load the \"channel-list.txt\" channels into the server")]
            public bool LoadChannelList{get;set;}

            [Option('f',"force-intial",Required = false,HelpText = "Force server to regenerate the initial manager")]
            public bool ForceInitialGeneration{get;set;}

            [Option('c',"check",Required = false,HelpText = "Check for any new videos from the channels")]
            public bool CheckVideos{get;set;}

            [Option('t',"time",Default = 3600,Required = false,HelpText = "Interval for YouTube Update Checks")]
            public int Interval{get;set;}

            [Option('e',"email",Required = false,HelpText = "Enable Email to send updates")]
            public bool EnableEmail{get;set;}

            [Option("test",Required = false,HelpText = "Test")]
            public bool tester{get;set;}
        }

        public static void YtBackground(){
            Utils.ColorPrint(ConsoleColor.Green,"Checking for new videos");
            ChannelManager.Instance.CheckForVideoUpdates();
            Utils.ColorPrint(ConsoleColor.Blue,"Finished new video check");
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

                    if(o.EnableEmail){
                        if(File.Exists(@"email-login.json")){
                            var json = JObject.Parse(File.ReadAllText(@"email-login.json"));

                            //Catch any 'bad' email-login.json files with bad values
                            try{
                                var credentials = new EmailCredentials(){
                                    Username = json["username"].ToString(),
                                    Password = json["password"].ToString(),
                                    EnableSSL = (bool)json["EnableSSL"],
                                    SmtpAddress = json["SMTP"].ToString()
                                };

                                ChannelManager.Instance.isEmailEnabled = true;
                                ChannelManager.Instance.email = new Email.Email(credentials);
                                
                                var receivers = json["recipients"];
                                ChannelManager.Instance.email.recipients = new List<string>();
                                foreach(var receiver in receivers){
                                    ChannelManager.Instance.email.recipients.Add(receiver.ToString());
                                }
                                
                            }
                            catch(Exception e){
                                Console.WriteLine("email-login.json is formatted incorrectly");
                            }
                        }
                    }

                    //Generate the initial files that are needed
                    if(!File.Exists(@"youtube-channels.json") || o.ForceInitialGeneration){
                        ChannelManager.Instance.GenerateInitialManager();
                    }

                    if(o.CheckVideos){
                        ChannelManager.Instance.LoadChannelList();
                        ChannelManager.Instance.CheckForVideoUpdates();
                        Environment.Exit(0);
                    }

                    #region Program Command Line
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
                                Utils.ColorPrint(ConsoleColor.Green,"Loading new channels from channel-list.txt...");
                                ChannelManager.Instance.AddChannels();
                                Utils.ColorPrint(ConsoleColor.Blue,"Finished loading new channels");
                                break;
                            default:
                                Console.WriteLine("Not a valid command");
                                break;
                        }
                    }
                    #endregion
                });
        }
    }
}
