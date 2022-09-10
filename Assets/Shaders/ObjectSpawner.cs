using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private int _type;
    private int _posRange = 45;
    private float H, S, V;

    private void Start()
    {
        if(_type == 2)
        {
            SpawnObjects(3);
        }
    }

    private void SpawnObjects(int max)
    {        
        int spawnCount = Random.Range(0, max);
        Debug.Log("Spawned objects = " + spawnCount);

        for (int i = 0; i < spawnCount; i++)
        {
            int spawnPosX = Random.Range(-_posRange, _posRange);
            int spawnPosY = Random.Range(-_posRange, _posRange);
            Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, 0);

            int prefabID = Random.Range(0, _prefabs.Length);
            Debug.Log("Prefab ID = " + prefabID);

            GameObject spawnedObject = Instantiate(_prefabs[prefabID], transform, false) as GameObject;
            spawnedObject.transform.localPosition = spawnPos;

            float objectScale = Random.Range(0.65f, 0.75f);
            spawnedObject.transform.localScale = new Vector3(objectScale, objectScale + 0.1f, objectScale);

            //Image childImage = spawnedObject.GetComponentInChildren<Image>();
            //Color c = childImage.color;
            //Color.RGBToHSV(c, out H, out S, out V);
            //H *= Random.Range(0.9f, 1.1f);
            //childImage.color = Color.HSVToRGB(H, S, V);
        }
    }
}
