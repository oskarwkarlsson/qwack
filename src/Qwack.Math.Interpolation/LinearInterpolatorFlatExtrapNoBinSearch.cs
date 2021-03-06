﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Math;

namespace Qwack.Math.Interpolation
{
    public class LinearInterpolatorFlatExtrapNoBinSearch : IInterpolator1D
    {
        const double xBump = 1e-10;

        private double[] _x;
        private double[] _y;
        private double[] _slope;
        private double _minX;
        private double _maxX;

        public LinearInterpolatorFlatExtrapNoBinSearch(double[] x, double[] y)
        {
            _x = x;
            _y = y;
            _minX = _x[0];
            _maxX = _x[x.Length - 1];
            CalculateSlope();
        }
                
        public LinearInterpolatorFlatExtrapNoBinSearch()
        { }

        private LinearInterpolatorFlatExtrapNoBinSearch(double[] x, double[] y, double[] slope)
        {
            _x = x;
            _y = y;
            _slope = slope;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindFloorPoint(double t)
        {
            var x = _x;
            for(int i = 1; i < x.Length;i++)
            {
                if(x[i] >= t)
                {
                    return i-1;
                }
            }
            throw new NotImplementedException();
        }

        private void CalculateSlope()
        {
            _slope = new double[_x.Length - 1];
            for (int i = 0; i < _slope.Length; i++)
            {
                _slope[i] = (_y[i + 1] - _y[i]) / (_x[i + 1] - _x[i]);
            }
        }

        public IInterpolator1D Bump(int pillar, double delta, bool updateInPlace = false)
        {
            var newY = _y[pillar] + delta;
            return UpdateY(pillar, newY, updateInPlace);
        }

        public double FirstDerivative(double x)
        {
            double x1 = Interpolate(x);
            double x2 = Interpolate(x + xBump);
            double d1 = (x2 - x1) / xBump;
            return d1;
        }

        public double Interpolate(double t)
        {
            if (t <= _minX)
            {
                return _y[0];
            }
            else if (t >= _maxX)
            {
                return _y[_y.Length - 1];
            }
            else
            {
                int k = FindFloorPoint(t);
                return _y[k] + (t - _x[k]) * _slope[k];
            }
        }

        public double SecondDerivative(double x)
        {
            double x1 = FirstDerivative(x);
            double x2 = FirstDerivative(x + xBump);
            double d2 = (x2 - x1) / xBump;
            return d2;
        }

        public IInterpolator1D UpdateY(int pillar, double newValue, bool updateInPlace = false)
        {
            if (updateInPlace)
            {
                var y = _y;
                var x = _x;
                y[pillar] = newValue;
                if (pillar < _slope.Length)
                {
                    _slope[pillar] = (y[pillar + 1] - y[pillar]) / (x[pillar + 1] - x[pillar]);
                }
                if (pillar != 0)
                {
                    pillar -= 1;
                    _slope[pillar] = (y[pillar + 1] - y[pillar]) / (x[pillar + 1] - x[pillar]);
                }
                return this;
            }
            else
            {
                var newY = new double[_y.Length];
                Buffer.BlockCopy(_y, 0, newY, 0, _y.Length * 8);
                var newSlope = new double[_slope.Length];
                Buffer.BlockCopy(_slope, 0, newSlope, 0, _slope.Length * 8);
                var returnValue = new LinearInterpolatorFlatExtrapNoBinSearch(_x, newY, newSlope).Bump(pillar, newValue, true);
                return returnValue;
            }
        }
    }
}
