using System.Collections.Generic;
using UnityEngine;

public class ClosestNeighborTree
{
    List<Vector3> positions;

    public void Start(ref List<Vector3> positionList)
    {
        positions = positionList;
    }

    public void Insert(Vector3 position)
    {

    }
}


class TreeNode
{
    uint startIndex;
    uint endIndex;
    BoundingBox boundingBox;

    TreeNode[] directions = new (4)

    public bool IsInBounds(Vector3 position)
    {
        if(position.x < boundingBox.upperLeft.x)    return false;
        if(position.y < boundingBox.upperLeft.y)    return false;
        if(position.x > boundingBox.bottomRight.x)  return false;
        if(position.y > boundingBox.bottomRight.y)  return false;
        return true;
    }
}

struct BoundingBox
{
    public Vector2 upperLeft;
    public Vector2 bottomRight;
}