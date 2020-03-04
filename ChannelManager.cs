using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading;
using System.Xml.Linq;

class ChannelManager{
    List<YoutubeChannel> channels;
    public ChannelManager(IEnumerable<YoutubeChannel> channels = null){
        if(channels != null)
            this.channels = channels.ToList();
    }

    /// <summary>
    /// Generate an inital database and channel list json using a txt file
    /// </summary>
    public void GenerateInitialManager(){
        List<YoutubeChannel> _channels = new List<YoutubeChannel>();

        if(File.Exists(@"channel-list.txt")){
            //Get all the ids
            List<string> s = File.ReadAllLines(@"channel-list.txt").ToList();

            //Get the author name
            foreach(var userId in s){
                _channels.Add(new YoutubeChannel{
                    author = getChannelAuthor(userId),
                    id = userId,
                });
                Thread.Sleep(500); //Probably good to sleep in between requests
            }

            //Let's save all of the gathered channels to the json
            //If it exists, start with this json
            JObject json;
            //If file is empty, create the file hierarchy
            if(!File.Exists(@"youtube-channels.json") || new FileInfo(@"youtube-channels.json").Length == 0){
                json = new JObject(new JProperty("channels"));
                File.WriteAllText(@"youtube-channels.json",JsonConvert.SerializeObject(json));
            }
            json = JObject.Parse(File.ReadAllText(@"youtube-channels.json"));
            JArray channelsArray = (JArray)json["channels"];
            //After checking if it exists, lets add all the channels with their ids to the json
            foreach(var channel in _channels){
                channelsArray.Add(new JObject{
                    new JProperty("author",channel.author),
                    new JProperty("channel-id",channel.id)
                });
            }
            //Set the class channels variable to the all of the new channels that were found and save the file
            SetChannels(json);
            SaveChannelList();
        }
        else{
            return;
        }
    }

    //TODO Search for username and actually find it everytime if there are spaces
    //Spaced user names are broken
    public void GenerateInitialManager(string filename="", bool isFile = false, string channelName = ""){
        List<YoutubeChannel> _channels;
        JObject json;
        if(isFile){
            //Get some sort of list of channels that we want to use to generate youtube-channels.json
            if(File.Exists(filename)){
                //Load all of the channel names
                List<string> s = File.ReadAllLines(filename).ToList();
                _channels = new List<YoutubeChannel>();
                foreach(var c in s){
                    _channels.Append(new YoutubeChannel{
                        author = c,
                        id = getChannelId(c)
                    });
                }
            } 
            else{
                return;
            }

            //Get all of the channels and add them to the json

            //If it exists, start with this json
            if(File.Exists(@"youtube-channels.json")){
                //If file is empty, create the file hierarchy
                if(new FileInfo(@"youtube-channels.json").Length == 0){
                    json = new JObject(new JProperty("channels"));
                }

                //After checking if it exists, lets add all the channels with their ids to the json
            }
        }
        else{
            Console.WriteLine("Added Something");
        }
    }

    private XDocument getChannelFeed(string id){
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

    private string getChannelId(string author){
        string result;

        //We get the channel html
        Thread.Sleep(500); //Sleep for half a second
        using(var client = new HttpClient()){
            HttpResponseMessage response = client.GetAsync("https://www.youtube.com/user/"+author).Result;
            response.EnsureSuccessStatusCode();
            result = response.Content.ReadAsStringAsync().Result;
        }

        //Process the html to get the channel id
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(result);
        string channelId = htmlDoc.DocumentNode.SelectSingleNode("/html/head/link[5]").Attributes[1].Value;
        return channelId.Replace("https://www.youtube.com/channel/","");
    }

    private string getChannelAuthor(string id){
        var name = new Parser(getChannelFeed(id)).ParseChannelName();
        return name.ToString();
    }

    /// <summary>
    /// Checks for Updates from Youtube Channels
    /// </summary>
    /// <param name="threads">Number of threads that should be used to check for video updates</param>
    /// <returns>List of videos that are new</returns>
    public List<Video> CheckForVideoUpdates(uint threads = 2){
        //Create a Queue so that the channels can be run down until empty by the threads
        var queue = new Queue<YoutubeChannel>(channels);

        //Check each channel by spawning more threads so it will be faster (only if set)
        
        //If there is a new video then add to a list all of the new videos that should be sent as a notification

        return null;
    }

    /// <summary>
    /// Check if the video is already in the database by id, if it is then return true which tells the program not to send a notification.
    /// </summary>
    /// <param name="video"></param>
    /// <returns>if the video is in the database</returns>
    private bool CheckNotifiedStatus(Video video){
        using(VideoContext db = new VideoContext()){
            var notNotified = db.videos
                .Where(x => x.id.Contains(video.id))
                .ToArray();
            return (notNotified.Length == 0); //if the length is 0 then we have not sent a notification. If not 0 then we have to send a notification     
        }
    }
    
    /// <summary>
    /// Save the channel list to a file that can be read later
    /// </summary>
    public void SaveChannelList(){
        //Create a properly formatted JSON object
        JObject json = new JObject(
            new JProperty("channels",
                new JArray(
                    from c in channels
                    orderby c.author
                    select new JObject(
                        new JProperty("author",c.author),
                        new JProperty("channel-id",c.id)
                    )
                ))
        );

        File.WriteAllText(@"youtube-channels.json",JsonConvert.SerializeObject(json));  
    }

    private void SetChannels(JObject json){
        channels = new List<YoutubeChannel>();
        var _channels = json["channels"];
        foreach(var channel in _channels){
            this.channels.Add(new YoutubeChannel{
                author = channel["author"].ToString(),
                id = channel["channel-id"].ToString()
            });
        }
    }

    /// <summary>
    /// Load the youtube channels from local storage
    /// </summary>
    public void LoadChannelList(){
        JObject json = JObject.Parse(File.ReadAllText(@"youtube-channels.json"));
        SetChannels(json);
    }
}