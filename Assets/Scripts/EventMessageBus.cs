using System;
using System.Collections.Generic;

public class EventMessageBus : IDisposable
{
    // This is a very simplistic solution for event dispatching but it is enough for what we need here
    private static EventMessageBus m_instance;
    private static List<Action<string>> m_eventListeners = new List<Action<string>>();
    private static event Action<string> m_stringEvent;

    public static EventMessageBus Instance => m_instance;

    public static void RegisterListener(Action<string> eventListener)
    {
        ValidateInstance();

        m_stringEvent -= eventListener;
        m_stringEvent += eventListener;

        m_eventListeners.Add(eventListener);
    }

    public static void DispatchEventMessage(string eventMessage)
    {
        ValidateInstance();

        if (m_stringEvent != null)
        {
            m_stringEvent.Invoke(eventMessage);
        }
    }

    private static void ValidateInstance()
    {
        if (m_instance == null)
        {
            m_instance = new EventMessageBus();
        }
    }

    public void Dispose()
    {
        foreach (var eventListener in m_eventListeners)
        {
            m_stringEvent -= eventListener;
        }

        m_stringEvent = null;
        m_eventListeners.Clear();
    }
}