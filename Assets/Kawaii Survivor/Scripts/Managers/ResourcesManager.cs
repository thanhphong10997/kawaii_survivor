using UnityEngine;

public static class ResourcesManager
{
    // Đường dẫn đến folder bên trong Resources folder khi dùng hàm Resources.Load
    const string statIconsDataPath = "Data/Stat Icons";
    const string objectDatasPath = "Data/Objects";

    private static StatIcon[] statIcons;
    public static Sprite GetStatIcon(Stat stat)
    {
        if (statIcons == null)
        {
            // Load StatIconData script object
            StatIconDataSO data = Resources.Load<StatIconDataSO>(statIconsDataPath);
            statIcons = data.StatIcons;
        }

        foreach (StatIcon statIcon in statIcons)
        {
            if (stat == statIcon.stat) return statIcon.icon;
        }

        Debug.LogError("No icon found for stat: " + stat);

        return null;
    }

    private static ObjectDataSO[] objectDatas;
    public static ObjectDataSO[] Objects
    {
        get
        {
            if (objectDatas == null)
                objectDatas = Resources.LoadAll<ObjectDataSO>(objectDatasPath);

            return objectDatas;
        }
        private set { }
    }
}
