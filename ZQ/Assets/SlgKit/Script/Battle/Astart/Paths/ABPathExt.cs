using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ABPath升级类
/// </summary>
public class ABPathExt : ABPath
{
    public  bool canTraverseWater = false;
    public override bool CanTraverse(GraphNode node)
    {
        //搜索路径时 , 如果遇到水节点和路径不可遍历水则返回 节点不可能被穿越
        if (node.Tag == (uint)GameDefine.AstartTag.Water && !canTraverseWater) return false;
        return base.CanTraverse(node);
    }
}
