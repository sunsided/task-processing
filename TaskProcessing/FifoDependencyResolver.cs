using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace TaskProcessing
{
    /// <summary>
    /// The default dependency resolving strategy
    /// </summary>
    public sealed class FifoDependencyResolver<TTask> : IDependencyResolvingStrategy<TTask> where TTask : ITask
    {
        /// <summary>
        /// Determines the insert position of a given task in a task queue
        /// </summary>
        /// <param name="task">The task to insert</param>
        /// <param name="currentTaskQueue">The current task queue</param>
        /// <returns>The insert position</returns>
        [Pure]
        public int ResolveDependency(TTask task, IList<TTask> currentTaskQueue)
        {
            var dependencies = task.GetDependencies();

            // if there are no dependencies, attach task to the end
            if (dependencies.Count == 0)
            {
                return currentTaskQueue.Count;
            }

            // in any other case, find the earliest dependency in the existing queue
            int targetIndex = currentTaskQueue.Count;
            foreach (var dependency in dependencies)
            {
                // determine the index
                // we can't use IndexOf here because of the type variance
                for (int currentIndex = 0; currentIndex < currentTaskQueue.Count; ++currentIndex)
                {
                    var taskInQueue = currentTaskQueue[currentIndex];
                    if (dependency.Equals(taskInQueue))
                    {
                        // early exit
                        if (currentIndex == 0) return 0;

                        // compare indices
                        targetIndex = Math.Min(targetIndex, currentIndex);
                        break;
                    }
                }
            }
            return targetIndex;
        }
    }
}
