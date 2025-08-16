using System;
using UnityEngine;

public class Chest : DroppableCurrency, ICollectable
{
    [Header("Actions")]
    public static Action<Chest> onCollected;

    protected override void Collected()
    {
        onCollected?.Invoke(this);
    }
}
