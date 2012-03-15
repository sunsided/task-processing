using System.Diagnostics.Contracts;

namespace TaskProcessing
{
    /// <summary>
    /// Interface for <see cref="ITaskProcessor{TTask}"/> factories
    /// </summary>
    /// <typeparam name="TTask"></typeparam>
    [ContractClass(typeof(TaskProcessorFactoryContracts<>))]
    public interface ITaskProcessorFactory<TTask> where TTask : ITask
    {
        /// <summary>
        /// Creates a <see cref="ITaskProcessor{TTask}"/> with the default strategy
        /// </summary>
        /// <returns>The <see cref="ITaskProcessor{TTask}"/></returns>
        ITaskProcessor<TTask> Create();

        /// <summary>
        /// Creates a <see cref="ITaskProcessor{TTask}"/> with the default strategy
        /// </summary>
        /// <returns>The <see cref="ITaskProcessor{TTask}"/></returns>
        ITaskProcessor<TTask> Create(IDependencyResolvingStrategy<TTask> strategy);
    }

    #region Contracts

    [ContractClassFor(typeof(ITaskProcessorFactory<>))]
    internal abstract class TaskProcessorFactoryContracts<TTask> : ITaskProcessorFactory<TTask> where TTask : ITask
    {
        public ITaskProcessor<TTask> Create()
        {
            Contract.Ensures(Contract.Result<ITaskProcessor<TTask>>() != null);
            return null;
        }

        public ITaskProcessor<TTask> Create(IDependencyResolvingStrategy<TTask> strategy)
        {
            Contract.Ensures(Contract.Result<ITaskProcessor<TTask>>() != null);
            return null;
        }
    }

    #endregion
}
