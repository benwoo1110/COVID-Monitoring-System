using System;

namespace COVIDMonitoringSystem.Core.TravelEntryMgr
{
    public class SHNConditionsBuilder
    {
        private static readonly CostCalculator DefaultSwapCalculator = tr => 200;
        
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
        
        public SHNConditionsBuilder WithSwapTest(CostCalculator calculator)
        {
            Conditions.RequireSwapTest = true;
            Conditions.SwapTestCost = calculator;
            return this;
        }

        public SHNConditionsBuilder WithTransport(CostCalculator calculator)
        {
            Conditions.RequireTransport = true;
            Conditions.TransportCost = calculator;
            return this;
        }

        public SHNConditionsBuilder WithDedicatedFacility(CostCalculator calculator)
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