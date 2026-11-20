using UnityEngine;

public class PlayerMouseRotation : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");

        transform.Rotate(0f, mouseX * mouseSensitivity, 0f);

    }
}
