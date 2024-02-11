using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.WSA;

public class Bird : MonoBehaviour
{
    [SerializeField] float maxDragDistance = 4;
    [SerializeField] public float launchPower = 150;
    LineRenderer lineRenderer;
    Vector3 startPosition;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        startPosition = transform.position;
        lineRenderer.enabled = false;
    }
    private void OnMouseUp()
    {
        Vector3 directionMagnitude = startPosition - transform.position;
        GetComponent<Rigidbody2D>().AddForce(directionMagnitude * launchPower);
        GetComponent<Rigidbody2D>().gravityScale = 1;
        lineRenderer.enabled = false;
    }
    void OnMouseDrag()
    {
        var destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        destination.z = 0;
        if (Vector2.Distance(destination, startPosition) > maxDragDistance)
        {
            destination = Vector3.MoveTowards(startPosition, destination, maxDragDistance);
        }
        transform.position = destination;
        lineRenderer.SetPosition(1,transform.position);
        lineRenderer.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }

        if(FindAnyObjectByType<Enemy>(FindObjectsInactive.Exclude) == null)
        {
            Debug.Log("Game Over!");
            var levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(levelToLoad);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Invoke(nameof(ReloadLevel), 5);
    }
    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
