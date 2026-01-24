using System;
using Rage;
using Rage.Native;

namespace Adam69Callouts.Common
{
    /// <summary>
    /// Centralized, defensive helpers for working with Ped inventory / weapons.
    /// Guards against invalid PedInventory and falls back to native functions.
    /// </summary>
    public static class SafeInventory
    {
        public static bool TryPedHasWeapon(Ped ped, WeaponHash weapon)
        {
            if (ped == null || !ped.Exists() || !ped.IsValid()) return false;

            try
            {
                return ped.Inventory != null && ped.Inventory.Weapons.Contains(weapon);
            }
            catch
            {
                try
                {
                    return (bool)NativeFunction.Natives.HAS_PED_GOT_WEAPON(ped, (int)weapon);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool TryPedHasWeapon(Ped ped, string weaponName)
        {
            if (string.IsNullOrEmpty(weaponName)) return false;

            if (Enum.TryParse(typeof(WeaponHash), weaponName, true, out var enumVal))
            {
                return TryPedHasWeapon(ped, (WeaponHash)enumVal);
            }

            if (ped == null || !ped.Exists() || !ped.IsValid()) return false;

            try
            {
                // fallback: use native by hash of the string name
                var hash = Game.GetHashKey(weaponName);
                return (bool)NativeFunction.Natives.HAS_PED_GOT_WEAPON(ped, (int)hash);
            }
            catch
            {
                return false;
            }
        }

        public static void SafeGiveWeapon(Ped ped, WeaponHash weapon, int ammo = 0, bool equip = true)
        {
            if (ped == null || !ped.Exists() || !ped.IsValid()) return;

            try
            {
                ped.Inventory.GiveNewWeapon(weapon, ammo, equip);
            }
            catch
            {
                try
                {
                    NativeFunction.Natives.GIVE_WEAPON_TO_PED(ped, (int)weapon, ammo, false, equip);
                }
                catch
                {
                    // best-effort - swallow
                }
            }
        }

        public static void SafeGiveWeapon(Ped ped, string weaponName, int ammo = 0, bool equip = true)
        {
            if (string.IsNullOrEmpty(weaponName)) return;

            if (Enum.TryParse(typeof(WeaponHash), weaponName, true, out var enumVal))
            {
                SafeGiveWeapon(ped, (WeaponHash)enumVal, ammo, equip);
                return;
            }

            if (ped == null || !ped.Exists() || !ped.IsValid()) return;

            try
            {
                // Attempt native fallback using hash of name
                var hash = Game.GetHashKey(weaponName);
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(ped, (int)hash, ammo, false, equip);
            }
            catch
            {
                // swallow - best effort
            }
        }

        public static void SafeEquipWeapon(Ped ped, WeaponHash weapon)
        {
            if (ped == null || !ped.Exists() || !ped.IsValid()) return;

            try
            {
                ped.Inventory.EquippedWeapon = weapon;
            }
            catch
            {
                try
                {
                    NativeFunction.Natives.SET_CURRENT_PED_WEAPON(ped, (int)weapon, true);
                }
                catch
                {
                    // swallow - best effort
                }
            }
        }

        public static void SafeEquipWeapon(Ped ped, string weaponName)
        {
            if (string.IsNullOrEmpty(weaponName)) return;

            if (Enum.TryParse(typeof(WeaponHash), weaponName, true, out var enumVal))
            {
                SafeEquipWeapon(ped, (WeaponHash)enumVal);
                return;
            }

            if (ped == null || !ped.Exists() || !ped.IsValid()) return;

            try
            {
                var hash = Game.GetHashKey(weaponName);
                NativeFunction.Natives.SET_CURRENT_PED_WEAPON(ped, (int)hash, true);
            }
            catch
            {
                // swallow - best effort
            }
        }
    }
}