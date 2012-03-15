using System;

namespace TaskProcessing
{
    /// <summary>
    /// Task completion event arguments
    /// </summary>
    public class TaskCompletionEventArgs : EventArgs
    {
        /// <summary>
        /// The completion information
        /// </summary>
        public TaskCompletion Completion { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCompletionEventArgs"/> class
        /// </summary>
        /// <param name="completion">The completion state</param>
        public TaskCompletionEventArgs(TaskCompletion completion)
        {
            Completion = completion;
        }
    }
}
 