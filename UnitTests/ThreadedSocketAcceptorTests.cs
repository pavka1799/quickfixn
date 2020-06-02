﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using QuickFix;

namespace UnitTests
{
    [TestFixture]
    public class ThreadedSocketAcceptorTests
    {
        private static string Config = $@"
[DEFAULT]
StartTime = 00:00:00
EndTime = 23:59:59
ConnectionType = acceptor
SocketAcceptHost = 127.0.0.1
SocketAcceptPort = 10000
FileStorePath = {TestContext.CurrentContext.TestDirectory}\store
FileLogPath = {TestContext.CurrentContext.TestDirectory}\log
UseDataDictionary = N

[SESSION]
SenderCompID = sender
TargetCompID = target
BeginString = FIX.4.4
";

        private static SessionSettings CreateSettings(string config)
        {
            return new SessionSettings(new StringReader(config));
        }

        private static ThreadedSocketAcceptor CreateAcceptor(string config)
        {
            var settings = CreateSettings(config);
            return new ThreadedSocketAcceptor(
                new NullApplication(),
                new FileStoreFactory(settings),
                settings,
                new FileLogFactory(settings));
        }

        [Test]
        public void TestRecreation()
        {
            StartStopAcceptor(Config);
            StartStopAcceptor(Config);
            StartStopAcceptor(Config);
        }

        private static void StartStopAcceptor(string config)
        {
            var acceptor = CreateAcceptor(config);
            acceptor.Start();
            acceptor.Dispose();
        }

        private static string ConfigWildcards = $@"
[DEFAULT]
StartTime = 00:00:00
EndTime = 23:59:59
ConnectionType = acceptor
SocketAcceptHost = 127.0.0.1
SocketAcceptPort = 10000
FileStorePath = {TestContext.CurrentContext.TestDirectory}\store
FileLogPath = {TestContext.CurrentContext.TestDirectory}\log
UseDataDictionary = N

[SESSION]
SenderCompID = *
TargetCompID = *
BeginString = FIX.4.4
";
        [Test]
        public void TestRecreationWildcards()
        {
            StartStopAcceptor(ConfigWildcards);
            StartStopAcceptor(ConfigWildcards);
            StartStopAcceptor(ConfigWildcards);
        }

    }
}
