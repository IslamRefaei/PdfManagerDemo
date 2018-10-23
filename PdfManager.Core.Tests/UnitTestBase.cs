using Autofac.Extras.Moq;
using NUnit.Framework;

namespace PdfManager.Core.Tests
{
    [TestFixture]
    public abstract class UnitTestBase
    {
        protected AutoMock AutoMock { get; private set; }

        [TearDown]
        protected virtual void TearDown()
        {
            AutoMock.Dispose();
        }

        [SetUp]
        protected virtual void SetUp()
        {
            AutoMock = AutoMock.GetLoose();
        }
    }
}
