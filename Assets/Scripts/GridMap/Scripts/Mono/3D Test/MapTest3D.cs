using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Utils;
using Assets.GridMap;

public class MapTest3D : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    private ChunkManager<MapCell> chunkManager;
    private MapCellGridVisualManager3D chunkVisualManager;

    private Generator generator;

    [SerializeField]
    public ActionBasedGeneratorSettings GeneratorSettings;

    public Material material;

    [SerializeField]
    private int xSize = 50;
    [SerializeField]
    private int ySize = 20;
    [SerializeField]
    private float cellSize = 1f;
    [SerializeField]
    private Vector3 originPoint = new Vector3(-25f,-10f);
    
    [HideInInspector]
    public bool SettingsFoldout;

    #region Game
    private void Start()
    {
        _ResetData();
        _Inizialise();
        _Generate();
    }  
    private void Update()
    {
        chunkManager.Update(); // usare un evento collegato al movimento della camera
        if (Input.GetMouseButtonDown(0))
        {
            //chunkManager.SetCell(UtilsClass.GetMouseWorldPosition(), generator.Generate(Vector3.one));
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(chunkManager.GetCell(UtilsClass.GetMouseWorldPosition()).ToString());
        }
    }
    #endregion
    //private void Reset()
    //{
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        DestroyImmediate(transform.GetChild(i));
    //    }
    //}

    #region Editor

    #endregion
    public void Generate()
    {
        _ResetData();
        _Inizialise();
        _Generate();
    }

    public void Load()
    {

    }

    public void Save()
    {

    }

    public void OnSettingsUpdated()
    {
        //_ResetData();
        //_Inizialise();
        _Generate();
    }

    private void _Generate()
    { 
        chunkManager?.Update();
        chunkVisualManager?.UpdateGridVisuals();
    }

    private void _ResetData()
    {
        generator = null;
        chunkManager = null;
        chunkVisualManager?.ResetData();
    }

    private void _Inizialise()
    {
        generator = new Generator(GeneratorSettings);
        chunkManager = new ChunkManager<MapCell>(mainCamera, xSize, ySize, cellSize, originPoint);
        chunkManager.SetGenerator(generator);
        chunkVisualManager = gameObject.GetComponent<MapCellGridVisualManager3D>();
        chunkVisualManager.SetChunkManager(chunkManager);
        chunkVisualManager.SetMaterial(material);
        chunkVisualManager.SetParentTransform(transform);
    }
}