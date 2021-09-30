using UnityEngine;

namespace CodeBase.Infrastructure.AssetsManagement
{
    public interface IAssets : IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 at);
    }
}