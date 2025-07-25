using Codice.Client.BaseCommands;
using R3.Triggers;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
namespace CustomUtility
{

#if UNITY_EDITOR
    // 진입점 체크용 클레스
    public class ButtonEditorTracker : MonoBehaviour
    {

        [Serializable]
        private struct EventTracker {
           public string className;
           public string methodName;
        }

        [ReadEditor] [SerializeField] private List<EventTracker> _eventList = new ();

        public void AddEvent(string className, string methodName) {
            _eventList.Add(new EventTracker { className = className, methodName = methodName });
        }

    }
#endif


    public static class EventHelper {
        // 기존 EventTrigger를 ObservableEventTrigger로 변경
        public static ObservableEventTrigger ToObservableEventTrigger(this EventTrigger trigger, string className, string methodName) {
#if UNITY_EDITOR
            // 진입점 체크
            trigger.gameObject.AddEventTracker(className, methodName);
#endif
            return trigger.gameObject.GetComponent<ObservableEventTrigger>() ?? trigger.gameObject.AddComponent<ObservableEventTrigger>();
           
        }
        public static void AddEventTracker<T>(this EventTrigger trigger, EventTriggerType type, Func<T> func, Action<T> action, string className, string methodName) where T : notnull {
            if (func is null)                         // func null 방지
                throw new ArgumentNullException(nameof(func));
       
            EventTrigger.Entry entry = new() {
                eventID = type
            };
            entry.callback.AddListener(e => {

                T t = func();
                action?.Invoke(t);
            
            });
            trigger.triggers.Add(entry);
            

#if UNITY_EDITOR
            // 진입점 체크
            trigger.gameObject.AddEventTracker(className, methodName);
#endif

        }

        public static void AddEventTracker(this GameObject obj, string className, string methodName) {
#if UNITY_EDITOR
            // 진입점 체크
            var tracker = obj.GetComponent<ButtonEditorTracker>() ?? obj.AddComponent<ButtonEditorTracker>();
            tracker.AddEvent(className, methodName);
#endif
        }
    }
}
