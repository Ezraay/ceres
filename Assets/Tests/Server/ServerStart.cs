using CardGame;
using NUnit.Framework;

namespace Tests.Server
{
    public class TestTCPServer
    {
        [SetUp]
        public void Setup()
        {
            
           
        }
        [Test]
        public void ServerStartTest(){
            var TestingServer = new TCPServer();
            TestingServer.Start();
            bool IsOpen = TestingServer.IsStarted();
            Assert.IsTrue(IsOpen);
        }
    }
}