namespace Amalgam.Tests
{
	using NUnit.Framework;
	using Should;
	using SpecsFor;

	public class EntityManagerTests
    {
		public class CreateEntity
		{
			public class given_a_request_to_create_a_new_entity : SpecsFor<EntityManager>
			{
				private Entity _result;

				protected override void When()
				{
					_result = SUT.CreateEntity();
				}

				[Test]
				public void then_it_should_return_a_new_entity_with_an_incremented_id()
				{
					_result.ShouldEqual(new Entity(SUT, 1));
				}
			}
		}
    }
}