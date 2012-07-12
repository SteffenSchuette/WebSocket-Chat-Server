//-----------------------------------------------------------------------------------
// <copyright file="WebSocketChatServerTest.cs" company="Hochschule Bremen">
//     Copyright (c) 2012 Steffen Schuette. All rights reserved.
// </copyright>
// <author>Steffen Schuette</author><email>steffen.schuette@web.de</email>
// <version>0.1.0-beta</version>
//-----------------------------------------------------------------------------------

namespace WebSocketChatServer.Test
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using NUnit.Framework;

    /// <summary>
    /// Test class for the MapWrapper class.
    /// </summary>
    [TestFixture]
    public class WebSocketChatServerTest
    {
        /// <summary>
        /// WebSocketChatServer object to be tested
        /// </summary>
        private WebSocketChatServer webSocketChatServer;

        /// <summary>
        /// Test the start of the WebSocketChatServer without parameters.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException), ExpectedMessage = "Illegal arguments. Usage: <hostname> <port>")]
        public void TestWithoutArgument()
        {
            // Test without arguments
            string[] tempArgs = new string[] { };
            this.webSocketChatServer = new WebSocketChatServer(tempArgs);
            this.webSocketChatServer.StartWebSocketChatServer();
        }

        /// <summary>
        /// Test the start of the WebSocketChatServer with one parameter.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException), ExpectedMessage = "Illegal arguments. Usage: <hostname> <port>")]
        public void TestWithOneArgument()
        {
            // Test with one argument
            string[] tempArgs = new string[] { "one" };
            WebSocketChatServer webSocketChatServer = new WebSocketChatServer(tempArgs);
            webSocketChatServer.StartWebSocketChatServer();
        }

        /// <summary>
        /// Test the start of the WebSocketChatServer with three parameters.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException), ExpectedMessage = "Illegal arguments. Usage: <hostname> <port>")]
        public void TestWithThreeArguments()
        {
            // Test with three argument
            string[] tempArgs = new string[] { "one", "two", "three" };
            WebSocketChatServer webSocketChatServer = new WebSocketChatServer(tempArgs);
            webSocketChatServer.StartWebSocketChatServer();
        }
    }
}