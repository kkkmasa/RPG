using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    GameObject cam;
    [SerializeField] float parallaxEffect;

    float xPosition;
    void Start()
    {
        cam = GameObject.Find("Main Camera");

        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
    }
}
