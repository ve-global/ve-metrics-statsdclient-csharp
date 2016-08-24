using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.Tests.TestClasses
{
    public interface IOverloadedMethodsClass
    {
        void Do(Foo foo);
        void Do(Bar bar);
    }

    public class OverloadedMethodsClass : IOverloadedMethodsClass
    {
        [StatsDTiming("dependencies.foo.test")]
        public void Do(Foo foo)
        {
        }

        [StatsDTiming("dependencies.bar.test")]
        public void Do(Bar bar)
        {
        }

        [StatsDTiming("dependencies.nonoverload.test")]
        public void NonOverloadedMethod(Foo foo)
        {
        }
    }
}
