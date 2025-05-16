using System.Collections.Generic;
using UnityEngine;

public static class DamagableCollisionCache
{
    static readonly Dictionary<Collider2D, IDamageable> damagableCache = new();

    public static void Register(IDamageable damagable)
    {
        MonoBehaviour monoBehaviour = damagable as MonoBehaviour;
        if(monoBehaviour != null)
        {
            Collider2D collider2D = monoBehaviour.GetComponent<Collider2D>();
            damagableCache[collider2D] = damagable;
        }
    }

    public static bool TryGet(Collider2D col, out IDamageable d)
        => damagableCache.TryGetValue(col, out d);

    public static void UnRegister(IDamageable damagable)
    {
        MonoBehaviour monoBehaviour = damagable as MonoBehaviour;
        if (monoBehaviour != null)
        {
            Collider2D collider2D = monoBehaviour.GetComponent<Collider2D>();
            damagableCache.Remove(collider2D);
        }
    }

}
