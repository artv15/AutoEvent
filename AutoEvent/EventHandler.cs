﻿using AutoEvent.API.Enums;
using AutoEvent.Interfaces;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using InventorySystem.Items.Firearms.Attachments;

namespace AutoEvent;
internal class EventHandler
{
    private readonly AutoEvent _plugin;
    public EventHandler(AutoEvent plugin)
    {
        _plugin = plugin;

        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
        Exiled.Events.Handlers.Server.SelectingRespawnTeam += OnSelectingRespawnTeam;
        Exiled.Events.Handlers.Map.Decontaminating += OnDecontaminating;
        Exiled.Events.Handlers.Map.PlacingBulletHole += OnPlacingBulletHole;
        Exiled.Events.Handlers.Map.PickupAdded += OnPickupAdded;
        Exiled.Events.Handlers.Player.SpawningRagdoll += OnSpawningRagdoll;
        Exiled.Events.Handlers.Player.Shooting += OnShooting;
        Exiled.Events.Handlers.Player.DroppingAmmo += OnDroppingAmmo;
        Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
        Exiled.Events.Handlers.Player.Handcuffing += OnHandcuffing;
        Exiled.Events.Handlers.Player.Dying += OnDying;
    }

    ~EventHandler()
    {
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
        Exiled.Events.Handlers.Server.SelectingRespawnTeam -= OnSelectingRespawnTeam;
        Exiled.Events.Handlers.Map.Decontaminating -= OnDecontaminating;
        Exiled.Events.Handlers.Map.PlacingBulletHole -= OnPlacingBulletHole;
        Exiled.Events.Handlers.Map.PickupAdded -= OnPickupAdded;
        Exiled.Events.Handlers.Player.SpawningRagdoll -= OnSpawningRagdoll;
        Exiled.Events.Handlers.Player.Shooting -= OnShooting;
        Exiled.Events.Handlers.Player.DroppingAmmo -= OnDroppingAmmo;
        Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
        Exiled.Events.Handlers.Player.Handcuffing -= OnHandcuffing;
        Exiled.Events.Handlers.Player.Dying -= OnDying;
    }

    private void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        if (AutoEvent.EventManager.CurrentEvent is Event activeEvent)
        {
            if (!activeEvent.EventHandlerSettings.HasFlag(EventFlags.IgnoreRespawnTeam))
            {
                ev.IsAllowed = false;
            }
        }
    }

    private void OnSelectingRespawnTeam(SelectingRespawnTeamEventArgs ev)
    {
        if (AutoEvent.EventManager.CurrentEvent is Event activeEvent)
        {
            if (!activeEvent.EventHandlerSettings.HasFlag(EventFlags.IgnoreRespawnTeam))
            {
                ev.IsAllowed = false;
            }
        }
    }
    
    private void OnDecontaminating(DecontaminatingEventArgs ev)
    {
        if (AutoEvent.EventManager.CurrentEvent is Event activeEvent)
        {
            if (!activeEvent.EventHandlerSettings.HasFlag(EventFlags.IgnoreDecontaminating))
            {
                ev.IsAllowed = false;
            }
        }
    }

    private void OnPlacingBulletHole(PlacingBulletHoleEventArgs ev)
    {
        if (AutoEvent.EventManager.CurrentEvent is Event activeEvent)
        {
            if (!activeEvent.EventHandlerSettings.HasFlag(EventFlags.IgnoreBulletHole))
            {
                ev.IsAllowed = false;
            }
        }
    }

    private void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
    {
        if (AutoEvent.EventManager.CurrentEvent is Event activeEvent)
        {
            if (!activeEvent.EventHandlerSettings.HasFlag(EventFlags.IgnoreRagdoll))
            {
                ev.IsAllowed = false;
            }
        }
    }

    private void OnPickupAdded(PickupAddedEventArgs ev)
    {
        if (AutoEvent.EventManager.CurrentEvent is Event activeEvent)
        {
            if (!activeEvent.EventHandlerSettings.HasFlag(EventFlags.IgnoreDroppingAmmo) && ev.Pickup.Type.IsAmmo())
            {
                ev.Pickup.Destroy();
            }
        }
    }

    private void OnShooting(ShootingEventArgs ev)
    {
        if (AutoEvent.EventManager.CurrentEvent is Event activeEvent)
        {
            if (activeEvent.EventHandlerSettings.HasFlag(EventFlags.IgnoreInfiniteAmmo))
                return;

            if (!Extensions.InfiniteAmmoList.ContainsKey(ev.Player))
                return;

            if (ev.Firearm.Type is ItemType.ParticleDisruptor)
                return;
            
            ushort amount = 1;
            if (ev.Firearm.Type is ItemType.GunShotgun && ev.Firearm.HasAttachment(AttachmentName.ShotgunDoubleShot))
            {
                amount = 2;
            }
            else if (ev.Firearm.Type is ItemType.GunCom45)
            {
                amount = 3;
            }
            
            ev.Player.AddAmmo(ev.Firearm.AmmoType, amount);
        }
    }

    private void OnDroppingAmmo(DroppingAmmoEventArgs ev)
    {
        if (AutoEvent.EventManager.CurrentEvent is Event activeEvent)
        {
            if (!activeEvent.EventHandlerSettings.HasFlag(EventFlags.IgnoreDroppingAmmo))
            {
                ev.IsAllowed = false;
            }
        }
    }

    private void OnDroppingItem(DroppingItemEventArgs ev)
    {
        if (AutoEvent.EventManager.CurrentEvent is Event activeEvent)
        {
            if (!activeEvent.EventHandlerSettings.HasFlag(EventFlags.IgnoreDroppingItem))
            {
                ev.IsAllowed = false;
            }
        }
    }

    private void OnHandcuffing(HandcuffingEventArgs ev)
    {
        if (AutoEvent.EventManager.CurrentEvent is Event activeEvent)
        {
            if (!activeEvent.EventHandlerSettings.HasFlag(EventFlags.IgnoreHandcuffing))
            {
                ev.IsAllowed = false;
            }
        }
    }

    private void OnDying(DyingEventArgs ev)
    {
        if (AutoEvent.EventManager.CurrentEvent is null)
            return;
        
        if (!ev.IsAllowed)
            return;
            
        if (Extensions.InfiniteAmmoList is not null && Extensions.InfiniteAmmoList.ContainsKey(ev.Player))
        {
            Extensions.InfiniteAmmoList.Remove(ev.Player);
        }
    }
}