using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Move : NetworkBehaviour
{

    //The Ship moves this amount every 0.02 Seconds.
    public float speed = 0.01f;
    public float turnSpeed = 0.01f;
    public float targetHeading = -45;
    public float currentHeading = 0;

    public string TurnMessage = "";

    public static GameObject InstanceShip;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<NetworkObject>().SetOwnershipStatus(NetworkObject.OwnershipStatus.Distributable);
        InstanceShip = gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateHeading();

        //Moves the ship foward
        transform.Translate(new Vector2(0,speed), Space.Self);


        //Rotates the ship to the target heading.
        if(!FindShortestAngle(currentHeading,targetHeading))
        {
            //print("Turning Left " + currentHeading + " < " + targetHeading);
            transform.Rotate(new Vector3(0, 0, turnSpeed), Space.Self);
        }
        else if(FindShortestAngle(currentHeading, targetHeading))
        {
            //print("Turning Right " + currentHeading + " > " + targetHeading);
            transform.Rotate(new Vector3(0, 0, -turnSpeed), Space.Self);
        }
    }

    //Finds which direction from angle1 will be shortest to angle2. true = right, and false = left.
    public bool FindShortestAngle(float Angle1 , float Angle2)
    {
        float RightDifference = Angle2 - Angle1;
        float LeftDifference = Angle1 - Angle2;

        if (RightDifference < 0)
            RightDifference = RightDifference + 360;
        if (RightDifference > 360)
            RightDifference = RightDifference - 360;
        if (LeftDifference < 0)
            LeftDifference = LeftDifference + 360;
        if (LeftDifference > 360)
            LeftDifference = LeftDifference - 360;

        //print("RightDifference " + RightDifference + " LeftDifference " + LeftDifference);

        if (RightDifference < LeftDifference)
            return true;
        else
            return false;

    }

    public void UpdateHeading()
    {
        // Sets the currentheading to the inverse eulerangel of the actual heading to stay consistent with the in game navigation.
        currentHeading = 360 - transform.rotation.eulerAngles.z;

        //If the heading falls bellow zero, it loops around to 360.
        if (targetHeading < 0)
            targetHeading += 360;
        else if (targetHeading > 360)
            targetHeading -= 360;
    }




    [Rpc(SendTo.Server)]
    public void turnRpc(float Degrees)
    {
        targetHeading += Degrees;

        UpdateHeading();

        CommandPromt.InstanceCommandPromt.DisplayCommandRpc(
            "< Initiating RCS Startup Sequence >\n " +
            "\n " +
            "Attempting Ignition of RCS Array 1….\n" +
            "RCS Array 1 Ignition Failed!\n " +
            "\n " +
            "Attempting Ignition of RCS Array 2….\n " +
            "RCS Array 2 Ignition Successful! \n " +
            "\n " +
            "Attempting Ignition of RCS Array 3….\n " +
            "RCS Array 3 Ignition Successful!\n " +
            "\n " +
            "Attempting Ignition of RCS Array 4….\n " +
            "RCS Array 4 Ignition Failed!\n " +
            "\n"
            + "\n<Turning Sequence Intiated. Turning ship to " + targetHeading + " degrees>");
    }

    [Rpc(SendTo.Server)]
    public void setSpeedRpc(float newSpeed)
    {
        speed = newSpeed;
    }

    [Rpc(SendTo.Server)]
    public void setTurnRateRpc(float newSpeed)
    {
        turnSpeed = newSpeed;
    }

    //[Rpc(SendTo.Server)]
    //public void setTurnRpc(float newHeading)
    //{
    //    transform.Rotate(new Vector3(0, 0, newHeading), Space.Self);
    //}

    [Rpc(SendTo.Server)]
    public void jumpRpc()
    {
        transform.Translate(new Vector2(0, 5), Space.Self);
    }

}
