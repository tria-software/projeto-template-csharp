using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ProjetoTemplate.API.Configuration
{
    public static class QuartzConfig
    {
        public static IServiceCollection AddQuartzConfig(this IServiceCollection services, IConfiguration configuration)
        {
            //var schedulerConfiguration = new SchedulerConfiguration();
            //new ConfigureFromConfigurationOptions<SchedulerConfiguration>(configuration.GetSection("Scheduler"))
            //                                                                        .Configure(schedulerConfiguration);

            //services.AddQuartz(q =>
            //{
            //    q.UseMicrosoftDependencyInjectionJobFactory();
            //    var jobKey = new JobKey("scheduler");
            //    q.AddJob<>(opts => opts.WithIdentity(jobKey));

            //    q.AddTrigger(opts => opts
            //        .ForJob(jobKey)
            //        .WithIdentity("trigger")
            //        //.WithCronSchedule("0 0 8 ? * * *") // roda todos os dias as 8:00
            //        .WithCronSchedule(schedulerConfiguration.ReportCron) // roda de 1 em 1 minuto (para modo de desenvolvimento)
            //    );               
            //});

            //services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            return services;
        }

        public class SchedulerConfiguration
        {
            public string ReportCron { get; set; }
        }
    }
}
