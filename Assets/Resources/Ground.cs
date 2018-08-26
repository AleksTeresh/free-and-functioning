using UnityEngine;

public class Ground : MonoBehaviour {

    public Terrain Terrain { get; private set; }

    public Material GetMaterial()
    {
        Material material = new Material(Shader.Find("Custom/FogOfWar"));
        Terrain = GetComponent<Terrain>();
        Terrain.materialType = Terrain.MaterialType.Custom;
        Terrain.materialTemplate = material;

        return material;
    }
}
