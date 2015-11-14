using System;
using System.Collections.Generic;
using System.Linq;

namespace Sequencegenerator
{
    public static class SequenceGenerator
    {
        public static IEnumerable<T> StartsWith<T>(params T[] seed)
        {
            return new List<T>(seed).AsEnumerable();
        }

        public static IEnumerable<T> NextSequence<T>(this IEnumerable<T> source, Func<T, T> processor)
        {
            while (true)
            {
                var lastValue = source.Last();

                var nextValue = processor.Invoke(lastValue);

                var newSequence = new List<T>() {nextValue};

                source = source.Concat(newSequence.AsEnumerable());

                yield return nextValue; 
             }
       }

        public static IEnumerable<T> NextSequence<T>(this IEnumerable<T> source, Func<T, T, T> processor)
        {
            while (true)
            {
                var lastValue = source.ElementAt(source.Count()-1);
                var lastValueButOne = source.ElementAt(source.Count()-2);

                var nextValue = processor.Invoke(lastValueButOne,lastValue);

                var newSequence = new List<T>() {nextValue};

                source = source.Concat(newSequence.AsEnumerable());

                yield return nextValue; 
             }
       }
    }
}
