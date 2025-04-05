using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLibrary : Singleton<DataLibrary>
{
    public Getter<SoundType> Sounds { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Sounds = new Getter<SoundType>(Resources.LoadAll<SoundType>("Sounds"));
    }
}

public class Getter<T>
{
    readonly Dictionary<string, T> nameToItem;
    public readonly T[] UnsortedArray;

    public readonly int Length;

    public Getter(T[] ts)
    {
        nameToItem = new Dictionary<string, T>();

        UnsortedArray = ts;
        Length = ts.Length;

        Initialize(ts);
    }

    protected virtual void Initialize(T[] ts)
    {
        for (int i = 0; i < ts.Length; i++)
        {
            if (ts[i] == null) { Debug.Log("Null Data!"); continue; }
            nameToItem.Add((ts[i] as Object).name, ts[i]);
        }
    }

    public T GetRandomExcluding(List<T> excluding)
    {
        List<T> list = new List<T>(UnsortedArray);

        for (int i = 0; i < excluding.Count; i++)
            list.Remove(excluding[i]);

        return list[Random.Range(0, list.Count)];
    }

    public T GetRandom() => UnsortedArray[Random.Range(0, UnsortedArray.Length)];

    public T this[string n]
    {
        get
        {
            nameToItem.TryGetValue(n, out T j);
            if (j == null) { Debug.LogError($"Couldn't get {n}"); }
            return j;
        }
    }

    public T this[int n] => UnsortedArray[n];
}

// public class ItemGetter<T> : Getter<T>
// {
//     Dictionary<int, T> idToItem;

//     public ItemGetter(T[] ts) : base(ts)
//     {
//         idToItem = new Dictionary<int, T>();

//         for (int i = 0; i < Length; i++)
//         {
//             if (ts[i] == null) { Debug.Log("Null Data!"); continue; }

//             Item item = ts[i] as Item;

//             idToItem.Add(item.Id, ts[i]);
//         }
//     }

//     public T GetItemWithId(int id) => idToItem[id];
// }

// public class SpecificItemGetter<T> : ItemGetter<T>
// {
//     T[] itemsSortedByTypeId;

//     public SpecificItemGetter(T[] ts) : base(ts)
//     {
//         itemsSortedByTypeId = new T[Length];

//         for (int i = 0; i < Length; i++)
//         {
//             if (ts[i] == null) { Debug.Log("Null Data!"); continue; }

//             Item item = ts[i] as Item;

//             //Debug.Log($"{item.Name} id: {item.TypeId}");

//             itemsSortedByTypeId[item.TypeId] = ts[i];
//         }
//     }

//     public T GetItemWithTypeId(int typeId) => itemsSortedByTypeId[typeId];
// }