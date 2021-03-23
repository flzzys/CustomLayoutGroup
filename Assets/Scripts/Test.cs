using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    public InventoryLayoutGroup inventoryLayoutGroup;
    public GameObject itemPrefab;

    public UIMover uiMover;

    private void Update() {
        if (Input.GetKeyDown("1")) {
            //uiMover.target.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            //uiMover.target.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            //uiMover.target.GetComponent<RectTransform>().sizeDelta = inventoryLayoutGroup.GetActualSize();
            //uiMover.target.GetComponent<RectTransform>().anchoredPosition = inventoryLayoutGroup.GetFirstItemPos();
            uiMover.Move(0, 1);

            //Instantiate(itemPrefab, inventoryLayoutGroup.transform);

        }
        if (Input.GetKeyDown("2")) {
            uiMover.Move(1, 0);
        }

        //QWE();

    }

    GameObject go;
    void QWE() {
        if (go) {
            Destroy(go);
        }

        Vector2 pos = inventoryLayoutGroup.GetFirstItemPos();

        go = new GameObject("qwe");
        go.AddComponent<RectTransform>();
        go.AddComponent<Image>().color = Color.black;
        go.transform.SetParent(FindObjectOfType<Canvas>().transform);

        go.transform.localScale = Vector3.one;
        go.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        go.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        go.GetComponent<RectTransform>().sizeDelta = inventoryLayoutGroup.GetActualSize();
    }
}
