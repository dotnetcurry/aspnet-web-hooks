using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GitHubRepoActivities.Models;
using Microsoft.AspNet.WebHooks;
using Newtonsoft.Json.Linq;

namespace GitHubRepoActivities.WebHookHandlers
{
    public class GitHubWebHookHandler : WebHookHandler
    {
        GitHubActivitiesRepository repository;

        public GitHubWebHookHandler()
        {
            repository = new GitHubActivitiesRepository();
        }

        public override Task ExecuteAsync(string receiver, WebHookHandlerContext context)
        {
            if ("GitHub".Equals(receiver,StringComparison.CurrentCultureIgnoreCase))
            {
                string action = context.Actions.First();
                var message = context.GetDataOrDefault<GitHubMessage>();
                message.SetActivityType();

                var activityToSave = message.ConvertToActivity();
                repository.AddNewActivity(activityToSave);                
            }

            return Task.FromResult(true);
        }
    }
}