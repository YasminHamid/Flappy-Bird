using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector3 startPosition;
    private float cameraSpeed = 3f;
    //this var is only to pause the movement of the camera for a few seconds before moving to the slingshot position
    private bool beingHandled = false;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        SlingShot.onShoot += SlingShot_OnShoot;
    }
    private void OnDestroy()
    {
        SlingShot.onShoot -= SlingShot_OnShoot;
    }

    public void SlingShot_OnShoot()
    {
        StartCoroutine("Reset");
    }
    // Update is called once per frame
    void Update()
    {
        if (this && !beingHandled)
            transform.position = Vector3.MoveTowards(
                transform.position, new Vector3(-8.35f, -1.1f, -10f), cameraSpeed * Time.deltaTime
                );
    }

    IEnumerator Reset()
    {
        beingHandled = true;
        transform.position = startPosition;
        // process pre-yield
        yield return new WaitForSeconds(3.0f);
        // process post-yield
        beingHandled = false;
    }
}
