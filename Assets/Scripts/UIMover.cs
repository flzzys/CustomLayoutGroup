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

//移动器参数
public class MoverParams {
    public Rect startRect;
    public Rect endRect;

    //改变Scale而非Size
    public bool changeScale;

    public UXMovement movement;

    //public MoverParams(RectTransform rect) {
        
    //}
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

    //移动
    public static void Move(RectTransform target, MoverParams moverParams, Action onComplete = null) {
        //改变Size
        if (!moverParams.changeScale) {

        }
        //改变Scale
        else {
            //如果没有设置
            if (moverParams.startRect == default(Rect)) {
                moverParams.startRect = new Rect {
                    position = target.position,
                    size = target.rect.size * target.localScale
                };
            }
            if (moverParams.endRect == default(Rect)) {
                moverParams.endRect = new Rect {
                    position = target.position,
                    size = target.rect.size
                };
            }

            //Scale
            Vector2 fromScale = moverParams.startRect.size / GetRealSize(target);
            target.localScale = fromScale;
            Vector2 toScale = moverParams.endRect.size / GetRealSize(target);
            target.DOScale(toScale, .5f);

            //Pos
            target.position = moverParams.startRect.position;
            target.DOMove(moverParams.endRect.position, .5f).OnComplete(() => {
                onComplete?.Invoke();
            });
        }
    }

    static Vector2 GetRealSize(RectTransform rectTransform) {
        return rectTransform.rect.size;
    }
}
