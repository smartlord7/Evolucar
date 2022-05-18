using System;

namespace Assets.EvolvingCars.TP2
{
    public static class MathUtil
    {
        public static float Gaussian(float f, float mean, float stdDev)
            => (float) Math.Exp(-0.5 * Math.Pow(f - mean, 2) / Math.Pow(stdDev, 2));
    }
}
