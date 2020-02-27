using System.Collections.Generic;
using System.Threading;

class ChannelManager{

    List<YoutubeChannel> channels;
    public ChannelManager(List<YoutubeChannel> channels){
        this.channels = channels;
    }

    /// <summary>
    /// Checks for Updates from Youtube Channels
    /// </summary>
    /// <param name="threads">Number of threads that should be used to check for video updates</param>
    /// <returns>List of videos that are new</returns>
    public List<Video> CheckForVideoUpdates(uint threads = 2){
        //Create a Queue so that the channels can be run down until empty by the threads

        //Check each channel by spawning more threads so it will be faster (only if set)
        
        //If there is a new video then add to a list all of the new videos that should be sent as a notification

        return null;
    }
}