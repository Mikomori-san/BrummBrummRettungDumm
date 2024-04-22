using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float timeUntilDestruction = 1;

    void Update()
    {
        timeUntilDestruction -= Time.deltaTime;

        if (timeUntilDestruction <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
