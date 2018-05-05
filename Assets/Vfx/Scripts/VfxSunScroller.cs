using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gfx
{
    public class VfxSunScroller : MonoBehaviour
    {
        public float scrollSpeed;
        public float scrollDistance;
        public Vector3 scrollDirection;

        private Vector3 initialPosition; 

        public void Awake()
        {
            initialPosition = transform.position;
        }

        public void Update()
        {
            transform.position += scrollSpeed * Time.deltaTime * scrollDirection;

            if ((transform.position - initialPosition).magnitude >= scrollDistance)
            {
                transform.position = initialPosition;
            }
        }
    }
}
