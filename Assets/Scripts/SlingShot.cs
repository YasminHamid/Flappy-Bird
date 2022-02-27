using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlingShot : MonoBehaviour
{
    public LineRenderer[] lineRenderers;
    public Transform[] stripPositions;
    public Transform center;
    public Transform idlePosition;

    public Vector3 currentPosition;

    private float maxLength = 3f;
    private float bottomBoundary = -3.5f;
    private float birdPositionOffset = -0.3f;
    public float force = 5f;

    bool isMouseDown;

    public GameObject birdPrefab;
    Rigidbody2D bird;
    Collider2D birdCollider;

    public static event Action onGameOver = delegate { };
    public static event Action onSuccess = delegate { };
    public static event Action onShoot = delegate { };

    //this boolean is to stop update method from calling onScucces many times before loading the next scene.
    private bool beingHandled = false;


    void Start()
    {
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);
        beingHandled = false;
        CreateBird();
    }

    void CreateBird()
    {
        if (GameManager.GetBirdShootCounter() > 0)
        {
            bird = Instantiate(birdPrefab).GetComponent<Rigidbody2D>();
            birdCollider = bird.GetComponent<Collider2D>();
            birdCollider.enabled = false;

            bird.isKinematic = true;

            ResetStrips();
        }
        else if(GameManager.GetAliveEnemiesNumber() != 0)
        {
            onGameOver();
        }
    }

    void Update()
    {
        if (!beingHandled && GameManager.GetAliveEnemiesNumber() == 0)
        {
            StartCoroutine("Success");
        }
            

        if (isMouseDown)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition
                - center.position, maxLength);

            currentPosition = ClampBoundary(currentPosition);

            SetStrips(currentPosition);

            if (birdCollider)
            {
                birdCollider.enabled = true;
            }
        }
        else
        {
            ResetStrips();
        }
    }

    IEnumerator Success()
    {
        beingHandled = true;
        onSuccess();
        // process pre-yield
        yield return new WaitForSeconds(3.0f);
        // process post-yield
        //beingHandled = false;
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        isMouseDown = false; 
        Shoot();
    }

    void Shoot()
    {
        bird.isKinematic = false;

        Scene m_Scene = SceneManager.GetActiveScene();

        if (m_Scene.name == "AngryBirdLevel2") //make the shoot in level 2, due to the distance
            force = force * 1.5f;

        Vector3 birdForce = (currentPosition - center.position) * force * -1;
        bird.velocity = birdForce;
        
        bird = null;
        birdCollider = null;

        GameManager.DecreseBirdShootCounter();

        onShoot();
        Invoke("CreateBird", 5);
    }

    void ResetStrips()
    {
        currentPosition = idlePosition.position;
        SetStrips(currentPosition);
    }

    void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (bird)
        {
            Vector3 dir = position - center.position;
            bird.transform.position = position + dir.normalized * birdPositionOffset;
            bird.transform.right = -dir.normalized;
        }
        
    }

    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
        return vector;
    }
}
