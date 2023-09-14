using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class EventManager
{
	private static EventManager eventManager = null;

	private Dictionary<string, List<Action<object>>> eventDictionary;

	public EventManager()
	{
	}

	public static EventManager instance
	{
		get
		{
			if (eventManager == null)
			{
				eventManager = new EventManager();
				eventManager.Init();
			}

			return eventManager;
		}
	}

	void Init()
	{
		if (eventDictionary == null)
		{
			eventDictionary = new Dictionary<string, List<Action<object>>>();
		}
	}

	public static void StartListening(string eventName, Action<object> listener)
	{
		if (instance.eventDictionary.TryGetValue(eventName, out var listeners))
		{
			if (!listeners.Contains(listener))
			{
				listeners.Add(listener);
			}
		}
		else
		{
			var list = new List<Action<object>>();
			list.Add(listener);
			instance.eventDictionary.Add(eventName, list);
		}
	}

	public static void StopListening(string eventName, Action<object> listener)
	{
		if (eventManager == null)
			return;

		if (instance.eventDictionary.TryGetValue(eventName, out var listeners))
		{
			if (listeners.Contains(listener))
			{
				listeners.Remove(listener);
			}
		}
	}

	public static void TriggerEvent(string eventName, object data = null)
	{
		if (instance.eventDictionary.TryGetValue(eventName, out var listeners))
		{
            for (int i = 0; i < listeners.Count; i++)
                listeners[i].Invoke(data);
		}
	}
}