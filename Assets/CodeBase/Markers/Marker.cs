using System;
using UnityEngine;

namespace CodeBase.Markers
{
    public class Marker : MonoBehaviour
    {
        private void Awake()
        {
            DestroyImmediate(gameObject);
        }
    }
}