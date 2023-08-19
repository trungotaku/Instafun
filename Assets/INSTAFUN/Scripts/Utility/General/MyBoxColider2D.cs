using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBoxColider2D : MonoBehaviour
{
    public Transform[] vertices;

    // Check if a point is inside the quadrilateral
    public bool IsPointInsideQuadrilateral(Vector2 point)
    {
        Vector2 A = vertices[0].position;
        Vector2 B = vertices[1].position;
        Vector2 C = vertices[2].position;
        Vector2 D = vertices[3].position;

        // Split the quadrilateral into two triangles: ABC and ACD
        bool isInTriangle1 = IsPointInTriangle(point, A, B, C);
        bool isInTriangle2 = IsPointInTriangle(point, A, C, D);

        // If the point is inside either of the triangles, it's inside the quadrilateral
        return isInTriangle1 || isInTriangle2;
    }

    // Check if a point is inside a triangle
    bool IsPointInTriangle(Vector2 point, Vector2 A, Vector2 B, Vector2 C)
    {
        float denominator = (B.y - C.y) * (A.x - C.x) + (C.x - B.x) * (A.y - C.y);
        float a = ((B.y - C.y) * (point.x - C.x) + (C.x - B.x) * (point.y - C.y)) / denominator;
        float b = ((C.y - A.y) * (point.x - C.x) + (A.x - C.x) * (point.y - C.y)) / denominator;
        float c = 1 - a - b;

        // If all barycentric coordinates are between 0 and 1, the point is inside the triangle
        return a >= 0 && a <= 1 && b >= 0 && b <= 1 && c >= 0 && c <= 1;
    }
}