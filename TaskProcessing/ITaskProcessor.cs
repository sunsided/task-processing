using System.Diagnostics.Contracts;

namespace TaskProcessing
{
    /// <summary>
    /// Processor for tasks
    /// </summary>
    /// <typeparam name="TTask">The type of the task</typeparam>
    [ContractClass(typeof(TaskProcessorContracts<>))]
    public interface ITaskProcessor<in TTask> where TTask : ITask
    {
        /// <summary>
        /// Adds a task
        /// </summary>
        /// <param name="task">The task to process</param>
        void AddTask(TTask task);
    }

    #region Contracts

    [ContractClassFor(typeof(ITaskProcessor<>))]
    internal abstract class TaskProcessorContracts<TTask> : ITaskProcessor<TTask> where TTask : ITask
    {
        public void AddTask(TTask task)
        {
            Contract.Requires(task != null);
        }
    }

    #endregion
}
