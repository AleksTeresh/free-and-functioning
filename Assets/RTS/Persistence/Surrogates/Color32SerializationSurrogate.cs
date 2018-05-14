using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Persistence
{
    public class Color32SerializationSurrogate : ISerializationSurrogate
    {
        // Method called to serialize a Vector3 object
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
        {

            Color32 color = (Color32)obj;
            info.AddValue("r", color.r);
            info.AddValue("g", color.g);
            info.AddValue("b", color.b);
            info.AddValue("a", color.a);
        }

        // Method called to deserialize a Vector3 object
        public System.Object SetObjectData(System.Object obj, SerializationInfo info,
                                           StreamingContext context, ISurrogateSelector selector)
        {

            Color32 color = (Color32)obj;
            color.r = (byte)info.GetValue("r", typeof(byte));
            color.g = (byte)info.GetValue("g", typeof(byte));
            color.b = (byte)info.GetValue("b", typeof(byte));
            color.a = (byte)info.GetValue("a", typeof(byte));
            obj = color;
            return obj;
        }
    }
}
