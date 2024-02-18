/*
create by pengyingh 170926
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PModule
{
    public class MConfig
    {
        public enum HF_ResMode
        {
            IDE,
            Debug_Test,
            Debug_Normal,
            Release
        }

        public static readonly string[] HF_ResMode_Name =
        {
            "IDE", "Debug_Test", "Debug_Normal", "Release"
        };

        public static List<string> SLT_PREORITY = new List<string>
            { "PNotifyCtr", "PLuaCtr", "PMTDispatcher", "PAppCtr", "PTipCtr", "PResCtr", "PNodeCtr" };

        // public const Debuger.DebugLevel DEBUG_LEVEL = Debuger.DebugLevel.Log; //None > Log > Warning > Error
        public const string VERSION_CLIENT = "0.0.0.1";
        public static string VERSION_RES = "0.0.0.0";
        public const HF_ResMode HF_TYPE = HF_ResMode.IDE; //0 IDE  1 debug内网  2 debug外网 3 RELEASE
        public const string HF_Basic_Dir = "Sys/Common"; //basic assets dir name    原版：Basic
        public const bool LocalResOnly = false; //true:没有remote resource, 供本地打包用

        public static List<string> HF_LocalArr = new List<string>
        {//通过资源导出时生成静态资源数组进行赋值
        }; //本地文件数组,不放资源防止资源移位造成不可同步  原版："Common","Animal", "Bag", "Battle", "BigMap", "Chat", "CityBuilding", "Common", "Embattle", "Gacha", "HeroDetail", "IdToRes", "Mail", "Mission", "MultiLang", "Pve", "RoleInfo", "WorldMap"

        public static List<string> HF_BasicUpdateArr = new List<string>
        {//通过资源导出时生成静态资源数组进行赋值
        }; //开机更新界面必须更新的资源,目前以服务器填写为准
        
        //需要提前下载的动态资源
        public static List<string> HF_BasicUpdateArrExtra = new List<string>
        {
           
        };

        //只会在本地用到，发资源prerelease/release时会去除
        public static List<string> HF_SimOnlyArr = new List<string>
        {
            "SimBtl"
        };

        public const string RESOURCE_DIR = "Module/Resources/";
        public const string ASSET_DIR = "UsrAssets"; //Contain All Assets Must Under Assets Dir
        public const string RES_DIR = "Res"; //All Resource
        public const string SRC_DIR = "Src"; //All Lua Files
        public const string MRES_NAME = "MRes.txt"; //素材列表 "MRes.bytes"（二进制，比txt慢放弃）
        public const string MRES_CONFIG_NAME = "MResCfg.txt"; //素材列表配置（保存本地文件主键数组、全部主键数组）
        public const string MRES_INNER_NAME = "MResInner.txt"; //在Resources中素材的素材列表名称  "MResInner.bytes"（二进制，比txt慢放弃）
        public const string PROTO_DIR = "Model/Proto"; //Proto 文件的导出相对路径 
        public const string CUSTOMLIB_DIR = "CustomLibs"; //导表时用到第三方lib库文件夹名称
        public const string PROTO_NAME = "GD_Url.pb.txt"; //Proto 文件名称
        public const string PROTO_JSON_NAME = "PbCatalog.json.txt"; //Proto id 映射

        public const string SCENE_FIRST = "GameLogin"; //First Scene Name，For HotFix Run Or Restart Game
        public const ushort SOCKET_VERSION = 1; //Socket Version
        public const ushort SOCKET_BCID_MAX = 999; //Socket Max Broadcast ID, Min Is 0

        public static Dictionary<int, string> QUALITY_KEYS = new Dictionary<int, string>
            { { 1, "hd" }, { 4, "ld" } }; //1 高清    2 中     3 低 , {2, "sd"} 去除sd

        public static Dictionary<string, Dictionary<string, int>> QUALITY_FORMAT_IOS =
            new Dictionary<string, Dictionary<string, int>>
            {
                { "hd", new Dictionary<string, int> { { "t", 48 }, { "h", 50 }, { "l", 51 }, { "x", 50 } } },
                { "ld", new Dictionary<string, int> { { "t", 48 }, { "h", 50 }, { "l", 51 }, { "x", 50 } } }
            };

        public static Dictionary<string, Dictionary<string, int>> QUALITY_FORMAT_ANDROID =
            new Dictionary<string, Dictionary<string, int>>
            {
                { "hd", new Dictionary<string, int> { { "t", 48 }, { "h", 50 }, { "l", 51 }, { "x", 50 } } },
                { "ld", new Dictionary<string, int> { { "t", 47 }, { "h", 47 }, { "l", 65 }, { "x", 65 } } }
            };

        //非Addressables模式使用下面参数
        public const string FRAMEWORK_PATH = "ToLuaFramework/ToLua/Lua"; //Lua Framework Path

        //public static List<string> BASIC_DIR = new List<string>{SRC_DIR + "/" + HF_Basic_Dir, RES_DIR + "/" + HF_Basic_Dir};   //Basic Dir In Package, Hotfix First
        public const string MANIFEST_NAME = "AssetManifest.txt"; //Hotfix File Name
        public const string MANIFEST_TOTAL_NAME = "AssetManifestTotal.txt"; //Hotfix File Name
        public const string TC_NAME = "TCData.txt"; //Temporary Compare File Name
        public const string TCDIR = "TCDir"; //Temporary Compare Dir
        public const string TCSDIR = "TCSDir"; //Temporary Compare Section Dir
        public const string CFG_NAME = "HotFixConfig.txt";
        public static List<string> EXCLUDE_LUA = new List<string> { "strict.lua.txt" };

        public static bool URP_SUPPORT = true;
        public static bool SCENE_CLEAN = true; //切换场景是否删除UI Canvas，重新创建
    }
}
