using System;

namespace youtube_xml_dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing XML Result");
            YoutubeChannel channel = new YoutubeChannel("Ben Eater","UCS0N5baNlQWJCUrhCEo8WlA");
            Parser parser = new Parser(channel.getChannelFeed());
            parser.ParseVideos();
        }
    }
}
