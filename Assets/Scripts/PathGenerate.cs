using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerate : MonoBehaviour
{
    [SerializeField] private GameObject[] pathPrefab;
    [SerializeField] private GameObject trap;
    [SerializeField] private Transform pathMain;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Path"))
        {
            Instantiate(pathPrefab[Random.Range(0, pathPrefab.Length)], new Vector3(0, -8.9f, 360), Quaternion.identity, pathMain);
            Instantiate(trap, new Vector3(0, 20.9f, 450), Quaternion.identity, pathMain);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Traps"))
        {
            Destroy(other.gameObject);
        }
    }
}
