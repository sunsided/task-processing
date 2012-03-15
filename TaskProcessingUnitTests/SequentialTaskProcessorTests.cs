using System.Threading;
using NUnit.Framework;
using TaskProcessing;

namespace TaskProcessingUnitTests
{
    [TestFixture]
    public class SequentialTaskProcessorTests
    {
        private ITaskProcessor<ITask> Processor;

        [SetUp]
        public void Setup()
        {
            Processor = new SequentialTaskProcessor<ITask>(new DependencyIgnoringStrategy());
        }

        [TearDown]
        public void Teardown()
        {
            Processor = null;
        }

        [Test]
        public void AddTask_startsProcessing([Values(1, 10, 100)] int repetitions, [Values(0, 1, 10)] int insertDelayMs, [Values(10)] int baseWaitTime)
        {
            Assume.That(repetitions, Is.GreaterThan(0));
            Assume.That(insertDelayMs, Is.AtLeast(0));
            Assume.That(baseWaitTime, Is.AtLeast(0));

            // create the tasks
            ITask[] tasks = new ITask[repetitions];
            for (int t=0; t<repetitions; ++t)
            {
                tasks[t] = new TestTask(true);
                Assert.That(tasks[t].TaskCompletionWaitHandle.WaitOne(0), Is.False);
            }

            // enqueue the tasks
            for (int t = 0; t < repetitions; ++t)
            {
                Processor.AddTask(tasks[t]);
                Thread.Sleep(insertDelayMs);
            }

            // wait for completion
            int maximumWaitTimeMs = insertDelayMs * repetitions + baseWaitTime;
            for (int t = 0; t < repetitions; ++t)
            {
                Assert.That(tasks[t].TaskCompletionWaitHandle.WaitOne(maximumWaitTimeMs), Is.True);
            }
        }
        
    }
}
