
using UnityEngine;

public static class TransformExtensions
{
    public static bool IsFirstSibling(this Transform transform)
    {
        return transform.GetSiblingIndex() == 0;
    }

    public static bool IsLastSibling(this Transform transform)
    {
        return transform.GetSiblingIndex() == transform.parent.childCount - 1;
    }
}
