using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class RollerAgent : Agent
{
    // Start is called before the first frame update
    Rigidbody rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
public Transform Target;
public override void OnEpisodeBegin(){

        if(this.transform.localPosition.y <0){
            //if iiit falls stop its movement
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3( 0, 0.5f, 0);
        }


        // move target 
        Target.localPosition =new Vector3(Random.value * 8-4,.5f,Random.value*8-4);
}



public override void CollectObservations(VectorSensor sensor){

    //target and agent positions
    sensor.AddObservation(Target.localPosition);
    sensor.AddObservation(this.transform.localPosition);


    //agent velocoty

    sensor.AddObservation(rBody.velocity.x);
    sensor.AddObservation(rBody.velocity.z);



}




public float forceMultiplier = 10;
public override void OnActionReceived(float[] vectorAction)
{
    // Actions, size = 2
    Vector3 controlSignal = Vector3.zero;
    controlSignal.x = vectorAction[0];
    controlSignal.z = vectorAction[1];
    rBody.AddForce(controlSignal * forceMultiplier);

    // Rewards
    float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

    // Reached target
    if (distanceToTarget < 1.42f)
    {
        SetReward(1.0f);
        EndEpisode();
    }

    // Fell off platform
    if (this.transform.localPosition.y < 0)
    {
        EndEpisode();
    }
}








public override void Heuristic(float[] actionsOut)
{
    actionsOut[0] = Input.GetAxis("Horizontal");
    actionsOut[1] = Input.GetAxis("Vertical");
}







   
}
