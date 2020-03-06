using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Parser{
    private JObject jsonDocument;

    public Parser(XDocument doc){
        this.jsonDocument = JObject.Parse(JsonConvert.SerializeXNode(doc));
    }

    public JToken ParseChannelName(){
        try{
            return jsonDocument["feed"]["author"]["name"];
        }
        catch{
            return null;
        }
    }

    private JToken[] ParseTitles(){
        try{
            return jsonDocument["feed"]["entry"].Select(x => x["title"]).ToArray();
        } 
        catch{
            return null;
        }
    }

    private JToken[] ParseIds(){
        try{
            return jsonDocument["feed"]["entry"].Select(x => x["link"]["@href"]).ToArray();
        }
        catch{
            return new JToken[1]{"0"};
        }
    }

    private JToken[] ParseUploadDate(){
        try{
            return jsonDocument["feed"]["entry"].Select(x => x["published"]).ToArray();
        }
        catch{
            return null;
        }
    }

    private JToken[] ParseThumbnail(){
        try{
            return jsonDocument["feed"]["entry"].Select(x => x["media:group"]["media:thumbnail"]["@url"]).ToArray();
        }
        catch{
            return null;
        }
    }

    public Video[] ParseVideos()
    {
        //Parse all the video details
        var videoTitles = ParseTitles(); //Get all of the video titles that the XML contains
        var videoIds = ParseIds();
        var videoUploadDates = ParseUploadDate();
        var videoThumbnails = ParseThumbnail();

        //Create and populate the video array
        Video[] videos = new Video[videoTitles.Length];
        for (int i = 0; i < videos.Length; i++)
        {
            videos[i] = new Video{
                name= videoTitles[i].ToString(),
                id=videoIds[i].ToString().Replace("https://www.youtube.com/watch?v=",""),
                views=0,
                uploadDate=DateTime.Parse(videoUploadDates[i].ToString()),
                thumbnail=videoThumbnails[i].ToString()
            };
        }
        return videos;
    }
}