using UnityEngine;

// 공간을 쪼갤 기준선에 대한 클래스
public class Line
{
    private Orientation orientation;
    private Vector2Int coordinate;

    public Orientation Orientation { get => orientation; set => orientation = value; }
    public Vector2Int Coordinate { get => coordinate; set => coordinate = value; }
    public Line(Orientation orientation, Vector2Int coordinates)
    {
        this.Orientation = orientation;
        this.Coordinate = coordinates;
    }

}

// Line의 수직, 수평 여부
public enum Orientation
{
    Horizontal = 0,
    Vertical = 1
}