namespace NexusApp.MailForm
{
    public interface IRazorViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync(string viewName, object model);
        Task<string> RenderViewToStringAsync(string viewName, string viewpath);

    }
}
