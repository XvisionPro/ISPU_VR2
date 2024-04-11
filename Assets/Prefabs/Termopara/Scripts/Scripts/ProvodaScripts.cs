using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class ProvodaScripts : MonoBehaviour
{
    [SerializeField] Transform Start;
    [SerializeField] Transform End;

    [SerializeField] float Length = 10;
    [Range(0, 1)]
    [SerializeField] float Rigidity = 0.5f;
    [SerializeField] int PointsCount = 50;
    [SerializeField] float Weight = 1;

    LineRenderer lr;
    Vector3[] points;

    void Update()
    {
        if (!lr)
            lr = GetComponent<LineRenderer>();

        if (points == null || points.Length != PointsCount)
            points = new Vector3[PointsCount];

        Lerp(Start, End, PointsCount);
        lr.positionCount = PointsCount;
        lr.SetPositions(points.ToArray());
    }

    private void Lerp(Transform start, Transform end, int count)
    {
        var rigidity = Mathf.Clamp01(Rigidity);
        var L = (Start.position - End.position);
        var D = L.magnitude + 0.001f;
        var DD = Mathf.Max(D, Length);
        var P0 = Start.position;
        var P1 = Start.position + Start.forward * DD * rigidity / 2;
        var P2 = End.position - End.forward * DD * rigidity / 2;
        var P3 = End.position;
        var overLength = Mathf.Max(0, Length - D);

        for (int i = 0; i < count; i++)
        {
            var t = (float)i / (count - 1);

            //Cubic Bezier
            var P01 = Vector3.Lerp(P0, P1, t);
            var P12 = Vector3.Lerp(P1, P2, t);
            var P23 = Vector3.Lerp(P2, P3, t);
            var P012 = Vector3.Lerp(P01, P12, t);
            var P123 = Vector3.Lerp(P12, P23, t);
            var P1234 = Vector3.Lerp(P012, P123, t);

            //add gravity
            var t1 = (t - 0.5f) * 2;//linear -1 : 0 : 1
            var t2 = t1 * t1;//parabola 1 : 0 : 1
            var t3 = 1 - t2;//parabola 0 : 1 : 0
            var gravity = Vector3.up * (t2 * t2 - 1) * Weight * t3 * (1 - rigidity) * overLength;

            P1234 += gravity;

            points[i] = P1234;
        }
    }
}