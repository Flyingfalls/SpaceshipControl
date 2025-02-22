using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float ScrollSpeed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(-ScrollSpeed, 0), Space.World);
    }
}
