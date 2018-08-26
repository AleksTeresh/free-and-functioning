using UnityEngine;

namespace Assets.Gfx
{
    public class TextureScroller : MonoBehaviour
    {
        public Material scrollableMaterial;
        public Vector2 direction = new Vector2(1, 0);
        public float speed = 1.0f;

        private Vector2 currentOffset;
        private Vector2 initialOfset;

        private void Start()
        {
            currentOffset = scrollableMaterial.GetTextureOffset("_MainTex");
            initialOfset = currentOffset;
        }

        private void Update()
        {
            currentOffset += direction * speed * Time.deltaTime;
            scrollableMaterial.SetTextureOffset("_MainTex", currentOffset);
        }

        private void OnDestroy()
        {
            scrollableMaterial.SetTextureOffset("_MainTex", initialOfset);
        }
    }
}
