using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace CasualtiesNewAscent;

public static class NewWorldgen
{
    private static double tileTimer = 0;
    private static readonly List<CrystalBehaviour> crystals = [];

    public static void CreateBackground3(WorldGeneration inst)
    {
        var chunks = inst.chunks;
        int width = chunks.GetUpperBound(0);
        int height = chunks.GetUpperBound(1);
        for (int x = chunks.GetLowerBound(0); x <= width; ++x)
        {
            for (int y = chunks.GetLowerBound(1); y <= height; ++y)
            {
                if (y < (int) (height * (1f / 3f)))
                {
                    inst.CreateBackground("ventsBackground", chunks[x, y]);
                    inst.CreateBackground("ventsBackgroundGlow", chunks[x, y], 1, true);
                }
                else if (y < (int)(height * (2f / 3f)))
                {
                    inst.CreateBackground("rockBackground", chunks[x, y]);
                }
                else
                {
                    inst.CreateBackground("fungalBackground", chunks[x, y]);
                }
            }
        }
    }

    public static void CreateBackground2(WorldGeneration inst)
    {
        var chunks = inst.chunks;
        int width = chunks.GetUpperBound(0);
        int height = chunks.GetUpperBound(1);
        for (int x = chunks.GetLowerBound(0); x <= width; ++x)
        {
            for (int y = chunks.GetLowerBound(1); y <= height; ++y)
            {
                if (y < (int) (height * (1f / 3f)))
                {
                    inst.CreateBackground("iceBackground", chunks[x, y]);
                }
                else if (y < (int)(height * (2f / 3f)))
                {
                    inst.CreateBackground("grassBackground", chunks[x, y]);
                }
                else
                {
                    inst.CreateBackground("wastelandBackground", chunks[x, y]);
                }
            }
        }
    }

    public static void CreateBackground1(WorldGeneration inst)
    {
        var chunks = inst.chunks;
        int width = chunks.GetUpperBound(0);
        int height = chunks.GetUpperBound(1);
        for (int x = chunks.GetLowerBound(0); x <= width; ++x)
        {
            for (int y = chunks.GetLowerBound(1); y <= height; ++y)
            {
                if (y < (int) (height * (1f / 3f)))
                {
                    inst.CreateBackground("sandBackground", chunks[x, y]);
                }
                else if (y < (int)(height * (2f / 3f)))
                {
                    inst.CreateBackground("soilBackground", chunks[x, y]);
                }
                else
                {
                    inst.CreateBackground("rockBackground", chunks[x, y]);
                }
            }
        }

        for (var i = 0; i < (double)(inst.chunkWidth * inst.chunkHeight) * 0.25f; ++i)
        {
            var vector2 = (Vector2)new Vector3(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(0, inst.halfHeight));
            ((GameObject)Object.Instantiate(Resources.Load("Special/wallholes"), vector2, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.value * 360f)), inst.GetClosestChunk(inst.WorldToBlockPos(vector2)).transform)).GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
        }
    }

    public static void CreateBackground0(WorldGeneration inst)
    {
        var chunks = inst.chunks;
        int width = chunks.GetUpperBound(0);
        int height = chunks.GetUpperBound(1) / 2;
        for (int x = chunks.GetLowerBound(0); x <= width; ++x)
        {
            for (int y = chunks.GetLowerBound(1); y <= height; ++y)
                inst.CreateBackground("crimsonback", chunks[x, y]);
        }

        for (var i = 0; i < (double)(inst.chunkWidth * inst.chunkHeight) * 0.4f; ++i)
        {
            var vector2 = (Vector2)new Vector3(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(0, inst.halfHeight / 2f));
            ((GameObject)Object.Instantiate(Resources.Load("Special/wallholes"), vector2, Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.value * 360f)), inst.GetClosestChunk(inst.WorldToBlockPos(vector2)).transform)).GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
        }

    }

    public static IEnumerator GenerateBiome8(WorldGeneration inst)
    {
        var marbleMap8 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        marbleMap8.SetNoiseType(FastNoiseLite.NoiseType.Value);
        marbleMap8.SetFrequency(0.06f);
        marbleMap8.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        marbleMap8.SetDomainWarpAmp(31f);
        var toxicNoise8 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        toxicNoise8.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
        toxicNoise8.SetFrequency(0.03f);
        toxicNoise8.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        toxicNoise8.SetDomainWarpAmp(31f);
        var caveNoise8 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        caveNoise8.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        caveNoise8.SetFrequency(0.03f);
        float num27 = inst.chunkWidth * (inst.chunkHeight / 3f) * Random.Range(0.8f, 0.9f);
        for (var index28 = 0; index28 < (double)num27; ++index28)
        {
            var vector2Int = new Vector2Int(Random.Range(0, (int)inst.width), Random.Range(0, (int) (inst.height * (1f / 3f))));
            int num28 = Random.Range(8, WorldGeneration.CHUNKSIZE);
            for (var index29 = 0; index29 < num28; ++index29)
            {
                if (vector2Int.x < inst.width && vector2Int.y + index29 < inst.height)
                {
                    inst.worldBlocks[vector2Int.x, index29 + vector2Int.y] = 21;
                    if (index29 % 5 == 0)
                    {
                        if (vector2Int.x + 1 < inst.width)
                            inst.worldBlocks[vector2Int.x + 1, index29 + vector2Int.y] = 21;
                        if (vector2Int.x - 1 >= 0)
                            inst.worldBlocks[vector2Int.x - 1, index29 + vector2Int.y] = 21;
                    }
                }
            }
        }

        for (var i1 = 0; i1 < inst.width; ++i1)
        {
            bool flag = Math.Abs(i1 - inst.halfWidth) > 3L;
            for (var y = 0; y < (int) (inst.height * (1f / 3f)); ++y)
            {
                float noise1 = marbleMap8.GetNoise(i1, y);
                var noise2 = (double)toxicNoise8.GetNoise(i1, y);
                float noise3 = caveNoise8.GetNoise(i1, y * 2.5f);
                if (noise2 < -0.30000001192092896)
                    inst.worldBlocks[i1, y] = 30;
                if (flag && noise1 > 0.6499999761581421)
                    inst.worldBlocks[i1, y] = 36;
                if (inst.worldBlocks[i1, y] > 0 && noise3 > 0.75)
                    inst.worldBlocks[i1, y] = 37;
            }

            if (Time.unscaledTimeAsDouble - tileTimer > 1.0)
            {
                yield return null;
                tileTimer = Time.unscaledTimeAsDouble;
            }
        }
    }

    public static IEnumerator GenerateBiome7(WorldGeneration inst)
    {
        var marbleMap7 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        marbleMap7.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        marbleMap7.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
        marbleMap7.SetFrequency(0.007f);
        marbleMap7.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.Hybrid);
        for (var i1 = 0; i1 < inst.width; ++i1)
        {
            for (var y = (int)(inst.height * (1f / 3f)); y < (int)(inst.height * (2f / 3f)); ++y)
            {
                float noise = marbleMap7.GetNoise(i1, y);
                inst.worldBlocks[i1, y] = noise <= 0.75 ? noise <= 0.5 ? noise <= 0.25 ? noise <= 0.0 ? noise <= -0.25 ? (ushort)30 : (ushort)19 : (ushort)18 : (ushort)17 : (ushort)13 : (ushort)1;
            }

            if (Time.unscaledTimeAsDouble - tileTimer > 1.0)
            {
                yield return null;
                tileTimer = Time.unscaledTimeAsDouble;
            }
        }

        crystals.Clear();
        string[] possibleCrystals =
        [
            "crystalmini",
            "crystalsmall",
            "crystalmedium",
            "crystalsmall",
            "crystalmedium",
            "crystalbig",
            "crystalsmall",
            "crystalmedium",
            "crystalbig",
            "crystalmedium",
            "crystalbig",
            "crystalmini",
            "crystalsmall",
            "crystalmedium",
            "crystalbig",
            "crystalmassive"
        ];
        for (var i1 = 0; i1 < 30; ++i1)
        {
            var b1 = new Vector2(Random.value * inst.width, inst.height * (2f / 3f));
            Vector2 vector2_1 = Vector2.down + Random.insideUnitCircle * 0.9f;
            Vector2 normalized1 = vector2_1.normalized;
            float f = Random.Range(4f, 18f);
            inst.SimpleBlockCircle(new Vector2Int((int)b1.x, (int)b1.y), Mathf.RoundToInt(f), 0);
            while ((double)b1.y > inst.height * (1f / 3f))
            {
                Vector2 a = b1;
                normalized1 += Random.insideUnitCircle * 0.75f;
                if (normalized1.y > 0.0)
                    normalized1.y = 0.0f;
                normalized1.Normalize();
                f = Mathf.Clamp(f + Random.Range(-2f, 2f), 4f, 18f);
                b1 += normalized1 * Random.Range(20f, 80f);
                Vector2 vector2_2 = b1 - a;
                vector2_1 = new Vector2(-vector2_2.y, vector2_2.x);
                Vector2 normalized2 = vector2_1.normalized;
                var vector2Int1 = new Vector2Int((int)a.x, (int)a.y);
                ref Vector2Int local1 = ref vector2Int1;
                var vector2Int2 = new Vector2Int((int)b1.x, (int)b1.y);
                ref Vector2Int local2 = ref vector2Int2;
                int thickness = Mathf.RoundToInt(f * 2f);
                inst.DrawEmptyLine(in local1, in local2, thickness);
                inst.SimpleBlockCircle(new Vector2Int((int)b1.x, (int)b1.y), Mathf.RoundToInt(f), 0);
                Vector2 vector2_3 = b1 + normalized2 * (f - 1f);
                Vector2 vector2_4 = b1 + normalized2 * (f + 3f);
                var vector2_5 = new Vector2(vector2_3.x, vector2_3.y);
                vector2Int1 = new Vector2Int((int)vector2_3.x, (int)vector2_3.y);
                ref Vector2Int local3 = ref vector2Int1;
                vector2Int2 = new Vector2Int((int)vector2_4.x, (int)vector2_4.y);
                ref Vector2Int local4 = ref vector2Int2;
                inst.DrawLine(in local3, in local4, 3, 10);
                Vector2 vector2_6 = b1 - normalized2 * (f - 1f);
                Vector2 vector2_7 = b1 - normalized2 * (f + 3f);
                var vector2_8 = new Vector2(vector2_6.x, vector2_6.y);
                vector2Int1 = new Vector2Int((int)vector2_6.x, (int)vector2_6.y);
                ref Vector2Int local5 = ref vector2Int1;
                vector2Int2 = new Vector2Int((int)vector2_7.x, (int)vector2_7.y);
                ref Vector2Int local6 = ref vector2Int2;
                inst.DrawLine(in local5, in local6, 3, 10);
                if (Random.value < 0.5)
                {
                    vector2Int1 = new Vector2Int((int)vector2_5.x, (int)vector2_5.y);
                    ref Vector2Int local7 = ref vector2Int1;
                    vector2Int2 = new Vector2Int((int)vector2_8.x, (int)vector2_8.y);
                    ref Vector2Int local8 = ref vector2Int2;
                    inst.DrawLine(in local7, in local8, 2, 11);
                }

                var num25 = (int)(Vector2.Distance(a, b1) * 0.15000000596046448);
                for (var index = 0; index < num25; ++index)
                {
                    Vector2 worldPos1 = inst.BlockToWorldPos(new Vector2Int((int)a.x, (int)a.y));
                    Vector2 worldPos2 = inst.BlockToWorldPos(new Vector2Int((int)b1.x, (int)b1.y));
                    float num26 = Random.value < 0.5 ? -1f : 1f;
                    Vector2 b2 = worldPos2;
                    var t = (double)Random.value;
                    Vector2 pos = Vector2.Lerp(worldPos1, b2, (float)t) + normalized2 * (f * num26);
                    crystals.Add(Utils.Create(possibleCrystals.PickRandom(), pos, Vector2.SignedAngle(Vector2.up, normalized2 * num26) + 180f).GetComponent<CrystalBehaviour>());
                }
            }

            if (Time.unscaledTimeAsDouble - tileTimer > 1.0)
            {
                yield return null;
                tileTimer = Time.unscaledTimeAsDouble;
            }
        }
    }

    public static IEnumerator GenerateBiome6(WorldGeneration inst)
    {
        var caveNoise6 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        caveNoise6.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        caveNoise6.SetFrequency(0.03f);
        caveNoise6.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        caveNoise6.SetDomainWarpAmp(900f);
        caveNoise6.SetCellularJitter(2f);
        caveNoise6.SetFractalType(FastNoiseLite.FractalType.PingPong);
        var toxicNoise6 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        toxicNoise6.SetFrequency(0.03f);
        toxicNoise6.SetFractalType(FastNoiseLite.FractalType.PingPong);
        var marbleMap6 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        marbleMap6.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        marbleMap6.SetFrequency(0.045f);
        marbleMap6.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
        marbleMap6.SetCellularReturnType(FastNoiseLite.CellularReturnType.Distance);
        marbleMap6.SetCellularJitter(1f);
        marbleMap6.SetFractalType(FastNoiseLite.FractalType.Ridged);
        marbleMap6.SetFractalLacunarity(1.5f);

        for (var i1 = 0; i1 < inst.width; ++i1)
        {
            for (var y = (int)(inst.height * (2f / 3f)); y < (long)inst.height; ++y)
            {
                if (caveNoise6.GetNoise(i1, y) < -0.4000000059604645)
                {
                    ushort num20 = 30;
                    if (y / (double)inst.height > 0.8999999761581421 && Random.value < (double)(y / (float)inst.height).Remap(0.9f, 1f, 0.0f, 1f))
                        num20 = 27;
                    inst.worldBlocks[i1, y] = toxicNoise6.GetNoise(i1, y) > 0.3499999940395355 ? (ushort)31 /*0x1F*/ : num20;
                }

                if (marbleMap6.GetNoise(i1, y) > 0.10000000149011612)
                    inst.worldBlocks[i1, y] = (ushort)Random.Range(3, 5);
                if (inst.worldBlocks[i1, y] > 0 && marbleMap6.GetNoise(i1, y) < -0.800000011920929)
                    inst.worldBlocks[i1, y] = 15;
            }

            if (Time.unscaledTimeAsDouble - tileTimer > 1.0)
            {
                yield return null;
                tileTimer = Time.unscaledTimeAsDouble;
            }
        }
    }

    public static void FinishBiome8(WorldGeneration inst)
    {
        PlaceLiquids(inst, 0, 128f, 6, 50);
    }

    public static void FinishBiome7(WorldGeneration inst)
    {
        foreach (CrystalBehaviour crystalBehaviour in crystals)
            crystalBehaviour.GenerationFinished();
        crystals.Clear();
    }

    public static void FinishBiome6(WorldGeneration inst)
    {
        PlaceLiquids(inst, 2, 25f, 1, 600);
        PlaceLiquids(inst, 2, 20f, 2, 150);
    }

    public static void EntitiesBiome8(WorldGeneration inst)
    {
        DistributeEntities(inst, 0, (GameObject)Resources.Load("animalcorpse"), 0.15f * inst.totalLootRarity, 0.2f * inst.totalLootRarity, checkFunc: (WorldGeneration.PlaceCheckDelegate)(pos => inst.GetBlock(pos) > (ushort)0 && inst.GetBlock(pos + Vector2Int.right) > (ushort)0 && inst.GetBlock(pos - Vector2Int.right) > (ushort)0));
        DistributeEntities(inst, 0, (GameObject)Resources.Load("sawblade"), 0.35f * inst.totalTrapRarity, 0.45f * inst.totalTrapRarity, randomFlip: true, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("beartrap"), 0.06f * inst.totalTrapRarity, 0.07f * inst.totalTrapRarity, 1f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("piston"), 0.2f * inst.totalTrapRarity, 0.33f * inst.totalTrapRarity, 3.125f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("bloodsuckerden"), 0.5f * inst.totalTrapRarity, 0.55f * inst.totalTrapRarity, 8f, spawnYOffsetDeviation: 5f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("pistonright"), 0.2f * inst.totalTrapRarity, 0.33f * inst.totalTrapRarity, 2f, isTrap: true, dir: Vector2.left);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("pistonleft"), 0.2f * inst.totalTrapRarity, 0.33f * inst.totalTrapRarity, 2f, isTrap: true, dir: Vector2.right);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("laserdrone"), 0.225f * inst.totalTrapRarity, 0.25f * inst.totalTrapRarity, 2f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("stoneplant"), 0.2f, 0.3f, 1.9f, 10f, 0.1f, randomFlip: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("lavalamp"), 2f, 2.25f, 1f, 10f, randomFlip: true, checkFunc: (WorldGeneration.PlaceCheckDelegate)(pos => inst.GetBlock(pos) == (ushort)36));
        DistributeEntities(inst, 0, (GameObject)Resources.Load("coldpuff"), 0.8f, 1f, 1f, 10f, randomFlip: true, checkFunc: (WorldGeneration.PlaceCheckDelegate)(pos => inst.GetBlock(pos) != (ushort)36));
        DistributeEntities(inst, 0, (GameObject)Resources.Load("spicevine"), 1.2f, 1.3f, 3f, 15f, 1f, randomFlip: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("spicevineceiling"), 1.2f, 1.3f, 3f, 15f, 1f, randomFlip: true, dir: Vector2.up);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("stalagmitedark"), 10f, 15f, 2.8f, spawnYOffsetDeviation: 0.15f, randomFlip: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("lavageyser"), 0.3f * inst.totalTrapRarity, 0.4f * inst.totalTrapRarity, 0.6f);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("corpse"), 0.15f * inst.totalLootRarity, 0.2f * inst.totalLootRarity, checkFunc: (WorldGeneration.PlaceCheckDelegate)(pos => inst.GetBlock(pos) > (ushort)0 && inst.GetBlock(pos + Vector2Int.right) > (ushort)0 && inst.GetBlock(pos - Vector2Int.right) > (ushort)0));
        DistributeEntities(inst, 0, (GameObject)Resources.Load("fan"), 0.1f * inst.totalTrapRarity, 0.15f * inst.totalTrapRarity, 0.625f, isTrap: true);
    }

    public static void EntitiesBiome7(WorldGeneration inst)
    {
        DistributeEntities(inst, 1, (GameObject) UnityEngine.Resources.Load("animalcorpse"), 0.5f * inst.totalLootRarity, 0.25f * inst.totalLootRarity, checkFunc: (WorldGeneration.PlaceCheckDelegate) (pos => inst.GetBlock(pos) > (ushort) 0 && inst.GetBlock(pos + Vector2Int.right) > (ushort) 0 && inst.GetBlock(pos - Vector2Int.right) > (ushort) 0));
        DistributeEntities(inst, 1, (GameObject) UnityEngine.Resources.Load("geyser"), 0.5f * inst.totalTrapRarity, 0.6f * inst.totalTrapRarity, 0.6f);
        DistributeEntities(inst, 1, (GameObject) UnityEngine.Resources.Load("corpse"), 0.25f * inst.totalLootRarity, 0.32f * inst.totalLootRarity, checkFunc: (WorldGeneration.PlaceCheckDelegate) (pos => inst.GetBlock(pos) > (ushort) 0 && inst.GetBlock(pos + Vector2Int.right) > (ushort) 0 && inst.GetBlock(pos - Vector2Int.right) > (ushort) 0));
        DistributeEntities(inst, 1, (GameObject) UnityEngine.Resources.Load("corecrystal"), 0.18f * inst.totalLootRarity, 0.2f * inst.totalLootRarity, 1.25f, randomFlip: true);
    }

    public static void EntitiesBiome6(WorldGeneration inst)
    {
        DistributeEntities(inst, 2, (GameObject)Resources.Load("shadecrawler"), 0.45f * inst.totalTrapRarity, 0.5f * inst.totalTrapRarity, 2f, 180f, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("overgrowntick"), 0.366f * inst.totalTrapRarity, 0.4f * inst.totalTrapRarity, 2f, 180f, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("brownshroom"), 6f, 7f, 0.9f, 10f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("mushroomtree"), 3f, 3.5f, 5f, 2f, 3f, randomFlip: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("glowshroom"), 2.2f, 2.3f, 1f, 10f, 0.5f, randomFlip: true, dir: Vector2.up);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("mushroomdropper"), 2.2f * inst.totalTrapRarity, 2.3f * inst.totalTrapRarity, 1f, 10f, randomFlip: true, dir: Vector2.up);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("heatfern"), 0.5f, 0.6f, 1.8f, 10f, 0.2f, randomFlip: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("bouncecap"), 1.1f, 1.2f, 1.5f, 10f);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("fungalibungali"), 1f, 1.1f, 0.5f, randomFlip: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("mushroomplatform"), 0.8f, 1f, 1f, spawnYOffsetDeviation: 1f, dir: Vector2.left);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("animalcorpse"), 1f * inst.totalLootRarity, 0.9f * inst.totalLootRarity, checkFunc: pos => inst.GetBlock(pos) > 0 && inst.GetBlock(pos + Vector2Int.right) > 0 && inst.GetBlock(pos - Vector2Int.right) > 0);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("mushroomplatform"), 0.8f, 1f, 1f, spawnYOffsetDeviation: 1f, dir: Vector2.right, forceFlip: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("sidestabber"), 0.5f * inst.totalTrapRarity, 0.6f * inst.totalTrapRarity, randomRotation: 20f, isTrap: true, dir: Vector2.left);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("sidestabberflip"), 0.5f * inst.totalTrapRarity, 0.6f * inst.totalTrapRarity, randomRotation: 20f, isTrap: true, dir: Vector2.right);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("leadbush"), 0.25f, 0.3f, 0.6f, 6f, 0.1f, randomFlip: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("beartrap"), 0.05f * inst.totalTrapRarity, 0.07f * inst.totalTrapRarity, 1f, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("jumppad"), 0.25f * inst.totalTrapRarity, 0.35f * inst.totalTrapRarity, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("caveticks"), 0.11f * inst.totalTrapRarity, 0.13f * inst.totalTrapRarity, 4.8f, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("hydreed"), 0.6f, 0.7f, 2.6f, 6f, 0.4f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("rag"), (float)(0.11999999731779099 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.5)), (float)(0.20000000298023224 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.5)), 1f);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("bananaplant"), 1.9f * inst.totalTrapRarity, 2f * inst.totalTrapRarity, 0.4f, 15f, 0.1f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("sawblade"), 0.5f * inst.totalTrapRarity, 0.6f * inst.totalTrapRarity, randomFlip: true, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("grabbershroom"), 0.5f * inst.totalTrapRarity, 0.6f * inst.totalTrapRarity, 1.8f, 10f, 0.2f, randomFlip: true, isTrap: true);
              float num17 = (float) (inst.chunkWidth * inst.chunkHeight / 3f) * UnityEngine.Random.Range(3f, 4f);
      for (int index = 0; (double) index < (double) num17; ++index)
      {
        Vector2 vector2_1 = new Vector2(UnityEngine.Random.Range((float) -inst.halfWidth, (float) inst.halfWidth), UnityEngine.Random.Range((float) inst.halfHeight * (1f/3f), (float) inst.halfHeight));
        if (!(bool) (UnityEngine.Object) Physics2D.OverlapPoint(vector2_1, LayerMask.GetMask("Ground")))
        {
          RaycastHit2D raycastHit2D5 = Physics2D.Raycast(vector2_1, Vector2.up, (float) (WorldGeneration.CHUNKSIZE * 4), LayerMask.GetMask("Ground"));
          RaycastHit2D raycastHit2D6 = Physics2D.Raycast(vector2_1, Vector2.down, (float) (WorldGeneration.CHUNKSIZE * 4), LayerMask.GetMask("Ground"));
          Vector2 vector2_2 = Vector2.Lerp(raycastHit2D5.point, raycastHit2D6.point, UnityEngine.Random.Range(0.2f, 0.9f));
          if ((bool) raycastHit2D5 && (bool) raycastHit2D6 && (double) raycastHit2D6.point.y < (double) inst.halfHeight - 5.0)
          {
            Color color = Color.Lerp(Color.gray, Color.white, UnityEngine.Random.value);
            GameObject gameObject5 = (GameObject) UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Special/mushroomropeend"), (Vector3) (vector2_2 + Vector2.down * 0.5f), Quaternion.identity);
            GameObject gameObject6 = (GameObject) UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Special/mushroomrope"), (Vector3) ((raycastHit2D5.point + vector2_2) * 0.5f), Quaternion.identity);
            gameObject6.GetComponent<SpriteRenderer>().size = new Vector2(2.5f, Mathf.Abs(raycastHit2D5.point.y - vector2_2.y));
            gameObject6.GetComponent<SpriteRenderer>().color = color;
            gameObject5.GetComponent<SpriteRenderer>().color = color;
            gameObject6.GetComponent<SpriteRenderer>().flipX = (double) UnityEngine.Random.value > 0.5;
            gameObject5.GetComponent<SpriteRenderer>().flipX = (double) UnityEngine.Random.value > 0.5;
            float x = UnityEngine.Random.Range(0.5f, 1f);
            gameObject5.transform.localScale = new Vector3(x, 1f);
            gameObject6.transform.localScale = new Vector3(x, 1f);
            float num18 = (float) ((1.0 - (double) x) * 16.0);
            Climbable component = gameObject6.GetComponent<Climbable>();
            component.points.Add(vector2_2);
            component.points.Add(raycastHit2D5.point);
            component.downwardsVelocity = num18;
          }
        }
      }
    }

    public static IEnumerator GenerateBiome5(WorldGeneration inst)
    {
        var biomeMap = new FastNoiseLite(Random.Range(0, int.MaxValue));
        biomeMap.SetFrequency(0.05f);
        biomeMap.SetFractalType(FastNoiseLite.FractalType.FBm);
        var marbleMap = new FastNoiseLite(Random.Range(0, int.MaxValue));
        marbleMap.SetFrequency(0.076f);
        marbleMap.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        marbleMap.SetFractalType(FastNoiseLite.FractalType.PingPong);
        var toxicNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        toxicNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        toxicNoise.SetFrequency(0.04f);
        toxicNoise.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
        toxicNoise.SetCellularReturnType(FastNoiseLite.CellularReturnType.Distance);
        toxicNoise.SetCellularJitter(1f);
        toxicNoise.SetFractalType(FastNoiseLite.FractalType.Ridged);
        toxicNoise.SetFractalLacunarity(1.5f);
        var caveNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        caveNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        caveNoise.SetFrequency(0.08f);
        caveNoise.SetDomainWarpType(FastNoiseLite.DomainWarpType.BasicGrid);
        caveNoise.SetDomainWarpAmp(51.5f);
        float num1 = inst.chunkWidth * (inst.chunkHeight / 3f) * Random.Range(0.35f, 0.5f);
        for (var i1 = 0; i1 < inst.width; ++i1)
        {
            for (var y = 0; y < (long)(inst.height * (1f / 3f)); ++y)
            {
                Color pixelBilinear = inst.iceMap.GetPixelBilinear(i1 / (float)inst.width, y / (float)inst.height);
                biomeMap.SetFrequency(inst.iceGenCurve.Evaluate(y / (float)inst.height));
                bool flag = pixelBilinear.b > pixelBilinear.g + 0.20000000298023224;
                if (pixelBilinear.r * (double)biomeMap.GetNoise(i1, y) > (flag ? -0.5 : inst.iceMinGenCurve.Evaluate(y / (float)inst.height)))
                    inst.worldBlocks[i1, y] = marbleMap.GetNoise(i1, y) > -0.25 ? (ushort)27 : (ushort)26;
                if (toxicNoise.GetNoise(i1, y) > 0.10000000149011612 && y < inst.height - 64U)
                    inst.worldBlocks[i1, y] = (ushort)Random.Range(3, 5);
                if (inst.worldBlocks[i1, y] > 0 && toxicNoise.GetNoise(i1, y) < -0.699999988079071)
                    inst.worldBlocks[i1, y] = 28;
                if (inst.worldBlocks[i1, y] == 26 && caveNoise.GetNoise(i1, y) > 0.44999998807907104)
                    inst.worldBlocks[i1, y] = 29;
                if (pixelBilinear.r > 0.5 && pixelBilinear.b < 0.5)
                    inst.worldBlocks[i1, y] = 14;
                if (pixelBilinear == Color.green)
                    inst.worldBlocks[i1, y] = 11;
            }

            if (Time.unscaledTimeAsDouble - tileTimer > 1.0)
            {
                yield return null;
                tileTimer = Time.unscaledTimeAsDouble;
            }
        }
    }

    public static IEnumerator GenerateBiome4(WorldGeneration inst)
    {
        var marbleMap = new FastNoiseLite(Random.Range(0, int.MaxValue));
        marbleMap.SetNoiseType(FastNoiseLite.NoiseType.Value);
        marbleMap.SetFractalType(FastNoiseLite.FractalType.Ridged);
        marbleMap.SetFractalOctaves(3);
        marbleMap.SetFractalLacunarity(2.29f);
        marbleMap.SetFractalGain(4f);
        marbleMap.SetFractalWeightedStrength(1.2f);
        marbleMap.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        marbleMap.SetDomainWarpAmp(41f);
        var biomeMap = new FastNoiseLite(Random.Range(0, int.MaxValue));
        biomeMap.SetFrequency(0.02f);
        biomeMap.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        biomeMap.SetDomainWarpAmp(25f);
        for (var i1 = 0; i1 < inst.width; ++i1)
        {
            for (var y = (int)(inst.height * (1f / 3f)); y < (long)(inst.height * (2f / 3f)); ++y)
            {
                marbleMap.SetFrequency((float)(0.01889999955892563 - y / (double)inst.height * (1.0 / 500.0)));
                float num18 = marbleMap.GetNoise(i1, y) + Random.Range(-0.1f, 0.1f);
                if (num18 > 0.15000000596046448 && num18 < 0.25)
                {
                    if (y / (double)inst.height < 0.15000000596046448)
                    {
                        var num19 = (float)((0.15000000596046448 - y / (double)inst.height) * 15.0);
                        inst.worldBlocks[i1, y] = Random.value < (double)num19 ? (ushort)26 : (ushort)23;
                    }
                    else
                        inst.worldBlocks[i1, y] = 23;
                }
                else if (num18 >= 0.25 && num18 < 0.44999998807907104)
                    inst.worldBlocks[i1, y] = 16 /*0x10*/;
                else if (num18 >= 0.44999998807907104 && num18 < 0.6600000262260437)
                    inst.worldBlocks[i1, y] = 15;
                else if (num18 >= 0.6600000262260437)
                    inst.worldBlocks[i1, y] = 19;

                if (biomeMap.GetNoise(i1, y) < -0.7350000143051147)
                    inst.worldBlocks[i1, y] = 0;
            }

            if (Time.unscaledTimeAsDouble - tileTimer > 1.0)
            {
                yield return null;
                tileTimer = Time.unscaledTimeAsDouble;
            }
        }
    }

    public static IEnumerator GenerateBiome3(WorldGeneration inst)
    {
        var biomeMap1 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        biomeMap1.SetNoiseType(FastNoiseLite.NoiseType.Value);
        biomeMap1.SetFrequency(0.086f);
        biomeMap1.SetFractalType(FastNoiseLite.FractalType.FBm);
        biomeMap1.SetFractalOctaves(3);
        biomeMap1.SetFractalGain(0.49f);
        biomeMap1.SetFractalWeightedStrength(2.34f);
        biomeMap1.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        biomeMap1.SetDomainWarpAmp(22f);
        var frequencyMap = new FastNoiseLite(Random.Range(0, int.MaxValue));
        frequencyMap.SetFrequency(3f / 500f);
        var dirtPerlinNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        dirtPerlinNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        dirtPerlinNoise.SetFrequency(0.02f);
        dirtPerlinNoise.SetFractalType(FastNoiseLite.FractalType.Ridged);
        dirtPerlinNoise.SetFractalGain(0.65f);
        var caveNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        caveNoise.SetFrequency(0.005f);
        caveNoise.SetFractalType(FastNoiseLite.FractalType.PingPong);
        caveNoise.SetFractalGain(0.35f);
        caveNoise.SetDomainWarpType(FastNoiseLite.DomainWarpType.BasicGrid);
        caveNoise.SetDomainWarpAmp(40f);
        var toxicNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        toxicNoise.SetFrequency(0.012f);
        toxicNoise.SetFractalType(FastNoiseLite.FractalType.PingPong);
        toxicNoise.SetFractalGain(0.3f);
        toxicNoise.SetDomainWarpType(FastNoiseLite.DomainWarpType.BasicGrid);
        toxicNoise.SetDomainWarpAmp(50f);
        var biomeMap2 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        biomeMap2.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        biomeMap2.SetFrequency(0.05f);
        biomeMap2.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
        biomeMap2.SetCellularReturnType(FastNoiseLite.CellularReturnType.Distance);
        biomeMap2.SetCellularJitter(1f);
        biomeMap2.SetFractalType(FastNoiseLite.FractalType.Ridged);
        biomeMap2.SetFractalLacunarity(1.5f);
        var marbleMap = new FastNoiseLite(Random.Range(0, int.MaxValue));
        marbleMap.SetFrequency(0.035f);
        marbleMap.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        marbleMap.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        marbleMap.SetDomainWarpAmp(100f);
        var i1 = 0;
        var minMarble = 1f;
        for (var i2 = 0; i2 < inst.width; ++i2)
        {
            for (var y = (int)(inst.height * (2f / 3f)); y < inst.height; ++y)
            {
                float noise = biomeMap1.GetNoise(i2, y);
                var num11 = (float)(frequencyMap.GetNoise(i2, y) * 0.25 + 0.10000000149011612);
                if (marbleMap.GetNoise(i2, y) <= (double)minMarble)
                {
                    inst.worldBlocks[i2, y] = noise <= (double)num11 || dirtPerlinNoise.GetNoise(i2, y) >= -0.4000000059604645 ? (ushort)0 : noise < num11 + 0.10000000149011612 ? (ushort)12 : (ushort)13;
                    if (caveNoise.GetNoise(i2, y) > 0.6499999761581421)
                        inst.worldBlocks[i2, y] = 17;
                    if (noise > 0.75)
                        inst.worldBlocks[i2, y] = 15;
                    if (biomeMap2.GetNoise(i2, y) > 0.10000000149011612)
                        inst.worldBlocks[i2, y] = (ushort)Random.Range(3, 5);
                    if (inst.worldBlocks[i2, y] > 0 && Random.value < 0.10000000149011612)
                        inst.worldBlocks[i2, y] = (ushort)(15 + Random.Range(0, 2));
                }
                else
                {
                    ++i1;
                    inst.worldBlocks[i2, y] = noise > (double)num11 ? dirtPerlinNoise.GetNoise(i2, y) < -0.10000000149011612 ? (ushort)18 : (ushort)19 : (ushort)0;
                    if (i1 > 100)
                    {
                        i1 = 0;
                        Object.Instantiate(Resources.Load("Special/marbleBackground"), inst.BlockToWorldPos(new Vector2Int(i2, y)), Quaternion.Euler(0.0f, 0.0f, Random.value * 360f));
                    }
                }

                if (toxicNoise.GetNoise(i2, y) < -0.800000011920929 && Random.value > 0.5 && Math.Abs(i2 - inst.halfWidth) > 3L)
                    inst.worldBlocks[i2, y] = 22;
                if (inst.worldBlocks[i2, y] > 0 && Random.value > (y + inst.halfHeight) / (double)inst.height)
                    inst.worldBlocks[i2, y] = 23;
            }

            if (Time.unscaledTimeAsDouble - tileTimer > 1.0)
            {
                yield return null;
                tileTimer = Time.unscaledTimeAsDouble;
            }
        }

        float num12 = inst.chunkWidth * (inst.chunkHeight / 3f) * Random.Range(0.25f, 0.3f);
        for (var index13 = 0; index13 < (double)num12; ++index13)
        {
            var vector2Int = new Vector2Int(Random.Range(0, (int)inst.width), Random.Range((int)(inst.height * (2f / 3f)), (int)inst.height));
            int num13 = Random.Range(6, 48 /*0x30*/);
            for (var index14 = 0; index14 < num13; ++index14)
            {
                for (var index15 = 0; index15 < 2; ++index15)
                {
                    if (vector2Int.x + index14 < inst.width && vector2Int.y + index15 < inst.height)
                        inst.worldBlocks[index14 + vector2Int.x, index15 + vector2Int.y] = 11;
                }
            }
        }

        float num14 = inst.chunkWidth * (inst.chunkHeight / 3f) * Random.Range(0.24f, 0.3f);
        for (var index16 = 0; index16 < (double)num14; ++index16)
        {
            var vector2Int = new Vector2Int(Random.Range(0, (int)inst.width), Random.Range((int)(inst.height * (2f / 3f)), (int)inst.height));
            int num15 = Random.Range(6, 48 /*0x30*/);
            for (var index17 = 0; index17 < num15; ++index17)
            {
                for (var index18 = 0; index18 < 2; ++index18)
                {
                    if (vector2Int.x + index18 < inst.width && vector2Int.y + index17 < inst.height)
                        inst.worldBlocks[index18 + vector2Int.x, index17 + vector2Int.y] = 11;
                }
            }
        }

        float num16 = inst.chunkWidth * (inst.chunkHeight / 3f) * Random.Range(0.5f, 0.6f);
        for (var index19 = 0; index19 < (double)num16; ++index19)
        {
            var vector2Int = new Vector2Int(Random.Range(0, (int)inst.width), Random.Range((int)(inst.height * (2f / 3f)), (int)inst.height));
            int num17 = Random.Range(4, 16 /*0x10*/);
            for (var index20 = 0; index20 < num17; ++index20)
            {
                for (var index21 = 0; index21 < num17; ++index21)
                {
                    if (vector2Int.x + index20 < inst.width && vector2Int.y + index21 < inst.height)
                        inst.worldBlocks[index20 + vector2Int.x, index21 + vector2Int.y] = 20;
                }
            }
        }
    }

    public static void FinishBiome5(WorldGeneration inst)
    {
        PlaceLiquids(inst, 0, 128f, (byte) 1, 32);
    }

    public static void FinishBiome4(WorldGeneration inst)
    {
        PlaceLiquids(inst, 1, 30f, 1, 128);
        PlaceLiquids(inst, 1, 10f, 2, 50);
    }

    public static void FinishBiome3(WorldGeneration inst)
    {
        PlaceLiquids(inst, 2, 50f,  1, 26);
        PlaceLiquids(inst, 2, 15f, 3, 128);
    }

    public static void EntitiesBiome5(WorldGeneration inst)
    {
        DistributeEntities(inst, 0, (GameObject)Resources.Load("glowplant"), 0.4f, 0.5f, 1.25f, 10f, 0.25f, randomFlip: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("heatfern"), 0.5f, 0.6f, 1.8f, 10f, 0.2f, randomFlip: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("frigiant"), 0.5f, 0.6f, 1.8f, 10f, 0.2f, randomFlip: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("leadbush"), 0.25f, 0.3f, 0.6f, 6f, 0.1f, randomFlip: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("spaceheater"), 0.4f * inst.totalLootRarity, 0.45f * inst.totalLootRarity, 1.5f);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("shadecrawler"), 0.2f * inst.totalTrapRarity, 0.3f * inst.totalTrapRarity, 2f, 180f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("stoneplant"), 0.1f, 0.2f, 1.9f, 10f, 0.1f, randomFlip: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("icestalagmite"), 4f * inst.totalTrapRarity, 5f * inst.totalTrapRarity, 2.8f, 10f, randomFlip: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("medcrate"), 0.18f * inst.totalLootRarity, 0.2f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("containercrate"), 0.05f * inst.totalLootRarity, 0.07f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("corpse"), 1.1f * inst.totalLootRarity, 1.2f * inst.totalLootRarity, checkFunc: pos => inst.GetBlock(pos) > 0 && inst.GetBlock(pos + Vector2Int.right) > 0 && inst.GetBlock(pos - Vector2Int.right) > 0);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("animalcorpse"), 1.4f * inst.totalLootRarity, 1.5f * inst.totalLootRarity, checkFunc: pos => inst.GetBlock(pos) > 0 && inst.GetBlock(pos + Vector2Int.right) > 0 && inst.GetBlock(pos - Vector2Int.right) > 0);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("droppings"), 3.7f, 4f);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("beartrap"), 0.09f * inst.totalTrapRarity, 0.12f * inst.totalTrapRarity, 1f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("jumppad"), 0.2f * inst.totalTrapRarity, 0.2f * inst.totalTrapRarity, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("spikestabber"), 0.4f * inst.totalTrapRarity, 0.5f * inst.totalTrapRarity, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("gunmine"), 0.3f * inst.totalTrapRarity, 0.35f * inst.totalTrapRarity, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("geyser"), 0.5f * inst.totalTrapRarity, 0.6f * inst.totalTrapRarity, 0.6f, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos));
        DistributeEntities(inst, 0, (GameObject)Resources.Load("rag"), (float)(0.11999999731779099 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.5)), (float)(0.20000000298023224 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.5)), 1f);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("snowstrider"), 0.19f * inst.totalTrapRarity, 0.22f * inst.totalTrapRarity, 4.8f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("drillpod"), 0.09f, 0.1f, spawnInGround: true, randomFlip: true, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("fan"), 0.2f * inst.totalTrapRarity, 0.2f * inst.totalTrapRarity, 0.625f, isTrap: true);
        float num14 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(2.6f, 2.8f) * inst.totalTrapRarity;
        for (var index = 0; index < (double)num14; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight + 20f, -inst.halfHeight / 3f));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, Vector2.up, WorldGeneration.CHUNKSIZE * 8, LayerMask.GetMask("Ground"));
                if (raycastHit2D && raycastHit2D.point.y < inst.halfHeight - 5.0)
                {
                    var gameObject = (GameObject)Object.Instantiate(Resources.Load("icestalactite"), raycastHit2D.point + Vector2.down * 1.5f, Quaternion.identity);
                    gameObject.GetComponent<BuildingEntity>().blockPlacedOn = inst.WorldToBlockPos(raycastHit2D.point + Vector2.up * 0.5f);
                    inst.ChunkUpdated[inst.WorldToBlockPos(raycastHit2D.point + Vector2.up * 0.5f).x / WorldGeneration.CHUNKSIZE, inst.WorldToBlockPos(raycastHit2D.point + Vector2.up * 0.5f).y / WorldGeneration.CHUNKSIZE].AddListener(gameObject.GetComponent<StalactiteDropper>().CheckSeating);
                    gameObject.transform.localScale = new Vector3(Random.Range(0.0f, 1f) > 0.5 ? -1f : 1f, 1f, 1f);
                }
            }
        }

        float playerY = PlayerCamera.main.body.transform.position.y;
        float num15 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(0.24f, 0.3f) * inst.totalTrapRarity;
        for (var index = 0; index < (double)num15; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight, -inst.halfHeight / 3f));
            if (vector2.y <= playerY + 16f)
                continue;
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                float num16 = Random.value > 0.5 ? 1f : -1f;
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, Vector2.right * num16, WorldGeneration.CHUNKSIZE, LayerMask.GetMask("Ground"));
                if (raycastHit2D)
                    ((GameObject)Object.Instantiate(Resources.Load("turret"), raycastHit2D.point + Vector2.left * num16, Quaternion.identity)).transform.localScale = new Vector2(-num16, 1f);
            }
        }
    }

    public static void EntitiesBiome4(WorldGeneration inst)
    {
        float num11 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(4f, 5f);
        for (var i = 0; i < num11; ++i)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight / 3f, inst.halfHeight / 3f));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D3 = Physics2D.Raycast(vector2, Vector2.up, WorldGeneration.CHUNKSIZE * 4, LayerMask.GetMask("Ground"));
                RaycastHit2D raycastHit2D4 = Physics2D.Raycast(vector2, Vector2.down, WorldGeneration.CHUNKSIZE * 4, LayerMask.GetMask("Ground"));
                Vector2Int blockPos = inst.WorldToBlockPos(raycastHit2D3.point - Vector2.up * 0.5f);
                if (raycastHit2D3 && raycastHit2D3.point.y < inst.halfHeight / 3f && raycastHit2D4 && raycastHit2D4.point.y > -inst.halfHeight / 3f && raycastHit2D4.point.y < inst.halfHeight / 3f - 5f)
                {
                    Color color = Color.Lerp(Color.gray, Color.white, Random.value);
                    var gameObject3 = (GameObject)Object.Instantiate(Resources.Load("Special/sandvinehook"), inst.BlockToWorldPos(blockPos), Quaternion.identity);
                    var gameObject4 = (GameObject)Object.Instantiate(Resources.Load("Special/sandvinerope"), (inst.BlockToWorldPos(blockPos) + raycastHit2D4.point) * 0.5f, Quaternion.identity);
                    gameObject4.GetComponent<SpriteRenderer>().size = new Vector2(2.5f, Mathf.Abs(inst.BlockToWorldPos(blockPos).y - raycastHit2D4.point.y));
                    gameObject4.GetComponent<SpriteRenderer>().color = color;
                    gameObject3.GetComponent<SpriteRenderer>().color = color;
                    gameObject4.GetComponent<SpriteRenderer>().flipX = Random.value > 0.5;
                    gameObject3.GetComponent<SpriteRenderer>().flipX = Random.value > 0.5;
                    float x = Random.Range(0.15f, 1f);
                    gameObject3.transform.localScale = new Vector3(x, 1f);
                    gameObject4.transform.localScale = new Vector3(x, 1f);
                    var num12 = (float)((1.0 - x) * 16.0);
                    var component = gameObject4.GetComponent<Climbable>();
                    component.points.Add(raycastHit2D4.point);
                    component.points.Add(raycastHit2D3.point);
                    component.downwardsVelocity = num12;
                }
            }
        }

        DistributeEntities(inst, 1, (GameObject)Resources.Load("glowplant"), 0.2f, 0.3f, 1.25f, 10f, 0.25f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("shadecrawler"), 0.45f * inst.totalTrapRarity, 0.5f * inst.totalTrapRarity, 2f, 180f, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("wallbiter"), 0.1f * inst.totalTrapRarity, 0.11f * inst.totalTrapRarity, 4.8f, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("thornbackyoung"), 0.24f * inst.totalTrapRarity, 0.26f * inst.totalTrapRarity, 4.8f, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("overgrowntick"), 0.1f * inst.totalTrapRarity, 0.12f * inst.totalTrapRarity, 4.8f, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("caveticks"), 0.15f * inst.totalTrapRarity, 0.16f * inst.totalTrapRarity, 4.8f, isTrap: true);
        for (var index = 0; index < 2; ++index)
            Object.Instantiate(Resources.Load("thornbackelder"), new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight / 3f, inst.halfHeight / 3f)), Quaternion.identity);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("stoneplant"), 0.4f, 0.5f, 1.9f, 10f, 0.1f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("ceilingrye"), 0.65f, 0.8f, 1f, 10f, 0.5f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos), dir: Vector2.up);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("medcrate"), 0.18f * inst.totalLootRarity, 0.2f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("containercrate"), 0.05f * inst.totalLootRarity, 0.07f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("foodbox"), 0.1f * inst.totalLootRarity, 0.13f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("corpse"), 1.1f * inst.totalLootRarity, 1.2f * inst.totalLootRarity, checkFunc: pos => inst.GetBlock(pos) > 0 && inst.GetBlock(pos + Vector2Int.right) > 0 && inst.GetBlock(pos - Vector2Int.right) > 0);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("animalcorpse"), 0.9f * inst.totalLootRarity, 0.95f * inst.totalLootRarity, checkFunc: pos => inst.GetBlock(pos) > 0 && inst.GetBlock(pos + Vector2Int.right) > 0 && inst.GetBlock(pos - Vector2Int.right) > 0);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("geotree"), 0.4f, 0.5f, 3f, 6f, 0.15f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("browncap"), 0.4f, 0.5f, 3f, 6f, 0.15f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("hydreed"), 0.6f, 0.7f, 2.6f, 6f, 0.4f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("leadbush"), 1.1f, 1.2f, 0.6f, 6f, 0.1f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("droppings"), 3.7f, 4f);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("pop"), 1f * inst.totalLootRarity, 1.1f * inst.totalLootRarity, 2f, 20f, 0.2f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("bananaplant"), 1.9f * inst.totalTrapRarity, 2f * inst.totalTrapRarity, 0.4f, 15f, 0.1f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("coil"), 0.2f * inst.totalTrapRarity, 0.3f * inst.totalTrapRarity, 2f, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("beartrap"), 0.1f * inst.totalTrapRarity, 0.2f * inst.totalTrapRarity, 1f, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("jumppad"), 0.25f * inst.totalTrapRarity, 0.35f * inst.totalTrapRarity, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("spikestabber"), 0.4f * inst.totalTrapRarity, 0.5f * inst.totalTrapRarity, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("geyser"), 0.7f, 0.8f, 0.6f, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("skullcrusher"), 1.1f * inst.totalTrapRarity, 1.2f * inst.totalTrapRarity, 1f, 10f, randomFlip: true, isTrap: true, dir: Vector2.up);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("drillpod"), 0.09f, 0.1f, spawnInGround: true, randomFlip: true, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("grabberplant"), 0.4f * inst.totalTrapRarity, 0.5f * inst.totalTrapRarity, isTrap: true);
        float num13 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(6f, 7f);
        for (var index = 0; index < (double)num13; ++index)
        {
            var position = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight / 3f, inst.halfHeight / 3f));
            Object.Instantiate(Resources.Load("wallflower"), position, Quaternion.Euler(0.0f, 0.0f, Random.value * 360f));
        }
    }

    public static void EntitiesBiome3(WorldGeneration inst)
    {
        DistributeEntities(inst, 2, (GameObject)Resources.Load("glowplant"), (float)(2.4000000953674316 * (inst.biomeDepth == 2 ? 1.0 : 2.0)), (float)(2.5 * (inst.biomeDepth == 2 ? 1.0 : 2.0)), 1.25f, 10f, 0.25f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("stoneplant"), (float)(0.4000000059604645 * (inst.biomeDepth == 2 ? 1.0 : 3.0)), (float)(0.5 * (inst.biomeDepth == 2 ? 1.0 : 3.0)), 1.9f, 10f, 0.1f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("cactus"), 1.4f, 1.6f, 2.1f, 10f, 0.3f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos), isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("sandrose"), 1.3f, 1.4f, 1.5f, 10f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("drybush"), 6f, 7f, 2f, 20f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("brownshroom"), 4f, 5f, 0.9f, 10f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("stalagmite"), 10f, 15f, 2.8f, spawnYOffsetDeviation: 0.15f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 18 || inst.GetBlock(pos) == 17 || inst.GetBlock(pos) == 19);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("jumppad"), 0.15f * inst.totalTrapRarity, 0.25f * inst.totalTrapRarity, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("fan"), 0.1f * inst.totalTrapRarity, 0.15f * inst.totalTrapRarity, 0.625f, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("landmine"), 0.13f * inst.totalTrapRarity, 0.16f * inst.totalTrapRarity, 0.4f, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("ceilingrye"), 0.08f, 0.1f, 1f, 10f, 0.5f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos), dir: Vector2.up);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("spentfuel"), 0.3f * inst.totalTrapRarity, 0.35f * inst.totalTrapRarity, 1.875f, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("soundcannon"), 0.4f * inst.totalTrapRarity, 0.45f * inst.totalTrapRarity, 1f, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("foodbox"), 0.1f * inst.totalLootRarity, 0.13f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("pop"), 3f * inst.totalLootRarity, 4f * inst.totalLootRarity, 2f, 20f, 0.2f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("coil"), 0.2f * inst.totalTrapRarity, 0.3f * inst.totalTrapRarity, 2f, isTrap: true);
        float num5 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(0.3f, 0.4f);
        for (var index = 0; index < (double)num5; ++index)
        {
            var position = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(inst.halfHeight / 3f, inst.halfHeight));
            Object.Instantiate(Resources.Load("oilpipe"), position, Quaternion.Euler(0.0f, 0.0f, Random.value * 360f));
        }

        var num6 = (float)(inst.chunkWidth * inst.chunkHeight * (double)Random.Range(0.12f, 0.15f) * inst.totalTrapRarity * (inst.biomeDepth == 2 ? 1.0 : 0.6600000262260437));
        for (var index = 0; index < (double)num6; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(inst.halfHeight / 3f, inst.halfHeight));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                float num7 = Random.value > 0.5 ? 1f : -1f;
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, Vector2.right * num7, WorldGeneration.CHUNKSIZE, LayerMask.GetMask("Ground"));
                if (raycastHit2D)
                    ((GameObject)Object.Instantiate(Resources.Load("turret"), raycastHit2D.point + Vector2.left * num7, Quaternion.identity)).transform.localScale = new Vector2(-num7, 1f);
            }
        }

        float num8 = inst.chunkWidth * inst.chunkHeight * Random.Range(1.5f, 2f) / 3f * inst.totalTrapRarity;
        for (var index = 0; index < (double)num8; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(inst.halfHeight / 3f, inst.halfHeight));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, Vector2.up, WorldGeneration.CHUNKSIZE * 2, LayerMask.GetMask("Ground"));
                if (raycastHit2D && raycastHit2D.point.y < inst.halfHeight - 5.0)
                {
                    var gameObject = (GameObject)Object.Instantiate(Resources.Load("stalactite"), raycastHit2D.point + Vector2.down * 1.5f, Quaternion.identity);
                    gameObject.GetComponent<BuildingEntity>().blockPlacedOn = inst.WorldToBlockPos(raycastHit2D.point + Vector2.up * 0.5f);
                    inst.ChunkUpdated[inst.WorldToBlockPos(raycastHit2D.point + Vector2.up * 0.5f).x / WorldGeneration.CHUNKSIZE, inst.WorldToBlockPos(raycastHit2D.point + Vector2.up * 0.5f).y / WorldGeneration.CHUNKSIZE].AddListener(gameObject.GetComponent<StalactiteDropper>().CheckSeating);
                    gameObject.transform.localScale = new Vector3(Random.Range(0.0f, 1f) > 0.5 ? -1f : 1f, 1f, 1f);
                }
            }
        }

        var num9 = (float)(inst.chunkWidth * inst.chunkHeight / 3f * (double)Random.Range(6f, 7f) * (inst.biomeDepth == 2 ? 1.0 : 0.10000000149011612));
        for (var index = 0; index < (double)num9; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(inst.halfHeight / 3f, inst.halfHeight));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D1 = Physics2D.Raycast(vector2, Vector2.up, WorldGeneration.CHUNKSIZE * 4, LayerMask.GetMask("Ground"));
                RaycastHit2D raycastHit2D2 = Physics2D.Raycast(vector2, Vector2.down, WorldGeneration.CHUNKSIZE * 4, LayerMask.GetMask("Ground"));
                Vector2Int blockPos = inst.WorldToBlockPos(raycastHit2D1.point - Vector2.up * 0.5f);
                if (raycastHit2D1 && raycastHit2D2 && raycastHit2D2.point.y > inst.halfHeight / 3f && raycastHit2D2.point.y < inst.halfHeight - 5.0)
                {
                    Color color = Color.Lerp(Color.gray, Color.white, Random.value);
                    var gameObject1 = (GameObject)Object.Instantiate(Resources.Load("Special/sandvinehook"), inst.BlockToWorldPos(blockPos), Quaternion.identity);
                    var gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Special/sandvinerope"), (inst.BlockToWorldPos(blockPos) + raycastHit2D2.point) * 0.5f, Quaternion.identity);
                    gameObject2.GetComponent<SpriteRenderer>().size = new Vector2(2.5f, Mathf.Abs(inst.BlockToWorldPos(blockPos).y - raycastHit2D2.point.y));
                    gameObject2.GetComponent<SpriteRenderer>().color = color;
                    gameObject1.GetComponent<SpriteRenderer>().color = color;
                    gameObject2.GetComponent<SpriteRenderer>().flipX = Random.value > 0.5;
                    gameObject1.GetComponent<SpriteRenderer>().flipX = Random.value > 0.5;
                    float x = Random.Range(0.15f, 1f);
                    gameObject1.transform.localScale = new Vector3(x, 1f);
                    gameObject2.transform.localScale = new Vector3(x, 1f);
                    var num10 = (float)((1.0 - x) * 16.0);
                    var component = gameObject2.GetComponent<Climbable>();
                    component.points.Add(raycastHit2D2.point);
                    component.points.Add(raycastHit2D1.point);
                    component.downwardsVelocity = num10;
                }
            }
        }

        DistributeEntities(inst, 2, (GameObject)Resources.Load("rag"), (float)(0.11999999731779099 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.5)), (float)(0.20000000298023224 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.5)), 1f);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("corpse"), (float)(0.75 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.0)), (float)(0.8199999928474426 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.0)), checkFunc: pos => inst.GetBlock(pos) > 0 && inst.GetBlock(pos + Vector2Int.right) > 0 && inst.GetBlock(pos - Vector2Int.right) > 0);
    }

    public static IEnumerator GenerateBiome2(WorldGeneration inst)
    {
        var biomeMap1 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        biomeMap1.SetNoiseType(FastNoiseLite.NoiseType.Value);
        biomeMap1.SetFrequency(0.086f);
        biomeMap1.SetFractalType(FastNoiseLite.FractalType.FBm);
        biomeMap1.SetFractalOctaves(2);
        biomeMap1.SetFractalGain(0.49f);
        biomeMap1.SetFractalWeightedStrength(2.34f);
        biomeMap1.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        biomeMap1.SetDomainWarpAmp(22f);
        var frequencyMap = new FastNoiseLite(Random.Range(0, int.MaxValue));
        frequencyMap.SetFrequency(3f / 500f);
        var dirtPerlinNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        dirtPerlinNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        dirtPerlinNoise.SetFrequency(0.02f);
        dirtPerlinNoise.SetFractalType(FastNoiseLite.FractalType.Ridged);
        dirtPerlinNoise.SetFractalGain(0.65f);
        var caveNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        caveNoise.SetFrequency(0.005f);
        caveNoise.SetFractalType(FastNoiseLite.FractalType.PingPong);
        caveNoise.SetFractalGain(0.35f);
        caveNoise.SetDomainWarpType(FastNoiseLite.DomainWarpType.BasicGrid);
        caveNoise.SetDomainWarpAmp(40f);
        var toxicNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        toxicNoise.SetFrequency(0.012f);
        toxicNoise.SetFractalType(FastNoiseLite.FractalType.PingPong);
        toxicNoise.SetFractalGain(0.3f);
        toxicNoise.SetDomainWarpType(FastNoiseLite.DomainWarpType.BasicGrid);
        toxicNoise.SetDomainWarpAmp(50f);
        var biomeMap2 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        biomeMap2.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        biomeMap2.SetFrequency(0.05f);
        biomeMap2.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
        biomeMap2.SetCellularReturnType(FastNoiseLite.CellularReturnType.Distance);
        biomeMap2.SetCellularJitter(1f);
        biomeMap2.SetFractalType(FastNoiseLite.FractalType.Ridged);
        biomeMap2.SetFractalLacunarity(1.5f);
        var marbleMap = new FastNoiseLite(Random.Range(0, int.MaxValue));
        marbleMap.SetFrequency(0.007f);
        marbleMap.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        marbleMap.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        marbleMap.SetDomainWarpAmp(100f);
        var i1 = 0;
        var minMarble = 0.45f;
        for (var i2 = 0; i2 < inst.width; ++i2)
        {
            for (var y = 0; y < (long)(inst.height * (1f / 3f)); ++y)
            {
                float noise = biomeMap1.GetNoise(i2, y);
                var num11 = (float)(frequencyMap.GetNoise(i2, y) * 0.25 + 0.10000000149011612);
                if (marbleMap.GetNoise(i2, y) <= (double)minMarble)
                {
                    inst.worldBlocks[i2, y] = noise <= (double)num11 || dirtPerlinNoise.GetNoise(i2, y) >= -0.4000000059604645 ? (ushort)0 : noise < num11 + 0.10000000149011612 ? (ushort)12 : (ushort)13;
                    if (caveNoise.GetNoise(i2, y) > 0.6499999761581421)
                        inst.worldBlocks[i2, y] = 17;
                    if (noise > 0.75)
                        inst.worldBlocks[i2, y] = 15;
                    if (biomeMap2.GetNoise(i2, y) > 0.10000000149011612)
                        inst.worldBlocks[i2, y] = (ushort)Random.Range(3, 5);
                }
                else
                {
                    ++i1;
                    inst.worldBlocks[i2, y] = noise > (double)num11 ? dirtPerlinNoise.GetNoise(i2, y) < -0.10000000149011612 ? (ushort)18 : (ushort)19 : (ushort)0;
                    if (i1 > 100)
                    {
                        i1 = 0;
                        Object.Instantiate(Resources.Load("Special/marbleBackground"), inst.BlockToWorldPos(new Vector2Int(i2, y)), Quaternion.Euler(0.0f, 0.0f, Random.value * 360f));
                    }
                }
            }

            if (Time.unscaledTimeAsDouble - tileTimer > 1.0)
            {
                yield return null;
                tileTimer = Time.unscaledTimeAsDouble;
            }
        }

        float num12 = inst.chunkWidth * (inst.chunkHeight / 3f) * Random.Range(0.25f, 0.3f);
        for (var index13 = 0; index13 < (double)num12; ++index13)
        {
            var vector2Int = new Vector2Int(Random.Range(0, (int)inst.width), Random.Range(0, (int)(inst.height * (1f / 3f))));
            int num13 = Random.Range(6, 48 /*0x30*/);
            for (var index14 = 0; index14 < num13; ++index14)
            {
                for (var index15 = 0; index15 < 2; ++index15)
                {
                    if (vector2Int.x + index14 < inst.width && vector2Int.y + index15 < (long)(inst.height * (1f / 3f)))
                        inst.worldBlocks[index14 + vector2Int.x, index15 + vector2Int.y] = 11;
                }
            }
        }

        float num14 = inst.chunkWidth * (inst.chunkHeight / 3f) * Random.Range(0.24f, 0.3f);
        for (var index16 = 0; index16 < (double)num14; ++index16)
        {
            var vector2Int = new Vector2Int(Random.Range(0, (int)inst.width), Random.Range(0, (int)(inst.height * (1f / 3f))));
            int num15 = Random.Range(6, 48 /*0x30*/);
            for (var index17 = 0; index17 < num15; ++index17)
            {
                for (var index18 = 0; index18 < 2; ++index18)
                {
                    if (vector2Int.x + index18 < inst.width && vector2Int.y + index17 < (long)(inst.height * (1f / 3f)))
                        inst.worldBlocks[index18 + vector2Int.x, index17 + vector2Int.y] = 11;
                }
            }
        }
    }

    public static IEnumerator GenerateBiome1(WorldGeneration inst)
    {
        var caveNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        caveNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        caveNoise.SetFrequency(0.06f);
        caveNoise.SetFractalOctaves(3);
        caveNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        caveNoise.SetFractalLacunarity(1.5f);
        var dirtPerlinNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        dirtPerlinNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        dirtPerlinNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        dirtPerlinNoise.SetFractalOctaves(7);
        dirtPerlinNoise.SetFrequency(0.035f);
        var frequencyMap = new FastNoiseLite(Random.Range(0, int.MaxValue));
        frequencyMap.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        frequencyMap.SetFrequency(0.00037f);
        var biomeMap1 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        biomeMap1.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        biomeMap1.SetFrequency(0.04f);
        biomeMap1.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
        biomeMap1.SetCellularReturnType(FastNoiseLite.CellularReturnType.Distance);
        biomeMap1.SetCellularJitter(1f);
        biomeMap1.SetFractalType(FastNoiseLite.FractalType.Ridged);
        biomeMap1.SetFractalLacunarity(1.5f);
        for (var i1 = 0; i1 < inst.width; ++i1)
        {
            for (var y = (int)(inst.height * (1f / 3f)); y < (int)(inst.height * (2f / 3f)); ++y)
            {
                caveNoise.SetFrequency((float)(0.05999999865889549 + frequencyMap.GetNoise(i1, y) * 0.009999999776482582));
                inst.worldBlocks[i1, y] = caveNoise.GetNoise(i1, y) > -0.7149999737739563 ? (ushort)1 : (ushort)0;
                float noise = dirtPerlinNoise.GetNoise(i1, y);
                if (inst.worldBlocks[i1, y] > 0 && noise < -0.10000000149011612)
                    inst.worldBlocks[i1, y] = noise < -0.33 ? (ushort)16 /*0x10*/ : (ushort)2;
                if (inst.worldBlocks[i1, y] > 0 && Random.Range(0.0f, 1f) > 0.9900000095367432)
                    inst.worldBlocks[i1, y] = (ushort)Random.Range(1, 5);
                if (biomeMap1.GetNoise(i1, y) > 0.10000000149011612)
                    inst.worldBlocks[i1, y] = (ushort)Random.Range(3, 5);
                if (inst.worldBlocks[i1, y] > 0 && biomeMap1.GetNoise(i1, y) < -0.800000011920929)
                    inst.worldBlocks[i1, y] = 15;
                if (y < inst.height * 0.5)
                {
                    var num2 = (float)(y / (double)inst.height * 2.0);
                    if (Random.Range(0.0f, 1f) > (double)num2 && inst.worldBlocks[i1, y] == 2)
                        inst.worldBlocks[i1, y] = 12;
                    if (y < inst.height * 0.33000001311302185 && Random.Range(0.0f, 1f) > num2 * 3.0 && inst.worldBlocks[i1, y] == 1)
                        inst.worldBlocks[i1, y] = 13;
                }
            }

            if (Time.unscaledTimeAsDouble - tileTimer > 1.0)
            {
                yield return null;
                tileTimer = Time.unscaledTimeAsDouble;
            }
        }

        float num3 = inst.chunkWidth * (inst.chunkHeight / 3f) * Random.Range(0.35f, 0.5f);
        for (var index1 = 0; index1 < (double)num3; ++index1)
        {
            var vector2Int = new Vector2Int(Random.Range(0, (int)inst.width), Random.Range((int)(inst.height * (1f / 3f)), (int)(inst.height * (2f / 3f))));
            int num4 = Random.Range(6, 64 /*0x40*/);
            for (var index2 = 0; index2 < num4; ++index2)
            {
                for (var index3 = 0; index3 < 3; ++index3)
                {
                    if (vector2Int.x + index2 < inst.width && vector2Int.y + index3 < inst.height)
                        inst.worldBlocks[index2 + vector2Int.x, index3 + vector2Int.y] = 5;
                }
            }
        }

        float num5 = inst.chunkWidth * (inst.chunkHeight / 3f) * Random.Range(0.35f, 0.5f);
        for (var index4 = 0; index4 < (double)num5; ++index4)
        {
            var vector2Int = new Vector2Int(Random.Range(0, (int)inst.width), Random.Range((int)(inst.height * (1f / 3f)), (int)(inst.height * (2f / 3f))));
            int num6 = Random.Range(6, 60);
            for (var index5 = 0; index5 < num6; ++index5)
            {
                for (var index6 = 0; index6 < 3; ++index6)
                {
                    if (vector2Int.x + index6 < inst.width && vector2Int.y + index5 < inst.height)
                        inst.worldBlocks[index6 + vector2Int.x, index5 + vector2Int.y] = 5;
                }
            }
        }
    }

    public static IEnumerator GenerateBiome0(WorldGeneration inst)
    {
        var caveNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        caveNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        caveNoise.SetFrequency(0.06f);
        caveNoise.SetFractalOctaves(3);
        caveNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        caveNoise.SetFractalLacunarity(1.5f);
        var dirtPerlinNoise = new FastNoiseLite(Random.Range(0, int.MaxValue));
        dirtPerlinNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        dirtPerlinNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        dirtPerlinNoise.SetFractalOctaves(7);
        dirtPerlinNoise.SetFrequency(0.035f);
        var frequencyMap = new FastNoiseLite(Random.Range(0, int.MaxValue));
        frequencyMap.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        frequencyMap.SetFrequency(0.00037f);
        var biomeMap1 = new FastNoiseLite(Random.Range(0, int.MaxValue));
        biomeMap1.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        biomeMap1.SetFrequency(0.04f);
        biomeMap1.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
        biomeMap1.SetCellularReturnType(FastNoiseLite.CellularReturnType.Distance);
        biomeMap1.SetCellularJitter(1f);
        biomeMap1.SetFractalType(FastNoiseLite.FractalType.Ridged);
        biomeMap1.SetFractalLacunarity(1.5f);
        for (var i1 = 0; i1 < inst.width; ++i1)
        {
            for (var y = (int)(inst.height * (2f / 3f)); y < (int)inst.height; ++y)
            {
                caveNoise.SetFrequency((float)(0.05999999865889549 + frequencyMap.GetNoise(i1, y) * 0.009999999776482582));
                inst.worldBlocks[i1, y] = caveNoise.GetNoise(i1, y) > -0.7149999737739563 ? (ushort)1 : (ushort)0;
                float noise = dirtPerlinNoise.GetNoise(i1, y);
                if (inst.worldBlocks[i1, y] > 0 && noise < -0.10000000149011612)
                    inst.worldBlocks[i1, y] = noise < -0.33 ? (ushort)16 /*0x10*/ : (ushort)2;
                if (inst.worldBlocks[i1, y] > 0 && Random.Range(0.0f, 1f) > 0.9900000095367432)
                    inst.worldBlocks[i1, y] = (ushort)Random.Range(1, 5);
                if (biomeMap1.GetNoise(i1, y) > 0.10000000149011612)
                    inst.worldBlocks[i1, y] = (ushort)Random.Range(3, 5);
                if (inst.worldBlocks[i1, y] > 0 && biomeMap1.GetNoise(i1, y) < -0.800000011920929)
                    inst.worldBlocks[i1, y] = 15;
            }

            if (Time.unscaledTimeAsDouble - tileTimer > 1.0)
            {
                yield return null;
                tileTimer = Time.unscaledTimeAsDouble;
            }
        }

        float num7 = inst.chunkWidth * (inst.chunkHeight / 3f) * Random.Range(0.35f, 0.4f);
        for (var index7 = 0; index7 < (double)num7; ++index7)
        {
            var vector2Int = new Vector2Int(Random.Range(0, (int)inst.width), Random.Range((int)(inst.height * (2f / 3f)), (int)inst.height));
            int num8 = Random.Range(6, 48 /*0x30*/);
            for (var index8 = 0; index8 < num8; ++index8)
            {
                for (var index9 = 0; index9 < 2; ++index9)
                {
                    if (vector2Int.x + index8 < inst.width && vector2Int.y + index9 < inst.height)
                        inst.worldBlocks[index8 + vector2Int.x, index9 + vector2Int.y] = 11;
                }
            }
        }

        float num9 = inst.chunkWidth * (inst.chunkHeight / 3f) * Random.Range(0.35f, 0.4f);
        for (var index10 = 0; index10 < (double)num9; ++index10)
        {
            var vector2Int = new Vector2Int(Random.Range(0, (int)inst.width), Random.Range((int)(inst.height * (2f / 3f)), (int)inst.height));
            int num10 = Random.Range(6, 48 /*0x30*/);
            for (var index11 = 0; index11 < num10; ++index11)
            {
                for (var index12 = 0; index12 < 2; ++index12)
                {
                    if (vector2Int.x + index12 < inst.width && vector2Int.y + index11 < inst.height)
                        inst.worldBlocks[index12 + vector2Int.x, index11 + vector2Int.y] = 11;
                }
            }
        }
    }

    public static void FinishBiome2(WorldGeneration inst)
    {
        PlaceLiquids(inst, 0, 50f,  1, 26);
        PlaceLiquids(inst, 0, 15f, 3, 128);
    }

    public static void FinishBiome1(WorldGeneration inst)
    {
        PlaceLiquids(inst, 1, 10f,  1, 400);
        PlaceLiquids(inst, 1, 18f,  2, 128);
    }

    public static void FinishBiome0(WorldGeneration inst)
    {
        PlaceLiquids(inst, 2, 128f, 1, 32);
    }

    public static void EntitiesBiome2(WorldGeneration inst)
    {
        DistributeEntities(inst, 0, (GameObject)Resources.Load("glowplant"), (float)(2.4000000953674316 * (inst.biomeDepth == 2 ? 1.0 : 2.0)), (float)(2.5 * (inst.biomeDepth == 2 ? 1.0 : 2.0)), 1.25f, 10f, 0.25f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos));
        DistributeEntities(inst, 0, (GameObject)Resources.Load("stoneplant"), (float)(0.4000000059604645 * (inst.biomeDepth == 2 ? 1.0 : 3.0)), (float)(0.5 * (inst.biomeDepth == 2 ? 1.0 : 3.0)), 1.9f, 10f, 0.1f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos));
        DistributeEntities(inst, 0, (GameObject)Resources.Load("cactus"), 1.4f, 1.6f, 2.1f, 10f, 0.3f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos), isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("sandrose"), 1.3f, 1.4f, 1.5f, 10f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos));
        DistributeEntities(inst, 0, (GameObject)Resources.Load("drybush"), 6f, 7f, 2f, 20f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos));
        DistributeEntities(inst, 0, (GameObject)Resources.Load("brownshroom"), 4f, 5f, 0.9f, 10f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 12 || inst.GetBlock(pos) == 13 || inst.isSoil(pos));
        DistributeEntities(inst, 0, (GameObject)Resources.Load("stalagmite"), 10f, 15f, 2.8f, spawnYOffsetDeviation: 0.15f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 18 || inst.GetBlock(pos) == 17 || inst.GetBlock(pos) == 19);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("jumppad"), 0.15f * inst.totalTrapRarity, 0.25f * inst.totalTrapRarity, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("fan"), 0.1f * inst.totalTrapRarity, 0.15f * inst.totalTrapRarity, 0.625f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("landmine"), 0.13f * inst.totalTrapRarity, 0.16f * inst.totalTrapRarity, 0.4f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("ceilingrye"), 0.08f, 0.1f, 1f, 10f, 0.5f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos), dir: Vector2.up);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("wallbiter"), 0.12f * inst.totalTrapRarity, 0.13f * inst.totalTrapRarity, 4.8f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("shadecrawler"), 0.2f * inst.totalTrapRarity, 0.2f * inst.totalTrapRarity, 4.8f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("droppings"), 0.75f, 0.82f);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("beartrap"), 0.1f * inst.totalTrapRarity, 0.2f * inst.totalTrapRarity, 1f, isTrap: true);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("barbedwirefence"), 0.7f * inst.totalTrapRarity, 0.8f * inst.totalTrapRarity, 4.8f, isTrap: true);
        float num5 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(0.3f, 0.4f);
        for (var index = 0; index < (double)num5; ++index)
        {
            var position = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight, -inst.halfHeight / 3f));
            Object.Instantiate(Resources.Load("oilpipe"), position, Quaternion.Euler(0.0f, 0.0f, Random.value * 360f));
        }

        float playerY = PlayerCamera.main.body.transform.position.y;
        var num6 = (float)((double)(inst.chunkWidth * inst.chunkHeight) / 3f * Random.Range(0.12f, 0.15f) * inst.totalTrapRarity * (inst.biomeDepth == 2 ? 1.0 : 0.6600000262260437));
        for (var index = 0; index < (double)num6; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight, -inst.halfHeight / 3f));
            if (vector2.y <= playerY + 16f)
                continue;
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                float num7 = Random.value > 0.5 ? 1f : -1f;
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, Vector2.right * num7, WorldGeneration.CHUNKSIZE, LayerMask.GetMask("Ground"));
                if (raycastHit2D)
                    ((GameObject)Object.Instantiate(Resources.Load("turret"), raycastHit2D.point + Vector2.left * num7, Quaternion.identity)).transform.localScale = new Vector2(-num7, 1f);
            }
        }

        float num8 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(1.5f, 2f) * inst.totalTrapRarity;
        for (var index = 0; index < (double)num8; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight + 20f, -inst.halfHeight / 3f));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, Vector2.up, WorldGeneration.CHUNKSIZE * 2, LayerMask.GetMask("Ground"));
                if (raycastHit2D && raycastHit2D.point.y < inst.halfHeight - 5.0)
                {
                    var gameObject = (GameObject)Object.Instantiate(Resources.Load("stalactite"), raycastHit2D.point + Vector2.down * 1.5f, Quaternion.identity);
                    gameObject.GetComponent<BuildingEntity>().blockPlacedOn = inst.WorldToBlockPos(raycastHit2D.point + Vector2.up * 0.5f);
                    inst.ChunkUpdated[inst.WorldToBlockPos(raycastHit2D.point + Vector2.up * 0.5f).x / WorldGeneration.CHUNKSIZE, inst.WorldToBlockPos(raycastHit2D.point + Vector2.up * 0.5f).y / WorldGeneration.CHUNKSIZE].AddListener(gameObject.GetComponent<StalactiteDropper>().CheckSeating);
                    gameObject.transform.localScale = new Vector3(Random.Range(0.0f, 1f) > 0.5 ? -1f : 1f, 1f, 1f);
                }
            }
        }

        var num9 = (float)((double)(inst.chunkWidth * inst.chunkHeight) / 3f * Random.Range(6f, 7f) * (inst.biomeDepth == 2 ? 1.0 : 0.10000000149011612));
        for (var index = 0; index < (double)num9; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight, -inst.halfHeight / 3f));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D1 = Physics2D.Raycast(vector2, Vector2.up, WorldGeneration.CHUNKSIZE * 4, LayerMask.GetMask("Ground"));
                RaycastHit2D raycastHit2D2 = Physics2D.Raycast(vector2, Vector2.down, WorldGeneration.CHUNKSIZE * 4, LayerMask.GetMask("Ground"));
                Vector2Int blockPos = inst.WorldToBlockPos(raycastHit2D1.point - Vector2.up * 0.5f);
                if (raycastHit2D1 && raycastHit2D1.point.y < -inst.halfHeight / 3f && raycastHit2D2)
                {
                    Color color = Color.Lerp(Color.gray, Color.white, Random.value);
                    var gameObject1 = (GameObject)Object.Instantiate(Resources.Load("Special/sandvinehook"), inst.BlockToWorldPos(blockPos), Quaternion.identity);
                    var gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Special/sandvinerope"), (inst.BlockToWorldPos(blockPos) + raycastHit2D2.point) * 0.5f, Quaternion.identity);
                    gameObject2.GetComponent<SpriteRenderer>().size = new Vector2(2.5f, Mathf.Abs(inst.BlockToWorldPos(blockPos).y - raycastHit2D2.point.y));
                    gameObject2.GetComponent<SpriteRenderer>().color = color;
                    gameObject1.GetComponent<SpriteRenderer>().color = color;
                    gameObject2.GetComponent<SpriteRenderer>().flipX = Random.value > 0.5;
                    gameObject1.GetComponent<SpriteRenderer>().flipX = Random.value > 0.5;
                    float x = Random.Range(0.15f, 1f);
                    gameObject1.transform.localScale = new Vector3(x, 1f);
                    gameObject2.transform.localScale = new Vector3(x, 1f);
                    var num10 = (float)((1.0 - x) * 16.0);
                    var component = gameObject2.GetComponent<Climbable>();
                    component.points.Add(raycastHit2D2.point);
                    component.points.Add(raycastHit2D1.point);
                    component.downwardsVelocity = num10;
                }
            }
        }

        DistributeEntities(inst, 0, (GameObject)Resources.Load("rag"), (float)(0.11999999731779099 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.5)), (float)(0.20000000298023224 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.5)), 1f);
        DistributeEntities(inst, 0, (GameObject)Resources.Load("corpse"), (float)(0.75 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.0)), (float)(0.8199999928474426 * inst.totalLootRarity * (inst.biomeDepth == 2 ? 1.0 : 2.0)), checkFunc: pos => inst.GetBlock(pos) > 0 && inst.GetBlock(pos + Vector2Int.right) > 0 && inst.GetBlock(pos - Vector2Int.right) > 0);
    }

    public static void EntitiesBiome1(WorldGeneration inst)
    {
        DistributeEntities(inst, 1, (GameObject)Resources.Load("glowplant"), 2.7f, 3.5f, 1.25f, 10f, 0.25f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("stoneplant"), 0.4f, 0.5f, 1.9f, 10f, 0.1f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("ceilingrye"), 0.3f, 0.4f, 1f, 10f, 0.5f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos), dir: Vector2.up);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("medcrate"), 0.18f * inst.totalLootRarity, 0.2f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("containercrate"), 0.05f * inst.totalLootRarity, 0.07f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("foodbox"), 0.1f * inst.totalLootRarity, 0.13f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("spikestabber"), 0.4f * inst.totalTrapRarity, 0.5f * inst.totalTrapRarity, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("shadecrawler"), 0.4f * inst.totalTrapRarity, 0.42f * inst.totalTrapRarity, 2f, 180f, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("corpse"), 1f * inst.totalLootRarity, 1.1f * inst.totalLootRarity, checkFunc: pos => inst.GetBlock(pos) > 0 && inst.GetBlock(pos + Vector2Int.right) > 0 && inst.GetBlock(pos - Vector2Int.right) > 0);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("animalcorpse"), 0.6f * inst.totalLootRarity, 0.7f * inst.totalLootRarity, checkFunc: pos => inst.GetBlock(pos) > 0 && inst.GetBlock(pos + Vector2Int.right) > 0 && inst.GetBlock(pos - Vector2Int.right) > 0);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("drillpod"), 0.09f, 0.1f, spawnInGround: true, randomFlip: true, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("barbedwirefence"), 0.6f * inst.totalTrapRarity, 0.8f * inst.totalTrapRarity, 4.8f, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("beartrap"), 0.2f * inst.totalTrapRarity, 0.25f * inst.totalTrapRarity, 1f, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("CaveTicks"), 0.15f * inst.totalTrapRarity, 0.2f * inst.totalTrapRarity, 4f, spawnYOffsetDeviation: 3f, isTrap: true);
        DistributeEntities(inst, 1, (GameObject)Resources.Load("geyser"), 1.6f, 1.8f, 0.6f, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("jumppad"), 0.6f * inst.totalTrapRarity, 0.8f * inst.totalTrapRarity, isTrap: true);
        float num1 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(0.2f, 0.3f) * inst.totalLootRarity;
        for (var index = 0; index < (double)num1; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight / 3f, inst.halfHeight / 3f));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, Vector2.down, WorldGeneration.CHUNKSIZE, LayerMask.GetMask("Ground"));
                if (raycastHit2D)
                    ((GameObject)Object.Instantiate(Resources.Load("bandage"), raycastHit2D.point + Vector2.up, Quaternion.Euler(0.0f, 0.0f, Random.Range(-0.0f, 360f)))).GetComponent<Item>().condition = Random.Range(1, 4) * 0.33f;
            }
        }

        float num2 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(0.8f, 1f);
        for (var index = 0; index < (double)num2; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight / 3f, inst.halfHeight / 3f));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                if (!Physics2D.Raycast(vector2, Vector2.down, 12f, LayerMask.GetMask("Ground")))
                {
                    RaycastHit2D raycastHit2D = Physics2D.Linecast(vector2, vector2 + Vector2.down * 25f, LayerMask.GetMask("Ground"));
                    Vector2 b = raycastHit2D ? raycastHit2D.point : vector2 + Vector2.down * 25f;
                    GameObject gameObject = Utils.Create("climbingropeextended", vector2, 0.0f);
                    var component = gameObject.GetComponent<Climbable>();
                    component.points.Add(b);
                    component.points.Add(vector2);
                    gameObject.transform.GetChild(0).localScale = new Vector2(1f, Vector2.Distance(vector2, b) / 25f);
                }
            }
        }

        float num3 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(0.3f, 0.5f);
        for (var index = 0; index < (double)num3; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight / 3f, inst.halfHeight / 3f));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, Vector2.down, WorldGeneration.CHUNKSIZE, LayerMask.GetMask("Ground"));
                if (raycastHit2D)
                {
                    var gameObject = (GameObject)Object.Instantiate(Resources.Load("droppings"), raycastHit2D.point + Vector2.up, Quaternion.identity);
                }
            }
        }

        DistributeEntities(inst, 1, (GameObject)Resources.Load("geotree"), 2.7f, 3f, 3f, 6f, 0.15f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("hydreed"), 1.4f, 1.6f, 2.6f, 6f, 0.4f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        DistributeEntities(inst, 1, (GameObject)Resources.Load("leadbush"), 2f, 2.2f, 0.6f, 6f, 0.1f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
    }

    public static void EntitiesBiome0(WorldGeneration inst)
    {
        DistributeEntities(inst, 2, (GameObject)Resources.Load("glowplant"), 2.7f, 3.5f, 1.25f, 10f, 0.25f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("stoneplant"), 0.4f, 0.5f, 1.9f, 10f, 0.1f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("ceilingrye"), 0.3f, 0.4f, 1f, 10f, 0.5f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos), dir: Vector2.up);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("medcrate"), 0.18f * inst.totalLootRarity, 0.2f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("containercrate"), 0.05f * inst.totalLootRarity, 0.07f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("foodbox"), 0.1f * inst.totalLootRarity, 0.13f * inst.totalLootRarity, 3f, 180f);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("spikestabber"), 0.4f * inst.totalTrapRarity, 0.5f * inst.totalTrapRarity, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("shadecrawler"), 0.4f * inst.totalTrapRarity, 0.42f * inst.totalTrapRarity, 2f, 180f, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("corpse"), 1f * inst.totalLootRarity, 1.1f * inst.totalLootRarity, checkFunc: pos => inst.GetBlock(pos) > 0 && inst.GetBlock(pos + Vector2Int.right) > 0 && inst.GetBlock(pos - Vector2Int.right) > 0);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("animalcorpse"), 0.6f * inst.totalLootRarity, 0.7f * inst.totalLootRarity, checkFunc: pos => inst.GetBlock(pos) > 0 && inst.GetBlock(pos + Vector2Int.right) > 0 && inst.GetBlock(pos - Vector2Int.right) > 0);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("drillpod"), 0.09f, 0.1f, spawnInGround: true, randomFlip: true, isTrap: true);
        DistributeEntities(inst, 2, (GameObject)Resources.Load("geyser"), 0.7f, 0.8f, 0.6f, checkFunc: pos => inst.GetBlock(pos) < 3 || inst.isSoil(pos));
        DistributeEntities(inst, 2, (GameObject)Resources.Load("jumppad"), 0.6f * inst.totalTrapRarity, 0.8f * inst.totalTrapRarity, isTrap: true);
        float num1 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(0.2f, 0.3f) * inst.totalLootRarity;
        for (var index = 0; index < (double)num1; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(inst.halfHeight / 3f, inst.halfHeight));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, Vector2.down, WorldGeneration.CHUNKSIZE, LayerMask.GetMask("Ground"));
                if (raycastHit2D)
                    ((GameObject)Object.Instantiate(Resources.Load("bandage"), raycastHit2D.point + Vector2.up, Quaternion.Euler(0.0f, 0.0f, Random.Range(-0.0f, 360f)))).GetComponent<Item>().condition = Random.Range(1, 4) * 0.33f;
            }
        }

        float num2 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(0.8f, 1f);
        for (var index = 0; index < (double)num2; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(inst.halfHeight / 3f, inst.halfHeight));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                if (!Physics2D.Raycast(vector2, Vector2.down, 12f, LayerMask.GetMask("Ground")))
                {
                    RaycastHit2D raycastHit2D = Physics2D.Linecast(vector2, vector2 + Vector2.down * 25f, LayerMask.GetMask("Ground"));
                    Vector2 b = raycastHit2D ? raycastHit2D.point : vector2 + Vector2.down * 25f;
                    GameObject gameObject = Utils.Create("climbingropeextended", vector2, 0.0f);
                    var component = gameObject.GetComponent<Climbable>();
                    component.points.Add(b);
                    component.points.Add(vector2);
                    gameObject.transform.GetChild(0).localScale = new Vector2(1f, Vector2.Distance(vector2, b) / 25f);
                }
            }
        }

        float num3 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(0.3f, 0.5f);
        for (var index = 0; index < (double)num3; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(inst.halfHeight / 3f, inst.halfHeight));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, Vector2.down, WorldGeneration.CHUNKSIZE, LayerMask.GetMask("Ground"));
                if (raycastHit2D)
                {
                    var gameObject = (GameObject)Object.Instantiate(Resources.Load("droppings"), raycastHit2D.point + Vector2.up, Quaternion.identity);
                }
            }
        }

        float num4 = inst.chunkWidth * inst.chunkHeight / 3f * Random.Range(0.015f, 0.02f) * inst.totalLootRarity;
        for (var index = 0; index < (double)num4; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(inst.halfHeight / 3f, inst.halfHeight));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, Vector2.down, WorldGeneration.CHUNKSIZE, LayerMask.GetMask("Ground"));
                if (raycastHit2D)
                {
                    var gameObject = (GameObject)Object.Instantiate(Resources.Load("fleshchunk"), raycastHit2D.point + Vector2.up, Quaternion.Euler(0.0f, 0.0f, Random.Range(-0.0f, 360f)));
                }
            }
        }

        inst.DistributeEntities((GameObject)Resources.Load("geotree"), 2.7f, 3f, 3f, 6f, 0.15f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        inst.DistributeEntities((GameObject)Resources.Load("hydreed"), 1.4f, 1.6f, 2.6f, 6f, 0.4f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
        inst.DistributeEntities((GameObject)Resources.Load("leadbush"), 2f, 2.2f, 0.6f, 6f, 0.1f, randomFlip: true, checkFunc: pos => inst.isSoil(pos));
    }

    public static IEnumerator GenerateBiome10(WorldGeneration inst)
    {
        var caveNoise = new FastNoiseLite(UnityEngine.Random.Range(0, int.MaxValue));
        caveNoise.SetFrequency(0.02f);
        caveNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        caveNoise.SetFractalOctaves(4);
        caveNoise.SetFractalLacunarity(1.7f);
        caveNoise.SetFractalGain(0.51f);
        caveNoise.SetFractalWeightedStrength(0.01f);
        var toxicNoise = new FastNoiseLite(UnityEngine.Random.Range(0, int.MaxValue));
        toxicNoise.SetFrequency(0.02f);
        toxicNoise.SetDomainWarpAmp(60.5f);
        toxicNoise.SetDomainWarpType(FastNoiseLite.DomainWarpType.OpenSimplex2);
        for (var i1 = 0; i1 < inst.width; ++i1)
        {
            for (var y = 0; y < inst.height / 2f; ++y)
            {
                float noise4 = caveNoise.GetNoise(i1, y);
                var noise5 = (double)toxicNoise.GetNoise(i1, y);
                if (noise4 > 0.25)
                    inst.worldBlocks[i1, y] = noise4 > 0.3499999940395355 ? (ushort)1 : (ushort)40;
                if (noise5 > 0.8799999952316284)
                    inst.worldBlocks[i1, y] = 41;
                if (y < 4 && (y < 3 || Random.value > 0.800000011920929))
                    inst.worldBlocks[i1, y] = 40;
            }

            if (Time.unscaledTimeAsDouble - tileTimer > 1.0)
            {
                yield return null;
                tileTimer = Time.unscaledTimeAsDouble;
            }
        }
    }

    public static void FinishBiome10(WorldGeneration inst)
    {
        PlaceLiquids(inst, 3, 100f, (byte) 7, 48);
        PlaceLiquids(inst, 3, 64f, (byte) 1, 32);
    }

    public static void EntitiesBiome10(WorldGeneration inst)
    {
        DistributeEntities(inst, 3, (GameObject)Resources.Load("mushlight"), 24f, 25f, 1.1f, 15f, 0.15f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 41);
        DistributeEntities(inst, 3, (GameObject)Resources.Load("crimsonbush"), 6f, 7f, 2f, 20f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 40 || inst.isSoil(pos));
        DistributeEntities(inst, 3, (GameObject)Resources.Load("crimsontree"), 3f, 4f, 2.5f, 20f, 0.375f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 40 || inst.isSoil(pos));
        DistributeEntities(inst, 3, (GameObject)Resources.Load("pussack"), 0.9f * inst.totalLootRarity, 0.9f * inst.totalLootRarity, 1f, 5f, 0.25f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 40 || inst.isSoil(pos));
        DistributeEntities(inst, 3, (GameObject)Resources.Load("stoneplant"), 1.5f, 1.6f, 1.9f, 10f, 0.1f, randomFlip: true);
        DistributeEntities(inst, 3, (GameObject)Resources.Load("ceilingrye"), 1.4f, 1.5f, 1f, 10f, 0.5f, randomFlip: true, dir: Vector2.up);
        DistributeEntities(inst, 3, (GameObject)Resources.Load("leadbush"), 3.5f, 3.6f, 0.6f, 6f, 0.1f, randomFlip: true);
        DistributeEntities(inst, 3, (GameObject)Resources.Load("brownshroom"), 4f, 5f, 0.9f, 10f, randomFlip: true);
        DistributeEntities(inst, 3, (GameObject)Resources.Load("pop"), 1f, 1f, 2f, 20f, 0.2f, randomFlip: true, checkFunc: pos => inst.GetBlock(pos) == 40 || inst.isSoil(pos));
        float num21 = inst.chunkWidth * inst.chunkHeight / 2f * Random.Range(1.5f, 1.5f);
        for (var index = 0; index < num21; ++index)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(-inst.halfHeight, 0));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")))
            {
                RaycastHit2D raycastHit2D7 = Physics2D.Raycast(vector2, Vector2.up, WorldGeneration.CHUNKSIZE * 4, LayerMask.GetMask("Ground"));
                RaycastHit2D raycastHit2D8 = Physics2D.Raycast(vector2, Vector2.down, WorldGeneration.CHUNKSIZE * 4, LayerMask.GetMask("Ground"));
                if (raycastHit2D7 && raycastHit2D8 && raycastHit2D8.point.y < 0f - 5.0f)
                {
                    Color color = Color.Lerp(Color.gray, Color.white, Random.value);
                    var gameObject7 = (GameObject)Object.Instantiate(Resources.Load("Special/veintop"), raycastHit2D7.point + Vector2.down * 0.5f, Quaternion.identity);
                    var gameObject8 = (GameObject)Object.Instantiate(Resources.Load("Special/veinloop"), (raycastHit2D7.point + raycastHit2D8.point) * 0.5f, Quaternion.identity);
                    var gameObject9 = (GameObject)Object.Instantiate(Resources.Load("Special/veinbottom"), raycastHit2D8.point + Vector2.up * 0.5f, Quaternion.identity);
                    gameObject8.GetComponent<SpriteRenderer>().size = new Vector2(2f, Mathf.Abs(raycastHit2D7.point.y - raycastHit2D8.point.y) - 1f);
                    gameObject8.GetComponent<SpriteRenderer>().color = color;
                    gameObject9.GetComponent<SpriteRenderer>().color = color;
                    gameObject7.GetComponent<SpriteRenderer>().color = color;
                    float x = Random.Range(0.5f, 1f);
                    gameObject7.transform.localScale = new Vector3(x, 1f);
                    gameObject8.transform.localScale = new Vector3(x, 1f);
                    gameObject9.transform.localScale = new Vector3(x, 1f);
                    var num22 = (float)((1.0 - x) * 10.0);
                    var component = gameObject8.GetComponent<Climbable>();
                    component.points.Add(raycastHit2D8.point);
                    component.points.Add(raycastHit2D7.point);
                    component.downwardsVelocity = num22;
                }
            }
        }
    }

    public static void PlaceLiquids(WorldGeneration inst, int band, float perChunk, byte type, int maxFill)
    {
        int minY;
        int maxY;
        float chanceDiv;
        switch (band)
        {
            case 0:
                minY = 64;
                maxY = (int) (inst.height * (1f / 3f));
                chanceDiv = 3f;
                break;
            case 1:
                minY = (int) (inst.height * (1f / 3f));
                maxY = (int) (inst.height * (2f / 3f));
                chanceDiv = 3f;
                break;
            case 2:
                minY = (int) (inst.height * (2f / 3f));
                maxY = (int) inst.height;
                chanceDiv = 3f;
                break;
            case 3:
                minY = 0;
                maxY = (int) (inst.height * 0.5f);
                chanceDiv = 2f;
                break;
            default:
                Plugin.Logger.LogError($"BAD BAND: {band}");
                return;
        }

        float num = inst.chunkWidth * inst.chunkHeight / chanceDiv * perChunk;
        for (var i = 0; i < num; ++i)
        {
            var pos = new Vector2Int(Random.Range(0, (int)inst.width), Random.Range(minY, maxY));
            FluidManager.main.StartFill(pos, type, maxFill);
        }
    }

    public static void DistributeEntities(
        WorldGeneration inst,
        int band,
        GameObject basObj,
        float minPerChunk,
        float maxPerChunk,
        float spawnYOffset = 0.0f,
        float randomRotation = 0.0f,
        float spawnYOffsetDeviation = 0.0f,
        bool spawnInGround = false,
        bool randomFlip = false,
        WorldGeneration.PlaceCheckDelegate checkFunc = null,
        bool isTrap = false,
        Vector2 dir = default(Vector2),
        bool forceFlip = false)
    {
        float playerY = PlayerCamera.main.body.transform.position.y;
        float minY;
        float maxY;
        float chanceDiv;
        switch (band)
        {
            case 0:
                minY = -inst.height * 0.5f;
                maxY = -inst.height * (1f / 3f);
                chanceDiv = 3f;
                break;
            case 1:
                minY = -inst.height * (1f / 3f);
                maxY = inst.height * (1f / 3f);
                chanceDiv = 3f;
                break;
            case 2:
                minY = inst.height * (1f / 3f);
                maxY = inst.height * 0.5f;
                chanceDiv = 3f;
                break;
            case 3:
                minY = -inst.height * 0.5f;
                maxY = 0f;
                chanceDiv = 2f;
                break;
            default:
                Plugin.Logger.LogError($"BAD BAND: {band}");
                return;
        }

        if (dir == Vector2.zero)
            dir = Vector2.down;
        float num = inst.chunkWidth * inst.chunkHeight / chanceDiv * Random.Range(minPerChunk, maxPerChunk);
        for (var index1 = 0; index1 < (double)num; ++index1)
        {
            var vector2 = new Vector2(Random.Range(-inst.halfWidth, inst.halfWidth), Random.Range(isTrap && band == 0 ? minY + 15f : minY, maxY));
            if (!(bool)(Object)Physics2D.OverlapPoint(vector2, LayerMask.GetMask("Ground")) | spawnInGround)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(vector2, dir, WorldGeneration.CHUNKSIZE, LayerMask.GetMask("Ground"));
                if (raycastHit2D && MathF.Abs(raycastHit2D.point.x) < inst.halfWidth - 1.0 && raycastHit2D.point.y > minY + 1.0 && raycastHit2D.point.y < maxY - 1.0 && (checkFunc == null || checkFunc(inst.WorldToBlockPos(raycastHit2D.point - Vector2.up * 0.5f))))
                {
                    var pos = raycastHit2D.point - dir * Random.Range(spawnYOffset - spawnYOffsetDeviation, spawnYOffset + spawnYOffsetDeviation);
                    if (isTrap && pos.y <= playerY + 12f)
                        continue;
                    var gameObject = Object.Instantiate(basObj, pos, Quaternion.Euler(0.0f, 0.0f, basObj.transform.eulerAngles.z + Random.Range(-randomRotation, randomRotation)));
                    if ((bool)(Object)gameObject.GetComponent<BuildingEntity>())
                    {
                        gameObject.GetComponent<BuildingEntity>().blockPlacedOn = inst.WorldToBlockPos(raycastHit2D.point + dir * 0.5f);
                        if (gameObject.GetComponent<BuildingEntity>().requireGround)
                        {
                            UnityEvent[,] chunkUpdated = inst.ChunkUpdated;
                            Vector2Int blockPos = inst.WorldToBlockPos(raycastHit2D.point + dir * 0.5f);
                            int index2 = blockPos.x / WorldGeneration.CHUNKSIZE;
                            blockPos = inst.WorldToBlockPos(raycastHit2D.point + dir * 0.5f);
                            int index3 = blockPos.y / WorldGeneration.CHUNKSIZE;
                            chunkUpdated[index2, index3].AddListener(gameObject.GetComponent<BuildingEntity>().CheckSeating);
                        }
                    }

                    if (randomFlip)
                        gameObject.transform.localScale = new Vector3(Random.Range(0.0f, 1f) > 0.5 ? -1f : 1f, 1f, 1f);
                    if (forceFlip)
                        gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
        }
    }
}