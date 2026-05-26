using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = System.Object;

public class DrawMesh : MonoBehaviour
{
   public new Camera camera;
   public float lineThickness = 1f;
   
   [Range(1,10)]
   public float Smoothness = 1;

   private EventSystem eventSystem;
   private GraphicRaycaster graphicRaycaster;

   private Canvas canvas;
   private Mesh mesh;
   private Vector3 lastMousePosition;

   private void Start()
   {
      eventSystem = FindObjectOfType<EventSystem>();

      foreach (GraphicRaycaster gr in FindObjectsOfType<GraphicRaycaster>())
      {
         if (gr.transform.Find("Letter") != null || ContainsTagInChildren(gr.transform, "Letter"))
         {
            graphicRaycaster = gr;
            canvas = gr.GetComponent<Canvas>();
            break;
         }
      }

      if (graphicRaycaster == null)
      {
         canvas = FindObjectOfType<Canvas>();
         graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
      }
   }

   private bool ContainsTagInChildren(Transform parent, string tag)
   {
      foreach (Transform child in parent.GetComponentsInChildren<Transform>())
      {
         if (child.CompareTag(tag)) return true;
      }
      return false;
   }


   public Vector3 GetMouseWorldPositionWithZ()
   {
      Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
      worldPosition.z = 0;
      return worldPosition;

   }

   private bool IsPointerOverLetterUi()
   {
      // UI Raycast
      PointerEventData pointerData = new PointerEventData(eventSystem)
      {
         position = Input.mousePosition
      };

      List<RaycastResult> results = new List<RaycastResult>();
      graphicRaycaster.Raycast(pointerData, results);

      foreach (RaycastResult result in results)
      {
         if (result.gameObject.CompareTag("Letter"))
            return true;
      }

      // 2D Physics Raycast (non-UI)
      Vector2 worldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
      RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
      if (hit.collider != null && hit.collider.CompareTag("Letter"))
         return true;

      return false;
   }

   private void Update()
   {
      
      if (Input.GetMouseButtonDown(0))
      {
         if (!IsPointerOverLetterUi()) return;

         if (mesh != null)
         {
            SaveCurrentMesh();
         }

          mesh = new Mesh();

         Vector3[] vertices = new Vector3[4];
         Vector2[] uv = new Vector2[4];
         int[] triangles = new int[6];

         vertices[0] = GetMouseWorldPositionWithZ();
         vertices[1] = GetMouseWorldPositionWithZ();
         vertices[2] = GetMouseWorldPositionWithZ();
         vertices[3] = GetMouseWorldPositionWithZ();

         uv[0] = Vector2.zero;
         uv[1] = Vector2.zero;
         uv[2] = Vector2.zero;
         uv[3] = Vector2.zero;

         triangles[0] = 0;
         triangles[1] = 3;
         triangles[2] = 1;
      
         triangles[3] = 1;
         triangles[4] = 3;
         triangles[5] = 2;

         mesh.vertices = vertices;
         mesh.uv = uv;
         mesh.triangles = triangles;
         mesh.MarkDynamic();

         GetComponent<MeshFilter>().mesh = mesh;

         lastMousePosition = GetMouseWorldPositionWithZ();
      }

      if (Input.GetMouseButton(0) && !IsPointerOverLetterUi())
      {
         lastMousePosition = GetMouseWorldPositionWithZ();
      }

      if (Input.GetMouseButton(0) && IsPointerOverLetterUi())
      {
         float minDistance = 1f - (Smoothness / 10);
         if (Vector3.Distance(GetMouseWorldPositionWithZ(), lastMousePosition) > minDistance)
         {
            Vector3[] vertices = new Vector3[mesh.vertices.Length + 2];
            Vector2[] uv = new Vector2[mesh.vertices.Length + 2];
            int[] triangles = new int[mesh.triangles.Length + 6];
         
            mesh.vertices.CopyTo(vertices,0);
            mesh.uv.CopyTo(uv,0);
            mesh.triangles.CopyTo(triangles,0);

            int vIndex = vertices.Length - 4;
            int vIndex0 = vIndex + 0;
            int vIndex1 = vIndex + 1;
            int vIndex2 = vIndex + 2;
            int vIndex3 = vIndex + 3;

            Vector3 mouseForwardVector = (GetMouseWorldPositionWithZ() - lastMousePosition).normalized;
            Vector3 normal2D = new Vector3(0, 0, -1f);
            Vector3 newVertexUp = GetMouseWorldPositionWithZ() + Vector3.Cross(mouseForwardVector, normal2D) * lineThickness;
            Vector3 newVertexDown = GetMouseWorldPositionWithZ() + Vector3.Cross(mouseForwardVector, normal2D * -1f) * lineThickness;

            vertices[vIndex2] = newVertexUp;
            vertices[vIndex3] = newVertexDown;
         
            uv[vIndex2] = Vector2.zero;
            uv[vIndex3] = Vector2.zero;

            int tIndex = triangles.Length - 6;
         
            triangles[tIndex + 0] = vIndex0;
            triangles[tIndex + 1] = vIndex2;
            triangles[tIndex + 2] = vIndex1;
         
            triangles[tIndex + 3] = vIndex1;
            triangles[tIndex + 4] = vIndex2;
            triangles[tIndex + 5] = vIndex3;


            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            lastMousePosition = GetMouseWorldPositionWithZ(); 
         }
         
      }
   }
   
   public void SaveCurrentMesh()
   {
      GameObject beforeMeshObj = new GameObject("SavedDrawMesh");
      beforeMeshObj.AddComponent<MeshFilter>().mesh = mesh;
      beforeMeshObj.AddComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
   }


   public void DeleteAllDrawMesh()
   {
      GameObject[] allObjects = FindObjectsOfType<GameObject>();

      foreach (var obj in allObjects)
      {
         if (obj.name == "SavedDrawMesh")
         {
            Destroy(obj);
         }
      }
   }
   
}
