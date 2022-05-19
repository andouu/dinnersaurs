using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    // Assumes that the player only goes straight, so will only load and unload chunks in a straight line (z direction)
    [Header("Settings")]
    public Vector3 InitialPosition; // the center of the starting chunks
    [SerializeField] private int _chunksAround = 4; // how many chunks ahead and behind to load

    [Header("Components")]
    [SerializeField] private CrosshairIndicate _crosshairIndicatorBehavior;
    [SerializeField] private GameObject _chunkPrefab; // SHOULD BE A SQUARE
    [SerializeField] private GameObject _player;
    
    // cache
    private Quaternion neg90 = Quaternion.Euler(new Vector3(-90, 90, 0));
    private List<GameObject> _loadedChunks = new List<GameObject>();
    private float _chunkWidth;
    private Vector3 _step;

    public void Reset()
    {
        foreach (GameObject chunk in _loadedChunks) Destroy(chunk);
        _loadedChunks.Clear();
        initChunks();
    }
    
    private void Start()
    {
        initChunks();
    }

    // Update is called once per frame
    void Update()
    {
        float zDistFromCenter = calcDist();
        if (Mathf.Abs(zDistFromCenter) > _chunkWidth)
        {
            if (zDistFromCenter > 0)
            {
                shiftLeft();
            }
            else
            {
                shiftRight();
            }
        }
    }

    private GameObject createChunk(Vector3 pos)
    {
        GameObject clone = Instantiate(_chunkPrefab, pos, neg90);
        clone.GetComponent<ChunkTerrain>().CrosshairIndicatorBehavior = _crosshairIndicatorBehavior;
        foreach (RandomWall wallBehavior in clone.GetComponentsInChildren<RandomWall>())
        {
            wallBehavior.Type = "straight";
        }
        return clone;
    }
    
    private void initChunks()
    {
        _chunkWidth = _chunkPrefab.GetComponent<Renderer>().bounds.extents.z;
        _step = new Vector3(0, 0, _chunkWidth * 2);
        Vector3 pos = InitialPosition + _chunksAround * -_step;
        for (int chunk = 0; chunk < _chunksAround * 2 + 1; chunk++)
        {
            GameObject clone = createChunk(pos);
            _loadedChunks.Add(clone);
            pos += _step;
        }
    }

    private float calcDist()
    {
        GameObject centerChunk = _loadedChunks[_chunksAround];
        float dist = _player.transform.position.z - centerChunk.transform.position.z;
        return dist;
    }

    private void shiftLeft()
    {
        Destroy(_loadedChunks[0]);
        _loadedChunks.RemoveAt(0);
        Vector3 newPosition = _loadedChunks.Last().transform.position + _step;
        GameObject newChunk = createChunk(newPosition);
        _loadedChunks.Add(newChunk);
    }

    private void shiftRight()
    {
        int lastIndex = _loadedChunks.Count - 1;
        Destroy(_loadedChunks[lastIndex]);
        _loadedChunks.RemoveAt(lastIndex);
        Vector3 newPosition = _loadedChunks[0].transform.position - _step;
        GameObject newChunk = createChunk(newPosition);
        _loadedChunks.Insert(0, newChunk);
    }
}
