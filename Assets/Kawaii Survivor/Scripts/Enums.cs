public enum GameState
{
    MENU,
    WEAPONSELECTION,
    GAME,
    GAMEOVER,
    STAGECOMPLETE,
    WAVETRANSITION,
    SHOP
}

public enum Stat
{
    Attack,
    AttackSpeed,
    CriticalChance,
    CriticalPercent,
    MoveSpeed,
    MaxHealth,
    Range,
    HealthRecoverySpeed,
    Armor,
    Luck,
    Dodge,
    Lifesteal
}

public static class Enums
{
    public static string FormatStatName(Stat stat)
    {
        string formated = "";
        string unformatedString = stat.ToString();
        // Giữ nguyên kí tự đầu tiên của chuỗi
        formated += unformatedString[0];

        // Loop từ index 1 để tránh trường hợp kí tự đầu tiên của từ đầu tiên sẽ bị thêm khoảng cách 
        for (int i = 1; i < unformatedString.Length; i++)
        {
            if (char.IsUpper(unformatedString[i])) formated += " ";
            formated += unformatedString[i];
        }

        return formated;
    }
}