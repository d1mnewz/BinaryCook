using BinaryCook.Core.Data.AggregateRoots;

namespace BinaryCook.Infrastructure.Caching.Tests
{
    public class TestAggregateRoot : AggregateRoot
    {
        public TestAggregateRoot(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}