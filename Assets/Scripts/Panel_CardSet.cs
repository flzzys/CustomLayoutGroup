using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_CardSet : MonoBehaviour {
    public RectTransform layoutParent;
    public InventoryLayoutGroup inventoryLayoutGroup;

    Rect startRect, endRect;
    public void InitPos(RectTransform[] poses) {
        //获取开始结束位置
        RectTransform from = poses[0];
        RectTransform to = poses[1];

        startRect = GetCardRect(from);
        endRect = new Rect {
            size = to.rect.size,
            position = to.position
        };
    }

     public void Move(bool show) {
        MoverParams moverParams;
        if (show) {
            moverParams = new MoverParams {
                startRect = startRect,
                endRect = endRect,
                changeScale = true
            };
        } else {
            moverParams = new MoverParams {
                startRect = endRect,
                endRect = startRect,
                changeScale = true
            };
        }

        //print($"Start: {moverParams.startRect.position}, {moverParams.startRect.size}. End: {moverParams.endRect.position}, {moverParams.endRect.size}");
        UIMover.Move(layoutParent, moverParams);
    }

    //获取用卡片适应框体的Rect
    Rect GetCardRect(RectTransform rectTransform) {
        //Size
        //获取Layout物体实际尺寸
        Vector2 size = inventoryLayoutGroup.GetActualSize();
        //获取Layout物体比例和目标物体尺寸比例
        Vector2 scale = rectTransform.rect.size / size;
        //本体目标大小
        Vector2 targetSize = layoutParent.rect.size * scale;

        //Pos
        Vector2 pos = inventoryLayoutGroup.GetFirstItemPos();
        Vector2 offset = pos - (Vector2)layoutParent.position;
        offset *= scale;
        Vector2 targetPos = (Vector2)rectTransform.position - offset;

        Rect rect = new Rect {
            size = targetSize,
            position = targetPos,
        };

        return rect;
    }

}
