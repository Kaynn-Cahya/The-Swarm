using Entities;
using MyBox;

namespace Managers {
    public class CacheManager : MonoSingleton<CacheManager> {

        internal GameCache<Enemy> EnemyCache { get; private set; }

        protected override void OnAwake() {
            EnemyCache = new GameCache<Enemy>();
        }
    }
}