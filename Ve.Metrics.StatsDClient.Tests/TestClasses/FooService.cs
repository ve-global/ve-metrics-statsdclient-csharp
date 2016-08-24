using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.Tests.TestClasses
{
    public interface IFooService
    {
        void TrackedMethod();
        void UntrackedMethod();
        void TrackedFormattedMethod();
        T TrackedGenericMethod<T>();
    }

    public class FooService : IFooService
    {
        [StatsDCounting("dependencies.fooservice.method")]
        [StatsDTiming("dependencies.fooservice.method")]
        public void TrackedMethod()
        {
        }

        [StatsDCounting("dependencies.{type}.{method}")]
        [StatsDTiming("dependencies.{type}.{method}")]
        public void TrackedFormattedMethod()
        {
        }

        [StatsDCounting("dependencies.{generic}")]
        [StatsDTiming("dependencies.{generic}")]
        public T TrackedGenericMethod<T>()
        {
            return default(T);
        }

        public void UntrackedMethod()
        {
        }
    }
}