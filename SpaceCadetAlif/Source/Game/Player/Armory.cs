using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace SpaceCadetAlif.Source.Game
{
    /// <summary>
    /// Handles everything related to the gun database, unlocked guns and current gun.
    /// </summary>
    class Armory
    {
        public int CurrentGun { get; private set; }                                     // An index into the list of unlocked guns.
        private List<Guns> mUnlockedGuns = new List<Guns>() { Guns.G32_C_Laser_Pistol,  // Can be added to with UnlockGun().
            Guns.Flouroantimonic_Shotgun,
            Guns.IT6_7_Rail_Gun,
            Guns.Magmatic_Nail_Gun, Guns.Symbionic_Hive_Oscillator };
        private Dictionary<Guns, GunData> mArmory = new Dictionary<Guns, GunData>();    // Loaded from a JSON file. Contains every gun.

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

        // Returns the GunData for the current gun.
        public GunData GetCurrentGun()
        {
            return mArmory[mUnlockedGuns[CurrentGun]];
        }

        // Simple loop through the list of unlocked guns.
        public int NextGun()
        {
            if (++CurrentGun >= mUnlockedGuns.Count)
            {
                CurrentGun = 0;
            }
            return CurrentGun;
        }
    }
}
