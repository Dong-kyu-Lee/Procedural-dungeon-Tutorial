using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BinarySpacePartitioner
{
    private RoomNode rootNode;
    public RoomNode RootNode { get => rootNode; }

    public BinarySpacePartitioner(int dungeonWidth, int dungeonLength)
    {
        rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);
        
    }

    // 루트 노드 크기를 쪼개면서 노드를 생성하는 함수이다.
    public List<RoomNode> PrepareNodesCollection(int maxIterations, int roomWidthMin, int roomLengthMin)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> listToReturn = new List<RoomNode>();
        graph.Enqueue(this.rootNode);
        listToReturn.Add(this.rootNode);
        int iterations = 0;
        while(iterations < maxIterations && graph.Count > 0)
        {
            iterations++;
            RoomNode currentNode = graph.Dequeue();
            if(currentNode.Width >= roomWidthMin*2 || currentNode.Length >= roomLengthMin*2)
            {
                SplitTheSpace(currentNode, listToReturn, roomLengthMin, roomWidthMin, graph);
            }
        }
        return listToReturn;
    }

    private void SplitTheSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomLengthMin, int roomWidthMin, Queue<RoomNode> graph)
    {
        // 공간을 자룰 기준선을 생성한다.
        Line line = GetLineDividingSpace(
            currentNode.BottomLeftAreaCorner, 
            currentNode.TopRightAreaCorner,
            roomWidthMin, roomLengthMin);

        RoomNode node1, node2;
        if(line.Orientation == Orientation.Horizontal) // 수평으로 잘랐을 경우
        {
            // node1 = 밑의 노드, node2 = 위의 노드
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(currentNode.TopRightAreaCorner.x, line.Coordinate.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(currentNode.BottomLeftAreaCorner.x, line.Coordinate.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }
        else // 수직으로 잘랐을 경우
        {
            // node1 = 왼쪽 노드, node2 = 오른쪽 노드
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(line.Coordinate.x, currentNode.TopRightAreaCorner.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(line.Coordinate.x, currentNode.BottomLeftAreaCorner.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }
        AddNewNodeToCollections(listToReturn, graph, node1);
        AddNewNodeToCollections(listToReturn, graph, node2);
    }

    private void AddNewNodeToCollections(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node1)
    {
        listToReturn.Add(node1);
        graph.Enqueue(node1);
    }

    // 공간을 나눌 라인을 설정하는 함수
    private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLengthMin)
    {
        Orientation orientation; // 라인을 수직으로 할 지, 수평으로 할 지
        bool lengthStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= 2 * roomLengthMin;
        bool widthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x) >= 2 * roomWidthMin;
        // 너비와 높이를 절반으로 나눴을 때, 두 값 모두 방의 최소 너비/높이보다 크면
        // 너비/높이 아무거나를 기준으로 자름.
        if (lengthStatus && widthStatus)
        {
            orientation = (Orientation)(UnityEngine.Random.Range(0, 2));
        }
        else if (widthStatus) // 높이가 충분히 작아서 너비로 자름
        {
            orientation = Orientation.Vertical;
        }
        else // 너비가 충분히 작아서 높이로 자름
        {
            orientation = Orientation.Horizontal;
        }

        return new Line(orientation, GetCoordinatesForOrientation(
            orientation,
            bottomLeftAreaCorner,
            topRightAreaCorner,
            roomWidthMin,
            roomLengthMin
            ));
    }

    // 노드 공간의 좌표와 방의 최소 너비/높이 값으로 자를 라인의 좌표를 랜덤으로 구함. 
    private Vector2Int GetCoordinatesForOrientation(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLengthMin)
    {
        Vector2Int coordinates = Vector2Int.zero;
        if(orientation == Orientation.Horizontal)
        {
            coordinates = new Vector2Int(0, 
                Random.Range(bottomLeftAreaCorner.y + roomLengthMin,
                topRightAreaCorner.y - roomLengthMin));
        }
        else
        {
            coordinates = new Vector2Int(
                Random.Range(bottomLeftAreaCorner.x + roomWidthMin,
                topRightAreaCorner.x - roomWidthMin), 0);
        }

        return coordinates;
    }
}