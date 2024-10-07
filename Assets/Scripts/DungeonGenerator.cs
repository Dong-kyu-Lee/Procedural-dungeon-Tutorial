using System;
using System.Collections.Generic;
using UnityEngine;

// 던전 렌덤 생성의 구현 클래스
public class DungeonGenerator
{
    List<RoomNode> allNodesCollection = new List<RoomNode>();
    private int dungeonWidth;
    private int dungeonLength;
    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }

    public List<Node> CalculateDungeon(int maxIterations, int roomWidthMin, int roomLengthMin,
        float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        // 트리의 모든 공간 노드를 생성
        allNodesCollection = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);
        // 모든 공간 노드 중 방이 될 노드(Leaf노드)만을 가져옴
        List<Node> roomSpaces = StructureHelper.TrverseGraphToExtractLowestLeafs(bsp.RootNode);
        // 공간 안에 방(Room)을 생성할 클래스
        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLengthMin, roomWidthMin);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces, roomBottomCornerModifier,
            roomTopCornerModifier, roomOffset);

        CorridorsGenerator corridorsGenerator = new CorridorsGenerator();
        var corridorList = corridorsGenerator.CreateCorridor(allNodesCollection, corridorWidth);

        return new List<Node>(roomList);
    }
}