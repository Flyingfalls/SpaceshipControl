using UnityEngine;
using Unity.Netcode;

public class HostController : NetworkBehaviour
{
    public GameObject Asteriod = null;

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.LocalClient.IsSessionOwner)
        {
            print("Attempting to place asteriod");
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(Asteriod, Input.mousePosition, new Quaternion(0, 0, 0, 0));
            }
        }       
    }
}
