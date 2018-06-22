using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace SpaceCadetAlif.Source.Game
{
    /// <summary>
    /// Struct for all data relevant to a gun. All data is stored in a JSON file.
    /// </summary>
    struct GunData
    {
        public Guns GunType { get; }            // Name of the gun. Used to identify it and store it in armory.
        public int FireRate { get; }            // Rate at which the gun can be fired or zero if semi-auto.
        public Vector2 BarrelPos { get; }       // Position of the tip of barrel.
        public Vector2 BarrelAngledPos { get; } // Position of the tip of the barrel when gun is angled.
        public BulletData BulletData { get; }   // Data related to the bullet's attributes.

        [JsonConstructor]
        public GunData(string gunType, int fireRate, int[] barrelPos, int[] barrelAngledPos, BulletData bulletData)
        {
            // Enums and Vector2s cannot be stored in a JSON file, so some black magic fuckery is used.
            GunType = (Guns)Enum.Parse(typeof(Guns), gunType);
            FireRate = fireRate;
            BarrelPos = new Vector2(barrelPos[0], barrelPos[1]);
            BarrelAngledPos = new Vector2(barrelAngledPos[0], barrelAngledPos[1]);
            BulletData = bulletData;
        }
    }
}
