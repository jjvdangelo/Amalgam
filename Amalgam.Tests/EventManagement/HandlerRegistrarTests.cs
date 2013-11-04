namespace Amalgam.Tests.EventManagement
{
	using System;
	using Amalgam.EventManagement;
	using NUnit.Framework;
	using Should;
	using SpecsFor;

	public class HandlerRegistrarTests
	{
		private static Func<Action<object>, Action<object>> HandlerFactory = x => y => x(y);

		public class FakeEvent { }

		internal class given_a_handler_is_tied_to_an_event : SpecsFor<HandlerRegistrar>
		{
			[Test]
			public void then_it_should_return_the_handler_given_the_event_type()
			{
				object result = null;
				var handler = HandlerFactory(x => result = x);
				var e = new FakeEvent();

				SUT.RegisterHandlerToEvent(e.GetType(), handler);
				SUT.GetHandlersForEvent(e.GetType(), x => x(e), () => { });

				result.ShouldBeSameAs(e);
			}
		}

		internal class given_a_handler_is_being_unregistered : SpecsFor<HandlerRegistrar>
		{
			[Test]
			public void then_it_should_not_be_called()
			{
				var e = new FakeEvent();
				object result = null;
				var handler = HandlerFactory(x => result = x);
				SUT.RegisterHandlerToEvent(e.GetType(), handler);

				SUT.UnregisterHandlerFromEvent(e.GetType(), handler);
				SUT.GetHandlersForEvent(e.GetType(), x => x(e), () => { });

				result.ShouldBeNull();
			}
		}

		internal class given_handlers_have_already_been_registered_and_one_is_being_unregistered : SpecsFor<HandlerRegistrar>
		{
			[Test]
			public void then_it_should_not_remove_any_others()
			{
				var e = new FakeEvent();
				object result1 = null;
				object result2 = null;
				var handler1 = HandlerFactory(x => result1 = x);
				var handler2 = HandlerFactory(x => result2 = x);
				SUT.RegisterHandlerToEvent(e.GetType(), handler1);
				SUT.RegisterHandlerToEvent(e.GetType(), handler2);

				SUT.UnregisterHandlerFromEvent(e.GetType(), handler1);
				SUT.GetHandlersForEvent(e.GetType(), x => x(e), () => { });

				result1.ShouldBeNull();
				result2.ShouldBeSameAs(e);
			}
		}

		internal class given_no_handlers_have_been_registered : SpecsFor<HandlerRegistrar>
		{
			[Test]
			public void then_it_should_execute_the_correct_path()
			{
				bool found = false;
				bool notFound = false;

				SUT.GetHandlersForEvent(typeof(FakeEvent), x => found = true, () => notFound = true);

				found.ShouldBeFalse();
				notFound.ShouldBeTrue();
			}
		}
	}
}