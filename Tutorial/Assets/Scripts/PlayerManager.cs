using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Speed")]
    [Range(0f, 1f)][SerializeField] private float maxSpeed;
    [Range(0f, 1f)][SerializeField] private float camSpeed;
    [Range(0f, 20f)][SerializeField] private float pathSpeed; 

    [Header("Particle")]
    [SerializeField] private ParticleSystem colliderParticale;
    [SerializeField] private ParticleSystem airEffect;
    [SerializeField] private ParticleSystem dustEffect;
    [SerializeField] private ParticleSystem ballTrialEffect;

    [SerializeField] private Transform path; 

    private Transform ball;
    private Vector3 startMousePos, startBallPos;
    private bool moveTheBall;
    private float velocity, camVelocityX, camVelocityY;
    private Camera cam;
    private Rigidbody rb;
    private Collider col;
    private Renderer ballRenderer;


    private void Start()
    {
        ball = transform;
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        ballRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && MenuManager.menuManager.GameState)
        {
            moveTheBall = true;
            ballTrialEffect.Play(); 
            Plane newPlane = new Plane(Vector3.up, 0f);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (newPlane.Raycast(ray, out var distance))
            {
                startMousePos = ray.GetPoint(distance);
                startBallPos = ball.position;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            moveTheBall = false;
        }
        if (moveTheBall)
        {
            Plane newPlane = new Plane(Vector3.up, 0f);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (newPlane.Raycast(ray, out var distance))
            {
                Vector3 mouseNewPos = ray.GetPoint(distance);
                Vector3 desireBallPos = mouseNewPos + startBallPos;

                desireBallPos.x = Mathf.Clamp(desireBallPos.x, -1.5f, 1.5f);

                ball.position = new Vector3(Mathf.SmoothDamp(ball.position.x, desireBallPos.x, ref velocity, maxSpeed), ball.position.y, ball.position.z);
            }
        }
        if (MenuManager.menuManager.GameState)
        {
            var pathNewPos = path.position;
            path.position = new Vector3(pathNewPos.x, pathNewPos.y, Mathf.MoveTowards(pathNewPos.z, -100000f, pathSpeed * Time.deltaTime));
        }
    }
    private void LateUpdate()
    {
        var cameraNewPos = cam.transform.position;
        if (rb.isKinematic)
        {
            cam.transform.position = new Vector3(Mathf.SmoothDamp(cameraNewPos.x, ball.transform.position.x, ref camVelocityX, camSpeed),
                Mathf.SmoothDamp(cameraNewPos.y, ball.transform.position.y + 3f, ref camVelocityY, camSpeed), cameraNewPos.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            MenuManager.menuManager.GameState = false;
            var newParticale = Instantiate(dustEffect, transform.position, Quaternion.identity);
            newParticale.GetComponent<Renderer>().material = ballRenderer.material;
            Destroy(gameObject);
        }
        switch (other.tag)
        {
            case "Red":
                other.gameObject.SetActive(false);
                ballRenderer.material = other.GetComponent<Renderer>().material;
                var newParticale = Instantiate(colliderParticale, transform.position, Quaternion.identity); 
                newParticale.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                ballTrialEffect.GetComponent<Renderer>().material = ballRenderer.material;
                break;
            case "Green":
                other.gameObject.SetActive(false);
                ballRenderer.material = other.GetComponent<Renderer>().material;
                var newParticale1 = Instantiate(colliderParticale, transform.position, Quaternion.identity);
                newParticale1.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                ballTrialEffect.GetComponent<Renderer>().material = ballRenderer.material;
                break;
            case "Blue":
                other.gameObject.SetActive(false);
                ballRenderer.material = other.GetComponent<Renderer>().material;
                var newParticale2 = Instantiate(colliderParticale, transform.position, Quaternion.identity);
                newParticale2.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                ballTrialEffect.GetComponent<Renderer>().material = ballRenderer.material;
                break;
            case "Yellow":
                other.gameObject.SetActive(false);
                ballRenderer.material = other.GetComponent<Renderer>().material;
                var newParticale3 = Instantiate(colliderParticale, transform.position, Quaternion.identity);
                newParticale3.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                ballTrialEffect.GetComponent<Renderer>().material = ballRenderer.material;
                break;
        }
        if (other.gameObject.name.Contains("ColorBall"))
        {
            MenuManager.score++;
            if (MenuManager.score > PlayerPrefs.GetInt("score"))
            {
                PlayerPrefs.SetInt("score", MenuManager.score);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Path"))
        {
            StartCoroutine(AirBall());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Path"))
        {
            rb.isKinematic = col.isTrigger = true;
            pathSpeed = 10;

            var airEffectMain = airEffect.main;
            airEffectMain.simulationSpeed = 4;

            var newParticale = Instantiate(dustEffect, transform.position, Quaternion.identity);
            newParticale.GetComponent<Renderer>().material = ballRenderer.material;
        }
    }   
    IEnumerator AirBall()
    {
        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = col.isTrigger = false;
        rb.velocity = new Vector3(0, 8.5f, 0);
        pathSpeed = pathSpeed * 7f;

        var airEffectMain = airEffect.main;
        airEffectMain.simulationSpeed = 10;
    }
}
