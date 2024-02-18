/*
create by pengyingh 210318
*/
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

namespace PModule
{
    public static class UnityEngineExt
    {
        public static bool Overlaps(this RectTransform a, RectTransform b) {
            return a.WorldRect().Overlaps(b.WorldRect());
        }
        public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse) {
            return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
        }
        public static bool OverlapsArea(this RectTransform a, RectTransform b, bool allowInverse, out Rect area) {
            area = new Rect();
            var r1 = a.WorldRect();
            var r2 = b.WorldRect();
            if (r1.Overlaps(r2))
            {
                var x1 = Mathf.Min(r1.xMax, r2.xMax);
                var x2 = Mathf.Max(r1.xMin, r2.xMin);
                var y1 = Mathf.Min(r1.yMax, r2.yMax);
                var y2 = Mathf.Max(r1.yMin, r2.yMin);
                area.x = Mathf.Min(x1, x2);
                area.y = Mathf.Min(y1, y2);
                area.width = Mathf.Max(0.0f, x1 - x2);
                area.height = Mathf.Max(0.0f, y1 - y2);
                return true;
            }

            return false;
        }

        public static Rect WorldRect(this RectTransform rectTransform) {
            var rect = rectTransform.rect;
            var rectTransformWidth = rect.width * rectTransform.lossyScale.x;
            var rectTransformHeight = rect.height * rectTransform.lossyScale.y;

            var position = rectTransform.position;
            return new Rect(position.x - rectTransformWidth * rectTransform.pivot.x, position.y - rectTransformHeight * rectTransform.pivot.y, rectTransformWidth, rectTransformHeight);
        }

        /// <summary>
        /// 获取Vector2 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector2 GetVector2(float x, float y) {
            return new Vector2(x, y);
        }

        /// <summary>
        /// 获取Vector3 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3 GetVector3(float x, float y, float z) {
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// 获取Vector4 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static Vector4 GetVector4(float x, float y, float z, float w) {
            return new Vector4(x, y, z, w);
        }

        /// <summary>
        /// 获取Quaternion 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static Quaternion GetQuaternion(float x, float y, float z, float w) {
            return new Quaternion(x, y, z, w);
        }

        /// <summary>
        /// 设置旋转角度 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetRotation(this Transform t, float x, float y, float z) {
            t.eulerAngles = new Vector3(x, y, z);
        }

        /// <summary>
        /// 设置局部角度 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetLocalRotation(this Transform t, float x, float y, float z) {
            t.localEulerAngles = new Vector3(x, y, z);
        }

        /// <summary>
        /// 设置局部缩放系数 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetLocalScale(this Transform t, float x, float y, float z) {
            t.localScale = new Vector3(x, y, z);
        }

        /// <summary>
        /// 设置坐标 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetPosition(this Transform t, float x, float y, float z) {
            t.position = new Vector3(x, y, z);
        }

        /// <summary>
        /// 设置局部坐标 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetLocalPosition(this Transform t, float x, float y, float z) {
            t.localPosition = new Vector3(x, y, z);
        }

        public static void SetAnchoredPosition(this Transform t, float x, float y, float z = 0f) {
            t.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(x, y, z);
        }

        public static void SetAnchoredPosition(this RectTransform t, float x, float y) {
            t.anchoredPosition = new Vector2(x, y);
        }

        /// <summary>
        /// 设置RectTransform尺寸
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetSizeDelta(this RectTransform t, float x, float y) {
            t.sizeDelta = new Vector2(x, y);
        }

        /// <summary>
        /// 设置旋转角度 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetRotation(this GameObject t, float x, float y, float z) {
            t.transform.eulerAngles = new Vector3(x, y, z);
        }

        /// <summary>
        /// 设置局部角度 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetLocalRotation(this GameObject t, float x, float y, float z) {
            t.transform.localEulerAngles = new Vector3(x, y, z);
        }

        /// <summary>
        /// 设置局部缩放系数 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetLocalScale(this GameObject t, float x, float y, float z) {
            t.transform.localScale = new Vector3(x, y, z);
        }

        /// <summary>
        /// 设置坐标 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetPosition(this GameObject t, float x, float y, float z) {
            t.transform.position = new Vector3(x, y, z);
        }

        /// <summary>
        /// 设置局部坐标 版本支持a>48(1.0.5) i>33(1.0.1)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetLocalPosition(this GameObject t, float x, float y, float z) {
            t.transform.localPosition = new Vector3(x, y, z);
        }

        public static void SetAnchoredPosition(this GameObject t, float x, float y, float z = 0) {
            t.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(x, y, z);
        }


        /// <summary>
        /// 设置RectTransform尺寸
        /// </summary>
        /// <param name="t"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetSizeDelta(this GameObject t, float x, float y) {
            t.GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
        }

		/// <summary>
		/// 设置节点可见性
		/// </summary>
		/// <param name="rootNode">根节点</param>
		/// <param name="nodeName">节点名称</param>
		/// <param name="isActive">是否可见</param>
		/// <returns></returns>
        public static Transform SetActiveByName (Transform rootNode, string nodeName, bool isActive)
        {
            var node = GetNodeFromChild(rootNode, nodeName);
            if (!node)
            {
                return null;
            }
            node.gameObject.SetActive(isActive);
            return node;
        }

        /// <summary>
        /// 设置节点可见性
        /// </summary>
        /// <param name="tr">节点transform</param>
        /// <param name="isActive">是否可见</param>
        /// <returns></returns>
        public static void SetActive (this Transform tr , bool isActive)
        {
            tr.gameObject.SetActive(isActive);
        }

        //克隆节点
        public static Transform CloneNode(Transform objModel, Transform parent = null)
        {
            var obj = Object.Instantiate(objModel);
            if (parent)
            {
                obj.SetParent(parent);
            }

            obj.localScale = objModel.localScale;
            obj.localPosition = objModel.localPosition;
            return obj;
        }

        //递归执行节点方法
        public static void RecursivelyChildren(Transform obj, Action<Transform> callback)
        {
            callback(obj);
            foreach (Transform child in obj) {
                RecursivelyChildren(child, callback);
            }
        }

        /// <summary>
        /// 递归找到隐藏的父节点
        /// </summary>
        /// <param name="tr"></param>
        /// <returns></returns>
        public static Transform FindUnactiveParent(Transform tr)
        {
            tr = tr.parent;
            if (!tr) return null;
            if (tr.gameObject.activeInHierarchy)
            {
                return null;
            }

            while (tr && tr.gameObject.activeSelf)
            {
                tr = tr.parent;
            }

            return tr;
        }

        /*使用案例
        local a = UnityEngineExt.FindUnactiveParents(UnityEngineExt.GetNodeFromChild(BigMapHUDLayer.thisBundle.node.transform, "prefab_missionTrack.prefab"))
        if a then
            for i = 0, a.Count - 1 do
                print(a[i].name)
            end
        end
        */
        /// <summary>
        /// 递归找到所有隐藏父节点
        /// </summary>
        /// <param name="tr"></param>
        /// <returns></returns>
        public static List<Transform> FindUnactiveParents(Transform tr)
        {
            var ret = new List<Transform>();

            while (tr = FindUnactiveParent(tr))
            {
                ret.Add(tr);
            }

            return ret;
        }

        public static void RecursivelyChildrenActive(Transform obj, Action<Transform> callback)
        {
            if (!obj.gameObject.activeInHierarchy) return;
            callback(obj);
            foreach (Transform child in obj) {
                RecursivelyChildrenActive(child, callback);
            }
        }

        //递归改变层标记
        public static void RecursivelyChangeLayer(Transform obj, int layerValue)
        {
            obj.gameObject.layer = layerValue;
            foreach (Transform child in obj.transform) {
                RecursivelyChangeLayer(child, layerValue);
            }
        }

        //检测节点是否有效
        public static bool IsValid(this UnityEngine.GameObject o)
        {
            return o && !o.Equals(null);
        }

        /// <summary>
        /// 检查Transform存在
        /// </summary>
        /// <param name="t">Transform</param>
        /// <returns></returns>
        public static bool IsValid(this UnityEngine.Transform t)
        {
            return t && !t.Equals(null);
        }

        //递归获取所有子节点
        public static List<Transform> GetChildrenDeep(Transform t, List<Transform> aList = null)
        {
            if (aList == null)
                aList = new List<Transform>();
            for (var n = 0; n < t.childCount; n++)
            {
                var c = t.GetChild(n);
                aList.Add(c);
                GetChildrenDeep(c, aList);
            }
            return aList;
        }

        //获取所有子节点
        public static List<Transform> GetChildren(Transform t)
        {
            var aList = new List<Transform>();
            for (var n = 0; n < t.childCount; n++)
            {
                aList.Add(t.GetChild(n));
            }
            return aList;
        }

        //遍历场景根节点，找到指定名称的所有子节点
        public static Transform GetNodeFromRoot(string name, Transform exld = null)
        {
            var arr = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootObj in arr)
            {
                if (rootObj.transform == exld) continue;
                if (rootObj.name.Equals(name) && rootObj.IsValid()) return rootObj.transform;
            }
            foreach (var rootObj in arr)
            {
                if (rootObj.transform == exld) continue;
                var ret = GetNodeFromChild(rootObj.transform, name);
                if (ret)
                {
                    return ret;
                }
            }
            return null;
        }

        public static Transform GetNodeFromRoot(string name, int level, Transform exld = null)
        {
            Transform ret;
            var arr = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootObj in arr)
            {
                if (rootObj.transform == exld) continue;
                if (rootObj.name.Equals(name) && rootObj.IsValid()) return rootObj.transform;
            }

            if (--level <= 0)
            {
                return null;
            }
            foreach (var rootObj in arr)
            {
                if (rootObj.transform == exld) continue;
                ret = GetNodeFromChild(rootObj.transform, name, level, exld);
                if (ret)
                {
                    return ret;
                }
            }
            return null;
        }

        //通过一组节点获取某节点
        public static Transform GetNodeFromChild(Transform t, string[] names, int[] idxArr = null)
        {
            if (idxArr == null)
            {
                foreach (var e in names)
                {
                    t = GetNodeFromChild(t, e);
                    if (!t) return null;
                }

                return t;
            }

            for (var i = 0; i < names.Length - 1; i++)
            {
                t = GetNodeFromChild(t, names[i]);
                if (!t) return null;
            }
            return GetNodeFromChild(t, names[names.Length - 1], null, idxArr);
        }

        //递归获得指定名字子节点
        public static Transform GetNodeFromChild(Transform t, string name, Transform exld = null, int[] idxArr = null, bool isFirst = true)
        {
            if (!t) return null;
            if (isFirst)
            {
                if (t == exld) return null;
                if (t.IsValid() && t.name.Equals(name))
                {
                    if (idxArr == null || --idxArr[0] <= 0)
                    {
                        return t;
                    }
                }
            }

            if (exld)
            {
                foreach (Transform t1 in t)
                {
                    if (t1 == exld) continue;
                    if (t1.IsValid() && t1.name.Equals(name))
                    {
                        if (idxArr == null || --idxArr[0] <= 0)
                        {
                            return t1;
                        }
                    }
                }

                foreach (Transform t1 in t)
                {
                    if (t1 == exld) continue;
                    var ret = GetNodeFromChild(t1, name, exld, idxArr, false);
                    if (ret)
                    {
                        return ret;
                    }
                }
            }
            else
            {
                foreach (Transform t1 in t)
                {
                    if (t1.IsValid() && t1.name.Equals(name))
                    {
                        if (idxArr == null || --idxArr[0] <= 0)
                        {
                            return t1;
                        }
                    }
                }

                foreach (Transform t1 in t)
                {
                    var ret = GetNodeFromChild(t1, name, null, idxArr, false);
                    if (ret)
                    {
                        return ret;
                    }
                }
            }


            return null;
        }


        //leve 为1的时候用 t.Find(name) 来寻找更快
        //递归获得指定名字子节点,并取得上面组件 level: 遍历层级
        public static Transform GetNodeFromChild(Transform t, string name, int level, Transform exld = null, int[] idxArr = null, bool isFirst = true)
        {
            if (!t || level <= 0) return null;
            level --;
            if (isFirst)
            {
                if (t == exld) return null;
                if (t.IsValid() && t.name.Equals(name))
                {
                    if (idxArr == null || --idxArr[0] <= 0)
                    {
                        return t;
                    }
                }
            }
            if (exld)
            {
                foreach (Transform t1 in t)
                {
                    if (t1 == exld) continue;
                    if (t1.IsValid() && t1.name.Equals(name))
                    {
                        if (idxArr == null || --idxArr[0] <= 0)
                        {
                            return t1;
                        }
                    }
                }

                foreach (Transform t1 in t)
                {
                    if (t1 == exld) continue;
                    var ret = GetNodeFromChild(t1, name, level, exld, idxArr, false);
                    if (ret)
                    {
                        return ret;
                    }
                }
            }
            else
            {
                foreach (Transform t1 in t)
                {
                    if (t1.IsValid() && t1.name.Equals(name))
                    {
                        if (idxArr == null || --idxArr[0] <= 0)
                        {
                            return t1;
                        }
                    }
                }

                foreach (Transform t1 in t)
                {
                    var ret = GetNodeFromChild(t1, name, level, null, idxArr, false);
                    if (ret)
                    {
                        return ret;
                    }
                }
            }

            return null;
        }

        //递归获得指定名字子节点,并取得上面组件
        public static T GetNodeFromChild<T>(Transform t, string name, Transform exld = null, int[] idxArr = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return t.GetComponent<T>();
            }
            var ret = GetNodeFromChild(t, name, exld, idxArr);
            if (ret)
            {
                return ret.GetComponent<T>();
            }
            return default;
        }

        //递归获得指定名字子节点的组件 level: 遍历层级
        public static T GetNodeFromChild<T>(Transform t, string name, int level, Transform exld = null, int[] idxArr = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return t.GetComponent<T>();
            }
            if (t == null) return default;
            var ret = GetNodeFromChild(t, name, level, exld, idxArr);
            if (ret != null)
            {
                return ret.GetComponent<T>();
            }
            return default;
        }

        //递归获得指定名字父节点
        public static Transform GetNodeFromParent(Transform t, string name)
        {
            if (!t) return null;
            var ret = t.parent;
            if (!ret)
            {
                return GetNodeFromRoot(name, t);
            }
            if (ret && ret.name.Equals(name) && ret.IsValid())
            {
                return ret;
            }
            ret = GetNodeFromChild(ret, name, t);
            return ret ? ret : GetNodeFromParent(ret, name);
        }

        //递归获得指定名字父节点 level: 遍历层级
        public static Transform GetNodeFromParent(Transform t, string name, int level)
        {
            if (!t || level <= 0) return null;
            level --;
            var ret = t.parent;
            if (!ret)
            {
                return GetNodeFromRoot(name, t);
            }
            if (ret.name.Equals(name) && ret.IsValid())
            {
                return ret;
            }
            t = ret;
            ret = GetNodeFromChild(ret, name, t);
            if (ret) return ret;
            return level <= 0 ? null : GetNodeFromParent(t, name, level);
        }

        //递归获得指定名字父节点,并取得上面组件
        public static T GetNodeFromParent<T>(Transform t, string name)
        {
            var ret = GetNodeFromParent(t, name);
            return ret ? ret.GetComponent<T>() : default;
        }

        /// <summary>
        /// 从根节点获取特性类型所有组
        /// </summary>
        /// <param name="t">根节点</param>
        /// <param name="cpnts">返回的组件数组</param>
        /// <param name="filterFunc">过滤方法可为空</param>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns></returns>
        public static bool GetCpntsFromParent<T>(Transform t, out List<T> cpnts, Func<T, bool> filterFunc = null)
        {
            var ret = false;
            cpnts = new List<T>();

            if (filterFunc == null)
            {
                while (t)
                {
                    var c = t.GetComponent<T>();
                    if (c != null)
                    {
                        cpnts.Add(c);
                    }

                    t = t.parent;
                }
            }
            else
            {
                while (t)
                {
                    var c = t.GetComponent<T>();
                    if (c != null && filterFunc(c))
                    {
                        cpnts.Add(c);
                    }

                    t = t.parent;
                }
            }

            return ret;
        }

        /// <summary>
        /// 从根节点获取指定组件（PnlCtr获取PNodeBdl用）
        /// </summary>
        /// <param name="t"></param>
        /// <param name="filterFunc">过滤方法（可为空）</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetCpntFromParent<T>(Transform t, Func<T, bool> filterFunc = null)
        {
            if (filterFunc == null)
            {
                while (t)
                {
                    var c = t.GetComponent<T>();
                    if (c != null)
                    {
                        return c;
                    }

                    t = t.parent;
                }
            }
            else
            {
                while (t)
                {
                    var c = t.GetComponent<T>();
                    if (c != null && filterFunc(c))
                    {
                        return c;
                    }
                    t = t.parent;
                }
            }

            return default;
        }

        //递归获得指定名字父节点,并取得上面组件 level: 遍历层级
        public static T GetNodeFromParent<T>(Transform t, string name, int level)
        {
            var ret = GetNodeFromParent(t, name, level);
            return ret ? ret.GetComponent<T>() : default;
        }

        //先向下在向上遍历所有节点
        public static Transform GetNode(Transform t, string name)
        {
            var ret = GetNodeFromChild(t, name);
            return ret ? ret : GetNodeFromParent(t, name);
        }

        public static Transform GetNode(Transform t, string name, ref int levelChild, ref int levelParent)
        {
            var ret = GetNodeFromChild(t, name, levelChild);
            return ret ? ret : GetNodeFromParent(t, name, levelParent);
        }

        public static T GetNode<T>(Transform t, string name)
        {
            var ret = GetNodeFromChild<T>(t, name);
            return ret != null ? ret : GetNodeFromParent<T>(t, name);
        }

        public static T GetNode<T>(Transform t, string name, ref int levelChild, int levelParent)
        {
            var ret = GetNodeFromChild<T>(t, name, levelChild);
            return ret != null ? ret : GetNodeFromParent<T>(t, name, levelParent);
        }

        public static RaycastHit? ScreenPointToRay(Camera c, Ray ray, int maskLayer = 1, float maxDistance = float.PositiveInfinity)
        {
            if (Physics.Raycast(ray, out var hit, maxDistance, maskLayer))
            {
                return hit;
            }

            return null;
        }

        public static RaycastHit? ScreenPointToRayList(Camera c, Ray ray, int maskLayer = 1, float maxDistance = float.PositiveInfinity , int index = 0)
        {
            var hits = Physics.RaycastAll(ray , maxDistance , maskLayer);
            Array.Reverse(hits);
            if (hits.Length > index)
            {
                return hits[index];
            }
            return null;
        }
        public static Vector3? HitWorldPos(Camera c, Vector2 screenPos, int layer, float maxDis = 500)
        {
            // 获取当前视角下屏幕中心和地图层碰撞的位置
            var ray = c.ScreenPointToRay(screenPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDis, layer))
            {
                return hit.point;
            }

            //Debug.Log($"x: {xPos}, y: {yPos}");
            return null;
        }

        /// <summary>
        /// 删除某个Transform下所有的字物体
        /// </summary>
        /// <param name="t"></param>
        /// <param name="destroyLater">是否延迟移除</param>
        public static void RemoveAllChildren(Transform t, bool destroyLater = true)
        {
            if (!t) return;
            if (destroyLater)
            {
                foreach (Transform child in t) {
                    Object.Destroy(child.gameObject);
                }
            }
            else
            {
                for (var i = t.childCount - 1; i >= 0; i --) {
                    Object.DestroyImmediate(t.GetChild(i).gameObject);
                }
            }
        }

        public static void DestroyExt(Transform t, bool destroyLater = true)
        {
            if (!t || !t.IsValid()) return;
            if (destroyLater)
            {
                Object.Destroy(t.gameObject);
            }
            else
            {
                Object.DestroyImmediate(t.gameObject);
            }
        }

        public static void DestroyExt(GameObject o, bool destroyLater = true)
        {
            if (!o || !o.IsValid()) return;
            if (destroyLater)
            {
                Object.Destroy(o);
            }
            else
            {
                Object.DestroyImmediate(o);
            }
        }

        /// <summary>
        /// 通过节点名字获取子节点上指定名称的组件
        /// </summary>
        /// <param name="tr">根节点</param>
        /// <param name="nodeName">父节点名称</param>
        /// <param name="cpntName">组件全称 UnityEngine.UI.Image</param>
        /// <returns></returns>
        public static object GetCpntFromChild(Transform tr, string nodeName, string cpntName)
        {
            return GetNodeFromChild(tr, nodeName)?.GetComponent(cpntName);
        }

        /// <summary>
        /// 通过类型字符串获取组件,优先判断是否从指定名字节点上，再从自身找，然后从子节点找
        /// </summary>
        /// <param name="cpntName"></param>
        /// <param name="tr"></param>
        /// <param name="nodeName"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static object GetCpnt(Transform tr, string nodeName, string cpntName, Func<object, bool> func = null)
        {
            if (!tr)
            {
                return default;
            }
            if (!string.IsNullOrEmpty(nodeName))
            {
                if (func == null)
                {
                    return GetCpntFromChild(tr, nodeName, cpntName);
                }
                var c1 = GetCpntFromChild(tr, nodeName, cpntName);
                if (c1 != null && func(c1))
                {
                    return c1;
                }
            }

            if (tr.TryGetComponent(PReflectCtr.FindType(cpntName), out var c2))
            {
                if (func == null || func(c2))
                {
                    return c2;
                }
            }

            var c3 = tr.GetComponentsInChildren(PReflectCtr.FindType(cpntName), true);
            if (c3.Length > 0)
            {
                if (func == null)
                {
                    return c3[0];
                }
                return c3.FirstOrDefault(func);
            }

            return default;
        }

        /// <summary>
        /// 获取组件通用方法
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="nodeName"></param>
        /// <param name="func"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetCpnt<T>(Transform tr, string nodeName, Func<T, bool> func = null)
        {
            if (!tr)
            {
                return default;
            }
            if (!string.IsNullOrEmpty(nodeName))
            {
                if (func == null)
                {
                    return GetNodeFromChild<T>(tr, nodeName);
                }
                var c1 = GetNodeFromChild<T>(tr, nodeName);
                if (c1 != null && func(c1))
                {
                    return c1;
                }
            }

            if (tr.TryGetComponent<T>(out var c2))
            {
                if (func == null || func(c2))
                {
                    return c2;
                }
            }

            var c3 = tr.GetComponentsInChildren<T>( true);
            if (c3.Length > 0)
            {
                if (func == null)
                {
                    return c3[0];
                }
                return c3.FirstOrDefault(func);
            }

            return default;
        }

        /// <summary>
        /// 通过节点名字获取根节点上指定名称的组件
        /// </summary>
        /// <param name="tr">根节点</param>
        /// <param name="nodeName">根节点名称</param>
        /// <param name="cpntName">组件全称 UnityEngine.UI.Image</param>
        /// <param name="unityDomain">是否组件名称前添加UnityEngine</param>
        /// <returns></returns>
        public static object GetCpntFromParent(Transform tr, string nodeName, string cpntName, bool unityDomain = false)
        {
            return GetNodeFromParent(tr, nodeName)?.GetComponent(unityDomain ? $"UnityEngine.{cpntName}" : cpntName);
        }

        /// <summary>
        /// 整个场景中获取指定名称的组件
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="cpntName">组件全称 UnityEngine.UI.Image</param>
        /// <param name="unityDomain">是否组件名称前添加UnityEngine</param>
        /// <returns></returns>
        public static object GetCpntFromRoot(string nodeName, string cpntName, bool unityDomain = false)
        {
            return GetNodeFromRoot(nodeName)?.GetComponent(unityDomain ? $"UnityEngine.{cpntName}" : cpntName);
        }
    }
}