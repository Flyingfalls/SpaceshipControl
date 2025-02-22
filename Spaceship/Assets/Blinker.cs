using UnityEngine;

public class Blinker : MonoBehaviour
{
    public float BlinkRate = 0.5f;
    private float currentBlink = 0;

    // Update is called once per frame
    void Update()
    {
        if (currentBlink <= 0)
        {
            currentBlink = BlinkRate;
            if (gameObject.GetComponent<SpriteRenderer>().isVisible == false)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else
        {
            currentBlink = currentBlink - Time.deltaTime;
        }
    }
}
