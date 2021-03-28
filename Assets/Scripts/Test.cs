using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Test : MonoBehaviour {
    public InventoryLayoutGroup inventoryLayoutGroup;
    public GameObject itemPrefab;

    public UIMover uiMover;

    public Sprite sprite;

    [Header("尺寸变化_显示")]
    public UXMovement movement_Size_Show;
    [Header("位置变化_显示")]
    public UXMovement movement_Pos_Show;

    [Header("尺寸变化_隐藏")]
    public UXMovement movement_Size_Hide;
    [Header("位置变化_隐藏")]
    public UXMovement movement_Pos_Hide;

    Rect startRect, endRect;

    IEnumerator Start() {
        GameObject go;

        for (int i = 0; i < 8; i++) {
            go = new GameObject("qwe");
            go.transform.SetParent(inventoryLayoutGroup.transform);
            go.AddComponent<RectTransform>();
            go.AddComponent<Image>().sprite = sprite;
        }


        RectTransform scroll = inventoryLayoutGroup.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>();

        RectTransform from = uiMover.rectTrasnfroms[1];
        RectTransform to = uiMover.rectTrasnfroms[2];

        //Size
        Vector2 size = inventoryLayoutGroup.GetActualSize();
        Vector2 scale = from.sizeDelta / size;

        //Pos
        Vector2 pos = inventoryLayoutGroup.GetFirstItemPos();
        Vector2 offset = pos - (Vector2)scroll.position;
        offset *= scale;
        Vector2 targetPos = (Vector2)from.position - offset;

        startRect = new Rect {
            size = scroll.rect.size * scale,
            position = targetPos,
        };
        endRect = new Rect {
            size = to.rect.size,
            position = to.position
        };

        yield break;
    }

    public RectTransform r;

    private void Update() {
        if (Input.GetKeyDown("1")) {
            Vector2 pos = inventoryLayoutGroup.GetFirstItemPos();
            Vector2 size = inventoryLayoutGroup.GetActualSize();
            uiMover.MoveWithScale(new UIPos(uiMover.rectTrasnfroms[1]), new UIPos { pos = pos, size = size }, movement_Pos_Show, movement_Size_Show);
        }
        if (Input.GetKeyDown("2")) {
            Vector2 pos = inventoryLayoutGroup.GetFirstItemPos();
            Vector2 size = inventoryLayoutGroup.GetActualSize();
            uiMover.MoveWithScale(new UIPos { pos = pos, size = size }, new UIPos(uiMover.rectTrasnfroms[1]), movement_Pos_Show, movement_Size_Show);
        }
        if (Input.GetKeyDown("3")) {
            uiMover.target.transform.localScale = Vector3.one;
            uiMover.Move(0, 1, movement_Pos_Show, movement_Size_Show);
        }
        if (Input.GetKeyDown("4")) {
           uiMover.target.transform.localScale = Vector3.one;
           uiMover.Move(1, 0, movement_Pos_Hide, movement_Size_Hide);
        }

        if (Input.GetKeyDown("q")) {
            RectTransform scroll = inventoryLayoutGroup.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>();
            Vector2 pos = inventoryLayoutGroup.GetFirstItemPos();
            Vector2 size = inventoryLayoutGroup.GetActualSize();
            print(size);

            RectTransform target = uiMover.rectTrasnfroms[1];

            Vector2 scale = target.sizeDelta / size;
            print(scale);
            //scroll.localScale = scale;

            Vector2 offset = pos - (Vector2)scroll.position;
            offset *= scale;
            Vector2 targetPos = (Vector2)target.position - offset;

            //uiMover.MoveWithScale(new UIPos { pos = (Vector2)target.position - offset, size = scale }, new UIPos(uiMover.rectTrasnfroms[2]), movement_Pos_Show, movement_Size_Show);

            scroll.DOMove(targetPos, .5f);
            scroll.DOScale(scale, .5f);
        }
        if (Input.GetKeyDown("w")) {
            RectTransform target = uiMover.rectTrasnfroms[2];

            RectTransform scroll = inventoryLayoutGroup.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>();
            scroll.DOMove(target.transform.position, .5f);
            scroll.DOScale(Vector2.one, .5f);

            print(target.rect.size);
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            RectTransform scroll = inventoryLayoutGroup.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>();

            MoverParams moverParams = new MoverParams {
                startRect = startRect,
                endRect = endRect,
                changeScale = true
            };
            UIMover.Move(scroll, moverParams);
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            RectTransform scroll = inventoryLayoutGroup.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>();

            MoverParams moverParams = new MoverParams {
                startRect = endRect,
                endRect = startRect,
                changeScale = true
            };
            UIMover.Move(scroll, moverParams);
        }
    }

}
