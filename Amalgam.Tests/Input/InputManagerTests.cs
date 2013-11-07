namespace Amalgam.Tests.Input
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Amalgam.Input;
	using SpecsFor;
	using NUnit.Framework;
	using Should;

	public class InputManagerTests
	{
		public class GetKeyboard
		{
			public class when_requesting_an_instance_of_the_keyboard : SpecsForInputManager
			{
				private Keyboard _result;

				protected override void When()
				{
				}

				[Test]
				public void then_it_should_return_an_instance_of_a_keyboard_controller()
				{
					_result.ShouldNotBeNull();
				}
			}
		}
	}

	public class SpecsForInputManager : SpecsFor<InputManager>
	{
		protected override void InitializeClassUnderTest()
		{
			SUT = InputManager.Instance;
		}
	}
}