using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace youtube_xml_dotnet{

    public class YoutubeCheckTask : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Program.YtBackground();
            return null; //TODO Probably a better way to do this?
        }
    }

    public class JobScheduler{
        public static void Start(int interval) {
            //var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            //scheduler.Start();
            var factory = new StdSchedulerFactory();
            
            IScheduler scheduler = factory.GetScheduler().Result;
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<YoutubeCheckTask>()
                .WithIdentity("testJob","group1")
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("YoutubeJob","group1")
                .StartNow()
                .WithSimpleSchedule(s => s
                    .WithIntervalInSeconds(interval)
                    .RepeatForever())
                .Build();
            
            scheduler.ScheduleJob(job,trigger);
        }
    }
    
}