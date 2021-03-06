﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qwack.Math.Interpolation
{
    public class InterpolatorFactory
    {
        public static IInterpolator1D GetInterpolator(double[] x, double[] y, Interpolator1DType kind, bool noCopy = false, bool isSorted = false)
        {
            if (!noCopy)
            {
                var newx = new double[x.Length];
                var newy = new double[y.Length];
                Buffer.BlockCopy(x, 0, newx, 0, x.Length * 8);
                Buffer.BlockCopy(y, 0, newy, 0, y.Length * 8);
                x = newx;
                y = newy;
            }
            if (!isSorted)
            {
                Array.Sort(x, y);
            }
            switch (kind)
            {
                case Interpolator1DType.LinearFlatExtrap:
                    if(x.Length < 200)
                    { 
                        return new LinearInterpolatorFlatExtrapNoBinSearch(x, y);
                    }
                    else
                    {
                        return new LinearInterpolatorFlatExtrap(x, y);
                    }
                default:
                    throw new InvalidOperationException($"We don't have a way of making a {kind} interpolator");
            }
        }
    }
}
