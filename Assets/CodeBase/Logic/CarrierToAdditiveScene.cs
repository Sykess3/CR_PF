using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Logic
{
    public class CarrierToAdditiveScene
    {
        private Scene _scene;

        public CarrierToAdditiveScene([NotNull] string sceneName)
        {
            _scene = SceneManager.GetSceneByName(sceneName);
            if (!_scene.isLoaded)
                CreateScene(sceneName);
        }

        public void MoveInstanceToScene<T>(T instance) where T : MonoBehaviour => 
            SceneManager.MoveGameObjectToScene(instance.gameObject, _scene);

        private void CreateScene(string sceneName)
        {
            _scene = SceneManager.CreateScene(sceneName);
        }
    }
}