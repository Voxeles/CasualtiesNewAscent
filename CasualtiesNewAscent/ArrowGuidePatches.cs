using HarmonyLib;
using UnityEngine;

namespace CasualtiesNewAscent;

[HarmonyPatch(typeof(ArrowGuide), nameof(ArrowGuide.Update))]
public static class ArrowGuidePatches
{
    public static void Postfix(ArrowGuide __instance)
    {
        if (!Plugin.CheckIfNewLayer(WorldGeneration.world.biomeDepth))
            return;

        Vector3 origPos = __instance.origPos;
        origPos.y *= -1f;
        if (Plugin.CheckIfLastLayer(WorldGeneration.world.biomeDepth))
            origPos.y = 0f;
        __instance.transform.position = origPos + Vector3.up * Mathf.Sin(Time.time);
        __instance.spriteRenderer.flipY = true;
        __instance.spriteRenderer.enabled = true;
    }
}

public static class ArrowGuideTriple
{
    public static GameObject Child1 = null;
    public static GameObject Child2 = null;

    public static void MakeTriple()
    {
        if (Child1 != null)
            return;

        var world = WorldGeneration.world;
        if (!world || !Plugin.CheckIfNewLayer(world.biomeDepth))
            return;

        var guide = Object.FindAnyObjectByType<ArrowGuide>();

        var parent = guide.gameObject;
        if (parent.name != "ArrowGuide")
            return;

        Child1 = Object.Instantiate(parent);
        Child2 = Object.Instantiate(parent, parent.transform, false);
        // Set the parent after instantiating or else we'll get three copies, since Instantiate copies children too
        Child1.transform.SetParent(parent.transform);
        // Align to chunk boundaries
        Child1.GetComponent<ArrowGuide>().origPos.y = 192f;
        Child2.GetComponent<ArrowGuide>().origPos.y = -128f;
    }

    public static void DestroyTriple()
    {
        GameObject.Destroy(Child1);
        Child1 = null;
        GameObject.Destroy(Child2);
        Child2 = null;
    }
}
