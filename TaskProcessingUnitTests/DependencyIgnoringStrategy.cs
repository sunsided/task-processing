using System.Collections.Generic;
using TaskProcessing;

namespace TaskProcessingUnitTests
{
    /// <summary>
    /// Strategy that ignores dependencies altogether
    /// </summary>
    internal class DependencyIgnoringStrategy : IDependencyResolvingStrategy<ITask>
    {
        public int ResolveDependency(ITask task, IList<ITask> currentTaskQueue)
        {
            return currentTaskQueue.Count;
        }
    }
}
