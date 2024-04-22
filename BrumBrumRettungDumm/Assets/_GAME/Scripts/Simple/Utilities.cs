using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ng System;

public static class Utilities
{
    #region Draw Arow

    public static void DrawArrow(Vector3 from, Vector3 to, Color color, bool drawAsGizmo = true, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float arrowHeadPositionAlongBaseLine = 1f)
    {
        if (drawAsGizmo)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(from, to);
        }
        else
        {
            Debug.DrawLine(from, to, color);
        }

        Vector3 direction = to - from;
        DrawArrowEnd(true, from, direction, color, arrowHeadLength, arrowHeadAngle, arrowHeadPositionAlongBaseLine);
    }

    public static void DrawArrow_Directional(Vector3 pos, Vector3 direction, Color color, bool drawAsGizmo = true, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float arrowHeadPositionAlongBaseLine = 1)
    {
        if (drawAsGizmo)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
        }
        else 
        { 
            Debug.DrawRay(pos, direction, color);
        }

        DrawArrowEnd(drawAsGizmo, pos, direction, color, arrowHeadLength, arrowHeadAngle, arrowHeadPositionAlongBaseLine);
    }

    private static void DrawArrowEnd(bool drawAsGizmo, Vector3 pos, Vector3 direction, Color color, float arrowHeadLength, float arrowHeadAngle, float arrowPosition)
    {
        Vector3 right = (Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back) * arrowHeadLength;
        Vector3 left = (Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back) * arrowHeadLength;
        Vector3 up = (Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back) * arrowHeadLength;
        Vector3 down = (Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back) * arrowHeadLength;

        Vector3 arrowTip = pos + (direction * arrowPosition);

        if (drawAsGizmo)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(arrowTip, right);
            Gizmos.DrawRay(arrowTip, left);
            Gizmos.DrawRay(arrowTip, up);
            Gizmos.DrawRay(arrowTip, down);
        }
        else
        {
            Debug.DrawRay(arrowTip, right, color);
            Debug.DrawRay(arrowTip, left, color);
            Debug.DrawRay(arrowTip, up, color);
            Debug.DrawRay(arrowTip, down, color);
        }
    }

    #endregion

    /// <summary>
    /// Randomly returns True or False
    /// </summary>
    /// <param name="chance">Between 0 and 1 | Higher = more likely to be True | Lower = more likely to be False | 0.5f = Equal Chances</param>
    /// <returns></returns>
    public static bool RandomBoolean(float chance = 0.5f)
    {
        return Random.value <= chance;
    }
}
