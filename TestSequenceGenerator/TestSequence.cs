using System;
using System.Linq;
using NUnit.Framework;
using Sequencegenerator;

namespace TestSequenceGenerator
{
    public class TestSequence
    {

        private Func<int, int> _integerIncrementer; 
        private Func<char, char> _charIncrementer; 
        private Func<int, int, int> _fibonacci;
        private Func<Tuple<string,char>, Tuple<string, char>> _nextCharstringApender; 

        [SetUp]
        public void SetUp()
        {
            _integerIncrementer = (lastvalue) => ++lastvalue;
            _charIncrementer = (lastvalue) => ++lastvalue;
            _fibonacci = (lastButOneValue, lastValue) => lastButOneValue + lastValue;
            _nextCharstringApender = (lastStringConcat) =>
            {
                var lastChar = lastStringConcat.Item2;
                ++lastChar;
                var newString = lastStringConcat.Item1 + lastChar;
                return new Tuple<string, char>(newString, lastChar);
            };
        }

        [Test]
        public void GeneratorStartSequenceAsEnumerableInt()
        {
            var startSequence = new int[] {10, 20, 30};

            var result = SequenceGenerator.StartsWith(startSequence).ToList();
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0], Is.EqualTo(10));
            Assert.That(result[2], Is.EqualTo(30));
        }

        [Test]
        public void GeneratorStartSequenceAsEnumerableChar()
        {
            var result = SequenceGenerator.StartsWith('a', 'b').ToList();
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo('a'));
            Assert.That(result[1], Is.EqualTo('b'));
        }


        [Test]
        public void ExetensionMethodNextSequenceAsInt()
        {
            var result = SequenceGenerator.StartsWith(2)
                .NextSequence(_integerIncrementer)
                .Take(1)
                .ToList();
            Assert.That(result.Count, Is.EqualTo(1),"Incorrect count");
            Assert.That(result[0], Is.EqualTo(3));
        }

        [Test]
        public void ExetensionMethodNextThreeSequenceAsEnumerableInt()
        {
            var result = SequenceGenerator.StartsWith(2)
                .NextSequence(_integerIncrementer)
                .Take(3)
                .ToList();
            Assert.That(result.Count, Is.EqualTo(3), "Incorrect count");
            Assert.That(result[0], Is.EqualTo(3));
            Assert.That(result[1], Is.EqualTo(4));
            Assert.That(result[2], Is.EqualTo(5));
        }

        [Test]
        public void ExetensionMethodNextThreeSequenceAsEnumerablechar()
        {
            var result = SequenceGenerator.StartsWith('b')
                .NextSequence(_charIncrementer)
                .Take(3)
                .ToList();
            Assert.That(result.Count, Is.EqualTo(3), "Incorrect count");
            Assert.That(result[0], Is.EqualTo('c'));
            Assert.That(result[1], Is.EqualTo('d'));
            Assert.That(result[2], Is.EqualTo('e'));
        }

        [Test]
        public void Fibonacci_NextValues()
        {
            var result = SequenceGenerator.StartsWith(1,1)
                .NextSequence(_fibonacci)
                .Take(3)
                .ToList();
            Assert.That(result.Count, Is.EqualTo(3), "Incorrect count");
            Assert.That(result[0], Is.EqualTo(2));
            Assert.That(result[1], Is.EqualTo(3));
            Assert.That(result[2], Is.EqualTo(5));
        }

        [Test]
        public void FibonacciWithMemoization_NextValues()
        {
            var result = SequenceGenerator.StartsWith(3,5)
                .NextSequence(_fibonacci)
                .Take(5)
                .ToList();
            Assert.That(result[0], Is.EqualTo(8));
            Assert.That(result[1], Is.EqualTo(13));
            Assert.That(result[2], Is.EqualTo(21));
            Assert.That(result[3], Is.EqualTo(34));
            Assert.That(result[4], Is.EqualTo(55));
        }


        [Test]
        public void FibonacciWithMemoization_MultipleCalls()
        {
            var firstResultSet = SequenceGenerator.StartsWith(1,1)
                .NextSequence(_fibonacci)
                .Take(4)
                .ToList();

            var result = SequenceGenerator.StartsWith(firstResultSet.ToArray())
                .NextSequence(_fibonacci)
                .Take(4)
                .ToList();

            Assert.That(result[0], Is.EqualTo(13));
            Assert.That(result[1], Is.EqualTo(21));
            Assert.That(result[2], Is.EqualTo(34));
            Assert.That(result[3], Is.EqualTo(55));
        }

        [Test]
        public void Fibonacci_FirstTenValues()
        {
            var startSequence = new int[] { 1, 1 };
            var result = startSequence.Concat(
                    SequenceGenerator.StartsWith(startSequence)
                    .NextSequence(_fibonacci)
                    .Take(8))
                .ToList();

            Assert.That(result.Count, Is.EqualTo(10), "Incorrect count");
            Assert.That(result[0], Is.EqualTo(1));
            Assert.That(result[1], Is.EqualTo(1));
            Assert.That(result[2], Is.EqualTo(2));
            Assert.That(result[3], Is.EqualTo(3));
            Assert.That(result[4], Is.EqualTo(5));
            Assert.That(result[5], Is.EqualTo(8));
            Assert.That(result[6], Is.EqualTo(13));
            Assert.That(result[7], Is.EqualTo(21));
            Assert.That(result[8], Is.EqualTo(34));
            Assert.That(result[9], Is.EqualTo(55));
        }


        [Test]
        public void nextcharStringAppnder_NextValues()
        {
            var result = SequenceGenerator.StartsWith(new Tuple<string,char>("abc",'c'))
                .NextSequence(_nextCharstringApender)
                .Take(3)
                .ToList();
            Assert.That(result.Count, Is.EqualTo(3), "Incorrect count");
            Assert.That(result[0].Item1, Is.EqualTo("abcd"));
            Assert.That(result[1].Item1, Is.EqualTo("abcde"));
            Assert.That(result[2].Item1, Is.EqualTo("abcdef"));
        }


    }
}
