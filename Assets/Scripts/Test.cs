using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    IEnumerator Start() {
        GameObject go;

        for (int i = 0; i < 8; i++) {
            go = new GameObject("qwe");
            go.transform.SetParent(inventoryLayoutGroup.transform);
            go.AddComponent<RectTransform>();
            go.AddComponent<Image>().sprite = sprite;
        }

        Vector2 pos = inventoryLayoutGroup.GetFirstItemPos();
        Vector2 size = inventoryLayoutGroup.GetActualSize();

        //uiMover.target.anchoredPosition = pos;
        uiMover.target.transform.position = pos;
        uiMover.target.sizeDelta = size;

        yield return new WaitForSeconds(.5f);

        Vector2 layoutSize = uiMover.rectTrasnfroms[1].sizeDelta / size;
        print(layoutSize);

        inventoryLayoutGroup.GetComponentInParent<ScrollRect>().transform.localScale = layoutSize;
        Vector2 layoutPos = uiMover.rectTrasnfroms[1].position;
        print((Vector2)Camera.main.ScreenToWorldPoint(new Vector2(inventoryLayoutGroup.padding.left, inventoryLayoutGroup.padding.top)));
        layoutPos += (Vector2)Camera.main.ScreenToWorldPoint(new Vector2(inventoryLayoutGroup.padding.left, inventoryLayoutGroup.padding.top));
        inventoryLayoutGroup.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().position = layoutPos;
        //inventoryLayoutGroup.GetComponentInParent<ScrollRect>().transform.position = layoutPos;
    }

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
            
        }
    }

}
