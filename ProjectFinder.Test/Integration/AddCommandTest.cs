using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectFinder.Command;

namespace ProjectFinder.Test.Integration
{
    [TestClass]
    public class AddCommandTest
    {
        [DataTestMethod]
        [DataRow("add alias")]
        public void TestPassedAddCommand(string argumentString)
        {
            var args = argumentString.Split(' ');
            Program.Main(args);
        }
    }
}