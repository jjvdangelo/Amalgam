namespace Amalgam.EventManagement
{
	using System;
	using NUnit.Framework;

	public class EventManagerTests
	{
		public class FakeEvent { }

		[Test]
		public void given_a_null_handler_when_registering_to_an_event()
		{
			Assert.Throws<ArgumentNullException>(() => EventManager.Instance.RegisterToEvent<FakeEvent>(null));
		}

		[Test]
		public void given_a_null_handler_when_unregistering_from_an_event()
		{
			Assert.Throws<ArgumentNullException>(() => EventManager.Instance.UnregisterFromEvent<FakeEvent>(null));
		}

		[Test]
		public void given_a_null_event_when_being_notified_of_a_new_event()
		{
			Assert.Throws<ArgumentNullException>(() => EventManager.Instance.Notify(null));
		}

		public void given_a_null_array_when_being_notified_of_new_events()
		{
			Assert.Throws<ArgumentNullException>(() => EventManager.Instance.Notify((object[])null));
		}
	}
}