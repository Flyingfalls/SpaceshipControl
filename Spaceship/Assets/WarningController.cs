using UnityEngine;

public class WarningController : MonoBehaviour
{
    public float WarningLength = 10;
    // Update is called once per frame
    void Update()
    {       

        WarningLength = WarningLength - Time.deltaTime;
        if(WarningLength < 0)
        {
            Destroy(gameObject);
        }
    }
}
