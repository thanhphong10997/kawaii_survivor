using System.Collections;
using UnityEngine;
using System;

public class Cash : DroppableCurrency, ICollectable
{
    [Header("Actions")]
    public static Action<Cash> onCollected;

    protected override void Collected()
    {
        onCollected?.Invoke(this);
    }
}
