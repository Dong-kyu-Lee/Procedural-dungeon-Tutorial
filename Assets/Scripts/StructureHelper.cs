using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StructureHelper
{
    // 트리의 Leaf노드. 즉, room에 해당하는 노드를 가져온다.
    public static List<Node> TrverseGraphToExtractLowestLeafs(Node parentNode)
    {
        Queue<Node> nodesToCheck = new Queue<Node>();
        List<Node> listToReturn = new List<Node>();
        // 첫 노드가 Leaf노드일 경우.
        if(parentNode.ChildrenNodeList.Count == 0)
        {
            return new List<Node>() { parentNode };
        }
        // Leaf 노드를 탐색하기 위해 첫 노드의 자식을 큐에 집어넣는다.
        foreach(var child in parentNode.ChildrenNodeList)
        {
            nodesToCheck.Enqueue(child);
        }
        // 큐에 들어간 자식노드 중 Leaf노드(자식이 없음)이면 List에 추가하고 그렇지 않으면
        // 그 노드의 자식 노드를 큐에 집어넣음. (BFS랑 비슷)
        while(nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();
            if(currentNode.ChildrenNodeList.Count == 0)
            {
                listToReturn.Add(currentNode);
            }
            else
            {
                foreach(var child in currentNode.ChildrenNodeList)
                {
                    nodesToCheck.Enqueue(child);
                }
            }
        }
        return listToReturn;
    }

    public static Vector2Int GenerateBottomLeftCornerBetween(
        Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        // 공간의 크기와 방의 크기를 일치시키지 않기 위해 방의 왼쪽 아래 좌표의 최대/최소를
        // offset만큼 유격을 둔다.
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;
        // 최종 왼쪽아래 좌표를 결정함.
        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)),
            Random.Range(minY, (int)(minY + (maxY - minY) * pointModifier)));
    }

    public static Vector2Int GenerateTopRightCornerBetween(
        Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        // 공간의 크기와 방의 크기를 일치시키지 않기 위해 방의 오른쪽 위 좌표의 최대/최소를
        // offset만큼 유격을 둔다.
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;
        // 최종 오른쪽위 좌표를 결정함.
        return new Vector2Int(
            Random.Range((int)(minX + (maxX-minX)*pointModifier), maxX),
            Random.Range((int)(minY + (maxY-minY)*pointModifier), maxY));
    }

    public static Vector2Int CalculateMiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 tempVector = sum / 2;
        return new Vector2Int((int)tempVector.x, (int)tempVector.y);
    }
}

public enum RelativePosition
{
    Up,
    Down,
    Right,
    Left
}