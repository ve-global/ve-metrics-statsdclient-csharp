using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.Abstract
{
    public abstract class InterceptorBase
    {
        protected IVeStatsDClient Client { get; set; }

        protected InterceptorBase(IVeStatsDClient client)
        {
            Client = client;
        }
        
        protected static T GetAttributeInternal<T>(Type type, string methodName, object[] arguments) where T : StatsDBase
        {
            var matchedMethods = type
                .GetMethods()
                .Where(x => x.Name == methodName)
                .ToList();

            var targetMethod = matchedMethods.Count > 1 
                ? matchedMethods.FirstOrDefault(x => MatchingParams(x.GetParameters(), arguments)) 
                : matchedMethods.FirstOrDefault();
            
            return targetMethod?
                .GetCustomAttributes(typeof(T), true)
                .FirstOrDefault() as T;
        }

        protected static bool MatchingParams(ParameterInfo[] method, object[] args)
        {
            return !args.Where((t, i) => t.GetType() != method[i].ParameterType).Any();
        }

        protected static string GetGenericName(string name, MethodBase methodBase)
        {
            var generic = new List<string>();
            var arguments = methodBase.GetGenericArguments();

            while (arguments.Any())
            {
                var argument = arguments.First();
                generic.Add(argument.Name.ToLowerInvariant());
                arguments = argument.GetGenericArguments();
            }

            name = name.Replace("{generic}", string.Join("-", generic));
            return name;
        }
    }
}
