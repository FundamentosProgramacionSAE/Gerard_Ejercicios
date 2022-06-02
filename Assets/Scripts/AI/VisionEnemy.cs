using UnityEngine;

namespace AI
{
    [CreateAssetMenu(fileName = "VisionEnemy", menuName = "AI/Vision Enemy")]
    public class VisionEnemy : ScriptableObject
    {
        [Header("View Parameters")] public float ViewRadius;
        [Range(0, 360)] public float ViewAngle;

        public float Offset;
        public LayerMask targetMask;
        public LayerMask obstacleMask;
    }
}