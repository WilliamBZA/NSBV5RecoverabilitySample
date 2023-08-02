using System;
using NServiceBus;

public class MyMessageHandler :
    IHandleMessages<MyMessage>
{
    IBus bus;

    public MyMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(MyMessage message)
    {
        Console.WriteLine("Processing message {0}", message.Id);
    }
}