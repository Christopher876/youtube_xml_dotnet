using System;

struct Video{
    public Video(string name, string id, uint views, DateTime uploadDate){
        this.name = name;
        this.id = id;
        this.views = views;
        this.uploadDate = uploadDate;
    }
    public string name;
    public string id;
    public uint views;
    public DateTime uploadDate;
}