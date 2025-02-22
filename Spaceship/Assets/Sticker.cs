using UnityEngine;

public class Sticker : MonoBehaviour
{
    public GameObject Base;
    public float zValue = 0;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(Base.transform.position.x, Base.transform.position.y,zValue);
    }
}
