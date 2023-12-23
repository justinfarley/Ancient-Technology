using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public float meshResolution;
    [SerializeField] private MeshFilter viewMeshFilter;
    private Enemy enemy;
    private Mesh viewMesh;

    public LayerMask targetMask, obstacleMask;

    public bool IsPlayerInView()
    {
        Collider2D[] targetsInView = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInView.Length; i++)
        {
            Transform target = targetsInView[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            float angle = Vector2.Angle(transform.right, dirToTarget);
            if ((enemy.isFacingRight && angle < viewAngle / 2) ||
                (!enemy.isFacingRight && angle > (180 - viewAngle / 2)))
            {
                float distToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    //is it a player
                    if (target.GetComponent<PlayerMovement>())
                    {
                        print("found player");
                        Debug.DrawLine(transform.position, target.position, Color.red);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        viewMesh = new Mesh();
        viewMesh.name = "FOV Mesh";
        viewMeshFilter.mesh = viewMesh;
    }
    private void LateUpdate()
    {
        DrawFieldOfView();
    }
    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);
        if (hit)
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
    }
    public void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.z - (!enemy.isFacingRight ? viewAngle / 2 + 90 : viewAngle / 2 - 90) + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
