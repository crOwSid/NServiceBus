﻿namespace NServiceBus.AcceptanceTests.Sagas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EndpointTemplates;
    using AcceptanceTesting;
    using Hosting;
    using NUnit.Framework;

    public class When_an_endpoint_is_started : NServiceBusAcceptanceTest
    {
        [Test]
        public void Host_information_should_be_available_through_DI()
        {
            var context = new Context();

            Scenario.Define(context)
                    .WithEndpoint<MyEndpoint>()
                    .Done(c => c.HostId != Guid.Empty)
                    .Run();

            Console.Out.WriteLine(context.HostDisplayName);
            Console.Out.WriteLine(string.Join(Environment.NewLine,context.HostProperties.Select(kvp => string.Format("{0}:{1}", kvp.Key, kvp.Value)).ToList()));
        }

        public class Context : ScenarioContext
        {
            public Guid HostId { get; set; }
            public string HostDisplayName { get; set; }

            public Dictionary<string, string> HostProperties { get; set; }
        }

        public class MyEndpoint : EndpointConfigurationBuilder
        {
            public MyEndpoint()
            {
                EndpointSetup<DefaultServer>();
            }

            class MyStartUpTask:IWantToRunWhenBusStartsAndStops
            {
                public HostInformation HostInformation { get; set; }

                public Context Context { get; set; }
                public void Start()
                {
                    Context.HostId = HostInformation.HostId;
                    Context.HostDisplayName = HostInformation.DisplayName;
                    Context.HostProperties = HostInformation.Properties;
                }

                public void Stop()
                {
                    
                }
            }
         
        }
    }
}