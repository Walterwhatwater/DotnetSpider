using System.Collections;

namespace Java2Dotnet.Spider.Core.Pipeline
{
	/// <summary>
	/// Pipeline that can collect and store results.
	/// </summary>
	public interface ICollectorPipeline : IPipeline
	{
		/// <summary>
		/// Get all results collected.
		/// </summary>
		/// <returns></returns>
		ICollection GetCollected();
	}
}
