using System;
using System.Collections.Generic;
using System.Text;

namespace CreateInstanceFromType.Tests.TestClasses
{
    public class MultiTwoParamCtor
    {
        public MultiTwoParamCtor(int intValue, string stringValue)
        {
            IntValue = intValue;
            StringValue = $"string: {stringValue}";
        }

        public MultiTwoParamCtor(int intValue, int? nullableIntValue)
        {
            IntValue = intValue;
            StringValue = $"int: {nullableIntValue}";
        }

        public int IntValue { get; }

        public string StringValue { get; }
    }
}
