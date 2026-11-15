using HarmonyLib;

namespace CasualtiesNewAscent;

[HarmonyPatch(typeof(Locale), nameof(Locale.LoadLanguage))]
public static class NewLocaleEntries
{
    public static void Postfix()
    {
        if (Locale.currentLang == null)
            return;
        Locale.currentLang.other.TryAdd("layertitle14", "<color=red>Hell IV</color>");
        Locale.currentLang.other.TryAdd("layertitle15", "<color=red>Hell III</color>");
        Locale.currentLang.other.TryAdd("layertitle16", "<color=red>Hell II</color>");
        Locale.currentLang.other.TryAdd("layertitle17", "<color=#FFD700>Hell I</color>");
    }
}