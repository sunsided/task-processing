using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace TaskProcessing
{
    /// <summary>
    /// Default task processor
    /// </summary>
    /// <typeparam name="TTask"></typeparam>
    public sealed class SequentialTaskProcessor<TTask> : ITaskProcessor<TTask> where TTask : ITask
    {
        /// <summary>
        /// The list of tasks to process
        /// </summary>
        private readonly IList<TTask> TaskList = new List<TTask>();

        /// <summary>
        /// The dependency resolver to be used
        /// </summary>
        private readonly IDependencyResolvingStrategy<TTask> DependencyStrategy;

        /// <summary>
        /// The mutex used to control access to the list
        /// </summary>
        private readonly Mutex ProducerConsumerMutex = new Mutex(false);

        /// <summary>
        /// The mutex that controls the processing task's lifetime
        /// </summary>
        private readonly Mutex ProcessingTaskLifetimeMutex = new Mutex(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialTaskProcessor{TTask}"/> class
        /// </summary>
        /// <param name="dependencyStrategy">The strategy used to resolve dependencies</param>
        public SequentialTaskProcessor(IDependencyResolvingStrategy<TTask> dependencyStrategy)
        {
            Contract.Requires(dependencyStrategy != null);
            DependencyStrategy = dependencyStrategy;
        }

        /// <summary>
        /// Adds a task
        /// </summary>
        /// <param name="task">The task to process</param>
        public void AddTask(TTask task)
        {
            if (!ProducerConsumerMutex.WaitOne(1000)) throw new WaitHandleCannotBeOpenedException("Deadlock detected.");
            try
            {
                // Find the insert position by resolving the task's dependencies
                int insertPosition = DependencyStrategy.ResolveDependency(task, TaskList);
                TaskList.Insert(insertPosition, task);

                // Create a new processing task if neccessary.
                // To do this, we first try to acquire the lifetime mutex.
                const int timeoutInMs = 10;
                if (ProcessingTaskLifetimeMutex.WaitOne(timeoutInMs))
                {
                    // Since we could acquire the mutex, no processing task has existed
                    // or the last processing task gave it up.
                    // (There is no deadlock possible here since the processing task itself needs the
                    // producer/consumer mutex to be able to do so, which is currently held by us.)
                    Task.Factory.StartNew(ProcessingLogic);
                }
            }
            finally
            {
                ProducerConsumerMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// The internal processing logic
        /// </summary>
        private void ProcessingLogic()
        {
            if (Thread.CurrentThread.Name == null) Thread.CurrentThread.Name = "Default Task Processor Logic";

            var taskList = TaskList;
            var producerConsumerMutex = ProducerConsumerMutex;

            while (true)
            {
                ITask task;

                // Acquire a new item
                producerConsumerMutex.WaitOne();
                try
                {
                    // only grab items if there is at least one available
                    if (taskList.Count > 0)
                    {
                        task = taskList[0];
                        Contract.Assume(task != null);

                        taskList.RemoveAt(0);
                    }
                    else
                    {
                        // at this point no items were available, so we stop processing
                        // By releasing the lifetime mutex the main thread is allowed to create a new task
                        ProcessingTaskLifetimeMutex.ReleaseMutex();
                        return;
                    }
                }
                finally
                {
                    producerConsumerMutex.ReleaseMutex();
                }

                // At this point an item was available, so we process it
                Contract.Assert(task != null);
                task.Execute();
            }
        }
    }
}
