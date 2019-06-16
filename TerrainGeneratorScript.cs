
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorScript : MonoBehaviour
{
    [SerializeField] private GameObject world;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private float distanceFromCenter;
    [SerializeField] private int numberOfTrees;

    private Vector3 worldCenter;

    private void Start()
    {
         worldCenter = world.transform.position;
        GameObject tempObj;
        for(int i = 0; i < numberOfTrees; ++i)
        {
            Vector3 pos = Random.onUnitSphere.normalized * distanceFromCenter - worldCenter;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, pos - worldCenter);
            tempObj = Instantiate(treePrefab, pos, rotation);
        }
    }
}
