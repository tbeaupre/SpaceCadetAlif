using Newtonsoft.Json;
using System;

namespace SpaceCadetAlif.Source.Game
{
    struct BulletData
    {
        public int Damage { get; }
        public float AngledSpeed { get; }
        public float Speed { get; }
        public OnHitEffects OnHit { get; }
        public BulletPaths Path { get; }

        [JsonConstructor]
        public BulletData(int damage, float speed, string onHit, string path)
        {
            Damage = damage;
            AngledSpeed = speed;
            Speed = (float)Math.Sqrt(2 * speed * speed);
            OnHit = (OnHitEffects)Enum.Parse(typeof(OnHitEffects), onHit);
            Path = (BulletPaths)Enum.Parse(typeof(BulletPaths), path);
        }
    }
}
