using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Messenger
{
    public static UnityAction OnLoginDone;
    public static UnityAction OnInventoryDataReceived;
    public static UnityAction OnInboxReceived;
    public static UnityAction OnDailyRewardChanged;
    public static UnityAction OnClanChanged;
    public static UnityAction OnLeaderboardReceived;
    public static UnityAction OnInitialized;
    public static UnityAction OnSceneChanged;

    public static void Broadcast(UnityAction action)
    {
        if (action != null) 
            action();
    }

    public static void Broadcast<T>(UnityAction<T> action, T arg0)
    {
        if (action != null)
            action(arg0);
    }

    public static void Broadcast<T, K>(UnityAction<T, K> action, T arg0, K arg1)
    {
        if (action != null)
            action(arg0, arg1);
    }
}
