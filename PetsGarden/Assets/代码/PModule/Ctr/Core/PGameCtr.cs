using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using static PModule.PSltCtr;
using UnityEngine;

namespace PModule
{
    [UnityEngine.DefaultExecutionOrder(-1)]
    public class PGameCtr : PSlt
    {
        public IEnumerator Start()
        {
            if (!string.Equals(SceneManager.GetActiveScene().name, MConfig.SCENE_FIRST))
            {
                yield break;
            }


        }
    }
}