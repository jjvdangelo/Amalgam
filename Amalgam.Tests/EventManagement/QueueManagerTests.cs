namespace Amalgam.EventManagement
{
	using NUnit.Framework;
	using Should;
	using SpecsFor;

	public class QueueManagerTests
	{
		internal class given_an_event_has_been_registered : SpecsFor<QueueManager>
		{
			public class FakeEvent { }

			[Test]
			public void then_it_should_return_the_registered_event()
			{
				var e = new FakeEvent();
				SUT.RegisterEvent(e);

				var events = SUT.GetEvents();

				events.ShouldContain(e);
				SUT.GetEvents().ShouldBeEmpty();
			}
		}
	}
}