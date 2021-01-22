using System;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class SHNConditionsBuilder
    {
        private static readonly Func<TravelEntry, double> DefaultSwapCalculator = tr => 200;
        
        private SHNConditions Conditions { get; set; }
        private TravelEntryMatcher Matcher { get; set; }

        public SHNConditionsBuilder()
        {
            Conditions = new SHNConditions();
        }

        public SHNConditionsBuilder WithQuarantineDays(int days)
        {
            Conditions.QuarantineDays = days;
            return this;
        }

        public SHNConditionsBuilder WithSwapTest()
        {
            return WithSwapTest(DefaultSwapCalculator);
        }
        
        public SHNConditionsBuilder WithSwapTest(Func<TravelEntry, double> calculator)
        {
            Conditions.RequireSwapTest = true;
            Conditions.SwapTestCost = calculator;
            return this;
        }

        public SHNConditionsBuilder WithTransport(Func<TravelEntry, double> calculator)
        {
            Conditions.RequireTransport = true;
            Conditions.TransportCost = calculator;
            return this;
        }

        public SHNConditionsBuilder WithDedicatedFacility(Func<TravelEntry, double> calculator)
        {
            Conditions.RequireDedicatedFacility = true;
            Conditions.DedicatedFacilityCost = calculator;
            return this;
        }

        public SHNConditionsBuilder WithMatcher(TravelEntryMatcher matcher)
        {
            Matcher = matcher;
            return this;
        }

        public SHNConditions Build()
        {
            ValidateCompleteness();
            SHNConditions.RegisterChargeCalculator(Matcher, Conditions);
            return Conditions;
        }

        private void ValidateCompleteness()
        {
            if (Conditions.QuarantineDays == -1 || Matcher == null)
            {
                throw new InvalidOperationException("SHNConditionsBuilder is incomplete!");
            }
        }
    }
}