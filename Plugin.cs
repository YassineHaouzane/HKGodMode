using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace HKPlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("hollow_knight.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log;
    public Harmony harmony { get; } = new(MyPluginInfo.PLUGIN_GUID);
    private void Awake()
    {
        Log = base.Logger;
        harmony.PatchAll();
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}



[HarmonyPatch(typeof(HeroController), nameof(HeroController.Pause))]
class GodModePatch 
{

    static void Postfix()
    {
        var hero = HeroController.instance;
        hero.playerData.isInvincible = !hero.playerData.isInvincible;
        Plugin.Log.LogInfo($"Invincibility status: {hero.playerData.isInvincible}");
        var tmpValue = hero.playerData.nailDamage;
        // No idea why this is not used for basic nail attacks
        hero.playerData.nailDamage = 500;
        Plugin.Log.LogInfo($"Player nail arts damage increased from {tmpValue} to : {hero.playerData.nailDamage}");
    }
}
