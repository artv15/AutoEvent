﻿using MapEditorReborn.API.Features.Objects;
using MEC;
using PluginAPI.Core;
using UnityEngine;

namespace AutoEvent.Games.Glass.Features;
public class GlassComponent : MonoBehaviour
{
    private BoxCollider collider;
    public float RegenerationDelay { get; set; } = 0;
    public void Init(float regenerationDelay)
    {
        RegenerationDelay = regenerationDelay;
    }
    private void Start()
    {
        collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(1, 10, 1);
    }
    void OnTriggerEnter(Collider other)
    {
        if (Player.Get(other.gameObject) is Player)
        {
            if (gameObject.TryGetComponent(out PrimitiveObject objectToy))
            {
                objectToy.IsStatic = false;
                objectToy.Position += Vector3.down * 5;
                
                if (RegenerationDelay > 0)
                {
                    Timing.CallDelayed(RegenerationDelay, () =>
                    {
                        objectToy.Position += Vector3.up * 5;
                    });
                }
            }
        }
    }
}
