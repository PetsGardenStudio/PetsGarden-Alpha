using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 地图控制器 : MonoBehaviour
{
    /// <summary>
    /// 存储地图边缘空白位置的集合。
    /// </summary>
    public HashSet<Vector2Int> edgeEmptySet = new HashSet<Vector2Int>();

    /// <summary>
    /// 存储地图上已被占用位置的集合。
    /// </summary>
    public HashSet<Vector2Int> occupySet = new HashSet<Vector2Int>();

    /// <summary>
    /// 存储地图上所有方块的字典，键为方块的位置，值为方块的游戏对象。
    /// </summary>
    public Dictionary<Vector2Int, GameObject> blockDict = new Dictionary<Vector2Int, GameObject>();

    /// <summary>
    /// 存储地图上所有装饰物的字典，键为装饰物的位置，值为装饰物的游戏对象。
    /// </summary>
    public Dictionary<Vector2Int, GameObject> decorationDict = new Dictionary<Vector2Int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 添加一个方块到地图上。
    /// </summary>
    /// <param name="pos">要添加方块的位置（Vector2Int）。</param>
    /// <param name="block">要添加的方块对象（GameObject）。</param>
    private void addBlock(Vector2Int pos, GameObject block)
    {
        this.edgeEmptySet.Remove(pos);
        this.blockDict.Add(pos, Instantiate(block, new Vector3(pos.x, 0.0f, pos.y), Quaternion.identity));
        addEdgeToEmptyList(pos);
    }

    /// <summary>
    /// 更新地图边缘空白位置集合。
    /// </summary>
    /// <param name="target">新添加的方块位置。</param>
    private void addEdgeToEmptyList(Vector2Int target)
    {
        List<Vector2Int> surrondings = new List<Vector2Int>() {
            target + new Vector2Int(0, 1),
            target + new Vector2Int(0, -1),
            target + new Vector2Int(-1, 0),
            target + new Vector2Int(1, 0)
        };
        foreach (Vector2Int surronding in surrondings)
        {
            if (!this.blockDict.ContainsKey(surronding))
            {
                this.edgeEmptySet.Add(surronding);
            }
        }
    }
}
