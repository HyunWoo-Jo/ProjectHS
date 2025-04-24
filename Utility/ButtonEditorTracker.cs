using System;
using System.Collections.Generic;
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

        [ReadOnly] [SerializeField] private List<EventTracker> _eventList = new ();

        public void AddEvent(string className, string methodName) {
            _eventList.Add(new EventTracker { className = className, methodName = methodName });
        }

    }
#endif


    public static class EventHelper {
        public static void AddTrigger(this EventTrigger trigger, EventTriggerType type, Action action, string className, string methodName) {
            EventTrigger.Entry entry = new() {
                eventID = type
            };
            entry.callback.AddListener(e => { action?.Invoke(); });
            trigger.triggers.Add(entry);

#if UNITY_EDITOR
            // 진입점 체크
            var tracker = trigger.gameObject.GetComponent<ButtonEditorTracker>() ?? trigger.gameObject.AddComponent<ButtonEditorTracker>();
            tracker.AddEvent(className, methodName);
#endif

        }
    }
}
