﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;

namespace TaskProcessing
{
    /// <summary>
    /// Interface for Tasks
    /// </summary>
    [ContractClass(typeof(TaskContracts))]
    public interface ITask
    {
        /// <summary>
        /// Executes the task
        /// </summary>
        void Execute();

        /// <summary>
        /// Adds a dependency
        /// </summary>
        /// <param name="dependency">The dependency to meet</param>
        void AddDependency(ITask dependency);

        /// <summary>
        /// Gets the dependencies
        /// </summary>
        /// <returns>The dependencies</returns>
        [Pure]
        IList<ITask> GetDependencies();

        /// <summary>
        /// Occurs when the work is done by either succeeding or failing
        /// </summary>
        event EventHandler<TaskCompletionEventArgs> WorkDone;

        /// <summary>
        /// Wait handle for task synchronization
        /// </summary>
        WaitHandle TaskCompletionWaitHandle { [Pure] get; }
    }

    #region Contracts

    [ContractClassFor(typeof(ITask))]
    internal abstract class TaskContracts : ITask
    {
        public abstract void Execute();

        public void AddDependency(ITask dependency)
        {
            Contract.Requires(dependency != null);
        }

        public IList<ITask> GetDependencies()
        {
            Contract.Ensures(Contract.Result<IList<ITask>>() != null);
            return null;
        }

        public event EventHandler<TaskCompletionEventArgs> WorkDone;

        public abstract WaitHandle TaskCompletionWaitHandle { get; }
    }

    #endregion Contracts

}
