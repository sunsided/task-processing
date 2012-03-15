using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProcessing
{
    /// <summary>
    /// Processor for tasks
    /// </summary>
    /// <typeparam name="TTask">The type of the task</typeparam>
    public interface ITaskProcessor<TTask> where TTask : ITask
    {
        /// <summary>
        /// Adds a task
        /// </summary>
        /// <param name="task">The task to process</param>
        void AddTask(TTask task);
    }
}
