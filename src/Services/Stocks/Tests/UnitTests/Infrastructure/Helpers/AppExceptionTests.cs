using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Helpers;
using Xunit;

namespace Tests.UnitTests.Infrastructure.Helpers
{
    public class AppExceptionTests
    {
        [Fact]
        public void SomeMethodShouldThrowAppException()
        {
            static void Act() => throw new AppException();

            var ex = Record.Exception(Act);

            Assert.NotNull(ex);
            Assert.IsType<AppException>(ex);
        }
    }
}