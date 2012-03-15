using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace TaskProcessing
{
    /// <summary>
    /// Strategy for resolving dependencies
    /// </summary>
    [ContractClass(typeof(DependencyResolverStrategyContracts<>))]
    public interface IDependencyResolverStrategy<TTask> where TTask : ITask
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

    [ContractClassFor(typeof(IDependencyResolverStrategy<>))]
    internal abstract class DependencyResolverStrategyContracts<TTask> : IDependencyResolverStrategy<TTask> where TTask : ITask
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
