using UnityEngine;

public class DeathNote : MonoBehaviour
{
    public float timeBeforeDestroy;

    void Start()
    {
        Destroy(gameObject, timeBeforeDestroy);
    }

}
