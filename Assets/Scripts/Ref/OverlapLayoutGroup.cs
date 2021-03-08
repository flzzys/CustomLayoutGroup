using UnityEngine;
using System.Collections;

namespace UnityEngine.UI {
    public class OverlapLayoutGroup : LayoutGroup {
        public override void CalculateLayoutInputHorizontal() {
            base.CalculateLayoutInputHorizontal(); // No Vertical equivalent, it just lists the children to include.
            CalculateLayoutInputForAxis(0);
        }

        public override void CalculateLayoutInputVertical() {
            CalculateLayoutInputForAxis(1);
        }

        void CalculateLayoutInputForAxis(int axis) {
            // We need to reserve space for the padding.
            float combinedPadding = (axis == 0 ? padding.horizontal : padding.vertical);
            float totalmin = combinedPadding;
            float totalpreferred = combinedPadding;
            // And for the largest child.
            float min = 0;
            float preferred = 0;
            for (int i = 0; i < rectChildren.Count; i++) {
                RectTransform child = rectChildren[i];
                min = Mathf.Max(min, LayoutUtility.GetMinSize(child, axis));
                preferred = Mathf.Max(preferred, LayoutUtility.GetPreferredSize(child, axis));
            }
            totalmin += min;
            totalpreferred += preferred;
            // We ignore flexible size for now, I have not decided what to do with it yet.
            SetLayoutInputForAxis(totalmin, totalpreferred, -1, axis);
        }

        public override void SetLayoutHorizontal() {
            SetLayoutAlongAxis(0);
        }

        public override void SetLayoutVertical() {
            SetLayoutAlongAxis(1);
        }

        void SetLayoutAlongAxis(int axis) {
            // Take all the space, except the padding.
            float combinedPadding = (axis == 0 ? padding.horizontal : padding.vertical);
            float size = rectTransform.rect.size[axis] - combinedPadding;
            // Everybody starts at the same place.
            float pos = GetStartOffset(axis, 0);
            // Overlap all the things.
            for (int i = 0; i < rectChildren.Count; i++) {
                RectTransform child = rectChildren[i];
                SetChildAlongAxis(child, axis, pos, size);
            }
        }
    }
}