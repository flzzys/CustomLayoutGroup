using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalLayout : LayoutGroup {

	public Vector2 spacing;

	public override void CalculateLayoutInputHorizontal() {
        base.CalculateLayoutInputHorizontal();

        float x = 0;
        float y = 0;

        for (int i = 0; i < rectChildren.Count; i++) {
            var item = rectChildren[i];

            SetChildAlongAxis(item, 0, x);
            SetChildAlongAxis(item, 1, y);

            y += item.rect.height + spacing.y;
        }
    }

    public override void CalculateLayoutInputVertical() {
    }

    public override void SetLayoutHorizontal() {
    }

    public override void SetLayoutVertical() {
	}

}
