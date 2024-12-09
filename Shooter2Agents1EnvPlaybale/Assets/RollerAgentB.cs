using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RollerAgentB : Agent
{
    public Rigidbody rBody;
    public float forceMultiplier = 10;
    public float bulletSpeed = 10;
    private float lastShotTime = 0.0f;
    private float shootCooldown = 1.0f;
    
    public GameObject bulletPrefab;

    public Transform Target;
    public RollerAgentA agentA;

    // Serialized field to monitor rotationAction
    [SerializeField]
    private float currentRotationAction;
    [SerializeField]
    private float[] currentOrientation;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");

        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0; // Set to 1 if space bar is pressed, otherwise 0
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(agentA.transform.localPosition);
        sensor.AddObservation(agentA.rBody.linearVelocity.x);
        sensor.AddObservation(agentA.rBody.linearVelocity.z);

        // Agent Orientation
        float angleInRadians = transform.eulerAngles.y * Mathf.Deg2Rad; // Convert degrees to radians
        float sinAngle = Mathf.Sin(angleInRadians); // Calculate sine of the angle
        float cosAngle = Mathf.Cos(angleInRadians); // Calculate cosine of the angle

        // Add sine and cosine as observations
        sensor.AddObservation(sinAngle);
        sensor.AddObservation(cosAngle);

        // Store current orientation as an array
        currentOrientation = new float[] { transform.eulerAngles.y, sinAngle, cosAngle };
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        float rotationAction = actionBuffers.ContinuousActions[0];
        currentRotationAction = rotationAction;

        // Directly increment the rotation around the y-axis
        transform.Rotate(Vector3.up, rotationAction * forceMultiplier * Time.deltaTime);

        int shootAction = actionBuffers.DiscreteActions[0]; // The first discrete action is used for shooting
        if (shootAction == 1 && Time.time >= lastShotTime + shootCooldown) // If cooldown has passed
        {
            Debug.Log("tried to shoot");
            Shot(); // Call the Shot method to instantiate and launch the bullet
            lastShotTime = Time.time; // Update the last shot time
        }
    }

    public void GrantReward(float reward)
    {
        AddReward(reward);
    }

    public void Shot()
    {
        // Instantiate the bullet at this object's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        // Get the Rigidbody component of the bullet
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // Set the bullet's velocity in this object's forward direction
        bulletRb.linearVelocity = transform.forward * bulletSpeed;
    }
}
