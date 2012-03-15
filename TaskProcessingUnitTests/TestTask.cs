using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using TaskProcessing;

namespace TaskProcessingUnitTests
{
    /// <summary>
    /// A simple test task that does nothing and will either succeed or fail
    /// </summary>
    internal class TestTask : ITask
    {
        /// <summary>
        /// The internal dependency list
        /// </summary>
        private readonly IList<ITask> Dependencies = new List<ITask>();

        /// <summary>
        /// The signal that the work is done
        /// </summary>
        private readonly ManualResetEvent WorkDoneEvent = new ManualResetEvent(false);

        /// <summary>
        /// Determines if this task should succeed
        /// </summary>
        private readonly bool WillSucceed;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TestTask"/> class
        /// </summary>
        /// <param name="willSucceed">If set to <c>true</c>, the task will succeed</param>
        public TestTask(bool willSucceed)
        {
            WillSucceed = willSucceed;
        }

        /// <summary>
        /// Executes the task
        /// </summary>
        public void Execute()
        {
            try
            {
                // foo
            }
            finally
            {
                WorkDoneEvent.Set();
                if (WorkDone != null) WorkDone(this, new TaskCompletionEventArgs(WillSucceed ? TaskCompletion.Succeeded : TaskCompletion.Failed));
            }
        }

        /// <summary>
        /// Adds a dependency
        /// </summary>
        /// <param name="dependency">The dependency</param>
        public void AddDependency(ITask dependency)
        {
            Dependencies.Add(dependency);
        }

        /// <summary>
        /// Gets all dependencies
        /// </summary>
        /// <returns></returns>
        public IList<ITask> GetDependencies()
        {
            return Dependencies;
        }

        /// <summary>
        /// Occurs when the work is done
        /// </summary>
        public event EventHandler<TaskCompletionEventArgs> WorkDone;

        public WaitHandle TaskCompletionWaitHandle
        {
            [Pure]
            get { return WorkDoneEvent; }
        }
    }
}
