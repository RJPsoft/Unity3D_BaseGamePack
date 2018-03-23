using UnityEngine;

public class BringToFront : MonoBehaviour
{

    void Awake()
    {
        transform.SetAsLastSibling();
    }
}
