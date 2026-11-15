using HarmonyLib;

namespace CasualtiesOnOurWayToParadise;

[HarmonyPatch(typeof(Locale), nameof(Locale.LoadLanguage))]
public static class NewLocaleEntries
{
    public static void Postfix()
    {
        if (Locale.currentLang == null)
            return;
        Locale.currentLang.other.TryAdd("layertitle14", "<color=#FFD700>ASCENT III</color>\nParadise is a mile away");
        Locale.currentLang.other.TryAdd("layertitle15", "<color=#FFD700>ASCENT II</color>\nParadise is a walk away");
        Locale.currentLang.other.TryAdd("layertitle16", "<color=#FFD700>ASCENT I</color>\nParadise is a step away");
    }
}