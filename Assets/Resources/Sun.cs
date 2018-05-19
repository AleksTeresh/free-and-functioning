using Persistence;
using UnityEngine;

public class Sun : MonoBehaviour
{
    //wrapper class for the main light in the scene

    public SunData GetData()
    {
        var data = new SunData();

        data.rotation = transform.rotation;
        data.position = transform.position;
        data.scale = transform.localScale;

        return data;
    }

    public void SetData (SunData data)
    {
        transform.rotation = data.rotation;
        transform.position = data.position;
        transform.localScale = data.scale;
    }
}
