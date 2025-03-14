using ControlBeeTest.Utils;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace ControlBeeTest.Tests.Utils;

[TestSubject(typeof(ActorUtils))]
public class ActorUtilsTest
{
    [Fact]
    public void SendSignalByActorTest()
    {
        var actor1 = MockActorFactory.Create("Actor1");
        var actor2 = MockActorFactory.Create("Actor2");
        ActorUtils.SendSignalByActor(actor1, actor2, "Hello");
        ActorUtils.VerifyGetSignalByActor(actor2, "Hello", Times.Once);
    }
}
