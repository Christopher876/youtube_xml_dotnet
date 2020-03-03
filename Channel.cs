using System.Xml.Linq;
using System.Net.Http;
using System;
using HtmlAgilityPack;
using System.IO;

class YoutubeChannel{
    public string author;
    public string id;

    public YoutubeChannel(){}

    public YoutubeChannel(string author){
        this.author = author;
    }

    public XDocument getChannelFeed(){
        string url = Utils.channelBaseUrl+id;
        string result = "";
        using (var client = new HttpClient())
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            result = response.Content.ReadAsStringAsync().Result;
        }
        return XDocument.Parse(result);
    }

    public void getChannelId(){
        string result;

        //We get the channel html
        using(var client = new HttpClient()){
            HttpResponseMessage response = client.GetAsync("https://www.youtube.com/user/"+author).Result;
            response.EnsureSuccessStatusCode();
            result = response.Content.ReadAsStringAsync().Result;
        }

        //Process the html to get the channel id
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(result);
        string channelId = htmlDoc.DocumentNode.SelectSingleNode("/html/head/link[5]").Attributes[1].Value;
        this.id = channelId.Replace("https://www.youtube.com/channel/","");
    }
}