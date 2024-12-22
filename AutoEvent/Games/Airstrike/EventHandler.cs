﻿using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.ThrowableProjectiles;
using MEC;
using PlayerRoles;

namespace AutoEvent.Games.Airstrike;
public class EventHandler
{
    Plugin _plugin;
    public EventHandler(Plugin plugin)
    {
        _plugin = plugin;
    }

    public void OnDying(DyingEventArgs ev)
    {
        if (!_plugin.Config.RespawnPlayersWithGrenades)
            return;
        
        // Timing.CallDelayed -> 1f
        ev.Player.Role.Set(RoleTypeId.ChaosConscript, RoleSpawnFlags.None);
        ev.Player.Position = _plugin.SpawnList.RandomItem().transform.position;
        ev.Player.ClearInventory();
        var item = ev.Player.AddItem(ItemType.GrenadeHE);
        Timing.CallDelayed(.1f, () =>
        {
            ev.Player.CurrentItem = item;
        });
        ev.Player.ShowHint("You have a grenade! Throw it at the people who are still alive!", 5f);
        ev.Player.IsGodModeEnabled = true;
    }

    public void OnThrownProjectile(ThrownProjectileEventArgs ev)
    {
        if (ev.Player.Role != RoleTypeId.ChaosConscript)
            return;
        
        Timing.CallDelayed(3f, () =>
        {
            var item = ev.Player.AddItem(ItemType.GrenadeHE);
            Timing.CallDelayed(.1f, () =>
            {
                ev.Player.CurrentItem = item;
            });
        });
    }

    public void OnHurting(HurtingEventArgs ev)
    {
        if (ev.DamageHandler.Type is DamageType.Explosion)
        {
            ev.IsAllowed = false;

            if (_plugin.Stage != 5)
            {
                ev.Player.Hurt(10, "Grenade");
            }
            else
            {
                ev.Player.Hurt(100, "Grenade");
            }
        }
    }
}