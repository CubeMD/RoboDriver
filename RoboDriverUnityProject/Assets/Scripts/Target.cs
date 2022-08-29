using UnityEngine;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour
{
    private const float SPAWN_DISTANCE = 25f;
    
    private void OnEnable()
    {
        ResetTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out RoboDriverAgent driverAgent))
        {
            driverAgent.TargetCollected();
            ResetTarget();
        }
    }

    public void ResetTarget()
    {
        Vector3 offset = new Vector3(
            Random.Range(-SPAWN_DISTANCE, SPAWN_DISTANCE),
            1.5f,
            Random.Range(-SPAWN_DISTANCE, SPAWN_DISTANCE));

        transform.localPosition = offset;
    }
}
