using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 地块控制器 : MonoBehaviour
{
    public Block block {  get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Block {
    public AreaTag areaCategory { get; private set; }
    public string blockType { get; private set; }
}
