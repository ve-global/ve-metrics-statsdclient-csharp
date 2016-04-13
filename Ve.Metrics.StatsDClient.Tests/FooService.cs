using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.Tests
{
    public interface IFooService
    {
        void TrackedMethod();
        void UntrackedMethod();
    }

    public class FooService : IFooService
    {
        [StatsDCounting("dependencies.fooservice.method")]
        [StatsDTiming("dependencies.fooservice.method")]
        public void TrackedMethod()
        {
        }

        public void UntrackedMethod()
        {
        }
    }
}