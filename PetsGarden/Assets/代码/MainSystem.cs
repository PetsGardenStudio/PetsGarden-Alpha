using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 主系统类，用于管理整个应用程序的核心功能和组件。
/// 这个类作为应用程序的中心点，负责协调和初始化各个子系统。
/// </summary>
public static class MainSystem
{
    /// <summary>
    /// 对BlockSystem类的唯一实例的引用。
    /// 这个公共静态字段允许应用程序的其他部分访问地块系统。
    /// </summary>
    public static BlockSystem blockSystem = BlockSystem.instance;

    /// <summary>
    /// 对CurrencySystem类的唯一实例的引用。
    /// 这个字段提供了全局访问点来处理与货币系统相关的操作和数据。
    /// </summary>
    public static CurrencySystem currencySystem = CurrencySystem.instance;

    /// <summary>
    /// 对GachaSystem类的唯一实例的引用。
    /// 这个字段允许应用程序的其他部分访问抽奖系统，用于管理和执行抽奖相关功能。
    /// </summary>
    public static GachaSystem gachaSystem = GachaSystem.instance;

    /// <summary>
    /// 对PetSystem类的唯一实例的引用。
    /// 请注意，PetWorkShopSystem，也就是宠物工坊的数据应该从这里获取
    /// 这个字段提供了全局访问点来处理与宠物系统相关的操作和数据。
    /// </summary>
    public static PetSystem petSystem = PetSystem.instance;
}


/// <summary>
/// 单例模式下的地块系统类。用于管理和控制地块相关的操作和数据。
/// 这个类使用单例设计模式，确保整个应用程序中只有一个地块系统实例。
/// 是线程安全的
/// </summary>
public class BlockSystem
{
    /// <summary>
    /// 地块系统的单例实例。
    /// </summary>
    private static BlockSystem onlyInstance = new BlockSystem();

    /// <summary>
    /// 私有的锁对象
    /// </summary>
    private static readonly object _lock = new object();

    /// <summary>
    /// 私有构造函数，防止外部通过new关键字创建类的实例。
    /// </summary>
    private BlockSystem() { }


    /// <summary>
    /// 获取地块系统类的唯一实例。这是一个公共的和静态的属性。
    /// 当外部代码需要访问地块系统实例时，将通过这个属性来获取。
    /// 是线程安全的
    /// </summary>
    public static BlockSystem instance
    {
        get
        {
            lock (_lock)
            {
                return onlyInstance;
            }
        }
    }

    /// <summary>
    /// 所有可以放置区块的位置，无论位置上有没有区块
    /// </summary>
    private HashSet<Vector2Int> allBlockSpots = new HashSet<Vector2Int>();

    /// <summary>
    /// 所有区域。即使是不足四格的区块也会被认定为一个区域，然后在后续再进行判定
    /// </summary>
    private List<BlockArea> areas = new List<BlockArea>();

    /// <summary>
    /// 在指定位置添加一个地块，并根据提供的区域标签将其分配到相应的区域中。
    /// 如果指定位置已存在地块或没有合法的邻接区域，则不会添加地块。
    /// </summary>
    /// <param name="pos">要添加地块的坐标。</param>
    /// <param name="block">要添加的地块对象。</param>
    /// <param name="tag">用于确定地块所属区域的区域标签。</param>
    /// <returns>
    /// 如果成功添加地块，则返回 true。
    /// 如果指定位置已有地块或无法找到合法邻接区域，则返回 false。
    /// </returns>
    public bool addBlock(Vector2Int pos, GameObject block, AreaTag tag)
    {
        if (allBlockSpots.Contains(pos))
        {
            List<BlockArea> legalNeighborArea = new List<BlockArea>();
            foreach (BlockArea area in areas)
            {
                if (!area.containsBlock(pos))
                {
                    return false;
                }
                if (area.isLegalNeighbor(pos, tag))
                {
                    legalNeighborArea.Add(area);
                }
            }
            if (legalNeighborArea.Count == 0)
            {
                BlockArea newArea = new BlockArea(tag);
                newArea.addBlock(pos, block);
                areas.Add(newArea);
                return true;
            }
            else if (legalNeighborArea.Count == 1)
            {
                legalNeighborArea[0].addBlock(pos, block);
                return true;
            }
            else
            {
                legalNeighborArea[0].addBlock(pos, block);
                for (int i = 0 + 1; i < legalNeighborArea.Count; i++)
                {
                    legalNeighborArea[0].mergeWith(legalNeighborArea[i]);
                    areas.Remove(legalNeighborArea[i]);
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 从区域中移除指定位置的地块。
    /// 此方法首先检查包含指定地块的区域，然后尝试移除地块。
    /// 如果移除地块会导致区域的不连通，它会创建新的 BlockArea 对象来代表分割后的区域。
    /// </summary>
    /// <param name="pos">要移除地块的位置。</param>
    /// <returns>
    /// 如果成功移除地块并保持区域的连通性，则返回 true。
    /// 如果移除地块导致区域分割，返回 false。
    /// </returns>
    public bool removeBlock(Vector2Int pos)
    {
        BlockArea areaForDeletion = null;
        List<BlockArea> replaceAreas = null;
        bool replaced = false;
        foreach (BlockArea area in areas)
        {
            if (area.containsBlock(pos))
            {
                if (!area.connectivityCheckWithoutBlock(pos, out List<BlockArea> newAreas))
                {
                    areaForDeletion = area;
                    replaceAreas = newAreas;
                    replaced = true;
                    break;
                }
                else
                {
                    area.removeBlock(pos);
                    return true;
                }
            }
        }
        areas.Remove(areaForDeletion);
        foreach (BlockArea area in replaceAreas)
        {
            areas.Add(area);
        }
        return replaced;
    }
}


/// <summary>
/// 单例模式下的代币系统类。用于管理和控制代币和付款有关的功能。
/// 这个类使用单例设计模式，确保整个应用程序中只有一个代币系统实例。
/// 是线程安全的
/// </summary>
public class CurrencySystem
{
    /// <summary>
    /// 代币系统的单例实例。
    /// </summary>
    private static CurrencySystem onlyInstance = new CurrencySystem();

    /// <summary>
    /// 私有的锁对象
    /// </summary>
    private static readonly object _lock = new object();

    /// <summary>
    /// 私有构造函数，防止外部通过new关键字创建类的实例。
    /// </summary>
    private CurrencySystem() { }

    /// <summary>
    /// 获取代币系统类的唯一实例。这是一个公共的和静态的属性。
    /// 当外部代码需要访问代币系统实例时，将通过这个属性来获取。
    /// 是线程安全的
    /// </summary>
    public static CurrencySystem instance
    {
        get
        {
            lock (_lock)
            {
                return onlyInstance;
            }
        }
    }

    /// <summary>
    /// 获取当前对象的金币数量。
    /// 这个属性只能在类内部被修改，以确保对硬币数量的更改是受控的。
    /// </summary>
    public int coins { get; private set; } = 0;

    /// <summary>
    /// 向账户中存入指定数量的金币。
    /// </summary>
    /// <param name="num">要存入的金币数量。</param>
    /// <returns>存入金币后的总金币数量。</returns>
    public int deposit(int num)
    {
        return coins += num;
    }

    /// <summary>
    /// 向账户中取出指定数量的金币。
    /// </summary>
    /// <param name="num">要取出的金币数量。</param>
    /// <returns>取出金币后的总金币数量。</returns>
    public int withdraw(int num)
    {
        return coins -= num;
    }
}

/// <summary>
/// 单例模式下的抽奖机系统类。用于管理和控制抽奖有关的功能。
/// 这个类使用单例设计模式，确保整个应用程序中只有一个抽奖系统实例。
/// 是线程安全的
/// </summary>
public class GachaSystem
{
    /// <summary>
    /// 抽奖系统的单例实例。
    /// </summary>
    private static GachaSystem onlyInstance = new GachaSystem();

    /// <summary>
    /// 私有的锁对象
    /// </summary>
    private static readonly object _lock = new object();

    /// <summary>
    /// 私有构造函数，防止外部通过new关键字创建类的实例。
    /// </summary>
    private GachaSystem() { }

    /// <summary>
    /// 获取抽奖系统类的唯一实例。这是一个公共的和静态的属性。
    /// 当外部代码需要访问抽奖系统实例时，将通过这个属性来获取。
    /// 是线程安全的
    /// </summary>
    public static GachaSystem instance
    {
        get
        {
            lock (_lock)
            {
                return onlyInstance;
            }
        }
    }

    // TODO: 添加抽奖系统所需的方法
}

/// <summary>
/// 单例模式下的宠物系统类。用于管理和控制宠物有关的功能。
/// 这个类使用单例设计模式，确保整个应用程序中只有一个宠物系统实例。
/// 是线程安全的
/// </summary>
public class PetSystem
{
    /// <summary>
    /// 宠物系统的单例实例。
    /// </summary>
    private static PetSystem onlyInstance = new PetSystem();

    /// <summary>
    /// 私有的锁对象
    /// </summary>
    private static readonly object _lock = new object();

    /// <summary>
    /// 宠物工坊系统的单例实例
    /// </summary>
    public static PetWorkShopSystem petWorkshopSystem { get; } = PetWorkShopSystem.instance;

    /// <summary>
    /// 私有构造函数，防止外部通过new关键字创建类的实例。
    /// </summary>
    private PetSystem() { }

    /// <summary>
    /// 获取宠物系统类的唯一实例。这是一个公共的和静态的属性。
    /// 当外部代码需要访问宠物系统实例时，将通过这个属性来获取。
    /// 是线程安全的
    /// </summary>
    public static PetSystem instance
    {
        get
        {
            lock (_lock)
            {
                return onlyInstance;
            }
        }
    }

    // TODO: 添加宠物系统所需的方法
}

/// <summary>
/// 单例模式下的宠物工坊系统类。用于管理和控制抽奖有关的功能。
/// 这个类使用单例设计模式，确保整个应用程序中只有一个宠物工坊系统实例。
/// 是线程安全的
/// </summary>
public class PetWorkShopSystem
{
    /// <summary>
    /// 宠物工坊系统的单例实例。
    /// </summary>
    private static PetWorkShopSystem onlyInstance = new PetWorkShopSystem();

    /// <summary>
    /// 私有的锁对象
    /// </summary>
    private static readonly object _lock = new object();

    /// <summary>
    /// 私有构造函数，防止外部通过new关键字创建类的实例。
    /// </summary>
    private PetWorkShopSystem() { }

    /// <summary>
    /// 获取宠物工坊系统类的唯一实例。这是一个公共的和静态的属性。
    /// 当外部代码需要访问宠物工坊系统实例时，将通过这个属性来获取。
    /// 是线程安全的
    /// </summary>
    public static PetWorkShopSystem instance
    {
        get
        {
            lock (_lock)
            {
                return onlyInstance;
            }
        }
    }

    // TODO: 添加宠物工坊系统所需的方法
}

/// <summary>
/// 区域对象，包含该区域内所有地块的信息
/// </summary>
public class BlockArea
{

    /// <summary>
    /// 空构造函数，未来序列化/反序列化用得到
    /// </summary>
    public BlockArea(AreaTag tag)
    {
        this.areaTag = tag;
    }

    /// <summary>
    /// 区域内所有地块的位置和游戏对象引用字典
    /// </summary>
    private Dictionary<Vector2Int, GameObject> blocks = new Dictionary<Vector2Int, GameObject>();

    /// <summary>
    /// 当前区域的属性标签
    /// </summary>
    public AreaTag areaTag { get; } = new AreaTag();

    /// <summary>
    /// 检查指定位置是否存在地块。
    /// </summary>
    /// <param name="pos">需要检查的二维位置。</param>
    /// <returns>
    /// 如果在指定位置存在地块，则返回 true。
    /// 如果不存在地块，则返回 false。
    /// </returns>
    public bool containsBlock(Vector2Int pos)
    {
        return blocks.ContainsKey(pos);
    }

    /// <summary>
    /// 在指定位置添加一个地块。
    /// 如果该位置已经存在地块，则不会执行添加操作。
    /// </summary>
    /// <param name="pos">要添加地块的二维位置。</param>
    /// <param name="block">要添加的地块对象。</param>
    /// <returns>
    /// 如果地块被成功添加，则返回 true。
    /// 如果该位置已存在地块，则返回 false。
    /// </returns>
    public bool addBlock(Vector2Int pos, GameObject block)
    {
        if (!containsBlock(pos))
        {
            blocks.Add(pos, block);
        }
        return !containsBlock(pos);
    }

    /// <summary>
    /// 判断指定位置是否可以作为当前区域的合法邻接位置。
    /// 要成为合法邻接位置，目标位置不应该已有地块，且目标区域标签应与当前区域标签相同。
    /// 此外，目标位置应与当前区域中至少一个地块的上下左右相邻。
    /// </summary>
    /// <param name="pos">需要判断的坐标。</param>
    /// <param name="targetTag">目标位置的区域标签。</param>
    /// <returns>
    /// 如果指定位置是合法的邻接位置，则返回 true。
    /// 如果不是合法邻接位置，则返回 false。
    /// </returns>
    public bool isLegalNeighbor(Vector2Int pos, AreaTag targetTag)
    {
        if (containsBlock(pos) || targetTag != areaTag) return false;
        foreach (Vector2Int blockPos in blocks.Keys)
        {
            List<Vector2Int> surrondings = BlockArea.getSurrondings(blockPos);
            if (surrondings.Contains(pos)) return true;
        }
        return false;
    }

    /// <summary>
    /// 将当前区域与另一个区域合并。
    /// 通过合并两个区域的地块字典来实现。
    /// 合并操作是通过将两个字典的内容合并到一起，以确保所有地块都包含在合并后的区域中。
    /// </summary>
    /// <param name="other">要与之合并的另一个 BlockArea 对象。</param>
    public void mergeWith(BlockArea other)
    {
        blocks = blocks.Union(other.blocks).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    /// <summary>
    /// 检查移除指定位置的地块后，区域是否保持连通性。
    /// 此方法通过广度优先搜索（BFS）算法来判断连通性。
    /// 如果移除地块导致区域不再连通，它会创建新的 BlockArea 对象表示分割后的区域。
    /// </summary>
    /// <param name="pos">预计移除地块的位置。</param>
    /// <param name="newAreas">
    /// 输出参数。如果移除地块导致区域分割，这里将包含所有分割后的新 BlockArea 对象。
    /// 如果区域保持连通，则此参数将被设置为 null。
    /// </param>
    /// <returns>
    /// 如果移除地块后区域仍保持连通，则返回 true。
    /// 如果区域因移除地块而不再连通，则返回 false。
    /// </returns>
    public bool connectivityCheckWithoutBlock(Vector2Int pos, out List<BlockArea> newAreas)
    {
        List<Vector2Int> blockSurrondings = BlockArea.getSurrondings(pos);
        HashSet<BlockArea> foundBlockAreas = new HashSet<BlockArea>(); 
        foreach (Vector2Int neighborPos in blockSurrondings)
        {
            bool alreadyVisited = false;
            foreach (BlockArea blockArea in foundBlockAreas)
            {
                if (blockArea.containsBlock(neighborPos))
                {
                    alreadyVisited = true;
                    break;
                }
            }
            if (this.blocks.ContainsKey(neighborPos) && !alreadyVisited)
            {
                if (!this.bfs(pos, neighborPos, out BlockArea newArea))
                {
                    foundBlockAreas.Add(newArea);
                }
                else
                {
                    newAreas = null; // 不更改地块连通性的情况下不需要读取newAreas，直接null掉
                    return true;
                }
            }
        }

        newAreas = foundBlockAreas.ToList();
        return false;
    }

    /// <summary>
    /// 获取指定位置周围的邻接位置。
    /// 此方法返回一个包含上、下、左、右四个方向邻接位置的列表。
    /// </summary>
    /// <param name="pos">需要获取邻接位置的二维位置。</param>
    /// <returns>包含指定位置上下左右四个邻接位置的列表。</returns>
    private static List<Vector2Int> getSurrondings(Vector2Int pos)
    {
        return new List<Vector2Int>() {
            pos + new Vector2Int(0, 1),
            pos + new Vector2Int(0, -1),
            pos + new Vector2Int(-1, 0),
            pos + new Vector2Int(1, 0)
        };
    }

    /// <summary>
    /// 使用广度优先搜索（BFS）算法遍历地块，从给定的起始位置开始，忽略指定位置。
    /// 此方法用于检查在不考虑指定忽略位置的情况下，地块是否仍然连通。
    /// 如果所有地块（除忽略位置外）仍然连通，则方法返回 true。
    /// 如果不连通，则创建一个新的 BlockArea 对象，包含所有访问过的地块，并返回 false。
    /// </summary>
    /// <param name="ignorePos">在搜索过程中需要忽略的地块位置。</param>
    /// <param name="beginPos">广度优先搜索的起始位置。</param>
    /// <param name="newArea">
    /// 输出参数。如果地块不连通，则这里将包含一个新的 BlockArea 对象，其中包含所有访问过的地块。
    /// 如果地块连通，则此参数将被设置为 null。
    /// </param>
    /// <returns>
    /// 如果所有地块（除忽略位置外）仍然连通，则返回 true。
    /// 如果不连通，则返回 false。
    /// </returns>
    private bool bfs(Vector2Int ignorePos, Vector2Int beginPos, out BlockArea newArea) { 
        Queue<Vector2Int> bfsQueue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        bfsQueue.Enqueue(beginPos);
        visited.Add(beginPos);

        while (bfsQueue.Count != 0)
        {
            Vector2Int searchingNode = bfsQueue.Dequeue();
            List<Vector2Int> directNeighbors = BlockArea.getSurrondings(searchingNode);
            foreach (Vector2Int directNeighbor in directNeighbors)
            {
                if (!visited.Contains(directNeighbor) && this.blocks.ContainsKey(directNeighbor) && directNeighbor != ignorePos)
                {
                    visited.Add(directNeighbor);
                    bfsQueue.Enqueue(directNeighbor);
                }
            }
        }

        if (visited.Count == this.blocks.Keys.Count - 1)
        {
            newArea = null; // 不更改地块连通性的情况下不需要读取newArea，直接null掉
            return true;
        }
        else
        {
            newArea = new BlockArea(this.areaTag);
            foreach (Vector2Int pos in visited)
            {
                newArea.addBlock(pos, this.blocks[pos]);
            }
            return false;
        }
    }

    /// <summary>
    /// 从区域中移除位于指定位置的地块。
    /// </summary>
    /// <param name="pos">需要移除地块的位置。</param>
    public void removeBlock(Vector2Int pos) {
        this.blocks.Remove(pos);
    }
}


/// <summary>
/// 区域属性标签，请注意这里是byte
/// 使用byte的原因是考虑到不可能超过255个区域标签，且未来需要频繁进行服务器同步
/// </summary>
public enum AreaTag : byte
{
    /// <summary>
    /// 火属性地块
    /// </summary>
    FIRE = 0,
    /// <summary>
    /// 草属性地块
    /// </summary>
    GRASS = 1,
    /// <summary>
    /// 水属性地块
    /// </summary>
    WATER = 2,
}