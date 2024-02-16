using UnityEngine;

public static class GetMedian
{
    public static Vector3 Median(Vector3 a, Vector3 b) { return (a + b) / 2; }

    public static int Median(int a, int b) { return (a + b) / 2; }

    public static float Median(float a, float b) { return (a + b) / 2; }
}
