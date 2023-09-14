using System;

namespace ExternalScripts
{
	public class EventController
	{
		public static EventController instance = new EventController();

		public EventController()
		{
		}

		public static void addEventListener(string type, Action<object> listener)
		{
			EventManager.StartListening(type, listener);
		}

		public static void removeEventListener(string type, Action<object> listener)
		{
			EventManager.StopListening(type, listener);
		}

		public static void dispatchEventWith(string type, object data = null)
		{
			EventManager.TriggerEvent(type, data);
		}
	}
}