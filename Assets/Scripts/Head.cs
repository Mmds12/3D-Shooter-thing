using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    public float bobbingAmount = 0.05f;
    public float bobbingSpeed = 5f;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if(!PlayerMovement.sliding)
        {

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                float verticalMovement = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;

                transform.localPosition = originalPosition + new Vector3(0f, verticalMovement, 0f);
            }
            else
            {
                transform.localPosition = originalPosition;
            }

        }
        
    }
}
