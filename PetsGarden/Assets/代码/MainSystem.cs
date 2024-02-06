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
    /// 使用这种方式，可以确保整个应用程序共享一个地块系统实例，
    /// 并且方便地从任何地方访问地块系统的功能。
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
    /// 所有区域
    /// </summary>
    private List<BlockArea> areas = new List<BlockArea>();

    /// <summary>
    /// 在指定位置添加一个方块，并根据提供的区域标签将其分配到相应的区域中。
    /// 如果指定位置已存在方块或没有合法的邻接区域，则不会添加方块。
    /// </summary>
    /// <param name="pos">要添加方块的坐标。</param>
    /// <param name="block">要添加的方块对象。</param>
    /// <param name="tag">用于确定方块所属区域的区域标签。</param>
    /// <returns>
    /// 如果成功添加方块，则返回 true。
    /// 如果指定位置已有方块或无法找到合法邻接区域，则返回 false。
    /// </returns>
    public bool addBlock(Vector2Int pos, GameObject block, AreaTag tag)
    {
        if (allBlockSpots.Contains(pos))
        {
            List<BlockArea> legalNeighborArea = new List<BlockArea>();
            foreach (BlockArea area in areas)
            {
                if (!area.blockExists(pos))
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

    // TODO: 添加地块系统所需的方法
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

    // TODO: 添加代币系统所需的方法
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
    /// 检查指定位置是否存在方块。
    /// </summary>
    /// <param name="pos">需要检查的二维位置。</param>
    /// <returns>
    /// 如果在指定位置存在方块，则返回 true。
    /// 如果不存在方块，则返回 false。
    /// </returns>
    public bool blockExists(Vector2Int pos)
    {
        return blocks.ContainsKey(pos);
    }

    /// <summary>
    /// 在指定位置添加一个方块。
    /// 如果该位置已经存在方块，则不会执行添加操作。
    /// </summary>
    /// <param name="pos">要添加方块的二维位置。</param>
    /// <param name="block">要添加的方块对象。</param>
    /// <returns>
    /// 如果方块被成功添加，则返回 true。
    /// 如果该位置已存在方块，则返回 false。
    /// </returns>
    public bool addBlock(Vector2Int pos, GameObject block)
    {
        if (!blockExists(pos))
        {
            blocks.Add(pos, block);
        }
        return !blockExists(pos);
    }

    /// <summary>
    /// 判断指定位置是否可以作为当前区域的合法邻接位置。
    /// 要成为合法邻接位置，目标位置不应该已有方块，且目标区域标签应与当前区域标签相同。
    /// 此外，目标位置应与当前区域中至少一个方块的上下左右相邻。
    /// </summary>
    /// <param name="pos">需要判断的坐标。</param>
    /// <param name="targetTag">目标位置的区域标签。</param>
    /// <returns>
    /// 如果指定位置是合法的邻接位置，则返回 true。
    /// 如果不是合法邻接位置，则返回 false。
    /// </returns>
    public bool isLegalNeighbor(Vector2Int pos, AreaTag targetTag)
    {
        if (blockExists(pos) || targetTag != areaTag) return false;
        foreach (Vector2Int blockPos in blocks.Keys)
        {
            List<Vector2Int> surrondings = new List<Vector2Int>() {
                blockPos + new Vector2Int(0, 1),
                blockPos + new Vector2Int(0, -1),
                blockPos + new Vector2Int(-1, 0),
                blockPos + new Vector2Int(1, 0)
            };
            if (surrondings.Contains(pos)) return true;
        }
        return false;
    }

    /// <summary>
    /// 将当前区域与另一个区域合并。
    /// 通过合并两个区域的方块字典来实现。
    /// 合并操作是通过将两个字典的内容合并到一起，以确保所有方块都包含在合并后的区域中。
    /// </summary>
    /// <param name="other">要与之合并的另一个 BlockArea 对象。</param>
    public void mergeWith(BlockArea other)
    {
        blocks = blocks.Union(other.blocks).ToDictionary(pair => pair.Key, pair => pair.Value);
    }
    // TODO：添加为区域增删和提取block的方法
    // TODO：添加设置areaTag的方法
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