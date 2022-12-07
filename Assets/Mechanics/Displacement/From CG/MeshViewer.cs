using System;
using UnityEditor;
using UnityEngine;

public class MeshViewer : MonoBehaviour
{
    [SerializeField]
    private bool useObj = false;

    public TextAsset OBJFile; // OBJ file to be loaded
    public bool isFlatShaded; // Should loaded mesh be flat shaded
    private OBJParser parser; // OBJ file format Parser 
    private MeshData meshData; // Parsed OBJ data
    private MeshFilter meshFilter; // MeshFilter reference, used to display a Unity Mesh
    private Mesh _mesh;

    private void OnValidate()
    {
        parser = new OBJParser();
        meshFilter = GetComponent<MeshFilter>();
        _mesh = meshFilter.sharedMesh;
        meshData = new MeshData();
    }

    // Start is called before the first frame update
    void Start()
    {
        parser ??= new OBJParser();
        meshFilter ??= GetComponent<MeshFilter>();
        _mesh = meshFilter.sharedMesh;
        meshData = new MeshData();
    }

    // Loads a given OBJ file, calculates its surface normals and displays it
    public void ShowMesh()
    {
        if (useObj)
        {
            meshData = parser.Parse(OBJFile);
        }
        else
        {
            meshData.FromUnityMesh(_mesh);
        }

        if (isFlatShaded)
        {
            meshData.MakeFlatShaded();
        }

        meshData.CalculateNormals();

        meshFilter.mesh = meshData.ToUnityMesh();
    }
}

// A custom inspector UI for this class 
[CustomEditor(typeof(MeshViewer))]
class MeshViewerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Show Mesh"))
        {
            var meshViewer = target as MeshViewer;
            meshViewer.ShowMesh();
        }
    }
}
