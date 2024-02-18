/* create by pengyingh 210510 */
using System;
using UnityEngine;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace PModule
{
	public interface IPSlt
	{
		void OnRestartSlt();
		void OnInitSlt();
		void OnDestroySlt();
		void DestroySlt();
		uint SltId { get; set; }
	}

	public class PSltMono: MonoBehaviour, IPSlt
	{
		public uint SltId { get; set; }
		[HideInInspector]
		public bool m_IsDestroy;
		public static T CreateNew<T>() where T : MonoBehaviour, IPSlt
		{
			var singleton = new GameObject ();
			var t = singleton.AddComponent<T> ();
			singleton.name = $"singleton_{typeof(T)}";
			if (Application.isPlaying)
				DontDestroyOnLoad (singleton);
			return t;
		}

		public virtual void OnRestartSlt()
		{
			OnDestroySlt();
			DestroySlt();
			m_IsDestroy = false;
		}
		public virtual void OnInitSlt() { }
		public virtual void OnDestroySlt() { }

		public virtual void DestroySlt()
		{
			DestroyImmediate(gameObject);
		}
	}
	public class PSlt: IPSlt
	{
		public bool m_IsDestroy;
		public uint SltId { get; set; }
		public static T CreateNew<T>() where T : class, IPSlt
		{
			return Activator.CreateInstance<T>();
		}

		public virtual void OnRestartSlt()
		{
			OnDestroySlt();
			DestroySlt();
			m_IsDestroy = false;
		}
		public virtual void OnInitSlt() { }
		public virtual void OnDestroySlt() { }
		public virtual void DestroySlt() { }
	}
	public class PSltCtr
	{
		private static MethodInfo m_PSltMonoCreateNew;
		private static MethodInfo m_PSltCreateNew;
		private static readonly PAutoId m_AutoId = new PAutoId((uint)(MConfig.SLT_PREORITY.Count + 1));
		private static readonly ConcurrentDictionary<Type, IPSlt> m_SltDic = new ConcurrentDictionary<Type, IPSlt>();
		private static bool IsRunning { get; set; } = true;
		private static readonly Thread mainThread = Thread.CurrentThread;

		public static IPSlt Slt(string tp, bool emptyCreate = true)
		{
			var t = PReflectCtr.FindType(tp);
			return t == null ? default : Slt(t, emptyCreate);
		}

		private static uint GetAutoId(Type t)
		{
			var name = t.ToString();
			name = name.Substring(name.LastIndexOf("."[0]) + 1);
			var idx = MConfig.SLT_PREORITY.IndexOf(name);
			if (idx != -1)
			{
				return (uint)(idx + 1);
			}
			return m_AutoId.AutoId;
		}

		//lua泛型加载不方便，这里使用类型参数
		public static IPSlt Slt(Type t, bool emptyCreate = true)
		{
			if (m_SltDic.TryGetValue(t, out var ret))
			{
				return ret;
			}
			if (!emptyCreate) return null;
			if (Thread.CurrentThread == mainThread && Application.isPlaying &&
			    !IsRunning) return default; //socket控制器用单例会涉及多线程，Application.isPlaying只能在主线程判断，如果程序即将结束不再返回单例

			try
			{
				if (t.IsSubclassOf(typeof(PSltMono)))
				{
					if (m_PSltMonoCreateNew == null)
					{
						m_PSltMonoCreateNew = PReflectCtr.GetReflectMethod("PModule.PSltMono", "CreateNew");
					}
					var bdl = (IPSlt)m_PSltMonoCreateNew.MakeGenericMethod(t).Invoke(null, null);
					if (bdl == null) return default;
					m_SltDic.TryAdd(t, bdl);
					bdl.SltId = GetAutoId(t);
					bdl.OnInitSlt();
					return bdl;
				}
				if (t.IsSubclassOf(typeof(PSlt)))
				{
					if (m_PSltCreateNew == null)
					{
						m_PSltCreateNew = PReflectCtr.GetReflectMethod("PModule.PSlt", "CreateNew");
					}
					var bdl = (IPSlt)m_PSltCreateNew.MakeGenericMethod(t).Invoke(null, null);
					if (bdl == null) return default;
					m_SltDic.TryAdd(t, bdl);
					bdl.SltId = GetAutoId(t);
					bdl.OnInitSlt();
					return bdl;
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"PSltCtr.Slt<{t}>error:{e}");
			}
			return default;
		}
		public static T Slt<T> (bool emptyCreate = true) where T: IPSlt
		{
			if (m_SltDic.TryGetValue(typeof(T), out var ret))
			{
				return (T)ret;
			}
			if (Thread.CurrentThread == mainThread && Application.isPlaying &&
			    !IsRunning) return default; //socket控制器用单例会涉及多线程，Application.isPlaying只能在主线程判断，如果程序即将结束不再返回单例
			if (!emptyCreate) return default;
			var t = typeof(T);

			try
			{
				if (t.IsSubclassOf(typeof(PSltMono)))
				{
					if (m_PSltMonoCreateNew == null)
					{
						m_PSltMonoCreateNew = PReflectCtr.GetReflectMethod("PModule.PSltMono", "CreateNew");
					}
					var bdl = (T)m_PSltMonoCreateNew.MakeGenericMethod(typeof(T)).Invoke(null, null);
					if (bdl == null) return default;
					m_SltDic.TryAdd(t, bdl);
					bdl.SltId = GetAutoId(t);
					bdl.OnInitSlt();
					return bdl;
				}
				if (t.IsSubclassOf(typeof(PSlt)))
				{
					if (m_PSltCreateNew == null)
					{
						m_PSltCreateNew = PReflectCtr.GetReflectMethod("PModule.PSlt", "CreateNew");
					}
					var bdl = (T)m_PSltCreateNew.MakeGenericMethod(typeof(T)).Invoke(null, null);
					if (bdl == null) return default;
					m_SltDic.TryAdd(t, bdl);
					bdl.SltId = GetAutoId(t);
					bdl.OnInitSlt();
					return bdl;
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"PSltCtr.Slt<{typeof(T)}>error:{e}");
			}
			return default;
		}

		public static void RestartSltAll()
		{
			if (IsRunning)
			{
				IsRunning = false;
				var list = m_SltDic.ToList();
				list.Sort((x, y) => (int)(y.Value.SltId - x.Value.SltId));
				list.ForEach(e=>
				{
					e.Value.OnRestartSlt();
				});
				m_SltDic.Clear();
				IsRunning = true;
			}
		}

		public static void DestroySltAll()
		{
			if (IsRunning)
			{
				IsRunning = false;
				var list = m_SltDic.ToList();
				list.Sort((x, y) => (int)(y.Value.SltId - x.Value.SltId));

				list.ForEach(e=>
				{
					e.Value.OnDestroySlt();
					e.Value.DestroySlt();
				});
				m_SltDic.Clear();
			}
		}

		public static bool DestroySlt(string tp)
		{
			var t = PReflectCtr.FindType(tp);
			if (t == null)
			{
				return false;
			}

			var ret = m_SltDic.TryRemove(t, out var bdl);
			if (ret)
			{
				bdl.DestroySlt();
			}

			return ret;
		}

		public static bool DestroySlt<T>(string tp)
		{
			if (m_SltDic.TryRemove(typeof(T), out var bdl))
			{
				bdl.DestroySlt();
				return true;
			}

			return false;
		}
	}
}

