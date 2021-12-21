﻿using HarmonyLib;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;
using static PerformanceOptimizer.ComponentCache;

namespace PerformanceOptimizer
{
    public class Optimization_FasterGetCompReplacement : Optimization
    {
        public override int DrawOrder => -99999;
        public List<MethodInfo> methodsCallingMapGetComp;
        public List<MethodInfo> methodsCallingWorldGetComp;
        public List<MethodInfo> methodsCallingGameGetComp;
        public List<MethodInfo> methodsCallingThingGetComp;
        public List<MethodInfo> methodsCallingThingTryGetComp;
        public List<MethodInfo> methodsCallingHediffTryGetComp;
        public List<MethodInfo> methodsCallingWorldObjectGetComp;

        public static HashSet<string> assembliesToSkip = new HashSet<string>
        {
            "System", "Cecil", "Multiplayer", "Prepatcher", "HeavyMelee", "0Harmony", "UnityEngine", "mscorlib", "ICSharpCode", "Newtonsoft", "TranspilerExplorer"
        };

        public static HashSet<string> typesToSkip = new HashSet<string>
        {
            "AnimalGenetics.ColonyManager+JobsWrapper", "AutoMachineTool", "NightVision.CombatHelpers", "RJWSexperience.UI.SexStatusWindow"
        };

        public static HashSet<string> methodsToSkip = new HashSet<string>
        {
            "Numbers.MainTabWindow_Numbers", "Numbers.OptionsMaker", "<PawnSelector>g__Action", "<AllBuildingsColonistWithComp>", "<FailOnOwnerStatus>", "Transpiler"
        };

        private static bool TypeValidator(Type type)
        {
            return !assembliesToSkip.Any(asmName => type.Assembly?.FullName?.Contains(asmName) ?? false) && !typesToSkip.Any(x => type.FullName.Contains(x));
        }

        private static List<Type> types;
        public static List<Type> GetTypesToParse()
        {
            if (types is null)
            {
                types = GenTypes.AllTypes.Where(type => TypeValidator(type)).Distinct().ToList();
            }
            return types;
        }

        public static List<MethodInfo> GetMethodsToParse(Type type)
        {
            List<MethodInfo> methods = new List<MethodInfo>();
            foreach (var method in AccessTools.GetDeclaredMethods(type))
            {
                if (method != null && !method.IsAbstract)
                {
                    try
                    {
                        var description = method.FullDescription();
                        if (!methodsToSkip.Any(x => description.Contains(x)))
                        {
                            if (!method.IsGenericMethod && !method.ContainsGenericParameters && !method.IsGenericMethodDefinition)
                            {
                                methods.Add(method);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //Log.Error("Exception: " + ex);
                    }
                }
            }
            return methods;
        }
        public static void ParseMethod(MethodInfo method, List<MethodInfo> methodsCallingMapGetComp, List<MethodInfo> methodsCallingWorldGetComp, List<MethodInfo> methodsCallingGameGetComp,
            List<MethodInfo> methodsCallingThingGetComp, List<MethodInfo> methodsCallingThingTryGetComp, List<MethodInfo> methodsCallingHediffTryGetComp
            , List<MethodInfo> methodsCallingWorldObjectGetComp)
        {
            try
            {
                var instructions = PatchProcessor.GetCurrentInstructions(method);
                foreach (var instr in instructions)
                {
                    if (instr.operand is MethodInfo mi)
                    {
                        if (mi.IsGenericMethod && mi.GetParameters().Length <= 0)
                        {
                            if (instr.opcode == OpCodes.Callvirt)
                            {
                                if (mi.Name == "GetComponent")
                                {
                                    var underlyingType = mi.GetUnderlyingType();
                                    if (!methodsCallingMapGetComp.Contains(method) && typeof(MapComponent).IsAssignableFrom(underlyingType))
                                    {
                                        methodsCallingMapGetComp.Add(method);
                                    }
                                    else if (!methodsCallingGameGetComp.Contains(method) && typeof(GameComponent).IsAssignableFrom(underlyingType))
                                    {
                                        methodsCallingGameGetComp.Add(method);
                                    }
                                    else if (!methodsCallingWorldGetComp.Contains(method) && typeof(WorldComponent).IsAssignableFrom(underlyingType))
                                    {
                                        methodsCallingWorldGetComp.Add(method);
                                    }
                                    else if (!methodsCallingWorldObjectGetComp.Contains(method) && typeof(WorldObjectComp).IsAssignableFrom(underlyingType))
                                    {
                                        methodsCallingWorldObjectGetComp.Add(method);
                                    }
                                }
                                else if (mi.Name == "GetComp")
                                {
                                    var underlyingType = mi.GetUnderlyingType();
                                    if (!methodsCallingThingGetComp.Contains(method) && typeof(ThingComp).IsAssignableFrom(underlyingType))
                                    {
                                        methodsCallingThingGetComp.Add(method);
                                    }
                                }
                            }
                        }
                        else if (mi.IsGenericMethod && mi.GetParameters().Length == 1)
                        {
                            if (instr.opcode == OpCodes.Call)
                            {
                                if (mi.Name == "TryGetComp")
                                {
                                    var underlyingType = mi.GetUnderlyingType();
                                    if (!methodsCallingThingTryGetComp.Contains(method) && typeof(ThingComp).IsAssignableFrom(underlyingType))
                                    {
                                        methodsCallingThingTryGetComp.Add(method);
                                    }
                                    else if (!methodsCallingHediffTryGetComp.Contains(method) && typeof(HediffComp).IsAssignableFrom(underlyingType))
                                    {
                                        methodsCallingHediffTryGetComp.Add(method);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public static MethodInfo genericMapGetComp = AccessTools.Method(typeof(ComponentCache), nameof(ComponentCache.GetMapComponentFast));
        private static IEnumerable<CodeInstruction> GetMapCompTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return PerformTranspiler("GetComponent", typeof(MapComponent), genericMapGetComp, OpCodes.Callvirt, 0, instructions);
        }

        public static MethodInfo genericWorldGetComp = AccessTools.Method(typeof(ComponentCache), nameof(ComponentCache.GetWorldComponentFast));
        private static IEnumerable<CodeInstruction> GetWorldCompTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return PerformTranspiler("GetComponent", typeof(WorldComponent), genericWorldGetComp, OpCodes.Callvirt, 0, instructions);
        }

        public static MethodInfo genericGameGetComp = AccessTools.Method(typeof(ComponentCache), nameof(ComponentCache.GetGameComponentFast));
        private static IEnumerable<CodeInstruction> GetGameCompTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return PerformTranspiler("GetComponent", typeof(GameComponent), genericGameGetComp, OpCodes.Callvirt, 0, instructions);
        }

        public static MethodInfo genericThingGetComp = AccessTools.Method(typeof(ComponentCache), nameof(ComponentCache.GetThingCompFast));
        private static IEnumerable<CodeInstruction> GetThingCompTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return PerformTranspiler("GetComp", typeof(ThingComp), genericThingGetComp, OpCodes.Callvirt, 0, instructions);
        }

        public static MethodInfo genericThingTryGetComp = AccessTools.Method(typeof(ComponentCache), nameof(ComponentCache.TryGetThingCompFast));
        private static IEnumerable<CodeInstruction> TryGetThingCompTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return PerformTranspiler("TryGetComp", typeof(ThingComp), genericThingTryGetComp, OpCodes.Call, 1, instructions);
        }

        public static MethodInfo genericHediffTryGetComp = AccessTools.Method(typeof(ComponentCache), nameof(ComponentCache.TryGetHediffCompFast));
        private static IEnumerable<CodeInstruction> TryGetHediffCompTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return PerformTranspiler("TryGetComp", typeof(HediffComp), genericHediffTryGetComp, OpCodes.Call, 1, instructions);
        }

        public static MethodInfo genericWorldObjectGetComp = AccessTools.Method(typeof(ComponentCache), nameof(ComponentCache.GetWorldObjectCompFast));
        private static IEnumerable<CodeInstruction> GetWorldObjectCompTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            return PerformTranspiler("GetComponent", typeof(WorldObjectComp), genericWorldObjectGetComp, OpCodes.Callvirt, 0, instructions);
        }
        public override OptimizationType OptimizationType => OptimizationType.Optimization;

        public override string Label => "PO.FasterGetCompReplacement".Translate();

        public override void DoPatches()
        {
            base.DoPatches();
            bool parse = false;
            if (methodsCallingMapGetComp is null)
            {
                methodsCallingMapGetComp = new List<MethodInfo>();
                methodsCallingWorldGetComp = new List<MethodInfo>();
                methodsCallingGameGetComp = new List<MethodInfo>();
                methodsCallingThingGetComp = new List<MethodInfo>();
                methodsCallingThingTryGetComp = new List<MethodInfo>();
                methodsCallingHediffTryGetComp = new List<MethodInfo>();
                methodsCallingWorldObjectGetComp = new List<MethodInfo>();
                parse = true;
            }
            DoPatchesAsync(parse);
        }
        public async void DoPatchesAsync(bool parse)
        {
            if (parse)
            {
                var methodsToParse = new HashSet<MethodInfo>();
                await Task.Run(() =>
                {
                    try
                    {
                        var types = GetTypesToParse();
                        foreach (var type in types)
                        {
                            foreach (var method in GetMethodsToParse(type))
                            {
                                methodsToParse.Add(method);
                            }
                        }
                        foreach (var method in methodsToParse)
                        {
                            ParseMethod(method, methodsCallingMapGetComp, methodsCallingWorldGetComp, methodsCallingGameGetComp, methodsCallingThingGetComp,
                                methodsCallingThingTryGetComp, methodsCallingHediffTryGetComp, methodsCallingWorldObjectGetComp);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Exception in Performance Optimizer: " + ex);
                    }
                });
            }

            foreach (var method in methodsCallingGameGetComp)
            {
                Patch(method, transpiler: GetMethod(nameof(Optimization_FasterGetCompReplacement.GetGameCompTranspiler)));
            }
            foreach (var method in methodsCallingWorldGetComp)
            {
                Patch(method, transpiler: GetMethod(nameof(Optimization_FasterGetCompReplacement.GetWorldCompTranspiler)));
            }
            foreach (var method in methodsCallingWorldObjectGetComp)
            {
                Patch(method, transpiler: GetMethod(nameof(Optimization_FasterGetCompReplacement.GetWorldObjectCompTranspiler)));
            }
            foreach (var method in methodsCallingMapGetComp)
            {
                Patch(method, transpiler: GetMethod(nameof(Optimization_FasterGetCompReplacement.GetMapCompTranspiler)));
            }
            foreach (var method in methodsCallingThingGetComp)
            {
                Patch(method, transpiler: GetMethod(nameof(Optimization_FasterGetCompReplacement.GetThingCompTranspiler)));
            }
            foreach (var method in methodsCallingThingTryGetComp)
            {
                Patch(method, transpiler: GetMethod(nameof(Optimization_FasterGetCompReplacement.TryGetThingCompTranspiler)));
            }
            foreach (var method in methodsCallingHediffTryGetComp)
            {
                Patch(method, transpiler: GetMethod(nameof(Optimization_FasterGetCompReplacement.TryGetHediffCompTranspiler)));
            }
        }

        public static bool CallsComponent(CodeInstruction codeInstruction, OpCode opcode, string methodName, Type baseCompType, int parameterLength, out Type curType)
        {
            if (codeInstruction.opcode == opcode && codeInstruction.operand is MethodInfo mi && mi.Name == methodName)
            {
                if (mi.IsGenericMethod && mi.GetParameters().Length == parameterLength)
                {
                    curType = mi.GetUnderlyingType();
                    if (baseCompType.IsAssignableFrom(curType))
                    {
                        return true;
                    }
                }
            }
            curType = null;
            return false;
        }

        private static IEnumerable<CodeInstruction> PerformTranspiler(string methodName, Type baseType, MethodInfo genericMethod, OpCode opcode, int parameterLength,
            IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            for (var i = 0; i < codes.Count; i++)
            {
                var instr = codes[i];
                if (CallsComponent(instr, opcode, methodName, baseType, parameterLength, out Type type))
                {
                    var methodToReplace = genericMethod.MakeGenericMethod(new Type[] { type });
                    instr.opcode = OpCodes.Call;
                    instr.operand = methodToReplace;
                }
                yield return instr;
            }
        }

        private static List<MethodInfo> clearMethods;
        public override void Clear()
        {
            if (clearMethods is null)
            {
                clearMethods = GetClearMethods();
            }
            foreach (var method in clearMethods)
            {
                method.Invoke(null, null);
            }
        }

        private static List<MethodInfo> GetClearMethods()
        {
            clearMethods = new List<MethodInfo>();
            foreach (var type in typeof(ThingComp).AllSubclasses())
            {
                try
                {
                    clearMethods.Add(typeof(ICache_ThingComp<>).MakeGenericType(type).GetMethod("Clear", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
                }
                catch (Exception ex)
                {
                }
            }
            foreach (var type in typeof(HediffComp).AllSubclasses())
            {
                try
                {
                    clearMethods.Add(typeof(ICache_HediffComp<>).MakeGenericType(type).GetMethod("Clear", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
                }
                catch (Exception ex)
                {
                }
            }

            foreach (var type in typeof(WorldObjectComp).AllSubclasses())
            {
                try
                {
                    clearMethods.Add(typeof(ICache_WorldObjectComp<>).MakeGenericType(type).GetMethod("Clear", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
                }
                catch (Exception ex)
                {
                }
            }

            foreach (var type in typeof(GameComponent).AllSubclasses())
            {
                try
                {
                    clearMethods.Add(typeof(ICache_GameComponent<>).MakeGenericType(type).GetMethod("Clear", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
                }
                catch (Exception ex)
                {
                }
            }

            foreach (var type in typeof(WorldComponent).AllSubclasses())
            {
                try
                {
                    clearMethods.Add(typeof(ICache_WorldComponent<>).MakeGenericType(type).GetMethod("Clear", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
                }
                catch (Exception ex)
                {
                }
            }
            foreach (var type in typeof(MapComponent).AllSubclasses())
            {
                try
                {
                    clearMethods.Add(typeof(ICache_MapComponent<>).MakeGenericType(type).GetMethod("Clear", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
                }
                catch (Exception ex)
                {
                }
            }
            return clearMethods;
        }
    }

    [StaticConstructorOnStartup]
    public static class ComponentCache
    {
        private static Stopwatch dictStopwatch = new Stopwatch();
        public static class ICache_ThingComp<T> where T : ThingComp
        {
            public static Dictionary<int, T> compsById = new Dictionary<int, T>();
            public static void Clear()
            {
                compsById.Clear();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetThingCompFast<T>(this ThingWithComps thingWithComps) where T : ThingComp
        {
            if (ICache_ThingComp<T>.compsById.TryGetValue(thingWithComps.thingIDNumber, out T val))
            {
                return val;
            }
            if (thingWithComps.comps == null)
            {
                ICache_ThingComp<T>.compsById[thingWithComps.thingIDNumber] = null;
                return null;
            }
            for (int i = 0; i < thingWithComps.comps.Count; i++)
            {
                if (thingWithComps.comps[i].GetType() == typeof(T))
                {
                    var val2 = thingWithComps.comps[i] as T;
                    ICache_ThingComp<T>.compsById[thingWithComps.thingIDNumber] = val2;
                    return val2;
                }
            }

            for (int i = 0; i < thingWithComps.comps.Count; i++)
            {
                T val3 = thingWithComps.comps[i] as T;
                if (val3 != null)
                {
                    ICache_ThingComp<T>.compsById[thingWithComps.thingIDNumber] = val3;
                    return val3;
                }
            }
            ICache_ThingComp<T>.compsById[thingWithComps.thingIDNumber] = null;
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T TryGetThingCompFast<T>(this Thing thing) where T : ThingComp
        {
            ThingWithComps thingWithComps = thing as ThingWithComps;
            if (thingWithComps == null)
            {
                return null;
            }
            return thingWithComps.GetThingCompFast<T>();
        }

        public static class ICache_HediffComp<T> where T : HediffComp
        {
            public static Dictionary<int, T> compsById = new Dictionary<int, T>();
            public static void Clear()
            {
                compsById.Clear();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T TryGetHediffCompFast<T>(this Hediff hd) where T : HediffComp
        {
            if (ICache_HediffComp<T>.compsById.TryGetValue(hd.loadID, out T val))
            {
                return val;
            }
            HediffWithComps hediffWithComps = hd as HediffWithComps;
            if (hediffWithComps == null)
            {
                ICache_HediffComp<T>.compsById[hd.loadID] = null;
                return null;
            }
            //dictStopwatch.Restart();
            if (hediffWithComps.comps == null)
            {
                ICache_HediffComp<T>.compsById[hd.loadID] = null;
                //dictStopwatch.LogTime("Dict approach: ");
                return null;
            }

            for (int i = 0; i < hediffWithComps.comps.Count; i++)
            {
                if (hediffWithComps.comps[i].GetType() == typeof(T))
                {
                    //RegisterComp(thingWithComps.comps[i].GetType());
                    //dictStopwatch.LogTime("Dict approach: ");
                    var val2 = hediffWithComps.comps[i] as T;
                    ICache_HediffComp<T>.compsById[hd.loadID] = val2;
                    return val2;
                }
            }

            for (int i = 0; i < hediffWithComps.comps.Count; i++)
            {
                T val3 = hediffWithComps.comps[i] as T;
                if (val3 != null)
                {
                    ICache_HediffComp<T>.compsById[hd.loadID] = val3;
                    return val3;
                }
            }
            ICache_HediffComp<T>.compsById[hd.loadID] = null;
            return null;
        }
        public static class ICache_WorldObjectComp<T> where T : WorldObjectComp
        {
            public static Dictionary<int, T> compsById = new Dictionary<int, T>();
            public static void Clear()
            {
                compsById.Clear();
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetWorldObjectCompFast<T>(this WorldObject worldObject) where T : WorldObjectComp
        {
            if (ICache_WorldObjectComp<T>.compsById.TryGetValue(worldObject.ID, out T val))
            {
                return val;
            }
            //dictStopwatch.Restart();
            if (worldObject.comps == null)
            {
                ICache_WorldObjectComp<T>.compsById[worldObject.ID] = null;
                //dictStopwatch.LogTime("Dict approach: ");
                return null;
            }
            for (int i = 0; i < worldObject.comps.Count; i++)
            {
                if (worldObject.comps[i].GetType() == typeof(T))
                {
                    //RegisterComp(thingWithComps.comps[i].GetType());
                    //dictStopwatch.LogTime("Dict approach: ");
                    var val2 = worldObject.comps[i] as T;
                    ICache_WorldObjectComp<T>.compsById[worldObject.ID] = val2;
                    return val2;
                }
            }

            for (int i = 0; i < worldObject.comps.Count; i++)
            {
                T val3 = worldObject.comps[i] as T;
                if (val3 != null)
                {
                    ICache_WorldObjectComp<T>.compsById[worldObject.ID] = val3;
                    return val3;
                }
            }
            ICache_WorldObjectComp<T>.compsById[worldObject.ID] = null;
            return null;
        }
        public static class ICache_MapComponent<T>
        {
            public static Dictionary<Map, T> compsByMap = new Dictionary<Map, T>();
            public static void Clear()
            {
                compsByMap.Clear();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetMapComponentFast<T>(this Map map) where T : MapComponent
        {
            if (!ICache_MapComponent<T>.compsByMap.TryGetValue(map, out T mapComp))
            {
                ICache_MapComponent<T>.compsByMap[map] = mapComp = map.GetComponent<T>();
            }
            //Log.Message("Returning map comp: " + mapComp + ", total count of map comps is " + map.components.Count);
            return mapComp;
        }
        public static class ICache_WorldComponent<T> where T : WorldComponent
        {
            public static World world;
            public static T component;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T GetComponent(World curWorld)
            {
                if (world != curWorld)
                {
                    world = curWorld;
                    component = curWorld.GetComponent<T>();
                }
                return component;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Clear()
            {
                world = null;
                component = null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetWorldComponentFast<T>(this World world) where T : WorldComponent
        {
            return ICache_WorldComponent<T>.GetComponent(world);
        }
        public static class ICache_GameComponent<T> where T : GameComponent
        {
            public static Game game;
            public static T component;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static T GetComponent(Game curGame)
            {
                if (game != curGame)
                {
                    game = curGame;
                    component = curGame.GetComponent<T>();
                }
                return component;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Clear()
            {
                game = null;
                component = null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetGameComponentFast<T>(this Game game) where T : GameComponent
        {
            return ICache_GameComponent<T>.GetComponent(game);
        }
    }
}
