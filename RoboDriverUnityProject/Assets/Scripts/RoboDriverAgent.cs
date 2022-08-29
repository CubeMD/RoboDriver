using MLAgentsDebugTool.Agent;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoboDriverAgent : DebuggableAgent
{
    [SerializeField] private float movementSpeed = 8.5f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Target target;
    
    private float areaHalfSize;

    public override void Initialize()
    {
        areaHalfSize = Academy.Instance.EnvironmentParameters.GetWithDefault("AreaSize", 50f) / 2;
        movementSpeed = Academy.Instance.EnvironmentParameters.GetWithDefault("AgentMovementSpeed", movementSpeed);
        rotationSpeed = Academy.Instance.EnvironmentParameters.GetWithDefault("AgentMovementSpeed", rotationSpeed);
    }

    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.localPosition = new Vector3(Random.Range(-areaHalfSize, areaHalfSize), 0, Random.Range(-areaHalfSize, areaHalfSize));
        transform.rotation = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);
        
        target.ResetTarget();
    }

    // public override void CollectObservations(VectorSensor sensor)
    // {
    //     base.CollectObservations(sensor);
    // }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int userInputAction = 1;

        if (Input.GetKey(KeyCode.A))
        {
            userInputAction = 0;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            userInputAction = 2;
        }
        
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = userInputAction;

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];
        
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
        
        if (action == 1)
        {
            rb.velocity = transform.forward * movementSpeed;
        }
        else if (action == 0)
        {
            rb.angularVelocity = Vector3.up * -rotationSpeed;
        }
        else if (action == 2)
        {
            rb.angularVelocity = Vector3.up * rotationSpeed;
        }

        base.OnActionReceived(actions);
    }

    public void TargetCollected()
    {
        AddReward(10f);
    }
}
