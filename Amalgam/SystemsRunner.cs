namespace Amalgam
{
	using System;
	using System.Diagnostics;
	using System.Threading;
	using System.Threading.Tasks;

	public class SystemsRunner
	{
		private readonly SystemManager _systems;

		public Task Run(CancellationToken token)
		{
			using (var source = CancellationTokenSource.CreateLinkedTokenSource(token))
			{
				return Task.Factory.StartNew(() =>
				{
					_systems.ForEach(system => system.Initialize());

					var t = 0d;
					var watch = Stopwatch.StartNew();
					
					while(source.Token.IsCancellationRequested == false)
					{
						var currentTick = watch.Elapsed.TotalSeconds;
						var frameTime = currentTick - t;
						t = currentTick;

						_systems.ForEach(system => system.Frame(frameTime));
					}

					_systems.ForEach(system => system.Shutdown());
				}, source.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
			}
		}

		public SystemsRunner(SystemManager systems)
		{
			_systems = systems;
		}
	}
}