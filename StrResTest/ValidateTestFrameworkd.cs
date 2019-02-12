using System;
using Xunit;

namespace StrResTest
{
    // just a simple test to make sure that the testing framework is functional
    public class ValidateTestFramework
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
