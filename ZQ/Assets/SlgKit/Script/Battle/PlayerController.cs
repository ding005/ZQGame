using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 移动范围
    /// </summary>
    int _range = 5;

    /// <summary>
    /// 是否能涉水
    /// </summary>
    public bool canTraverseWater = false;

    Animator animator;
    void Start()
    {
        //StartCoroutine(testUpdata());
        //StartCoroutine(testUpdataAB_path());

        //把世界坐标转换成地图坐标
        worldPos2MapPos();
        animator = transform.GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public Int3 mapPos
    {
        get
        {
            return AstarPath.active.GetNearest(this.transform.position).node.position;
        }
    }

    void worldPos2MapPos()
    {
        this.transform.position = (Vector3)this.mapPos;
        Debug.Log("this.transform.position == "+this.transform.position);
    }

    IEnumerator testUpdata()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            ShowMoveRange();
        }
    }

    internal void Ready()
    {
        this.startMovePos = this.transform.position;
        animator.CrossFade("ready", 0.2f);
    }

    IEnumerator testUpdataAB_path()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            showABPath();
        }
    }

    [ContextMenu("显示范围")]
    public void ShowMoveRange()
    {
        getMovePath((Path path) =>
        {
            GridMeshManager.Instance.ShowPath(path.path);
        }
        );
    }

    public void getMovePath(System.Action<Path> onPathSearchCompleteCallBack)
    {
        int moveScore = this._range * 1000 * 3;

        var searchPath = MoveRangConStantPath.Construct(this.transform.position, moveScore, canTraverseWater,
            (Path path) =>
            {
                path.path = (path as MoveRangConStantPath).allNodes;
                onPathSearchCompleteCallBack.Invoke(path);
                
            }
            );
        AstarPath.StartPath(searchPath, true);
    }

    public Transform targetPos;
    public Vector3 startMovePos;
    void showABPath()
    {
        GetMoveAPPathCallBack(this.transform.position, targetPos.position,
            (ABPath path) =>
             {
                 GridMeshManager.Instance.ShowPath(path.path);
             }

            );
    }

    /// <summary>
    /// 获取两点之间的行走路径
    /// </summary>
    /// <param name="p_start"></param>
    /// <param name="end"></param>
    /// <param name="v_path"></param>
    public void GetMoveAPPathCallBack(Vector3 p_start , Vector3 end ,Action<ABPath> v_path)
    {
        Vector3 p_endPos = (Vector3)AstarPath.active.GetNearest(end, new NNCPlayerMove()).node.position;
        ABPath mPath = ABPath.Construct(p_start, p_endPos,
            (Path path) =>
            {
                ABPath m_path = path as ABPath;

                v_path(m_path);
            }
            );
        var p_startNode = AstarPath.active.GetNearest(p_start).node;
        mPath.nnConstraint = new NNCMoveAbPath(p_startNode);

        AstarPath.StartPath(mPath, true);
    }
}
