using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Persistence
{
    public static class PersistenceManager
    {
        public static BinaryFormatter AddSurrogatesToBinaryFormatter (BinaryFormatter bf)
        {
            // enable Vector3 and Quaternion serialization
            SurrogateSelector surrogateSelector = new SurrogateSelector();
            Vector3SerializationSurrogate vector3SS = new Vector3SerializationSurrogate();
            QuaternionSerializationSurrogate quaternionSS = new QuaternionSerializationSurrogate();
            ColorSerializationSurrogate colorSS = new ColorSerializationSurrogate();
            Color32SerializationSurrogate color32SS = new Color32SerializationSurrogate();
            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SS);
            surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSS);
            surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), colorSS);
            surrogateSelector.AddSurrogate(typeof(Color32), new StreamingContext(StreamingContextStates.All), color32SS);
            bf.SurrogateSelector = surrogateSelector;

            return bf;
        }
    }
}
