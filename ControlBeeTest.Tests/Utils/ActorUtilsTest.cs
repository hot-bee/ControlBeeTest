using System;
using ControlBee.Exceptions;
using ControlBee.Models;
using ControlBee.Services;
using ControlBeeTest.Utils;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace ControlBeeTest.Tests.Utils;

[TestSubject(typeof(ActorUtils))]
public class ActorUtilsTest : ActorFactoryBase
{
    [Fact]
    public void SendSignalByActorTest()
    {
        var actor1 = MockActorFactory.Create("Actor1");
        var actor2 = MockActorFactory.Create("Actor2");
        ActorUtils.SendSignalByActor(actor1, actor2, "Hello");
        ActorUtils.VerifyGetSignalByActor(actor1, actor2, "Hello", Times.Once);
    }

    [Fact]
    public void EnsureAllStatusFalseTest()
    {
        var actor = ActorFactory.Create<Actor>("MyActor");
        actor.SetStatus("ReadyToDo", Guid.Empty);
        actor.SetStatus("Done", false);
        actor.SetStatusByActor("Peer", "ReadyToDo", Guid.Empty);
        actor.SetStatusByActor("Peer", "Done", false);
        ActorUtils.EnsureAllStatusFalse(actor);
    }

    [Fact]
    public void EnsureAllStatusFalseWithGuidTest()
    {
        var actor = ActorFactory.Create<Actor>("MyActor");
        actor.SetStatusByActor("Peer", "ReadyToDo", Guid.NewGuid());
        Assert.Throws<ValueError>(() => ActorUtils.EnsureAllStatusFalse(actor));
    }

    [Fact]
    public void SetupActionOnSignalTest()
    {
        var sendMock = new SendMock();
        var actor1 = MockActorFactory.Create("Actor1");
        var actor2 = MockActorFactory.Create("Actor2");

        var callCount = 0;
        sendMock.SetupActionOnSignal(
            actor1,
            actor2,
            "Hello",
            message =>
            {
                callCount++;
            }
        );
        ActorUtils.SendSignal(actor1, actor2, "Hello");
        ActorUtils.SendSignal(actor1, actor2, "Hello");
        Assert.Equal(1, callCount);
    }
}
