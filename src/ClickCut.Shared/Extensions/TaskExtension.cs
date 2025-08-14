namespace ClickCut.Shared.Extensions;

public static class TaskExtensions
{
	public static async Task Catch(this Task task, Func<Exception, Task> handler)
	{
		try
		{
			await task;
		}
		catch (Exception ex)
		{
			await handler(ex);
		}
	}
}