using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorScript : MonoBehaviour
{
    [SerializeField] private GameObject world;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private float distanceFromCenter;

    private Vector3 worldCenter;

    private void Start()
    {
        worldCenter = world.transform.position;

        for(int i = 0; i < 100; ++i)
        {
            GameObject tree = Instantiate(treePrefab);
            Vector3 randomVector = new Vector3(UnityEngine.Random.Range(-180, 180), UnityEngine.Random.Range(-180, 180), UnityEngine.Random.Range(-180, 180)).normalized;
            tree.transform.position = randomVector * 25f - worldCenter;
            Quaternion rotation = Quaternion.LookRotation(tree.transform.position - worldCenter);
            tree.transform.rotation = rotation;
        }
    }

    private void Update()
    {

    }

}
