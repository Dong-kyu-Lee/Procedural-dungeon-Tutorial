using System;
using System.Collections.Generic;
using System.Linq;

public class CorridorsGenerator
{
    public CorridorsGenerator()
    {
    }

    public List<Node> CreateCorridor(List<RoomNode> allNodesCollection, int corridorWidth)
    {
        List<Node> corridorList = new List<Node>();
        Queue<RoomNode> structuresToCheck = new Queue<RoomNode>(
            allNodesCollection.OrderByDescending(node => node.TreeLayerIndex).ToList());
        while(structuresToCheck.Count > 0)
        {
            var node = structuresToCheck.Dequeue();
            // 자식이 없는 노드일 경우 복도 생성을 하지 않음.
            // 자식 노드들을 복도로 잇는 작업을 수행하기 때문.
            if(node.ChildrenNodeList.Count == 0)
            {
                continue;
            }
            CorridorNode corridor = new CorridorNode(node.ChildrenNodeList[0], node.ChildrenNodeList[1],
                corridorWidth);
            corridorList.Add(corridor);
        }

        return corridorList;
    }
}