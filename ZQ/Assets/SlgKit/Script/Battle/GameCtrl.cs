using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    PlayerController[] players;
    PlayerController curSelect;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindObjectsOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        var hitWorldPoint = MouseRayCast();
        if (hitWorldPoint != null)
        {
            Debug.Log("点击的位置 = " + hitWorldPoint);
            var hitMapNode = AstarPath.active.GetNearest((Vector3)hitWorldPoint).node;
            var hitMapPos = hitMapNode.position;
            //通过地图坐标获取人物
            var hitPlayer = SelectPlayer(hitMapPos);
            if (curSelect == null)
            {
                curSelect = hitPlayer;
                if (curSelect != null)
                {
                    curSelect.ShowMoveRange();
                    curSelect.Ready();
                }
            }
        }
    }

    private PlayerController SelectPlayer(Int3 hitMapPos)
    {
        foreach (PlayerController player in players)
        {

            if (player.mapPos == hitMapPos)
            {
                return player;
            }
            
        }
        return null;
    }

    /// <summary>
    /// ? 代表可返回空
    /// </summary>
    /// <returns></returns>
    Vector3? MouseRayCast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            bool result =  Physics.Raycast(ray, out raycastHit);
            if (result)
            {
                return raycastHit.point;
            }
        }
        return null;
    }
}
