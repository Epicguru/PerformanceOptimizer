﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using Verse;

namespace PerformanceOptimizer
{
    class PerformanceOptimizerSettings : ModSettings
    {
        public static bool hideResourceReadout = true;
        public static bool hideBottomButtonBar = true;
        public static bool hideBottomRightOverlayButtons = true;
        public static bool minimizeAlertsReadout = true;
        public static bool hideSpeedButtons = true;
        public static bool disableSpeedButtons = false;
        public static bool cacheFindAllowedDesignator = true;

        public static bool fasterGetCompReplacement = true;
        public static bool disableSteamManagerCallbacksChecks = true;
        public static bool disablePlantSwayShaderUpdateIfSwayDisabled = true;
        public static bool disableSoundsCompletely = false;

        public static bool disableReportProbablyMissingAttributes = true;
        public static bool disableLogHarmonyPatchIssueErrors = true;
        public static bool cacheCustomDataLoadMethodOf = true;
        public static bool cacheHasGenericDefinition = true;
        public static bool fixCheckForDuplicateNodes = true;

        public static int IsQuestLodgerRefreshRate = 30;
        public static int IsTeetotalerRefreshRate = 500;
        public static int CurrentExpectationForPawnRefreshRate = 1000;
        public static int CurrentExpectationForMapRefreshRate = 1000;
        public static int TotalMoodOffsetRefreshRate = 500;
        public static int GetThoughtGroupsInDisplayOrderRefreshRate = 1;
        public static int BreakThresholdMinorRefreshRate = 300;
        public static int BreakThresholdMajorRefreshRate = 300;
        public static int BreakThresholdExtremeRefreshRate = 300;
        public static int AmbientTemperatureRefreshRate = 120;
        public static int FindAllowedDesignatorRefreshRate = 120;
        public static int CurrentInstantBeautyRefreshRate = 600;
        public static int GetStyleDominanceRefreshRate = 2000;
        public static int CheckCurrentToilEndOrFailThrottleRate = 10;

        public static bool IsQuestLodgerCacheActive = true;
        public static bool IsTeetotalerCacheActive = true;
        public static bool CurrentExpectationForPawnCacheActive = true;
        public static bool CurrentExpectationForMapCacheActive = true;
        public static bool TotalMoodOffsetCacheActive = true;
        public static bool GetThoughtGroupsInDisplayOrderCacheActive = true;
        public static bool BreakThresholdMinorCacheActive = true;
        public static bool BreakThresholdMajorCacheActive = true;
        public static bool BreakThresholdExtremeCacheActive = true;
        public static bool AmbientTemperatureCacheActive = true;
        public static bool CurrentInstantBeautyCacheActive = true;
        public static bool GetStyleDominanceCacheActive = true;
        public static bool FindAllowedDesignatorCacheActive = true;
        public static bool CheckCurrentToilEndOrFailThrottleActive = true;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref hideResourceReadout, "hideResourceReadout", true);
            Scribe_Values.Look(ref hideBottomButtonBar, "hideBottomButtonBar", true);
            Scribe_Values.Look(ref hideBottomRightOverlayButtons, "hideBottomRightOverlayButtons", true);
            Scribe_Values.Look(ref minimizeAlertsReadout, "minimizeAlertsReadout", true);
            Scribe_Values.Look(ref hideSpeedButtons, "hideSpeedButtons", true);
            Scribe_Values.Look(ref disableSpeedButtons, "disableSpeedButtons", false);
            Scribe_Values.Look(ref cacheFindAllowedDesignator, "cacheFindAllowedDesignator", true);
            Scribe_Values.Look(ref fasterGetCompReplacement, "fasterGetCompReplacement", true);
            Scribe_Values.Look(ref disableSteamManagerCallbacksChecks, "disableSteamManagerCallbacksChecks", true);
            Scribe_Values.Look(ref disablePlantSwayShaderUpdateIfSwayDisabled, "disablePlantSwayShaderUpdateIfSwayDisabled", true);
            Scribe_Values.Look(ref disableSoundsCompletely, "disableSoundsCompletely", false);
            Scribe_Values.Look(ref disableReportProbablyMissingAttributes, "disableReportProbablyMissingAttributes", true);
            Scribe_Values.Look(ref disableLogHarmonyPatchIssueErrors, "disableLogHarmonyPatchIssueErrors", true);
            Scribe_Values.Look(ref cacheCustomDataLoadMethodOf, "cacheCustomDataLoadMethodOf", true);
            Scribe_Values.Look(ref cacheHasGenericDefinition, "cacheHasGenericDefinition", true);
            Scribe_Values.Look(ref fixCheckForDuplicateNodes, "fixCheckForDuplicateNodes", true);
            Scribe_Values.Look(ref IsQuestLodgerRefreshRate, "IsQuestLodgerRefreshRate", 30);
            Scribe_Values.Look(ref IsTeetotalerRefreshRate, "IsTeetotalerRefreshRate", 500);
            Scribe_Values.Look(ref CurrentExpectationForPawnRefreshRate, "CurrentExpectationForPawnRefreshRate", 1000);
            Scribe_Values.Look(ref CurrentExpectationForMapRefreshRate, "CurrentExpectationForMapRefreshRate", 1000);
            Scribe_Values.Look(ref TotalMoodOffsetRefreshRate, "TotalMoodOffsetRefreshRate", 500);
            Scribe_Values.Look(ref GetThoughtGroupsInDisplayOrderRefreshRate, "GetThoughtGroupsInDisplayOrderRefreshRate", 1);
            Scribe_Values.Look(ref BreakThresholdMinorRefreshRate, "BreakThresholdMinorRefreshRate", 300);
            Scribe_Values.Look(ref BreakThresholdMajorRefreshRate, "BreakThresholdMajorRefreshRate", 300);
            Scribe_Values.Look(ref BreakThresholdExtremeRefreshRate, "BreakThresholdExtremeRefreshRate", 300);
            Scribe_Values.Look(ref AmbientTemperatureRefreshRate, "AmbientTemperatureRefreshRate", 120);
            Scribe_Values.Look(ref FindAllowedDesignatorRefreshRate, "FindAllowedDesignatorRefreshRate", 120);
            Scribe_Values.Look(ref CurrentInstantBeautyRefreshRate, "CurrentInstantBeautyRefreshRate", 600);
            Scribe_Values.Look(ref GetStyleDominanceRefreshRate, "GetStyleDominanceRefreshRate", 2000);
            Scribe_Values.Look(ref CheckCurrentToilEndOrFailThrottleRate, "CheckCurrentToilEndOrFailThrottleRate", 10);
            Scribe_Values.Look(ref IsQuestLodgerCacheActive, "IsQuestLodgerCacheActive", true);
            Scribe_Values.Look(ref IsTeetotalerCacheActive, "IsTeetotalerCacheActive", true);
            Scribe_Values.Look(ref CurrentExpectationForPawnCacheActive, "CurrentExpectationForPawnCacheActive", true);
            Scribe_Values.Look(ref CurrentExpectationForMapCacheActive, "CurrentExpectationForMapCacheActive", true);
            Scribe_Values.Look(ref TotalMoodOffsetCacheActive, "TotalMoodOffsetCacheActive", true);
            Scribe_Values.Look(ref GetThoughtGroupsInDisplayOrderCacheActive, "GetThoughtGroupsInDisplayOrderCacheActive", true);
            Scribe_Values.Look(ref BreakThresholdMinorCacheActive, "BreakThresholdMinorCacheActive", true);
            Scribe_Values.Look(ref BreakThresholdMajorCacheActive, "BreakThresholdMajorCacheActive", true);
            Scribe_Values.Look(ref BreakThresholdExtremeCacheActive, "BreakThresholdExtremeCacheActive", true);
            Scribe_Values.Look(ref AmbientTemperatureCacheActive, "AmbientTemperatureCacheActive", true);
            Scribe_Values.Look(ref CurrentInstantBeautyCacheActive, "CurrentInstantBeautyCacheActive", true);
            Scribe_Values.Look(ref GetStyleDominanceCacheActive, "GetStyleDominanceCacheActive", true);
            Scribe_Values.Look(ref FindAllowedDesignatorCacheActive, "FindAllowedDesignatorCacheActive", true);
            Scribe_Values.Look(ref CheckCurrentToilEndOrFailThrottleActive, "CheckCurrentToilEndOrFailThrottleActive", true);
        }
        public void DoSettingsWindowContents(Rect inRect)
        {
            var totalHeight = 620;
            Rect rect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height - 20);
            Rect rect2 = new Rect(0f, 0f, inRect.width - 30f, totalHeight);
            Widgets.BeginScrollView(rect, ref scrollPosition, rect2, true);

            var cacheSettingsHeight = (14 * 24) + 8 + 30;
            Listing_Standard cacheSection = new Listing_Standard();
            Rect topRect = new Rect(inRect.x, inRect.y - 30, inRect.width - 30, cacheSettingsHeight);
            cacheSection.Begin(topRect);
            var cacheSettings = cacheSection.BeginSection(cacheSettingsHeight - 20, 10, 10);
            if (cacheSettings.ButtonTextLabeled("PO.CacheSettings".Translate(), "Reset".Translate()))
            {
                IsQuestLodgerRefreshRate = 30;
                IsTeetotalerRefreshRate = 500;
                CurrentExpectationForPawnRefreshRate = 1000;
                CurrentExpectationForMapRefreshRate = 1000;
                TotalMoodOffsetRefreshRate = 500;
                GetThoughtGroupsInDisplayOrderRefreshRate = 1;
                BreakThresholdMinorRefreshRate = 300;
                BreakThresholdMajorRefreshRate = 300;
                BreakThresholdExtremeRefreshRate = 300;
                AmbientTemperatureRefreshRate = 120;
                FindAllowedDesignatorRefreshRate = 120;
                CurrentInstantBeautyRefreshRate = 600;
                GetStyleDominanceRefreshRate = 2000;
                CheckCurrentToilEndOrFailThrottleRate = 10;

                IsQuestLodgerCacheActive = true;
                IsTeetotalerCacheActive = true;
                CurrentExpectationForPawnCacheActive = true;
                CurrentExpectationForMapCacheActive = true;
                TotalMoodOffsetCacheActive = true;
                GetThoughtGroupsInDisplayOrderCacheActive = true;
                BreakThresholdMinorCacheActive = true;
                BreakThresholdMajorCacheActive = true;
                BreakThresholdExtremeCacheActive = true;
                AmbientTemperatureCacheActive = true;
                CurrentInstantBeautyCacheActive = true;
                GetStyleDominanceCacheActive = true;
                FindAllowedDesignatorCacheActive = true;
                CheckCurrentToilEndOrFailThrottleActive = true;
            }

            cacheSettings.GapLine(8);
            cacheSettings.CheckboxLabeledWithSlider("PO.QuestLodger".Translate(), "PO.RefreshRate", ref IsQuestLodgerCacheActive, ref IsQuestLodgerRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.IsTeetotaler".Translate(), "PO.RefreshRate", ref IsTeetotalerCacheActive, ref IsTeetotalerRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.CurrentExpectationForPawn".Translate(), "PO.RefreshRate", ref CurrentExpectationForPawnCacheActive, ref CurrentExpectationForPawnRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.CurrentExpectationForMap".Translate(), "PO.RefreshRate", ref CurrentExpectationForMapCacheActive, ref CurrentExpectationForMapRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.TotalMoodOffset".Translate(), "PO.RefreshRate", ref TotalMoodOffsetCacheActive, ref TotalMoodOffsetRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.BreakThresholdMinor".Translate(), "PO.RefreshRate", ref BreakThresholdMinorCacheActive, ref BreakThresholdMinorRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.BreakThresholdMajor".Translate(), "PO.RefreshRate", ref BreakThresholdMajorCacheActive, ref BreakThresholdMajorRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.BreakThresholdExtreme".Translate(), "PO.RefreshRate", ref BreakThresholdExtremeCacheActive, ref BreakThresholdExtremeRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.AmbientTemperature".Translate(), "PO.RefreshRate", ref AmbientTemperatureCacheActive, ref AmbientTemperatureRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.CurrentInstantBeauty".Translate(), "PO.RefreshRate", ref CurrentInstantBeautyCacheActive, ref CurrentInstantBeautyRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.GetStyleDominance".Translate(), "PO.RefreshRate", ref GetStyleDominanceCacheActive, ref GetStyleDominanceRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.FindAllowedDesignator".Translate(), "PO.RefreshRate", ref FindAllowedDesignatorCacheActive, ref FindAllowedDesignatorRefreshRate);
            cacheSettings.CheckboxLabeledWithSlider("PO.CheckCurrentToilEndOrFail".Translate(), "PO.ThrottleRate", ref CheckCurrentToilEndOrFailThrottleActive, ref CheckCurrentToilEndOrFailThrottleRate);

            cacheSection.EndSection(cacheSettings);
            cacheSection.End();

            var sectionHeightSize = (7 * 24) + 8 + 10;
            var sectionWidth = ((inRect.width - 30) / 2f) - 8;
            Rect uiSettingsRect = new Rect(inRect.x, topRect.yMax + 15, sectionWidth, sectionHeightSize + 20);
            Listing_Standard topLeftSection = new Listing_Standard();
            topLeftSection.Begin(uiSettingsRect);
            var uiSection = topLeftSection.BeginSection(sectionHeightSize, 10, 10);
            if (uiSection.ButtonTextLabeled("PO.UISettings".Translate(), "Reset".Translate()))
            {
                hideResourceReadout = true;
                hideBottomButtonBar = true;
                hideBottomRightOverlayButtons = true;
                minimizeAlertsReadout = true;
                hideSpeedButtons = true;
                disableSpeedButtons = false;
            }
            uiSection.GapLine(8);
            uiSection.CheckboxLabeled("PO.HideResourceReadout".Translate(), ref hideResourceReadout);
            uiSection.CheckboxLabeled("PO.HideBottomButtonBar".Translate(), ref hideBottomButtonBar);
            uiSection.CheckboxLabeled("PO.HideBottomRightOverlayButtons".Translate(), ref hideBottomRightOverlayButtons);
            uiSection.CheckboxLabeled("PO.MinimizeAlertsReadout".Translate(), ref minimizeAlertsReadout);
            uiSection.CheckboxLabeled("PO.HideSpeedButtons".Translate(), ref hideSpeedButtons);
            uiSection.CheckboxLabeled("PO.DisableSpeedButtons".Translate(), ref disableSpeedButtons);
            
            topLeftSection.EndSection(uiSection);
            topLeftSection.End();
            
            Listing_Standard miscSettingsSection = new Listing_Standard();
            Rect miscSettingsRect = new Rect(uiSettingsRect.xMax + 15, uiSettingsRect.y, sectionWidth, uiSettingsRect.height);
            miscSettingsSection.Begin(miscSettingsRect);
            var miscSettings = miscSettingsSection.BeginSection(sectionHeightSize, 10, 10);
            if (miscSettings.ButtonTextLabeled("PO.MiscSettings".Translate(), "Reset".Translate()))
            {
                fasterGetCompReplacement = true;
                cacheFindAllowedDesignator = true;
                disableSteamManagerCallbacksChecks = true;
                disablePlantSwayShaderUpdateIfSwayDisabled = true;
                disableSoundsCompletely = false;
            }
            miscSettings.GapLine(8);
            miscSettings.CheckboxLabeled("PO.FasterGetCompReplacement".Translate(), ref fasterGetCompReplacement);
            miscSettings.CheckboxLabeled("PO.DisableSteamManagerCallbacksChecks".Translate(), ref disableSteamManagerCallbacksChecks);
            miscSettings.CheckboxLabeled("PO.DisablePlantSwayShaderUpdateIfSwayDisabled".Translate(), ref disablePlantSwayShaderUpdateIfSwayDisabled);
            miscSettings.CheckboxLabeled("PO.DisableSoundsCompletely".Translate(), ref disableSoundsCompletely);
            miscSettings.CheckboxLabeled("PO.FixCheckForDuplicateNodes".Translate(), ref fixCheckForDuplicateNodes);
            miscSettingsSection.EndSection(miscSettings);
            miscSettingsSection.End();

            //Rect gameLoadSettingsRect = new Rect(inRect.x, uiSettingsRect.yMax + 15, sectionWidth, sectionHeightSize + 20);
            //Listing_Standard bottomLeftSection = new Listing_Standard();
            //bottomLeftSection.Begin(gameLoadSettingsRect);
            //var gameLoadSettings = bottomLeftSection.BeginSection(sectionHeightSize, 10, 10);
            //if (gameLoadSettings.ButtonTextLabeled("PO.GameLoadSettings".Translate(), "Reset".Translate()))
            //{
            //    disableReportProbablyMissingAttributes = true;
            //    disableLogHarmonyPatchIssueErrors = true;
            //    cacheCustomDataLoadMethodOf = true;
            //    cacheHasGenericDefinition = true;
            //    fixCheckForDuplicateNodes = true;
            //}
            //
            //gameLoadSettings.GapLine(8);
            //gameLoadSettings.CheckboxLabeled("PO.DisableReportProbablyMissingAttributes".Translate(), ref disableReportProbablyMissingAttributes);
            //gameLoadSettings.CheckboxLabeled("PO.DisableLogHarmonyPatchIssueErrors".Translate(), ref disableLogHarmonyPatchIssueErrors);
            //gameLoadSettings.CheckboxLabeled("PO.CacheCustomDataLoadMethodOf".Translate(), ref cacheCustomDataLoadMethodOf);
            //gameLoadSettings.CheckboxLabeled("PO.CacheHasGenericDefinition".Translate(), ref cacheHasGenericDefinition);
            //
            //bottomLeftSection.EndSection(gameLoadSettings);
            //bottomLeftSection.End();

            Widgets.EndScrollView();
        }

        private static Vector2 scrollPosition = Vector2.zero;
    }
}
