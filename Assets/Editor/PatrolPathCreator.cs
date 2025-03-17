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
        Remove
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
        if(currentEditType == EditType.Edit)
        {
            HandlePathEdit(sceneView);
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

        }

        for (int i = 0; i < currentPatrolPath.pathPoints.Count - 1; i++)
        {
            Handles.DrawLine(currentPatrolPath.pathPoints[i], currentPatrolPath.pathPoints[i + 1]);
        }

    }
    private int currentlyEditingIndex = -1;
    private bool isCurrentlyEditing = false;

    private void HandlePathEdit(SceneView sceneView)
    {
        Event e = Event.current;
        Vector2 worldPos = GetWorldMousePosition(sceneView);

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            int closestIndex = -1;
            float minDistance = 0.5f; // Max selection distance

            for (int i = 0; i < currentPatrolPath.pathPoints.Count; i++)
            {
                float distance = Vector2.Distance(worldPos, currentPatrolPath.pathPoints[i]);
                if (distance < minDistance)
                {
                    closestIndex = i;
                    minDistance = distance;
                }
            }

            if (closestIndex == -1)
            {
                isCurrentlyEditing = false;
                GUIUtility.hotControl = 0;
            }
            else
            {
                isCurrentlyEditing = true;
                currentlyEditingIndex = closestIndex;
                GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
                e.Use();
            }
        }

        if (isCurrentlyEditing && e.type == EventType.MouseDrag && e.button == 0)
        {
            currentPatrolPath.pathPoints[currentlyEditingIndex] = GetSnappedPosition(worldPos);
            EditorUtility.SetDirty(currentPatrolPath);
            e.Use();
        }

        if (e.type == EventType.MouseUp && e.button == 0)
        {
            isCurrentlyEditing = false;
            GUIUtility.hotControl = 0;
            e.Use();
        }

        HandleInsertBoxes(sceneView);
    }

    private void HandleInsertBoxes(SceneView sceneView)
    {
        Event e = Event.current;
        for (int i = 0; i < currentPatrolPath.pathPoints.Count - 1; i++)
        {
            Vector2 middlePoint = (currentPatrolPath.pathPoints[i] + currentPatrolPath.pathPoints[i + 1]) / 2;
            float boxSize = 0.25f;
            Handles.DrawSolidRectangleWithOutline(new Rect(middlePoint.x - boxSize / 2, middlePoint.y - boxSize / 2, boxSize, boxSize), Color.white, Color.white);

            if(isCurrentlyEditing == false)
            {
                float minDist = 0.5f; //minimum distance to click insertion icon;

                Vector2 mousePos = GetWorldMousePosition(sceneView);
                if (e.type == EventType.MouseDown && e.button == 0 && Vector2.Distance(mousePos, middlePoint) <= minDist)
                {
                    currentPatrolPath.pathPoints.Insert(i+1, middlePoint);
                    EditorUtility.SetDirty(currentPatrolPath);
                    e.Use();
                    break;
                }
            }
           
        }
   
    }

    private void HandlePathDraw(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Vector2 worldPos = GetWorldMousePosition(sceneView);
            worldPos = GetSnappedPosition(worldPos);
            
            currentPatrolPath.pathPoints.Add(worldPos);
            EditorUtility.SetDirty(currentPatrolPath);
            RepaintEditorWindow();
        }
    }

    private void HandlePathRemove(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Vector2 worldPos = GetWorldMousePosition(sceneView);
            Vector2 closestPoint = currentPatrolPath.pathPoints.OrderBy(x => Vector2.Distance(worldPos, x)).FirstOrDefault(x => Vector2.Distance(worldPos, x) <= 0.5f);
            currentPatrolPath.pathPoints.Remove(closestPoint);
            EditorUtility.SetDirty(currentPatrolPath);
            RepaintEditorWindow();
        }
    }
    Vector2 GetSnappedPosition(Vector2 position)
    {
        if(!snapping)
        {
            return position;
        }
    
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

    private Vector2 GetWorldMousePosition(SceneView sceneView)
    {
        Vector2 mousePos = Event.current.mousePosition;
        mousePos.y = sceneView.camera.pixelHeight - mousePos.y;
        Vector2 worldPos = sceneView.camera.ScreenToWorldPoint(mousePos);
        return worldPos;
    }
    private string ConvertToRelativeFilePath(string filePath)
    {
        return "Assets" + filePath.Replace(Application.dataPath, "");
    }
}
