using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField] private Transform[] controlPoints;

    private Vector3 gizmosPosition;

    private void OnDrawGizmos()
    {

        int index = 0;
        while(index <= controlPoints.Length - 3)
        {
            if(index == 0)
            {
                Vector3 pt1 = controlPoints[index].position;
                Vector3 pt2 = controlPoints[index + 1].position;
                Vector3 pt3 = controlPoints[index + 2].position;
                Vector3 pt4 = controlPoints[index + 3].position;

                DrawBezierCurve(pt1, pt2, pt3, pt4);
                index += 3;

            } else
            {
                Vector3 pt1 = controlPoints[index].position;

                float xd = 2 * pt1.x - controlPoints[index - 1].position.x;
                float yd = 2 * pt1.y - controlPoints[index - 1].position.y;
                float zd = 2 * pt1.z - controlPoints[index - 1].position.z;

                Vector3 pt2 = new Vector3(xd, yd, zd);
                Vector3 pt3 = controlPoints[index + 1].position;
                Vector3 pt4 = controlPoints[index + 2].position;

                DrawBezierCurve(pt1, pt2, pt3, pt4);
                index += 2;
            }
        }








       /* for (int index = 0; index <= controlPoints.Length - 2; index += 2)
        {
            DrawQuadraticCurve(controlPoints[index].position, controlPoints[index + 1].position, controlPoints[index + 2].position);
        }*/
    }

   
    private void DrawQuadraticCurve(Vector3 pt1, Vector3 pt2, Vector3 pt3)
    {
        for (float i = 0; i <= 1; i += 0.005f)
        {

            float u = 1 - i;
            float uu = u * u;
            float ii = i * i;

            
            gizmosPosition = Mathf.Pow(1 - i, 2) * pt1 + 2 * (1 - i) * i * pt2 + Mathf.Pow(i, 2) * pt3;
            Gizmos.DrawSphere(gizmosPosition, 0.25f);
        }

        Gizmos.DrawLine(pt1, pt2);
        Gizmos.DrawLine(pt2, pt3);

    }











   private void DrawBezierCurve(Vector3 pt1, Vector3 pt2, Vector3 pt3, Vector3 pt4)
    {
        for (float i = 0; i <= 1; i += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - i, 3) * pt1 + 3 * Mathf.Pow(1 - i, 2) * i * pt2 + 3 * (1 - i) * Mathf.Pow(i, 2) * pt3 + Mathf.Pow(i, 3) * pt4;
            Gizmos.DrawSphere(gizmosPosition, 0.25f);
        }

        Gizmos.DrawLine(pt1, pt2);
        Gizmos.DrawLine(pt3, pt4);

    }
}
