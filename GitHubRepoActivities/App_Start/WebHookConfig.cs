using System.Web.Http;

namespace GitHubRepoActivities
{
    public static class WebHookConfig
    {
        public static void Register(HttpConfiguration config)
        {
			config.InitializeReceiveGitHubWebHooks();
        }
    }
}
