using System;
using UnityEngine;

public static class GameObjectExtensions
{
    public static void ExecuteIfNotNull(this GameObject gameObject, Action actionToExecute)
    {
        if (gameObject != null)
        {
            actionToExecute.Invoke();
        }
    }
}