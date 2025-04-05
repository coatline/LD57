using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class C
{
    public static void DrawRaycast(Vector2 origin, Vector2 direction, float length)
    {
        Vector2 endPoint = origin + (direction * length);
        Gizmos.DrawLine(origin, endPoint);
    }

    public static Resolution ClosestResolution(float targetWidth, float targetHeight, double refreshRate)
    {
        Resolution bestMatch = Screen.resolutions
    .Where(res => res.width == targetWidth && res.height == targetHeight)
    .OrderByDescending(res => Mathf.Abs((float)(res.refreshRateRatio.value - refreshRate)))
    .FirstOrDefault();
        return bestMatch;
    }

    public static float PercentageToDecibals(float percent) => Mathf.Log10(percent) * 20f;

    public static Color RandColor() => new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1f);

    public static float PercentageRange(float min, float max, float percent) => (percent * (max - min)) + min;

    public static void SetParticleSystemStartColor(ParticleSystem particleSystem, Color color)
    {
        var mainModule = particleSystem.main;
        mainModule.startColor = color;
    }

    public static Vector2 GetSizeOfSprite(Sprite sprite) => new Vector2(sprite.rect.size.x / sprite.pixelsPerUnit, sprite.rect.size.y / sprite.pixelsPerUnit);

    public static Vector2 GetCameraZoomRadius(Camera camera, float orthoSize) => new Vector2(orthoSize * camera.aspect, orthoSize);

    public static void SetColorNotAlpha(MaskableGraphic graphic, Color color) => graphic.color = new Color(color.r, color.g, color.b, graphic.color.a);
    public static void SetColorNotAlpha(SpriteRenderer sr, Color color) => sr.color = new Color(color.r, color.g, color.b, sr.color.a);

    public static void SetAlpha(ref Color col, float newAlpha) => col = new Color(col.r, col.g, col.b, newAlpha);
    public static void SetAlpha(SpriteRenderer sr, float newAlpha) => sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);
    public static void SetAlpha(MaskableGraphic graphic, float newAlpha) => graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, newAlpha);

    public static string DisplayTimeFromSeconds(int totalSeconds)
    {
        int seconds = totalSeconds % 60;
        int minutes = (totalSeconds / 60) % 60;
        int hours = (totalSeconds / 3600) % 24;
        int days = (totalSeconds / 86400);

        string timeUsedText = "";  // Start with seconds

        if (seconds > 0)
            timeUsedText = $"{seconds}s";

        if (minutes > 0)
            timeUsedText = $"{minutes}m " + timeUsedText;  // Add minutes before seconds

        if (hours > 0)
            timeUsedText = $"{hours}h " + timeUsedText;    // Add hours before minutes and seconds

        if (days > 0)
            timeUsedText = $"{days}d " + timeUsedText;     // Add days before hours, minutes, and seconds

        if (timeUsedText.Length == 0)
            timeUsedText = $"{seconds}s";

        return timeUsedText;
    }

    public static float GetPosFromSortedInMiddle(int index, float spacing, int totalPositions)
    {
        return (index * spacing) - (totalPositions - 1) * (spacing / 2);
    }

    public static Vector2 VectorAverage(Vector2 first, Vector2 second)
    {
        return (first + second) / 2f;
    }

    public static bool IsInViewport(Transform target, Camera camera)
    {
        // Convert the target's position from world space to viewport space
        Vector3 viewportPosition = camera.WorldToViewportPoint(target.position);

        // Check if the target is within the camera's viewport
        bool isInViewport = viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                            viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                            viewportPosition.z >= 0;

        return isInViewport;
    }

    public static int GetValueCountFromDictionaryWithList<T, G>(Dictionary<T, List<G>> dict, T key)
    {
        if (dict.TryGetValue(key, out List<G> list))
            return list.Count;
        return 0;
    }

    public static void RemoveFromDictionaryWithList<T, G>(Dictionary<T, List<G>> dict, T key, G value)
    {
        if (dict.TryGetValue(key, out List<G> list))
        {
            list.Remove(value);

            if (list.Count == 0)
                dict.Remove(key);
        }
    }

    public static void AddToDictionaryWithList<T, G>(Dictionary<T, List<G>> dict, T key, G value)
    {
        if (dict.TryGetValue(key, out List<G> list))
            list.Add(value);
        else
            dict.Add(key, new List<G> { value });
    }

    /// <summary>
    /// Probably have to subtract 90 degrees for correct operation.
    /// </summary>
    public static float AngleFromPosition(Vector3 pivotPosition, Vector3 pos)
    {
        float angleRad = Mathf.Atan2(pos.y - pivotPosition.y, pos.x - pivotPosition.x);
        float angleDeg = Mathf.Rad2Deg * angleRad;
        return angleDeg;
    }

    public static Vector2Int ToOctant(Vector2 vec)
    {
        float angle = Mathf.Atan2(vec.y, vec.x);
        int octant = Mathf.RoundToInt(8 * angle / (2 * Mathf.PI) + 8) % 8;

        CompassDir dir = (CompassDir)octant;

        switch (dir)
        {
            case CompassDir.NE: return new Vector2Int(1, 1);
            case CompassDir.N: return new Vector2Int(0, 1);
            case CompassDir.E: return new Vector2Int(1, 0);
            case CompassDir.SE: return new Vector2Int(1, -1);
            case CompassDir.S: return new Vector2Int(0, -1);
            case CompassDir.SW: return new Vector2Int(-1, -1);
            case CompassDir.W: return new Vector2Int(-1, 0);
            case CompassDir.NW: return new Vector2Int(-1, 1);
            default: return Vector2Int.zero;
        }
    }

    public static Object FindObjectOfNameFromArray(string name, Object[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].name == name)
                return array[i];
        }

        return null;
    }

    public static Vector3 MultiplyVector3s(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    enum CompassDir
    {
        E = 0, NE = 1,
        N = 2, NW = 3,
        W = 4, SW = 5,
        S = 6, SE = 7
    };
}