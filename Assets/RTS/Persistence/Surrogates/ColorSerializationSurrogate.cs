﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Persistence
{
    public class ColorSerializationSurrogate : ISerializationSurrogate
    {
        // Method called to serialize a Vector3 object
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
        {

            Color color = (Color)obj;
            info.AddValue("r", color.r);
            info.AddValue("g", color.g);
            info.AddValue("b", color.b);
            info.AddValue("a", color.a);
        }

        // Method called to deserialize a Vector3 object
        public System.Object SetObjectData(System.Object obj, SerializationInfo info,
                                           StreamingContext context, ISurrogateSelector selector)
        {

            Color color = (Color)obj;
            color.r = (float)info.GetValue("r", typeof(float));
            color.g = (float)info.GetValue("g", typeof(float));
            color.b = (float)info.GetValue("b", typeof(float));
            color.a = (float)info.GetValue("a", typeof(float));
            obj = color;
            return obj;
        }
    }
}