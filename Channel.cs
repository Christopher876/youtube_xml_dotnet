using System.Xml.Linq;
using System.Net.Http;
using System;

class YoutubeChannel{
    private readonly string baseUrl = "https://www.youtube.com/feeds/videos.xml?channel_id="; 
    public string author;
    public string id;
    public YoutubeChannel(string author, string url){
        this.author = author;
        this.id = url;
    }

    public XDocument getChannelFeed(){
        string url = baseUrl+id;
        string result = "";
        using (var client = new HttpClient())
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            result = response.Content.ReadAsStringAsync().Result;
        }
        return XDocument.Parse(result);
    }
}