using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.tj.Events
{
    public class EventManager<T> : MonoBehaviour where T : BaseEvent
    {
        public delegate void OnEventHandlerDelegate(T evt);

        private static Dictionary<System.Type , List<OnEventHandlerDelegate>> listeners;
        private static Dictionary<System.Type, List<OnEventHandlerDelegate>> Listeners
        {
            get
            {
                if (listeners == null)
                    listeners = new Dictionary<System.Type, List<OnEventHandlerDelegate>>();

                return listeners;
            }
        }

        public static void RegisterListener(OnEventHandlerDelegate listener) 
        {
            if (!Listeners.ContainsKey(typeof(T)))
                Listeners[typeof(T)] = new List<OnEventHandlerDelegate>();

              Listeners[typeof(T)].Add(listener);
        }

        public static void RemoveListener(OnEventHandlerDelegate listener)
        {
            if (!Listeners.ContainsKey(typeof(T)))
                return;

            Listeners[typeof(T)].Remove(listener);

        }

        public static void DispatchEvent(T evt)
        {
            foreach (OnEventHandlerDelegate listener in Listeners[typeof(T)])
                listener.Invoke(evt);
        }

    }
}