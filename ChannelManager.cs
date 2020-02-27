using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

class ChannelManager{
    List<YoutubeChannel> channels;
    public ChannelManager(IEnumerable<YoutubeChannel> channels = null){
        if(channels != null)
            this.channels = channels.ToList();
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

    /// <summary>
    /// Load the youtube channels from local storage
    /// </summary>
    public void LoadChannelList(){
        JObject json = JObject.Parse(File.ReadAllText(@"youtube-channels.json"));
        var channels = json["channels"];
        foreach(var channel in channels){
            this.channels.Append(new YoutubeChannel(channel["author"].ToString(),channel["channel-id"].ToString()));
        }
    }
}