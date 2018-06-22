using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace SpaceCadetAlif.Source.Game
{
    class Armory
    {
        public int CurrentGun { get; private set; }
        private List<Guns> mUnlockedGuns = new List<Guns>() { Guns.G32_C_Laser_Pistol,
            Guns.Flouroantimonic_Shotgun,
            Guns.IT6_7_Rail_Gun,
            Guns.Magmatic_Nail_Gun, Guns.Symbionic_Hive_Oscillator };
        private Dictionary<Guns, GunData> mArmory = new Dictionary<Guns, GunData>();

        public Armory()
        {
            CurrentGun = 0;
            Stream stream = new MemoryStream(Properties.Resources.armory);
            using (StreamReader r = new StreamReader(stream))
            {
                string json = r.ReadToEnd();
                List<GunData> gunList = JsonConvert.DeserializeObject<List<GunData>>(json);
                foreach (GunData gun in gunList)
                {
                    mArmory.Add(gun.GunType, gun);
                }
            }
        }

        public void UnlockGun(Guns gun) {
            mUnlockedGuns.Add(gun);
        }

        public GunData GetCurrentGun()
        {
            return mArmory[mUnlockedGuns[CurrentGun]];
        }

        public int NextGun()
        {
            CurrentGun++;
            if (CurrentGun >= mUnlockedGuns.Count)
            {
                CurrentGun = 0;
            }

            return CurrentGun;
        }
    }
}
