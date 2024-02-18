/*
create by pengyingh 170922
*/
using UnityEngine;
public class GameEntry : MonoBehaviour {
    private void Start ()
    {
        StartCoroutine(PModule.PSltCtr.Slt<PModule.PGameCtr>().Start());
    }
}