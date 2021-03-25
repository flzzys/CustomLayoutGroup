using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//UI位置
public struct UIPos {
    public Vector2 pos;
    public Vector2 size;

    public UIPos(RectTransform rect) {
        pos = rect.transform.position;
        size = rect.sizeDelta;
    }
}

public class UIMover : MonoBehaviour {
    [Header("初始位置")]
    public RectTransform[] rectTrasnfroms;

    [Header("移动物体")]
    public RectTransform target;

    //移动UI，改变尺寸
    void Move(UIPos from, UIPos to, UXMovement movement_Size, UXMovement movement_Pos, Action onComplete = null) {
        target.sizeDelta = from.size;
        target.position = from.pos;

        target.DOSizeDelta(to.size, movement_Size.duration).SetEase(movement_Size.curve);
        target.DOMove(to.pos, movement_Pos.duration).SetEase(movement_Pos.curve).OnComplete(() => {
            onComplete?.Invoke();
        });
    }

    //移动UI，改变缩放而非尺寸
    public void MoveWithScale(UIPos from, UIPos to, UXMovement movement_Size, UXMovement movement_Pos, Action onComplete = null) {
        Vector2 fromSize = from.size / target.sizeDelta;
        target.localScale = fromSize;
        target.position = from.pos;

        Vector2 toSize = to.size / target.sizeDelta;
        target.DOScale(toSize, movement_Size.duration).SetEase(movement_Size.curve);
        target.DOMove(to.pos, movement_Pos.duration).SetEase(movement_Pos.curve).OnComplete(() => {
            onComplete?.Invoke();
        });
    }

    public void Move(int fromIndex, int toIndex, UXMovement movement_Size, UXMovement movement_Pos, Action onComplete = null) {
        Move(new UIPos(rectTrasnfroms[fromIndex]), new UIPos(rectTrasnfroms[toIndex]), movement_Pos, movement_Size, onComplete);
    }
    public void MoveWithScale(int fromIndex, int toIndex, UXMovement movement_Pos, UXMovement movement_Size, Action onComplete = null) {
        MoveWithScale(new UIPos(rectTrasnfroms[fromIndex]), new UIPos(rectTrasnfroms[toIndex]), movement_Pos, movement_Size, onComplete);
    }
}
