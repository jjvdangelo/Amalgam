namespace Amalgam.Systems
{
	using System;
	using System.Collections.Generic;
	using Amalgam.Entities;
	using NUnit.Framework;
	using Should;
	using SpecsFor;
	using SpecsFor.ShouldExtensions;

	public class SystemManagerTests
	{
		public class RegisterSystem
		{
			public class given_a_new_system_that_has_not_been_registered : SpecsFor<SystemManager>
			{
				private readonly SystemBase _system = new FakeSystem(null);

				protected override void When()
				{
					SUT.RegisterSystem(_system);
				}

				[Test]
				public void then_it_should_append_the_system()
				{
					SUT.UnregisterAll().ShouldContain(_system);
				}
			}

			public class given_multiple_systems_that_have_not_been_registered : SpecsFor<SystemManager>
			{
				private readonly SystemBase[] _systems = { new FakeSystem(null), new FakeSystem(null) };

				protected override void When()
				{
					SUT.RegisterSystem(_systems);
				}

				[Test]
				public void then_it_should_append_the_systems()
				{
					var registered = SUT.UnregisterAll();
					foreach(var system in _systems)
					{
						registered.ShouldContain(system);
					}
				}
			}

			public class given_other_systems_have_already_been_registered : SpecsFor<SystemManager>
			{
				private readonly SystemBase[] _systems = { new FakeSystem(null), new FakeSystem(null) };

				protected override void Given()
				{
					SUT.RegisterSystem(new FakeSystem(null));
				}

				protected override void When()
				{
					SUT.RegisterSystem(_systems);
				}

				[Test]
				public void then_it_should_append_the_system()
				{
					var registered = SUT.UnregisterAll();
					foreach(var system in _systems)
					{
						registered.ShouldContain(system);
					}
				}
			}
		}

		public class UnregisterSystem
		{
			public class given_a_system_has_been_registered : SpecsFor<SystemManager>
			{
				private readonly SystemBase _system = new FakeSystem(null);

				protected override void Given()
				{
					SUT.RegisterSystem(_system);
				}

				protected override void When()
				{
					SUT.UnregisterSystem(_system);
				}

				[Test]
				public void then_it_should_remove_the_system()
				{
					SUT.ForEach(x => x.ShouldNotBeSameAs(_system));
				}
			}

			public class when_removing_multiple_systems_that_have_been_registered : SpecsFor<SystemManager>
			{
				private readonly SystemBase[] _systems = { new FakeSystem(null), new FakeSystem(null) };

				protected override void Given()
				{
					SUT.RegisterSystem(_systems);
				}

				protected override void When()
				{
					SUT.UnregisterSystem(_systems);
				}

				[Test]
				public void then_it_should_remove_the_systems()
				{
					var registeredSystems = new List<SystemBase>();
					SUT.ForEach(registeredSystems.Add);
					registeredSystems.ShouldBeEmpty();
				}
			}
		}

		public class UnregisterAll
		{
			public class given_systems_have_been_registered : SpecsFor<SystemManager>
			{
				private readonly SystemBase[] _systems = { new FakeSystem(null), new FakeSystem(null) };
				private SystemBase[] _result;

				protected override void Given()
				{
					SUT.RegisterSystem(_systems);
				}

				protected override void When()
				{
					_result = SUT.UnregisterAll();
				}

				[Test]
				public void then_it_should_return_the_currently_registered_systems()
				{
					_result.ShouldLookLike(_systems);
				}

				[Test]
				public void then_it_should_clear_all_systems()
				{
					var registeredSystems = new List<SystemBase>();
					SUT.ForEach(registeredSystems.Add);
					registeredSystems.ShouldBeEmpty();
				}
			}
		}

		public class ForEach
		{
			public class when_running_each_system : SpecsFor<SystemManager>
			{
				[Test]
				public void then_it_should_run_the_action_on_each_system()
				{
					var systems = new[] { new FakeSystem(null), new FakeSystem(null) };
					SUT.RegisterSystem(systems);

					SUT.ForEach(system => system.Frame(1));

					foreach(var system in systems)
					{
						system.Dt.ShouldEqual(1, Double.Epsilon);
					}
				}

				[Test]
				public void then_it_should_only_run_the_action_on_the_current_set_of_systems()
				{
					var system1 = new FakeSystem(null);
					var system2 = new FakeSystem(null);
					bool wasCalled = false;

					system1.Action = () => SUT.RegisterSystem(system2);
					system2.Action = () => wasCalled = true;

					SUT.RegisterSystem(system1);
					SUT.ForEach(x => ((FakeSystem)x).DoWork());

					wasCalled.ShouldBeFalse();
					var registeredSystems = new List<SystemBase>();
					SUT.ForEach(registeredSystems.Add);
					registeredSystems.Count.ShouldEqual(2);
				}
			}
		}

		public class FakeSystem : SystemBase
		{
			public double Dt;
			public Action Action;
			public int InitCount;
			public int ShutdownCount;

			public void DoWork()
			{
				Action();
			}

			public override void Initialize()
			{
				++InitCount;
			}

			public override void Shutdown()
			{
				++ShutdownCount;
			}

			public override void Frame(double dt)
			{
				Dt = dt;
			}

			public FakeSystem(EntityManager manager)
				: base(manager) { }
		}
	}
}