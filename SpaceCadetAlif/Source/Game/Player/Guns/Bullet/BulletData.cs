using Newtonsoft.Json;
using System;

namespace SpaceCadetAlif.Source.Game
{
    /// <summary>
    /// Struct of all data relevant to a bullet. ALl data is stored in a JSON file.
    /// </summary>
    struct BulletData
    {
        public int Damage { get; }          // Damage applied to enemies when hit.
        public float AngledSpeed { get; }   // Speed of bullet in X and Y when flying at angle.
        public float Speed { get; }         // Speed of bullet in X when flying straight.
        public OnHitEffects OnHit { get; }  // On hit effect to be applied to enemies when hit.
        public BulletPaths Path { get; }    // Trajectory of the bullet.

        [JsonConstructor]
        public BulletData(int damage, float speed, string onHit, string path)
        {
            Damage = damage;
            AngledSpeed = speed;
            Speed = (float)Math.Sqrt(2 * speed * speed); // Use pythagorean theorem to calculate speed when moving straight.
            // Enums cannot be stored in a JSON file, so some black magic fuckery is used on strings.
            OnHit = (OnHitEffects)Enum.Parse(typeof(OnHitEffects), onHit);
            Path = (BulletPaths)Enum.Parse(typeof(BulletPaths), path);
        }
    }
}
