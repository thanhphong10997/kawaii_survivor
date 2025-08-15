using System.Collections;
using UnityEngine;
using System;

public class Candy : DroppableCurrency, ICollectable
{
    [Header("Actions")]
    public static Action<Candy> onCollected;

    protected override void Collected()
    {
        onCollected?.Invoke(this);
    }
}
