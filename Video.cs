using System;

class Video{
    public Video(string name, string id, uint views, DateTime uploadDate, string thumbnail){
        this.name = name;
        this.id = id;
        this.views = views;
        this.uploadDate = uploadDate;
        this.thumbnail = thumbnail;
    }
    public string name;
    public string id;
    public uint views;
    public DateTime uploadDate;
    public string thumbnail;
}