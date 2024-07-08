using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolManager : MonoBehaviour
{
    
    private int maxAmmountOfPrefabs = 10;
    public GameObject prefab;

    private List<GameObject> pooledObjects = new List<GameObject>();

    private static PoolManager _instance;

    public static PoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < maxAmmountOfPrefabs; i++)
        {
            GameObject _prefab = Instantiate(prefab);
            
            _prefab.name = _prefab.name + i;
            _prefab.transform.SetParent(gameObject.transform);
            pooledObjects.Add(_prefab);
            _prefab.SetActive(false);
        }
    }

    // Update is called once per frame
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        //TODO Sino hay, hay que spawnear mas pero eso veremos luego, tiene que ser dinamico para que acapare una cantidad de recursos dinamica y no de forma estatica.
        Debug.LogWarning("No more Objects In Pool");
        return null;
    }

}
