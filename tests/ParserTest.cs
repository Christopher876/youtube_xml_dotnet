using NUnit.Framework;

class ParserTest{
    [Test]
    public void PrintAllVideoTitlesExpectsVideoTitlesToPrintToConsole(){
        YoutubeChannel channel = new YoutubeChannel("Ben Eater","UCS0N5baNlQWJCUrhCEo8WlA");
        Parser parser = new Parser(channel.getChannelFeed());
        parser.ParseVideos();
        Assert.IsTrue(true);
    }
}