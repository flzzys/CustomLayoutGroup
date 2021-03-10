using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class InventoryLayoutGroup : MonoBehaviour {
    [Header("外边距")]
    public RectOffset padding;

    [Header("间距")]
    public Vector2 spacing;

    [Header("尺寸")]
    public Vector2 size = new Vector2(100, 200);

    [Header("缩放")]
    [Range(0.1f, 1)]
    public float scale = 1;

    //最大尺寸倍率，超过这个尺寸增加行列数
    public float maxSizeMultiplier = 1.5f;

    //实际尺寸
    Vector2 scaledSize {
        get {
            return size * scale;
        }
    }

    Vector2 actualSize;
    int columnCount;
    int rowCount;

    RectTransform rectTransform {
        get {
            return GetComponent<RectTransform>();
        }
    }

    float previousWidth;
    int lastChildCount;
    void Update() {
        //如果尺寸或子物体数量变化，更新
        if (rectTransform.rect.width != previousWidth || transform.childCount != lastChildCount) {
            //print("change");
            previousWidth = rectTransform.rect.width;
            lastChildCount = transform.childCount;
        } else {
            return;
        }

        UpdateLayout();
    }

    private void OnValidate() {
        UpdateLayout();
    }

    void UpdateLayout() {
        //列数
        columnCount = Mathf.FloorToInt((float)rectTransform.rect.width / (scaledSize.x + spacing.x));
        columnCount = Mathf.Max(columnCount, 1);
        //行数
        rowCount = Mathf.CeilToInt((float)rectTransform.childCount / columnCount);

        //子物体偏好尺寸倍率
        float s = (rectTransform.rect.width - padding.horizontal - spacing.x * (columnCount - 1)) / columnCount / scaledSize.x;
        s = Mathf.Min(s, maxSizeMultiplier);
        //实际尺寸
        actualSize = scaledSize * s;

        Vector2 startingPos = new Vector2(padding.left - padding.right, -padding.top);
        startingPos += new Vector2(rectTransform.rect.width / 2 - (actualSize.x + spacing.x) * ((float)(columnCount - 1) / 2), -actualSize.y / 2);
        //设置子物体位置
        int index = 0;
        foreach (Transform child in transform) {
            int column = index % columnCount;
            int row = index / columnCount;

            child.GetComponent<RectTransform>().sizeDelta = size;
            child.localScale = Vector3.one * scale * s;

            Vector2 pos = startingPos;
            pos += new Vector2(column * (actualSize.x + spacing.x), -row * (actualSize.y + spacing.y));
            child.GetComponent<RectTransform>().anchoredPosition = pos;

            //锚点
            child.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            child.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);

            index++;
        }

        //修改本体高度
        float totalPreferredHeight = padding.vertical + actualSize.y * rowCount + spacing.y * (rowCount - 1);
        var rect = rectTransform;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, totalPreferredHeight);
    }

    public Vector2 GetFirstItemPos() {
        UpdateLayout();
        Vector2 pos = new Vector2(actualSize.x, -actualSize.y) / 2 + new Vector2(padding.left, -padding.top);
        
        return pos;
    }
    public Vector2 GetActualSize() {
        return actualSize;
    }
}
