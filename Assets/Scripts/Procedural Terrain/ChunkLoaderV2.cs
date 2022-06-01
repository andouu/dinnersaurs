using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkLoaderV2 : MonoBehaviour
{
    [Header("Settings")]
    [HideInInspector] public Vector3 InitialPosition = Vector3.zero; // the center of the starting chunks
    [SerializeField] private int _chunksAround = 8; // how many chunks ahead and behind to load
    //TODO: weight the probability of spawning straight and turn chunks
    //[SerializeField, Range(0f, 1f)] private float _straightChunkWeighting;
    //[SerializeField, Range(0f, 1f)] private float _turnChunkWeighting;

    [Header("Components")]
    [SerializeField] private CrosshairIndicate _crosshairIndicatorBehavior;
    [SerializeField] private GameObject _chunkPrefab; // SHOULD BE A SQUARE
    [SerializeField] private GameObject _player;
    
    // cache
    private Quaternion neg90 = Quaternion.Euler(new Vector3(-90, 90, 0));
    private Queue<Chunk> _loadedChunks = new Queue<Chunk>();
    private float _chunkWidth;
    private Vector3 _step;

    public Chunk CenterChunk;

    private enum ChunkTypes
    {
        straight,
        left,
        right
    }

    public class Chunk
    {
        public GameObject gameObject;
        public string orientation;

        public Chunk(GameObject go, string o)
        {
            gameObject = go;
            orientation = o;
        }
    }

    private void Awake()
    {
        initiateChunks();
        CenterChunk = _loadedChunks.ElementAt(_chunksAround);
    }

    // Update is called once per frame
    void Update()
    {
        float centerDist = calcDist();
        if (centerDist > _chunkWidth)
        {
            shift(); // only shift if player is going forward
        }
    }

    private void shift()
    {
        Destroy(_loadedChunks.First().gameObject);
        _loadedChunks.Dequeue();
        Vector3 newPosition = _loadedChunks.Last().gameObject.transform.position + _step;
        Chunk newChunk = createChunk(newPosition);
        _loadedChunks.Enqueue(newChunk);
    }

    // for simplicity's sake, chunks cannot rotate more than 90 degrees from the original orientation
    private void initiateChunks()
    {
        _chunkWidth = _chunkPrefab.GetComponent<Renderer>().bounds.extents.z;
        _step = new Vector3(0, 0, _chunkWidth * 2);
        Vector3 pos = InitialPosition + _chunksAround * -_step;
        for (int chunk = 0; chunk < _chunksAround * 2 + 1; chunk++)
        {
            if (chunk == 0)
            {
                _loadedChunks.Enqueue(firstChunk(pos));
                continue;
            }
            Chunk clone = createChunk(pos);
            _loadedChunks.Enqueue(clone);
            pos = _loadedChunks.Last().gameObject.transform.position + _step;
        }
    }
    
    private float calcDist()
    {
        Chunk chunk = _loadedChunks.ElementAt(_chunksAround);
        GameObject centerChunk = chunk.gameObject;
        float dist;
        if (chunk.orientation == "straight")
        {
            dist = _player.transform.position.z - centerChunk.transform.position.z;
        }

        if (chunk.orientation == "left")
        {
            dist = -(_player.transform.position.x - centerChunk.transform.position.x);
        }

        dist = _player.transform.position.x - centerChunk.transform.position.x;
        return dist;
    }

    private Quaternion getAngle(string orientation)
    {
        //return neg90;
        if (orientation == "straight")
        {
            return neg90;
        }
        if (_loadedChunks.Last().orientation == "left")
        {
            return neg90 * Quaternion.Euler(new Vector3(0, 0, -90f));
        }
        return neg90 * Quaternion.Euler(new Vector3(0, 0, 90f));
    }

    private string getOrientation(int type, string orientation)
    {
        if (type == 0)
        {
            return orientation;
        }

        if (orientation == "straight")
        {
            return ((ChunkTypes) type).ToString();
        }

        return "straight";
    }

    private int getRandomType(string orientation)
    {
        if (orientation == "straight")
        {
            return Random.Range(0, 3);
        }

        if (orientation == "right")
        {
            return Random.Range(0, 2);
        }
        
        // handle left orientation
        if (Random.Range(0, 2) == 0)
        {
            return 0;
        }

        return 2;
    }

    private Vector3 rotatePos(Vector3 pos, string orientation)
    {
        if (orientation == "straight")
        {
            return pos;
        }

        Vector3 original = pos -= _step;
        Vector3 stepHorz = new Vector3(_step.z, 0, 0);
        if (orientation == "left")
        {
            return original - stepHorz;
        }

        return original + stepHorz;
    }

    private Chunk createChunk(Vector3 pos)
    {
        string orientation = _loadedChunks.Last().orientation;
        int randomType = getRandomType(orientation);
        Quaternion properAngle = getAngle(orientation);
        Vector3 properPos = rotatePos(pos, orientation); 
        GameObject clone = Instantiate(_chunkPrefab, properPos, properAngle);
        
        RandomWall[] wallBehaviors = clone.GetComponentsInChildren<RandomWall>();
        foreach (RandomWall wallBhv in wallBehaviors)
        {
            wallBhv.Type = ((ChunkTypes) randomType).ToString();
        }
        clone.GetComponent<ChunkTerrain>().CrosshairIndicatorBehavior = _crosshairIndicatorBehavior;
        clone.transform.parent = transform.parent;
        
        string properOrientation = getOrientation(randomType, orientation);
        Chunk newChunk = new Chunk(clone, properOrientation);
        return newChunk;
    }

    private Chunk firstChunk(Vector3 pos)
    {
        int randomType = getRandomType("straight");
        Quaternion properAngle = getAngle("straight");
        GameObject clone = Instantiate(_chunkPrefab, pos, properAngle);
        
        RandomWall[] wallBehaviors = clone.GetComponentsInChildren<RandomWall>();
        foreach (RandomWall wallBhv in wallBehaviors)
        {
            wallBhv.Type = ((ChunkTypes) randomType).ToString();
        }
        clone.GetComponent<ChunkTerrain>().CrosshairIndicatorBehavior = _crosshairIndicatorBehavior;
        
        string properOrientation = getOrientation(randomType, "straight");
        Chunk newChunk = new Chunk(clone, properOrientation);
        return newChunk;
    }
}
