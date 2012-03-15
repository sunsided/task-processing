namespace TaskProcessing
{
    /// <summary>
    /// Factory for task processors
    /// </summary>
    /// <typeparam name="TTask"></typeparam>
    public class TaskProcessorFactory<TTask> : ITaskProcessorFactory<TTask> where TTask : ITask
    {
        /// <summary>
        /// Creates a <see cref="ITaskProcessor{TTask}"/> with the default strategy
        /// </summary>
        /// <returns>The <see cref="ITaskProcessor{TTask}"/></returns>
        public ITaskProcessor<TTask> Create()
        {
            return Create(null);
        }

        /// <summary>
        /// Creates a <see cref="ITaskProcessor{TTask}"/> with the default strategy
        /// </summary>
        /// <returns>The <see cref="ITaskProcessor{TTask}"/></returns>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strategy">The strategy to use</param>
        /// <returns>The <see cref="ITaskProcessor{TTask}"/></returns>
        public virtual ITaskProcessor<TTask> Create(IDependencyResolvingStrategy<TTask> strategy)
        {
            return new SequentialTaskProcessor<TTask>(strategy ?? new DefaultFifoDependencyResolver<TTask>());
        }
    }
}
