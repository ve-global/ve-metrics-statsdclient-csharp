using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.Tests.TestClasses
{
    public interface ITestGeneric<in T>
    {
        void Do(T thing);
    }

    public class TestGenerics : ITestGeneric<Foo>, ITestGeneric<Bar>
    {
        [StatsDTiming("dependencies.foo.test")]
        public void Do(Foo thing)
        {
        }

        [StatsDTiming("dependencies.bar.test")]
        public void Do(Bar thing)
        {
        }
    }
    
    public class Foo
    {
    }

    public class Bar
    {
    }
}
