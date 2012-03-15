using NUnit.Framework;
using TaskProcessing;

namespace TaskProcessingUnitTests
{
    [TestFixture]
    class TaskProcessorFactoryTests
    {
        private ITaskProcessorFactory<ITask> Factory;

        [SetUp]
        public void Setup()
        {
            Factory = new TaskProcessorFactory<ITask>();
        }

        [TearDown]
        public void Teardown()
        {
            Factory = null;
        }

        [Test]
        public void Create_withNull_doesNotThrowAndReturnsInstance()
        {
            ITaskProcessor<ITask> processor = null;
            Assert.That(() => { processor = Factory.Create(null); }, Throws.Nothing);
            Assert.That(processor, Is.Not.Null);
        }

        [Test]
        public void Create_withNoParameter_isIdenticalWithNullParameter()
        {
            Assert.That(Factory.Create().GetType(), Is.SameAs(Factory.Create(null).GetType()));
        }
    }
}
