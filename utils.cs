using System;

public class Utils{
    public static readonly string channelBaseUrl = "https://www.youtube.com/feeds/videos.xml?channel_id=";

    public static void ColorPrint(ConsoleColor color, string output){
        Console.ForegroundColor = color;
        Console.WriteLine(output);
        Console.ResetColor();
    }
}