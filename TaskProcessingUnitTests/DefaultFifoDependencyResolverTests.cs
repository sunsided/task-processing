using System;
using System.Collections.Generic;
using NUnit.Framework;
using TaskProcessing;

namespace TaskProcessingUnitTests
{
    [TestFixture]
    public class DefaultFifoDependencyResolverTests
    {
        private IDependencyResolvingStrategy<ITask> Strategy;

        [SetUp]
        public void Setup()
        {
            Strategy = new FifoDependencyResolver<ITask>();
        }

        [TearDown]
        public void TearDown()
        {
            Strategy = null;
        }

        [Test]
        public void Resolve_withoutDependencies_enqueuesAtTheEnd([Values(1, 5, 10)] int count)
        {
            Assume.That(count, Is.GreaterThan(0));
            IList<ITask> list = new List<ITask>(count);

            for (int c=0; c<count; ++c)
            {
                ITask task = new TestTask(true);
                int result = Strategy.ResolveDependency(task, list);
                Assert.That(result, Is.EqualTo(list.Count));
            }
        }

        [Test]
        public void Resolve_withUnknownDependencies_enqueuesAtTheEnd([Values(1, 5, 10)] int count)
        {
            Assume.That(count, Is.GreaterThan(0));
            IList<ITask> list = new List<ITask>(count);

            for (int c = 0; c < count; ++c)
            {
                // create task and add random dependency
                ITask task = new TestTask(true);
                task.AddDependency(new TestTask(true));

                // determine position
                int result = Strategy.ResolveDependency(task, list);
                Assert.That(result, Is.EqualTo(list.Count));
            }
        }

        const int maximumDependencyCount = 100;

        [Test]
        public void Resolve_withDependency_enquesBefore([Values(1, 5)] int count, [Random(1, maximumDependencyCount, 5)] int dependencies)
        {
            Assume.That(count, Is.GreaterThan(0));
            Assume.That(dependencies, Is.GreaterThan(0));

            // Create list of tasks
            IList<ITask> list = new List<ITask>(maximumDependencyCount);
            for (int d = 0; d < maximumDependencyCount; ++d )
            {
                list.Add(new TestTask(true));
            }

            // repeat until sundown
            for (int repetition = 0; repetition < count; ++repetition)
            {
                // pick random dependencies
                IList<ITask> pickedDependencies = new List<ITask>(dependencies);
                int smallestDependencyIndex = maximumDependencyCount;
                Random random = new Random();
                for (int d = 0; d < dependencies; ++d)
                {
                    int position;
                    ITask picked;
                    do
                    {
                        position = random.Next(0, maximumDependencyCount);
                        picked = list[position];
                    } while (pickedDependencies.Contains(picked));

                    // remember the smallest index and the task
                    smallestDependencyIndex = Math.Min(smallestDependencyIndex, position);
                    pickedDependencies.Add(picked);
                }

                // create a task and add the dependencies
                ITask task = new TestTask(true);
                for (int d = 0; d < dependencies; ++d)
                {
                    task.AddDependency(pickedDependencies[d]);
                }

                // assert that the resolved index is smaller than or equal to
                // the index of the earliest dependency
                int resolvedIndex = Strategy.ResolveDependency(task, list);
                Assert.That(resolvedIndex, Is.LessThanOrEqualTo(smallestDependencyIndex));
            }
        }
    }
}
