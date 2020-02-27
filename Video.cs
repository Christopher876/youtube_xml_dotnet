using System;
using Microsoft.EntityFrameworkCore;

public class VideoContext : DbContext{
    public DbSet<Video> videos {get; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=videos.db");
}
public class Video{
    public Video(string name, string id, uint views, DateTime uploadDate, string thumbnail){
        this.name = name;
        this.id = id;
        this.views = views;
        this.uploadDate = uploadDate;
        this.thumbnail = thumbnail;
    }
    public string name{get;set;}
    public string id{get;set;}
    public uint views{get;set;}
    public DateTime uploadDate{get;set;}
    public string thumbnail{get;set;}
}