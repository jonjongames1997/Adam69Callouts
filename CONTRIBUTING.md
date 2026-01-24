# CONTRIBUTING.md

## Guidelines

- Always check for null and call `Exists()` before operating on Rage entities (Ped, Vehicle, Blip, etc.).
- When accessing `Ped.Inventory`, guard access with try/catch because `PedInventory` can be invalid; fall back to native functions (`GIVE_WEAPON_TO_PED`, `SET_CURRENT_PED_WEAPON`, `HAS_PED_GOT_WEAPON`) when Inventory is unavailable.
- Use utility helpers in `Adam69Callouts.Common.SafeInventory` for safe inventory operations. These helpers centralize try/catch logic, provide string and enum overloads for weapon identifiers, and fall back to natives when required.
- Use `.IsValid()` to confirm entities where appropriate.
- Follow the .editorconfig rules for formatting.

## SafeInventory Usage (recommended)

- Prefer `SafeInventory.TryPedHasWeapon(ped, weapon)` over direct `ped.Inventory` checks.
- Prefer `SafeInventory.SafeGiveWeapon(ped, weapon, ammo, equip)` over `ped.Inventory.GiveNewWeapon(...)`.
- Prefer `SafeInventory.SafeEquipWeapon(ped, weapon)` over setting `ped.Inventory.EquippedWeapon` directly.

### Examples

```csharp
using Adam69Callouts.Common;

if (suspect != null && suspect.Exists() && suspect.IsValid())
{
    if (!SafeInventory.TryPedHasWeapon(suspect, "WEAPON_COMBATPISTOL"))
    {
        SafeInventory.SafeGiveWeapon(suspect, "WEAPON_COMBATPISTOL", 500, true);
    }
}
```

### Rationale

- `PedInventory` may be invalid for some ped instances and will throw exceptions when accessed. Centralizing the guards and fallbacks reduces duplicated try/catch blocks and the chance of unhandled exceptions.

## Code style & formatting

- Follow rules in `.editorconfig`.
- Namespace convention: project code uses the `Adam69Callouts` root namespace and `Adam69Callouts.Callouts` for callouts and `Adam69Callouts.Common` for helpers.
- Log using `LoggingManager.Log` and `Game.LogTrivial` according to the project's logging pattern. Preserve `Settings.EnableLogs` guard usage.

## File updates

- Whenever modifying callout files to use `SafeInventory`, ensure you add `using Adam69Callouts.Common;` at the top of the file.
- Always check entity validity before calling `AttachBlip`, `Delete`, `Dismiss`, `Tasks`, or inventory operations.