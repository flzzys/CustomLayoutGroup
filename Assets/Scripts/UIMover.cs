using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UIMover : MonoBehaviour {
    [Header("初始位置")]
    public RectTransform pos1;
    [Header("目标位置")]
    public RectTransform pos2;

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

    public void UIMover_Show() {
        target.sizeDelta = pos1.sizeDelta;
        target.position = pos1.position;

        target.DOSizeDelta(pos2.sizeDelta, movement_Size_Show.duration).SetEase(movement_Size_Show.curve);
        target.DOMove(pos2.position, movement_Pos_Show.duration).SetEase(movement_Pos_Show.curve);
    }

    public void UIMover_Hide(Action onComplete = null) {
        target.sizeDelta = pos2.sizeDelta;
        target.position = pos2.position;

        target.DOSizeDelta(pos1.sizeDelta, movement_Size_Hide.duration).SetEase(movement_Size_Hide.curve);
        target.DOMove(pos1.position, movement_Pos_Hide.duration).SetEase(movement_Pos_Hide.curve).OnComplete(() => {
            onComplete?.Invoke();
        });
    }
}
