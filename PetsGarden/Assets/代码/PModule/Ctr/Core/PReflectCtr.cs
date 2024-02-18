/* create: pengyingh 17 09 22 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using PModule;
using static PModule.PSltCtr;

public static class PReflectCtr
{
    private static Dictionary<string, Type> m_Types = new Dictionary<string, Type>();
    //泛型方法绑定泛型类型
    public static MethodInfo MakeGenericMethodExt(this MethodInfo m, params string[] typeArgs)
    {
        try
        {
            return m.MakeGenericMethod(typeArgs.Select(e=>FindType(e)).ToArray());
        }
        catch (Exception)
        {
            return null;
        }
    }
    /// <summary>
    /// Search for a method by name and parameter types.
    /// Unlike GetMethod(), does 'loose' matching on generic
    /// parameter types, and searches base interfaces.
    /// </summary>
    /// <exception cref="AmbiguousMatchException"/>
    public static MethodInfo GetMethodExt(this Type thisType,
        string name,
        params Type[] parameterTypes)
    {
        return GetMethodExt(thisType,
            name,
            BindingFlags.Instance
            | BindingFlags.Static
            | BindingFlags.Public
            | BindingFlags.NonPublic
            | BindingFlags.FlattenHierarchy,
            parameterTypes);
    }

    /// <summary>
    /// Search for a method by name, parameter types, and binding flags.
    /// Unlike GetMethod(), does 'loose' matching on generic
    /// parameter types, and searches base interfaces.
    /// </summary>
    /// <exception cref="AmbiguousMatchException"/>
    public static MethodInfo GetMethodExt(this Type thisType,
        string name,
        BindingFlags bindingFlags,
        params Type[] parameterTypes)
    {
        MethodInfo matchingMethod = null;

        // Check all methods with the specified name, including in base classes
        GetMethodExt(ref matchingMethod, thisType, name, bindingFlags, parameterTypes);

        // If we're searching an interface, we have to manually search base interfaces
        if (matchingMethod == null && thisType.IsInterface)
        {
            foreach (Type interfaceType in thisType.GetInterfaces())
                GetMethodExt(ref matchingMethod,
                    interfaceType,
                    name,
                    bindingFlags,
                    parameterTypes);
        }

        return matchingMethod;
    }

    private static void GetMethodExt(ref MethodInfo matchingMethod,
        Type type,
        string name,
        BindingFlags bindingFlags,
        params Type[] parameterTypes)
    {
        // Check all methods with the specified name, including in base classes
        foreach (MethodInfo methodInfo in type.GetMember(name,
            MemberTypes.Method,
            bindingFlags))
        {
            // Check that the parameter counts and types match,
            // with 'loose' matching on generic parameters
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            if (parameterInfos.Length == parameterTypes.Length)
            {
                int i = 0;
                for (; i < parameterInfos.Length; ++i)
                {
                    if (!parameterInfos[i].ParameterType
                        .IsSimilarType(parameterTypes[i]))
                        break;
                }

                if (i == parameterInfos.Length)
                {
                    if (matchingMethod == null)
                        matchingMethod = methodInfo;
                    else
                        throw new AmbiguousMatchException(
                            "More than one matching method found!");
                }
            }
        }
    }

    /// <summary>
    /// Special type used to match any generic parameter type in GetMethodExt().
    /// </summary>
    public class T
    {
    }

    /// <summary>
    /// Determines if the two types are either identical, or are both generic
    /// parameters or generic types with generic parameters in the same
    ///  locations (generic parameters match any other generic paramter,
    /// but NOT concrete types).
    /// </summary>
    private static bool IsSimilarType(this Type thisType, Type type)
    {
        // Ignore any 'ref' types
        if (thisType.IsByRef)
            thisType = thisType.GetElementType();
        if (type.IsByRef)
            type = type.GetElementType();

        // Handle array types
        if (thisType.IsArray && type.IsArray)
            return thisType.GetElementType().IsSimilarType(type.GetElementType());

        // If the types are identical, or they're both generic parameters
        // or the special 'T' type, treat as a match
        if (thisType == type || ((thisType.IsGenericParameter || thisType == typeof(T))
                                 && (type.IsGenericParameter || type == typeof(T))))
            return true;

        // Handle any generic arguments
        if (!thisType.IsGenericType || !type.IsGenericType) return false;
        var thisArguments = thisType.GetGenericArguments();
        var arguments = type.GetGenericArguments();
        if (thisArguments.Length != arguments.Length) return false;
        for (var i = 0; i < thisArguments.Length; ++i)
        {
            if (!thisArguments[i].IsSimilarType(arguments[i]))
                return false;
        }

        return true;
    }

    public static MethodInfo GetReflectMethod(string classWithNamespace, string methodName, Type[] methodTypes = null,
        Type TMethod = null)
    {
        var type = FindType(classWithNamespace);
        var methodInfo = methodTypes == null ? type?.GetMethodExt(methodName) : type?.GetMethodExt(methodName, methodTypes);
        if (methodInfo == null) return null;
        if (TMethod != null)
        {
            methodInfo = methodInfo.MakeGenericMethod(TMethod);
        }

        return methodInfo;
    }

    public static object ReflectMethodInvoke(string classWithNamespace, string methodName, Type[] methodTypes,
        Type TMethod, object thisBundle = null, params object[] args)
    {
        return GetReflectMethod(classWithNamespace, methodName, methodTypes, TMethod)?.Invoke(thisBundle, args);
    }

    //动态生成泛型T   List<int>       ReflectGenericType(typeof(List<>), new Type[]{typeof(int)})
    public static Type GetReflectGenericType(Type geneTp, Type[] typeArgs)
    {
        return geneTp?.MakeGenericType(typeArgs);
    }

    public static void RemoveCacheType(string typeName)
    {
        if (m_Types.ContainsKey(typeName))
        {
            m_Types.Remove(typeName);
        }
    }

    public static void ClearCacheTypes()
    {
        m_Types.Clear();
    }

    public static Type FindType(string typeName, bool unityDomain = false)
    {
        typeName = unityDomain ? $"UnityEngine.{typeName}" : typeName;
        if (m_Types.TryGetValue(typeName, out var tp))
        {
            return tp;
        }

        tp = AppDomain.CurrentDomain.GetAssemblies()
            .Select(x => x.GetType(typeName, false))
            .FirstOrDefault(x => x != null);

        if (tp != null)
        {
            m_Types.Add(typeName, tp);
        }

        return tp;
    }
    ///  <summary>
    ///  移除delegate绑定的所有方法
    ///  </summary>
    ///  <param name="classObj">对象 </param>
    ///  <param name="eventName">事件名 </param>
    ///  <returns>委托列 </returns>
    public static bool RemoveAllEvents<T>(T classObj, string eventName)
    {
        var eventBdl = (Delegate) GetFieldInfo(classObj, eventName)?.GetValue(classObj);
        var invokeList = eventBdl?.GetInvocationList();
        if (invokeList == null) return false;
        foreach (var del in invokeList)
        {
            typeof(T).GetEvent(eventName).RemoveEventHandler(classObj, del);
        }
        return true;
    }

    public static FieldInfo GetFieldInfo<T>(T classObj, string fieldName)
    {
        return classObj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
    }

    public static PropertyInfo GetProperty<T>(T classObj, string fieldName)
    {
        return classObj.GetType().GetProperty(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
    }

}