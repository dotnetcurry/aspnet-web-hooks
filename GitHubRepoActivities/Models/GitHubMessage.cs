using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace GitHubRepoActivities.Models
{
    public class GitHubMessage
    {
        public JObject[] Commits { get; set; }
        public JObject Issue { get; set; }
        public JObject Comment { get; set; }
        public string Action { get; set; }
        public GitHubActivityType ActivityType { get; private set; }

        public void SetActivityType()
        {
            //If the payload has commits, it is a commit
            if (Commits != null && Commits.Length > 0)
            {
                ActivityType = GitHubActivityType.Commit;
            }
            //if the payload has Issues property set and Action is set to opened, an issue is opened
            else if (Issue != null && string.Equals("OPENED", Action, StringComparison.CurrentCultureIgnoreCase))
            {
                ActivityType = GitHubActivityType.IssueOpened;
            }
            //if the payload has Issues property set and Action is set to closed, an issue is closed
            else if (Issue != null && string.Equals("CLOSED", Action, StringComparison.CurrentCultureIgnoreCase))
            {
                ActivityType = GitHubActivityType.IssueClosed;
            }
            //if the payload has comment set, it is a comment
            else if(Comment != null && string.Equals("CREATED", Action, StringComparison.CurrentCultureIgnoreCase))
            {
                ActivityType = GitHubActivityType.Comment;
            }
        }

        public GitHubActivity ConvertToActivity()
        {
            var activity = new GitHubActivity();

            activity.ActivityType = ActivityType.ToString();
            if (ActivityType == GitHubActivityType.IssueClosed || ActivityType == GitHubActivityType.IssueOpened)
            {
                activity.Link = Issue.GetValue("html_url").Value<string>();
                activity.Description = Issue.GetValue("title").Value<string>();
            }
            else if (ActivityType == GitHubActivityType.Commit)
            {
                activity.Link = Commits[0].GetValue("url").Value<string>();
                activity.Description = Commits[0].GetValue("message").Value<string>();
            }
            else
            {
                activity.Link = Comment.GetValue("html_url").Value<string>();
                activity.Description = Comment.GetValue("body").Value<string>();
            }

            return activity;
        }
    }

    public enum GitHubActivityType
    {
        Commit,
        IssueOpened,
        IssueClosed,
        Comment
    }
}