using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sisusa.Common.ReturnTypes.Tests
{
    internal class UserTest(string name, int age)
    {
        public string Name { get; init; } = name;

        public int Age { get; init; } = age;

        public UserTest(string name):this(name, 0) {
            Console.WriteLine($"Ctor called with name {name}");
        }
    }
}
