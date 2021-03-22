using UnityEngine;
using System.Collections.Generic;

namespace UnityEngine.UI {
	//��Ƭ���֣����趨��Ƭ�ߴ�����ߴ籶�ʣ�������Χ����������
	public class CardLayoutGroup : LayoutGroup {
		#region public����

		[Header("���")]
		public Vector2 spacing;

		[Header("�ߴ�")]
		public Vector2 size = new Vector2(100, 100);
		//����
		[Range(0.1f, 1)]
		public float scale = 1;

		//���ߴ籶�ʣ���������ߴ�����������
		public float maxSizeMultiplier = 1.5f;

		#endregion

		#region private

		//������
		public int columns { get; private set; }
		public int rows { get; private set; }

		//Ŀ�����е��������
		private int[,] cellIndexAtGridRef;

		private int[] cellColumn;
		private int[] cellRow;

		//���п��и�
		private float totalColumnWidth;
		private float totalRowHeight;

		//��ȡ��Ŀ�����е����������
		public int GetCellIndexAtGridRef(int column, int row) {
			if (column >= 0 && column < columns && row >= 0 && row < rows)
				return cellIndexAtGridRef[column, row];
			else
				return -1;
		}

		//��ȡ��������������
		public int GetCellColumn(int cellIndex) {
			if (cellIndex >= 0 && cellIndex < rectChildren.Count)
				return cellColumn[cellIndex];
			else
				return -1;
		}
		public int GetCellRow(int cellIndex) {
			if (cellIndex >= 0 && cellIndex < rectChildren.Count)
				return cellRow[cellIndex];
			else
				return -1;
		}

		//��ȡָ�����е�λ��
		public float GetColumnPositionWithinGrid(int column) {
			if (column <= 0 || column >= columns)
				return 0;

			float pos = 0;
			for (int c = 0; c < column; c++) {
				pos += GetScaledSize.x + spacing.x;
			}
			return pos;
		}
		public float GetRowPositionWithinGrid(int row) {
			if (row <= 0 || row >= rows)
				return 0;

			float pos = 0;
			for (int r = 0; r < row; r++) {
				pos += GetScaledSize.y + spacing.y;
			}
			return pos;
		}

		//��ȡ������ߴ�
		float GetSize(int axis) {
			return axis == 0 ? size.x : size.y;
		}
		//��ȡ���������ź�ߴ�
		Vector2 GetScaledSize {
            get {
				return size * scale;
            }
        }

		//��ȡ������ƫ�óߴ�
		Vector2 GetPreferredSize {
            get {
				float s = (rectTransform.rect.width - padding.horizontal - spacing.x * (columns - 1)) / columns / GetScaledSize.x;
				return size * scale * s;
			}
        }

		#endregion

        public void GetFirstItemPos() {
			GameObject go = new GameObject("go");
			go.AddComponent<RectTransform>();
			go.transform.SetParent(transform.parent);
			go.transform.localScale = Vector3.one;
			go.transform.position = transform.GetChild(0).GetComponent<RectTransform>().position;
			go.GetComponent<RectTransform>().sizeDelta = size * scale;

			print(transform.GetChild(0).GetComponent<RectTransform>().position);
		}

		//��ʼ��
		private void InitializeLayout() {
			//����
			columns = Mathf.FloorToInt((float)rectTransform.rect.width / (GetScaledSize.x + spacing.x));
			columns = Mathf.Max(columns, 1);
			//����
			rows = Mathf.CeilToInt((float)rectTransform.childCount / columns);

			//print($"����:{rows}, ����:{columns}");

			cellIndexAtGridRef = new int[columns, rows];
			cellColumn = new int[rectChildren.Count];
			cellRow = new int[rectChildren.Count];
			//cellPreferredSizes = new Vector2[rectChildren.Count];

			//���п��и�
			totalColumnWidth = GetScaledSize.x * columns;
			totalRowHeight = GetScaledSize.y * rows;

			for (int a = 0; a < columns; a++) {
				for (int b = 0; b < rows; b++) {
					cellIndexAtGridRef[a, b] = -1;
				}
			}

			int cOrigin = 0;
			int rOrigin = 0;
			int cNext = 1;
			int rNext = 1;
			int c = cOrigin;
			int r = rOrigin;

			for (int cell = 0; cell < rectChildren.Count; cell++) {
				cellIndexAtGridRef[c, r] = cell;
				cellColumn[cell] = c;
				cellRow[cell] = r;
				//cellPreferredSizes[cell] = new Vector2(GetScaledSize.x, GetScaledSize.y);

				// next
				c += cNext;
				if (c < 0 || c >= columns) {
					c = cOrigin;
					r += rNext;
				}
			}

			//�޸ı���߶�
			float totalPreferredHeight = padding.vertical + totalRowHeight + spacing.y * (rows - 1);
			var rect = rectTransform;
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, totalPreferredHeight);
		}

		#region ����

		public override void CalculateLayoutInputHorizontal() {
			base.CalculateLayoutInputHorizontal();

			InitializeLayout();

			//��С���
			float totalMinWidth = padding.horizontal;
			//ƫ�ÿ��
			float totalPreferredWidth = padding.horizontal + totalColumnWidth + spacing.x * (columns - 1);

			SetLayoutInputForAxis(totalMinWidth, totalPreferredWidth, -1, 0);
		}
		public override void CalculateLayoutInputVertical() {
			float totalMinHeight = padding.vertical;
			float totalPreferredHeight = padding.vertical + totalRowHeight + spacing.y * (rows - 1);

			SetLayoutInputForAxis(totalMinHeight, totalPreferredHeight, -1, 1);
		}

		public override void SetLayoutHorizontal() {
			SetCellsAlongAxis(0);
		}
		public override void SetLayoutVertical() {
			SetCellsAlongAxis(1);
		}

        #endregion

        //------------------------------------------------------------------------------------------------------
        private void SetCellsAlongAxis(int axis) {
			//Դ��
			float space = (axis == 0 ? rectTransform.rect.width : rectTransform.rect.height);
			float extraSpace = space - LayoutUtility.GetPreferredSize(rectTransform, axis);
			float gridOrigin = (axis == 0 ? padding.left : padding.top);
			if (axis == 0) {
				if (childAlignment == TextAnchor.UpperCenter || childAlignment == TextAnchor.MiddleCenter || childAlignment == TextAnchor.LowerCenter) {
					gridOrigin += extraSpace / 2f;
				} else if (childAlignment == TextAnchor.UpperRight || childAlignment == TextAnchor.MiddleRight || childAlignment == TextAnchor.LowerRight) {
					gridOrigin += extraSpace;
				}
			} else {
				if (childAlignment == TextAnchor.MiddleLeft || childAlignment == TextAnchor.MiddleCenter || childAlignment == TextAnchor.MiddleRight) {
					gridOrigin += extraSpace / 2f;
				} else if (childAlignment == TextAnchor.LowerLeft || childAlignment == TextAnchor.LowerCenter || childAlignment == TextAnchor.LowerRight) {
					gridOrigin += extraSpace;
				}
			}

			//����ƫ�óߴ�
			float s = (rectTransform.rect.width - padding.horizontal - spacing.x * (columns - 1)) / columns / GetScaledSize.x;
			//print(s);

			//����������λ��
			for (int i = 0; i < rectChildren.Count; i++) {
				//���к�
				int colrow = (axis == 0 ? GetCellColumn(i) : GetCellRow(i));

				// Column/row origin
				float cellOrigin = gridOrigin + (axis == 0 ? GetColumnPositionWithinGrid(colrow) : GetRowPositionWithinGrid(colrow));

				//������ߴ磬���
				float cellSpace = (axis == 0 ? GetScaledSize.x : GetScaledSize.y);
				//float cellSpace = (axis == 0 ? GetPreferredSize.x : GetPreferredSize.y);
				float cellSize = GetSize(axis);
				float cellExtraSpace = cellSpace - cellSize;

				//cellExtraSpace = 

				// If cell should stretch, place there. If not, place within cell space according to cell alignment and its preferred size
				int thisCellAlignment = 0;
				
				if (thisCellAlignment == 0)
					cellOrigin += cellExtraSpace / 2f;
				if (thisCellAlignment == 1)
					cellOrigin += cellExtraSpace;

				SetChildAlongAxis(rectChildren[i], axis, cellOrigin, cellSize);
				rectChildren[i].localScale = Vector3.one * scale;
			}
		}
	}
}
