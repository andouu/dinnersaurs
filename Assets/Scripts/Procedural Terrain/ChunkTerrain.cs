using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkTerrain : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float _treeSpawnChance;
    [SerializeField, Range(0f, 1f)] private float _nestSpawnChance;
    [SerializeField] private float _treeSize = 1f;
    [SerializeField] private float _nestSize = 1f;

    [Header("Components")]
    [SerializeField] private GameObject _chunkPrefab;
    [SerializeField] private GameObject _treePrefab;
    [SerializeField] private GameObject _nestPrefab;
    
    // cache
    private float _chunkWidth;
    private float _chunkHeight;
    private float _chunkHeightScale;

    private bool done = false; 
        
    private void Awake()
    {
        _chunkWidth = _chunkPrefab.GetComponent<Renderer>().bounds.extents.z;
        _chunkHeight = _chunkPrefab.GetComponent<Renderer>().bounds.extents.y;
        Vector3 chunkLocalScale = _chunkPrefab.transform.localScale;
        _chunkHeightScale = 1f / (chunkLocalScale.z / chunkLocalScale.x);
    }

    void FixedUpdate()
    {
        if (done)
            return;
        float randomNumber = Random.Range(1, 100);
        if (randomNumber > 100 - _treeSpawnChance * 100)
        {
            Vector3 randomPos = randomPtOnQuad(gameObject) + new Vector3(0, 2f, 0);
            GameObject tree = Instantiate(_treePrefab, randomPos, Quaternion.identity, gameObject.transform);
            undistortTransform(tree, 1f / _treeSize);
        }

        if (randomNumber > 100 - _nestSpawnChance * 100)
        {
            Vector3 neg90 = new Vector3(-90, 0, 0);
            Vector3 randomPos = randomPtOnQuad(gameObject) + new Vector3(0, 0.4f, 0);
            GameObject nest = Instantiate(_nestPrefab, randomPos, Quaternion.Euler(neg90), gameObject.transform);
            nest.transform.localScale = new Vector3(0.15f, 0.15f, 1.1f) * _nestSize;
        }

        done = true;
    }

    // local position on quad
    private Vector3 randomPtOnQuad(GameObject origin)
    {
        float widthWithPadding = _chunkWidth - 1.5f;
        float randomX = Random.Range(-widthWithPadding, widthWithPadding);
        float randomZ = Random.Range(-widthWithPadding, widthWithPadding);
        Vector3 originTransform = origin.transform.position;

        float yLevel = calcHeight(new Vector2(randomX, randomZ), origin);
        
        return new Vector3(originTransform.x + randomX, yLevel + 0.35f, originTransform.z + randomZ);
    }

    private float calcHeight(Vector2 pos, GameObject o)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(pos.x, o.transform.position.y, pos.y), Vector3.down, out hit))
        {
            return hit.point.y;
        }
        return 0;
    }
    
    // vertical compression due to parent transform
    private void undistortTransform(GameObject o, float scaleFactor)
    {
        Vector3 otls = new Vector3(1, _chunkHeightScale, 1);
        // shrink
        otls *= 1f / scaleFactor;
        o.transform.localScale = otls;
    }
}
