﻿using Verse;

namespace PerformanceOptimizer
{
    public abstract class Optimization_RefreshRate : Optimization
    {
        public override void Reset()
        {
            base.Reset();
            refreshRateStatic = refreshRate = RefreshRateByDefault;
        }
        public virtual int RefreshRateByDefault => 0;

        public int refreshRate;

        public static int refreshRateStatic;
        public void SetRefreshRate()
        {
            refreshRateStatic = refreshRate;
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref refreshRate, "refreshRate");
        }
    }
}
