using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Assets.FogOfWar;

public class FogOfWar : MonoBehaviour {

    #region Private
    private const int MAX_TERRAIN_HEIGHT = 600;

    [SerializeField]
    private int interpolationFrames = 6;
    [SerializeField]
    private int textureWidth;
    [SerializeField]
    private int textureHeight;
    [SerializeField]
    private Vector2 mapSize;
    [SerializeField]
    private Material fogMaterial;
    [SerializeField]
    private TextAsset heightMap;
    [SerializeField]
    private int heightMapWidth;
    [SerializeField]
    private int heightMapHeight;

    // objects that reveal fog of war
    private List<Revealer> revealers;

    private Texture2D shadowMap;
    private Color32[] pixels;
    private int[] heightMapData;
    private int interpolateStartFrame;
    private Texture2D lastShadowMap;
    #endregion

    public void SetRevealers (List<WorldObject> revealers)
    {
        this.revealers = revealers.Select(p => new Revealer(p, p.GetComponent<NavMeshAgent>())).ToList();
    }

    private void Awake()
    {
        shadowMap = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        lastShadowMap = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);

        pixels = shadowMap.GetPixels32();

        for (var i = 0; i < pixels.Length; ++i)
        {
            pixels[i] = Color.black;
        }

        shadowMap.SetPixels32(pixels);
        shadowMap.Apply();
        lastShadowMap.SetPixels32(pixels);
        lastShadowMap.Apply();

        fogMaterial.SetTexture("_ShadowMap", shadowMap);
        fogMaterial.SetTexture("_LastShadowMap", lastShadowMap);

        byte[] heightBytes = heightMap.bytes;
        heightMapData = new int[heightBytes.Length / 2];

        var j = 0;
        for (var i = 0; i < heightBytes.Length && j < heightMapData.Length; i += 2, ++j)
        {
            heightMapData[j] = (heightBytes[i + 1] << 0x08) | heightBytes[i];
        }
    }

    private void Start()
    {
        UpdateShadowMap();
    }

    private void Update()
    {
        if (Time.frameCount % interpolationFrames == 0)
        {
            lastShadowMap.SetPixels32(pixels);
            lastShadowMap.Apply();

            for (var i = 0; i < pixels.Length; ++i)
            {
                pixels[i].r = 0;
            }

            UpdateShadowMap();

            interpolateStartFrame = Time.frameCount;

            shadowMap.SetPixels32(pixels);
            shadowMap.Apply();
        }

        fogMaterial.SetFloat("_interpolationValue", (Time.frameCount - interpolateStartFrame) / (float)interpolationFrames);
    }

    private void OnDestroy()
    {
        shadowMap = null;
        pixels = null;
    }

    private void UpdateShadowMap()
    {
        foreach (var revealer in revealers)
        {
            // if the revealer is dead, ignore it
            if (!revealer.WorldObject)
            {
                return;
            }

            DrawFilledMidpointCircleSinglePixelVisit(
                revealer.WorldObject.transform.position,
                (int)revealer.WorldObject.detectionRange,
                revealer.NavMeshAgent ? revealer.NavMeshAgent.height : 0 // change 0 for default heit of non-moving objects
            );
        }
    }

    private void DrawFilledMidpointCircleSinglePixelVisit(Vector3 position, int radius, float agentHeight)
    {
        int x = Mathf.RoundToInt(radius * (textureWidth / mapSize.x));
        int y = 0;
        int radiusError = 1 - x;

        var centerX = Mathf.RoundToInt(position.x * (textureWidth / mapSize.x));
        var centerY = Mathf.RoundToInt(position.z * (textureHeight / mapSize.y));

        while (x >= y)
        {
            int startX = -x + centerX;
            int endX = x + centerX;
            FillRow(startX, endX, y + centerY, (int)position.y, agentHeight);
            if (y != 0)
            {
                FillRow(startX, endX, -y + centerY, (int)position.y, agentHeight);
            }

            ++y;

            if (radiusError < 0)
            {
                radiusError += 2 * y + 1;
            }
            else
            {
                if (x >= y)
                {
                    startX = -y + 1 + centerX;
                    endX = y - 1 + centerX;
                    FillRow(startX, endX, x + centerY, (int)position.y, agentHeight);
                    FillRow(startX, endX, -x + centerY, (int)position.y, agentHeight);
                }
                --x;
                radiusError += 2 * (y - x + 1);
            }
        }
    }

    private void FillRow(int startX, int endX, int row, int height, float agentHeight)
    {
        int index;
        for (var x = startX; x < endX; ++x)
        {
            index = x + row * textureWidth;
            if (index > -1 && index < pixels.Length && HeightCheck(x, row, height, agentHeight))
            {
                pixels[index].r = 255;
                pixels[index].g = 255;
            }
        }
    }

    private bool HeightCheck(int x, int y, int height, float agentHeight)
    {
        if (textureWidth != heightMapWidth - 1 && textureHeight != heightMapHeight - 1)
        {
            var widthRatio = (float)heightMapWidth / textureWidth;
            var heightRatio = (float)heightMapHeight / textureHeight;

            x = (int)(x * widthRatio);
            y = (int)(y * heightRatio);
        }

        if (y * heightMapWidth + x > heightMapData.Length || y * heightMapWidth + x < 0)
        {
            return false;
        }

        float convertedHeight = ((float)height / MAX_TERRAIN_HEIGHT) * ushort.MaxValue;
        float convertedAgentHeight = (agentHeight / MAX_TERRAIN_HEIGHT) * ushort.MaxValue;

        return convertedHeight >= heightMapData[y * heightMapWidth + x] - convertedAgentHeight;
    }
}
