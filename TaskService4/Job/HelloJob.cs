using Quartz;

namespace TaskService4
{
    /// <summary>
    /// 
    /// </summary>
    public class HelloJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Greetings from HelloJob!");
        }
    }
}
