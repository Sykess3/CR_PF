using CodeBase.Infrastructure.AssetsManagement;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    public class Tools
    {
        [MenuItem("GameObject/Environment/Obstacle")]
        public static void CreateObstacle()
        {
            GameObject obstacle = Resources.Load<GameObject>(AssetsPaths.ObstacleMarker);
            Object.Instantiate(obstacle);
        }
        
        [MenuItem("GameObject/Environment/Grid")]
        public static void Create()
        {
            GameObject obstacle = Resources.Load<GameObject>(AssetsPaths.GridMarker);
            Object.Instantiate(obstacle);
        }
    }
}