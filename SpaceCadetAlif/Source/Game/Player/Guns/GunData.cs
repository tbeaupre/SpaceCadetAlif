using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace SpaceCadetAlif.Source.Game
{
    struct GunData
    {
        public Guns GunType { get; }
        public int FireRate { get; }
        public Vector2 BarrelPos { get; }
        public Vector2 BarrelAngledPos { get; }
        public BulletData BulletData { get; }

        [JsonConstructor]
        public GunData(string gunType, int fireRate, int[] barrelPos, int[] barrelAngledPos, BulletData bulletData)
        {
            GunType = (Guns)Enum.Parse(typeof(Guns), gunType);
            FireRate = fireRate;
            BarrelPos = new Vector2(barrelPos[0], barrelPos[1]);
            BarrelAngledPos = new Vector2(barrelAngledPos[0], barrelAngledPos[1]);
            BulletData = bulletData;
        }
    }
}
