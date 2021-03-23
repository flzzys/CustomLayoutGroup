using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UIMover : MonoBehaviour {
    [Header("初始位置")]
    public RectTransform[] rectTrasnfroms;
    public Rect rects;

    [Header("移动物体")]
    public RectTransform target;

    [Header("尺寸变化_显示")]
    public UXMovement movement_Size_Show;
    [Header("位置变化_显示")]
    public UXMovement movement_Pos_Show;

    [Header("尺寸变化_隐藏")]
    public UXMovement movement_Size_Hide;
    [Header("位置变化_隐藏")]
    public UXMovement movement_Pos_Hide;

    public void Move(Rect from, Rect to, Action onComplete = null) {
        target.sizeDelta = from.size;
        target.position = from.position;

        target.DOSizeDelta(to.size, movement_Size_Show.duration).SetEase(movement_Size_Show.curve);
        target.DOMove(to.position, movement_Pos_Show.duration).SetEase(movement_Pos_Show.curve).OnComplete(() => {
            onComplete?.Invoke();
        });
    }

    public void Move(int fromIndex, int toIndex, Action onComplete = null) {
        Move(rectTrasnfroms[fromIndex].rect, rectTrasnfroms[toIndex].rect, onComplete);
    }
}
