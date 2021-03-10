using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public InventoryLayoutGroup inventoryLayoutGroup;

    void Start() {
        print(inventoryLayoutGroup.GetFirstItemPos());

        Vector2 pos = inventoryLayoutGroup.GetFirstItemPos();

        GameObject go = new GameObject("qwe");
        go.AddComponent<RectTransform>();
        go.transform.SetParent(FindObjectOfType<Canvas>().transform);

        go.transform.localScale = Vector3.one;
        go.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        go.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        go.GetComponent<RectTransform>().sizeDelta = inventoryLayoutGroup.GetActualSize();
    }

}
