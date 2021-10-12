using CodeBase.DataStructures.ObjectPool;
using JetBrains.Annotations;

namespace CodeBase.Towers
{
    public class ProjectilePool : GenericObjectPool<Projectile>
    {
        public ProjectilePool(Projectile prefab, [NotNull] string additiveSceneName, int count = 0) : base(prefab, additiveSceneName, count)
        {
        }
    }
}