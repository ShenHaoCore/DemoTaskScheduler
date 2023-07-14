using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using TaskService4;

LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

StdSchedulerFactory factory = new StdSchedulerFactory();
IScheduler scheduler = await factory.GetScheduler();
await scheduler.Start();

IJobDetail job = JobBuilder.Create<HelloJob>().WithIdentity("job1", "group1").Build();
ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger1", "group1").StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever()).Build();

await scheduler.ScheduleJob(job, trigger);
await Task.Delay(TimeSpan.FromSeconds(60));

await scheduler.Shutdown();

Console.WriteLine("Press any key to close the application. Bye!");
Console.ReadKey();