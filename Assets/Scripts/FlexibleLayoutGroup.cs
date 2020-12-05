using UnityEngine;
using UnityEngine.UI;

public class FlexibleLayoutGroup : LayoutGroup
{
    public enum FitType
    {
        Fixed,
        Width,
        Height
    }

    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;

    public FitType fitType;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        var sqrRt = Mathf.Sqrt(transform.childCount);

        rows = Mathf.CeilToInt(sqrRt);
        columns = Mathf.CeilToInt(sqrRt);

        switch (fitType)
        {
            case FitType.Width:
                rows = Mathf.CeilToInt(transform.childCount / (float) columns);
                break;

            case FitType.Height:
                columns = Mathf.CeilToInt(transform.childCount / (float) rows);
                break;
        }

        var parentWidth = rectTransform.rect.width;
        var parentHeight = rectTransform.rect.height;

        var cellWidth = parentWidth / columns - spacing.x / columns * 2;
        var cellHeight = parentHeight / rows - spacing.y / rows * 2;

        cellSize.x = cellWidth;
        cellSize.y = cellHeight;
        for (var i = 0; i < rectChildren.Count; i++)
        {
            var rowCount = i / columns;
            var colCount = i % columns;

            var item = rectChildren[i];

            var xPos = cellSize.x * colCount + spacing.x * colCount;
            var yPos = cellSize.y * rowCount + spacing.y * rowCount;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical()
    {
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
}