using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    [Header("Mesh Settings")]
    private const int _mapChunkSize = 241;
    [Range(0, 6)][SerializeField] private int _lod; // level of detail
    [SerializeField] private float _heightMultiplier;
    [SerializeField] private AnimationCurve _heightCurve;

    [Header("Noise Map Settings")]
    [SerializeField] private int _seed;
    [SerializeField] private Vector2 _offset;
    [Min(0.0001f)][SerializeField] private float _scale = 0.5f;
    [Range(0f, 1f)][SerializeField] private float _persistance = 0.5f;
    [Min(1f)][SerializeField] private float _lacunarity = 2f;
    [Min(0)][SerializeField] private int _numOctaves = 2;
    [SerializeField] private TerrainType[] _regions;

    [Header("Nav Mesh")]
    [SerializeField] private NavMeshSurface _navMeshSurface;

    [System.Serializable]
    public struct TerrainType
    {
        public string Name;
        public float Height;
        public Color Color;
    }

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private MeshData _meshData;

    private float[,] _noiseMap;
    private Color[] _colorMap;

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
        generateNoiseMap();
        _meshData = MeshGenerator.GenerateTerrainMesh(_noiseMap, _heightMultiplier, _heightCurve, _lod);
        Mesh mesh = _meshData.CreateMesh();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
        _navMeshSurface.BuildNavMesh();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        generateNoiseMap();
        _meshData = MeshGenerator.GenerateTerrainMesh(_noiseMap, _heightMultiplier, _heightCurve, _lod);
        Mesh mesh = _meshData.CreateMesh();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
        _navMeshSurface.BuildNavMesh();
    }

    private void generateNoiseMap()
    {
        _noiseMap = Noise.GenerateNoiseMap(_mapChunkSize, _mapChunkSize, _seed, _scale, _numOctaves, _persistance, _lacunarity, _offset);
        _colorMap = new Color[_mapChunkSize * _mapChunkSize];
        
        for (int y = 0; y < _mapChunkSize; y++)
        {
            for (int x = 0; x < _mapChunkSize; x++)
            {
                float currentHeight = _noiseMap[x, y];
                for (int region = 0; region < _regions.Length; region++)
                {
                    if (currentHeight <= _regions[region].Height)
                    {
                        _colorMap[y * _mapChunkSize + x] = _regions[region].Color;
                        break;
                    }
                }
            }
        }
    }
}
