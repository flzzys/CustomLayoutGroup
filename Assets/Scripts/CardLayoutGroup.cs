using UnityEngine;
using System.Collections.Generic;

namespace UnityEngine.UI {
	//卡片布局：可设定卡片尺寸和最大尺寸倍率，超过范围增加行列数
	public class CardLayoutGroup : LayoutGroup {
		#region Var

		[Header("间距")]
		[SerializeField] protected Vector2 m_Spacing = Vector2.zero;
		public Vector2 spacing { get { return m_Spacing; } set { SetProperty(ref m_Spacing, value); } }

		public enum Constraint { FixedColumnCount = 0, FixedRowCount = 1 }
		[Header("限制行列数")]
		[SerializeField] protected Constraint m_Constraint = Constraint.FixedColumnCount;
		public Constraint constraint { get { return m_Constraint; } set { SetProperty(ref m_Constraint, value); } }

		[Range(1, 10)]
		[SerializeField] protected int m_ConstraintCount = 3;
		public int constraintCount { get { return m_ConstraintCount; } set { SetProperty(ref m_ConstraintCount, Mathf.Max(1, value)); } }

		[Header("尺寸")]
		public Vector2 size = new Vector2(100, 100);
		[Range(0.1f, 1)]
		public float scale = 1;

		//最大尺寸倍率，超过这个尺寸增加行列数
		public float maxSizeMultiplier = 1.5f;

		#endregion

		#region private

		//行列数
		public int columns { get; private set; }
		public int rows { get; private set; }

		//目标行列的物体序号
		private int[,] cellIndexAtGridRef;

		private int[] cellColumn;
		private int[] cellRow;

		//物体偏好尺寸
		private Vector2[] cellPreferredSizes;

		//总行宽列高
		private float totalColumnWidth;
		private float totalRowHeight;

		//获取在目标行列的子物体序号
		public int GetCellIndexAtGridRef(int column, int row) {
			if (column >= 0 && column < columns && row >= 0 && row < rows)
				return cellIndexAtGridRef[column, row];
			else
				return -1;
		}

		//获取子物体所在行列
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

		//获取指定行列的位置
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

		//获取子物体尺寸
		float GetSize(int axis) {
			return axis == 0 ? size.x : size.y;
		}
		//获取子物体缩放后尺寸
		Vector2 GetScaledSize {
            get {
				return size * scale;
            }
        }

		#endregion

		protected override void Start() {
			
		}

        public void GetFirstItemPos() {
			GameObject go = new GameObject("go");
			go.AddComponent<RectTransform>();
			go.transform.SetParent(transform.parent);
			go.transform.localScale = Vector3.one;
			go.transform.position = transform.GetChild(0).GetComponent<RectTransform>().position;
			go.GetComponent<RectTransform>().sizeDelta = size * scale;

			print(transform.GetChild(0).GetComponent<RectTransform>().position);
		}

		//初始化
		private void InitializeLayout() {
			//列数
			columns = Mathf.FloorToInt((float)rectTransform.rect.width / (GetScaledSize.x + spacing.x));
			columns = Mathf.Max(columns, 1);
			//行数
			rows = Mathf.CeilToInt((float)rectTransform.childCount / columns);

			//print($"行数:{rows}, 列数:{columns}");

			cellIndexAtGridRef = new int[columns, rows];
			cellColumn = new int[rectChildren.Count];
			cellRow = new int[rectChildren.Count];
			cellPreferredSizes = new Vector2[rectChildren.Count];

			//总行宽列高
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
				cellPreferredSizes[cell] = new Vector2(GetScaledSize.x, GetScaledSize.y);

				// next
				c += cNext;
				if (c < 0 || c >= columns) {
					c = cOrigin;
					r += rNext;
				}
			}

			//修改本体高度
			float totalPreferredHeight = padding.vertical + totalRowHeight + spacing.y * (rows - 1);
			var rect = rectTransform;
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, totalPreferredHeight);
		}

		#region 设置

		public override void CalculateLayoutInputHorizontal() {
			base.CalculateLayoutInputHorizontal();

			InitializeLayout();

			//最小宽度
			float totalMinWidth = padding.horizontal;
			//偏好宽度
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
			// Get origin
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

			// Set cells
			for (int i = 0; i < rectChildren.Count; i++) {
				
				int colrow = (axis == 0 ? GetCellColumn(i) : GetCellRow(i));

				// Column/row origin
				float cellOrigin = gridOrigin + (axis == 0 ? GetColumnPositionWithinGrid(colrow) : GetRowPositionWithinGrid(colrow));

				// Column/row size and space
				float cellSpace = (axis == 0 ? GetScaledSize.x : GetScaledSize.y);
				var child = rectChildren[i];
				float cellSize = GetSize(axis);
				float cellExtraSpace = cellSpace - cellSize;

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
