using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    void Update()
    {
        Destroy(gameObject, 1.5f);
    }
}
