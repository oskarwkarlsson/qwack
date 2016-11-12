﻿using Qwack.Dates;

namespace Qwack.Core.Basic
{
    public class Currency
    {
        public Currency(string ccy)
        {
            Ccy = ccy;
        }

        public string Ccy { get; }
        public DayCountBasis DayCount { get; set; }
        public Calendar SettlementCalendar { get; set; }

        public override bool Equals(object x)
        {
            var x1 = x as Currency;
            return (x1 != null) && (x1.Ccy == Ccy);
        }

        public override int GetHashCode()
        {
            return Ccy.GetHashCode();
        }

        public static bool operator ==(Currency x, Currency y)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)x == null) || ((object)y == null))
            {
                return false;
            }
            return x.Ccy == y.Ccy;
        }

        public static bool operator !=(Currency x, Currency y)
        {
            return !(x == y);
        }
    }
}