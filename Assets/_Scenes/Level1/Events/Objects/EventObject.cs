using Persistence;
using UnityEngine;

namespace Events
{
    public abstract class EventObject : MonoBehaviour
    {
        public int ObjectId { get; set; }
        [HideInInspector] public bool triggerred = false;

        public virtual EventObjectData GetData()
        {
            var data = new GateAlarmTriggerData();

            data.type = name.Contains("(") ? name.Substring(0, name.IndexOf("(")).Trim() : name;
            data.position = transform.position;
            data.rotation = transform.rotation;
            data.triggerred = triggerred;
            data.objectId = ObjectId;

            return data;
        }

        public virtual void SetData(EventObjectData data)
        {
            name = data.type;
            transform.position = data.position;
            transform.rotation = data.rotation;
            triggerred = data.triggerred;
            ObjectId = data.objectId;
        }
    }
}
