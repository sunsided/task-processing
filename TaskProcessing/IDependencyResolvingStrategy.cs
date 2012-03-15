using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace TaskProcessing
{
    /// <summary>
    /// Strategy for resolving dependencies
    /// </summary>
    [ContractClass(typeof(DependencyResolvingStrategyContracts<>))]
    public interface IDependencyResolvingStrategy<TTask> where TTask : ITask
    {
        /// <summary>
        /// Determines the insert position of a given task in a task queue
        /// </summary>
        /// <param name="task">The task to insert</param>
        /// <param name="currentTaskQueue">The current task queue</param>
        /// <returns>The insert position</returns>
        [Pure]
        int ResolveDependency(TTask task, IList<TTask> currentTaskQueue);
    }

    #region Contracts

    [ContractClassFor(typeof(IDependencyResolvingStrategy<>))]
    internal abstract class DependencyResolvingStrategyContracts<TTask> : IDependencyResolvingStrategy<TTask> where TTask : ITask
    {
        public int ResolveDependency(TTask task, IList<TTask> currentTaskQueue)
        {
            Contract.Requires(task != null);
            Contract.Requires(currentTaskQueue != null);
            Contract.Requires(Contract.ForAll(currentTaskQueue, value => value != null));
            Contract.Ensures(Contract.Result<int>() >= 0 && Contract.Result<int>() <= currentTaskQueue.Count);
            return 0;
        }
    }

    #endregion
}
