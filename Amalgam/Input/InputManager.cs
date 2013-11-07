namespace Amalgam.Input
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Forms;

	public abstract class Keyboard
	{
		private readonly Dictionary<Keys, Action> _actions;

		internal Dictionary<Keys, Action> Actions { get { return _actions; } }
		
		protected Keyboard(IEnumerable<KeyValuePair<Keys, Action>> actions)
		{
			_actions = actions.ThrowIfNull("actions")
							  .ToDictionary(x => x.Key, x => x.Value);
		}
	}

	public interface IKeyboardRegistrar
	{
		void Register(Keyboard keyboard);
		void Unregister(Keyboard keyboard);
	}

	public class InputManager : IKeyboardRegistrar
	{
		public static readonly InputManager Instance = new InputManager();
		private readonly KeyboardRegistrar _registrar = new KeyboardRegistrar();

		void IKeyboardRegistrar.Register(Keyboard keyboard)
		{

		}

		void IKeyboardRegistrar.Unregister(Keyboard keyboard)
		{

		}

		private InputManager() { }
		static InputManager() { }
	}
}