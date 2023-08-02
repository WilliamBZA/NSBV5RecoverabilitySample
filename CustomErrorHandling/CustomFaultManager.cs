using System;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Faults;
using NServiceBus.Logging;
using NServiceBus.Transports;
using NServiceBus.Unicast;

public class CustomFaultManager :
    IManageMessageFailures
{
    ISendMessages sender;
    MessageForwardingInCaseOfFaultConfig config;
    static ILog Log = LogManager.GetLogger(typeof(CustomFaultManager));
    Address localAddress;

    public CustomFaultManager(ISendMessages sender, IProvideConfiguration<MessageForwardingInCaseOfFaultConfig> config)
    {
        this.sender = sender;
        this.config = config.GetConfiguration();
    }

    #region MoveToErrorQueue
    public void SerializationFailedForMessage(TransportMessage message, Exception e)
    {
        // Nothing to do here
    }

    public void ProcessingAlwaysFailsForMessage(TransportMessage message, Exception e)
    {
        if (e is MyCustomException)
        {
            // Send this message to the queue dedicated to processing the failing messages
            SendToSecondaryQueue(message);
        }
    }
    #endregion

    void SendToSecondaryQueue(TransportMessage message)
    {
        // Use the name of the secondary endpoint here
        sender.Send(message, new SendOptions("Samples.SecondaryCustomErrorHandling"));
        Log.WarnFormat("Message {0} was moved to the secondary queue.", message.Id);
    }

    public void Init(Address address)
    {
        localAddress = address;
    }
}