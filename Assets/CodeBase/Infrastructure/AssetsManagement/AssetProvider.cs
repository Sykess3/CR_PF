using UnityEngine;

namespace CodeBase.Infrastructure.AssetsManagement
{
    public class AssetProvider : IAssets
    {
        public GameObject Instantiate(string path)
        {
            var gameObject = Resources.Load<GameObject>(path);
            return Object.Instantiate(gameObject);
        }
        public GameObject Instantiate(string path, Vector3 at)
        {
            var gameObject = Resources.Load<GameObject>(path);
            return Object.Instantiate(gameObject, at, Quaternion.identity);
        }
    }
}