namespace CreateInstanceFromType.Tests
{
    using System;
    using TestClasses;
    using Xunit;

    public class CreateInstanceFromType2020RuntimeArgsTest
    {
        [Fact]
        public void ShouldUseAParameterlessCtor()
        {
            var instance = (Parameterless)CreateInstanceFromType2020RuntimeArgs
                .GetInstance(typeof(Parameterless));

            Assert.NotNull(instance);
        }

        [Fact]
        public void ShouldUseAParameterlessValueTypeCtor()
        {
            var instance = (Guid)CreateInstanceFromType2020RuntimeArgs
                .GetInstance(typeof(Guid));

            Assert.Equal(default, instance);
        }

        [Fact]
        public void ShouldUseASingleParameterCtor()
        {
            var instance = (OneParamCtor)CreateInstanceFromType2020RuntimeArgs
                .GetInstance(typeof(OneParamCtor), "hello!");

            Assert.NotNull(instance);
            Assert.Equal("hello!", instance.Value);
        }
        
        [Fact]
        public void ShouldUseASingleParameterValueTypeCtor()
        {
            var instance = (Guid)CreateInstanceFromType2020RuntimeArgs
                .GetInstance(typeof(Guid), "5e55498a-86e1-495c-b829-0c5170346ef5");

            Assert.Equal(Guid.Parse("5e55498a-86e1-495c-b829-0c5170346ef5"), instance);
        }

        [Fact]
        public void ShouldUseATwoParameterCtor()
        {
            var instance = (TwoParamCtor)CreateInstanceFromType2020RuntimeArgs
                .GetInstance(typeof(TwoParamCtor), "hello again!", 123);

            Assert.NotNull(instance);
            Assert.Equal("hello again!", instance.StringValue);
            Assert.Equal(123, instance.IntValue);
        }

        [Fact]
        public void ShouldSelectACtorFromArguments()
        {
            var twoParamInstance = (MultiCtor)CreateInstanceFromType2020RuntimeArgs
                .GetInstance(typeof(MultiCtor), "hello there!", 456);

            Assert.NotNull(twoParamInstance);
            Assert.Equal("hello there!", twoParamInstance.StringValue);
            Assert.Equal(456, twoParamInstance.IntValue);
            Assert.Equal(default, twoParamInstance.DateValue);

            var oneParamInstance = (MultiCtor)CreateInstanceFromType2020RuntimeArgs
                .GetInstance(typeof(MultiCtor), "hello you!");

            Assert.NotNull(oneParamInstance);
            Assert.Equal("hello you!", oneParamInstance.StringValue);
            Assert.Equal(default, oneParamInstance.IntValue);
            Assert.Equal(default, twoParamInstance.DateValue);

            var threeParamInstance = (MultiCtor)CreateInstanceFromType2020RuntimeArgs
                .GetInstance(typeof(MultiCtor), "hello blah!", 999, DateTime.MinValue);

            Assert.NotNull(threeParamInstance);
            Assert.Equal("hello blah!", threeParamInstance.StringValue);
            Assert.Equal(999, threeParamInstance.IntValue);
            Assert.Equal(DateTime.MinValue, threeParamInstance.DateValue);
        }

        [Fact]
        public void ShouldHandleANullArgument()
        {
            var threeParamInstance = (MultiCtor)CreateInstanceFromType2020RuntimeArgs
                .GetInstance(typeof(MultiCtor), default(string), 456, DateTime.MaxValue);

            Assert.NotNull(threeParamInstance);
            Assert.Null(threeParamInstance.StringValue);
            Assert.Equal(456, threeParamInstance.IntValue);
            Assert.Equal(DateTime.MaxValue, threeParamInstance.DateValue);
        }

        [Fact]
        public void ShouldErrorIfNoMatchingConstructorExists()
        {
            var ctorNotFoundEx = Assert.Throws<NotSupportedException>(() =>
            {
                CreateInstanceFromType2020RuntimeArgs
                    .GetInstance(typeof(OneParamCtor), DateTime.MinValue, DateTime.MaxValue);
            });

            Assert.Equal("Failed to find a matching constructor", ctorNotFoundEx.Message);
        }

        [Fact]
        public void ShouldErrorIfNullArgumentAndNoMatchingConstructorExists()
        {
            var ctorNotFoundEx = Assert.Throws<NotSupportedException>(() =>
            {
                CreateInstanceFromType2020RuntimeArgs
                    .GetInstance(typeof(MultiTwoParamCtor), DateTime.Now, default(string));
            });

            Assert.Contains("Failed to find a matching constructor", ctorNotFoundEx.Message);
            Assert.Contains("null arguments", ctorNotFoundEx.Message);
        }

        [Fact]
        public void ShouldErrorIfNullArgumentAndMultipleMatchingConstructorsExist()
        {
            var ctorNotFoundEx = Assert.Throws<NotSupportedException>(() =>
            {
                CreateInstanceFromType2020RuntimeArgs
                    .GetInstance(typeof(MultiTwoParamCtor), 123, default(string));
            });

            Assert.Contains("Failed to find a single matching constructor", ctorNotFoundEx.Message);
            Assert.Contains("null arguments", ctorNotFoundEx.Message);
        }
    }
}
