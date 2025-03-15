using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class PatrolPathCreator : EditorWindow
{
    private PatrolPath previousPatrolPath;
    private PatrolPath currentPatrolPath;
    Vector2 scrollPosition;
    private bool snapping = true;
    private float snappingDistance = 0.5f;
    private enum EditType
    {
        None,
        Draw,
        Edit,
        Remove,
        Insert
    }
    private EditType currentEditType;

    [MenuItem("Window/VerdantTools/Patrol Paths Creator")]
    public static void ShowWindow()
    {
        GetWindow<PatrolPathCreator>("Patrol Paths Creator");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += HandleSceneActions;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= HandleSceneActions;
    }

    public void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        if(GUILayout.Button("Create New Path"))
        {
            CreateNewPatrolPath();
        }

        currentPatrolPath = (PatrolPath)EditorGUILayout.ObjectField("Patrol Path", currentPatrolPath, typeof(PatrolPath), false);
        if(previousPatrolPath != currentPatrolPath)
        {
            SceneView.RepaintAll();
        }
        previousPatrolPath = currentPatrolPath;

        currentEditType = (EditType)EditorGUILayout.EnumPopup(new GUIContent("Edit Type"), currentEditType);

        snapping = EditorGUILayout.Toggle("Snapping", snapping);
  
        GUILayout.EndScrollView();
    }

    void RepaintEditorWindow()
    {
        Repaint();
    }

    private void HandleSceneActions(SceneView sceneView)
    {
        Debug.Log(currentPatrolPath);

        HandleHandlesPlacement();

        if (currentEditType == EditType.Draw)
        {
            HandlePathDraw(sceneView);
        }

        if(currentEditType == EditType.Remove)
        {
            HandlePathRemove(sceneView);
        }
    }

    private void HandleHandlesPlacement()
    {
        if (currentPatrolPath == null)
        {
            return;
        }
        if (currentPatrolPath.pathPoints == null)
        {
            return;
        }

        for (int i = 0; i < currentPatrolPath.pathPoints.Count; i++)
        {
            Vector2 point = currentPatrolPath.pathPoints[i];
            Handles.DrawSolidDisc(point, Vector3.forward, 0.2f);

            if (currentEditType == EditType.Edit)
            {
                Vector2 newPosition = Handles.PositionHandle(point, Quaternion.identity);
                newPosition = GetSnappedPosition(newPosition);
                currentPatrolPath.pathPoints[i] = newPosition;
            }
        }

        for (int i = 0; i < currentPatrolPath.pathPoints.Count - 1; i++)
        {
            Handles.DrawLine(currentPatrolPath.pathPoints[i], currentPatrolPath.pathPoints[i + 1]);
        }

    }


    private void HandlePathDraw(SceneView sceneView)
    {
        if(Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Vector2 mousePos = Event.current.mousePosition;
            mousePos.y = sceneView.camera.pixelHeight - mousePos.y;
            Vector2 worldPos = sceneView.camera.ScreenToWorldPoint(mousePos);
            if (snapping)
            {
                worldPos = GetSnappedPosition(worldPos);
            }
            currentPatrolPath.pathPoints.Add(worldPos);
            RepaintEditorWindow();
        }
    }

    private void HandlePathRemove(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Vector2 mousePos = Event.current.mousePosition;
            mousePos.y = sceneView.camera.pixelHeight - mousePos.y;
            Vector2 worldPos = sceneView.camera.ScreenToWorldPoint(mousePos);
            Vector2 closestPoint = currentPatrolPath.pathPoints.OrderBy(x => Vector2.Distance(worldPos, x)).FirstOrDefault(x => Vector2.Distance(worldPos, x) <= 0.5f);
            currentPatrolPath.pathPoints.Remove(closestPoint);
            RepaintEditorWindow();
        }
    }
    Vector2 GetSnappedPosition(Vector2 position)
    {
        return new Vector2(RoundTo(position.x, snappingDistance), RoundTo(position.y, snappingDistance));
    }

    float RoundTo(float number, float roundTo)
    {
        return Mathf.Round(number / roundTo) * roundTo;
    }

    private void CreateNewPatrolPath()
    {
        string filePath = EditorUtility.SaveFilePanel("Create new patrol path", "Assets/ScriptableObjects/PatrolPaths", "New Patrol Path", "asset");
        filePath = ConvertToRelativeFilePath(filePath);

        currentPatrolPath = CreateInstance<PatrolPath>();
        AssetDatabase.CreateAsset(currentPatrolPath, filePath);
    }

    private string ConvertToRelativeFilePath(string filePath)
    {
        return "Assets" + filePath.Replace(Application.dataPath, "");
    }
}
