using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Parser{
    public List<Video> videos;
    private JObject jsonDocument;

    public Parser(XDocument doc){
        this.jsonDocument = JObject.Parse(JsonConvert.SerializeXNode(doc));
    }

    private JToken[] ParseTitles(){
        return jsonDocument["feed"]["entry"].Select(x => x["title"]).ToArray();
    }

    private JToken[] ParseIds(){
        return jsonDocument["feed"]["entry"].Select(x => x["link"]["@href"]).ToArray();
    }

    public Video[] ParseVideos()
    {
        var videoTitles = ParseTitles(); //Get all of the video titles that the XML contains
        var videoIds = ParseIds();
        Video[] videos = new Video[videoTitles.Length];
        for (int i = 0; i < videos.Length; i++)
        {
            videos[i] = new Video(name: videoTitles[i].ToString(),id:videoIds[i].ToString(),0,new DateTime());
        }
        return null;
    }
}