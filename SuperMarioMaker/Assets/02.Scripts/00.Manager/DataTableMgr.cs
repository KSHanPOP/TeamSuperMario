using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataTableMgr
{
    private static Dictionary<Type, DataTable> tables = new Dictionary<Type, DataTable>();
    private static bool isLoaded = false;

    public static void LoadAll()
    {
       
        isLoaded = true;
    }

    public static DataTable<T> Load<T>(DataTable<T> table, string filepath) where T : ICSVParsing, new()
    {
        if (tables.Count != 0 && tables.ContainsKey(typeof(T)))
        {
            tables.Remove(typeof(T));
        }
        tables.Add(typeof(T), new DataTable<T>(filepath));
        return tables[typeof(T)] as DataTable<T>;
    }

    public static DataTable<T> GetTable<T>() where T : ICSVParsing, new()
    {
        if (!isLoaded)
            LoadAll();

        return tables[typeof(T)] as DataTable<T>;
    }
    //public static Sprite LoadIcon(string id)
    //{
    //    IconData iconData = GetTable<IconData>().Get(id);
    //    if (iconData != null)
    //    {
    //        Sprite sprite = Resources.Load<Sprite>(iconData.iconName);
    //        return sprite;
    //    }
    //    return null;
    //}
}