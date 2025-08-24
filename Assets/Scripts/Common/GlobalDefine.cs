using UnityEngine;

public static class GlobalDefine
{
    public const string ITEM_ICONS_PATH = "Textures/ItemIcons/";
    public const string STAT_ICONS_PATH = "Textures/StatIcons/";

    public static string[] StatStrings = {
        "flat_move_speed",
        "flat_attack_range",
        "flat_attack_damage",
        "flat_attack_speed",
        "flat_crit_chance",
        "flat_crit_damage",
        "flat_health_point",
        "percent_move_speed",
        "percent_attack_range",
        "percent_attack_damage",
        "percent_attack_speed",
        "percent_health_point"
    };

    public const string STAT_MOVESPEED_FLAT = "flat_move_speed";
    public const string STAT_ATTACKRANGE_FLAT = "flat_attack_range";
    public const string STAT_ATTACKDAMAGE_FLAT = "flat_attack_damage";
    public const string STAT_ATTACKSPEED_FLAT = "flat_attack_speed";
    public const string STAT_CRITICALCHANCE_FLAT = "flat_critical_chance";
    public const string STAT_CRITICALDAMAGE_FLAT = "flat_critical_damage";
    public const string STAT_HEALTHPOINT_FLAT = "flat_health_point";

    public const string STAT_MOVESPEED_PERCENT = "percent_move_speed";
    public const string STAT_ATTACKRANGE_PERCENT = "percent_attack_range";
    public const string STAT_ATTACKDAMAGE_PERCENT = "percent_attack_damage";
    public const string STAT_ATTACKSPEED_PERCENT = "percent_attack_speed";
    public const string STAT_HEALTHPOINT_PERCENT = "percent_health_point";

}
