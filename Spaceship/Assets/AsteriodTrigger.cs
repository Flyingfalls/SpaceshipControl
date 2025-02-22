using UnityEngine;

public class AsteriodTrigger : MonoBehaviour
{
    public float TriggerRange = 2;
    public GameObject WarningDisplay = null;
    public Vector3 WarningPosition = new Vector3(0,0,0);
    public GameObject WarningDisplayRadar = null;
    public void Update()
    {
        if (Vector2.Distance(Move.InstanceShip.transform.position, gameObject.transform.position) < TriggerRange)
        {
            Destroy(gameObject);
            Instantiate(WarningDisplay,WarningPosition, new Quaternion(0,0,0,0));

            GameObject newWarning = Instantiate(WarningDisplayRadar);
            newWarning.GetComponent<Sticker>().Base = Move.InstanceShip;
        }

    }
}
