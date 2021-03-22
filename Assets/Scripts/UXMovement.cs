using UnityEngine;

//UX移动
[System.Serializable]
public class UXMovement {
    [Header("持续时间")]
    public float duration = .1f;
    [Header("移动曲线")]
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
}