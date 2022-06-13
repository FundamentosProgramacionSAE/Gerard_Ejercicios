using UnityEngine;


[System.Serializable]
public struct MinMaxFloat
{
    public float Min;
    public float Max;

    public float GetValueFromRatio()
    {
        return Random.Range(Min, Max);
    }
}


[System.Serializable]
public struct MinMaxInt
{
    public int Min;
    public int Max;

    public int GetValueFromRatio()
    {
        return (int)Random.Range(Min, Max + 1);
    }
}

[System.Serializable]
public struct MinMaxColor
{
    [ColorUsage(true, true)] public Color Min;
    [ColorUsage(true, true)] public Color Max;

    public Color GetValueFromRatio(float ratio)
    {
        return Color.Lerp(Min, Max, ratio);
    }
}

[System.Serializable]
public struct MinMaxVector3
{
    public Vector3 Min;
    public Vector3 Max;

    public Vector3 GetValueFromRatio(float ratio)
    {
        return Vector3.Lerp(Min, Max, ratio);
    }
}

