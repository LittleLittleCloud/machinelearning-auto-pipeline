﻿// <copyright file="OptionBuilderTest.cs" company="BigMiao">
// Copyright (c) BigMiao. All rights reserved.
// </copyright>

using FluentAssertions;
using Microsoft.ML;
using MLNet.Sweeper;
using System.Collections.Generic;
using Xunit;

namespace MLNet.AutoPipeline.Test
{
    public class OptionBuilderTest
    {
        [Fact]
        public void OptionBuilder_should_create_default_option()
        {
            var builder = new TestOptionBuilderWithSweepableAttributeOnly();
            var option = builder.CreateDefaultOption();
            option.LongOption.Should().Equals(10);
            option.FloatOption.Should().Equals(1f);
            option.StringOption.Should().Equals("str");
        }

        [Fact]
        public void OptionBuilder_should_build_option_from_parameter_set()
        {
            var builder = new TestOptionBuilderWithSweepableAttributeOnly();
            var input = new List<IParameterValue>()
            {
                new LongParameterValue("LongOption", 2),
                new FloatParameterValue("FloatOption", 2f),
                new DiscreteParameterValue("StringOption", "2"),
            };

            var paramSet = new ParameterSet(input);

            var option = builder.BuildOption(paramSet);
            option.LongOption.Should().Equals(2);
            option.FloatOption.Should().Equals(2f);
            option.StringOption.Should().Equals("2");
        }

        [Fact]
        public void OptionBuilder_should_work_with_random_sweeper()
        {
            var context = new MLContext();
            var builder = new TestOptionBuilderWithSweepableAttributeOnly();
            var maximum = 10;
            var sweeperOption = new UniformRandomSweeper.Option();

            var randomSweeper = new UniformRandomSweeper(sweeperOption);
            randomSweeper.SweepableParamaters = builder.ValueGenerators;

            foreach (var sweeperOutput in randomSweeper.ProposeSweeps(maximum))
            {
                maximum -= 1;
                var option = builder.BuildOption(sweeperOutput);
                option.LongOption
                      .Should()
                      .BeLessOrEqualTo(100)
                      .And
                      .BeGreaterOrEqualTo(0);

                option.FloatOption
                      .Should()
                      .BeLessOrEqualTo(100f)
                      .And
                      .BeGreaterOrEqualTo(0f);

                option.StringOption
                      .Should()
                      .BeOneOf(new string[] { "str1", "str2", "str3", "str4" });

                maximum.Should().BeGreaterThan(-2);
            }
        }

        [Fact]
        public void OptionBuilder_should_build_option_using_field_with_parameter_attribute()
        {
            var optionBuilder = new TestOptionBuilderWithParameterAttributeOnly();
            var option1 = optionBuilder.CreateDefaultOption();
            option1.FloatOption.Should().Be(100f);
            option1.LongOption.Should().Be(100L);
            option1.StringOption.Should().Be(string.Empty);
        }

        [Fact]
        public void OptionBuilder_should_build_option_using_field_with_sweepable_parameter_attribute()
        {
            var optionBuilder = new TestOptionBuilderWithSweepableAttributeOnly();
            var option1 = optionBuilder.CreateDefaultOption();
            option1.FloatOption.Should().Be(0f);
            option1.LongOption.Should().Be(0);
            option1.StringOption.Should().Be("str1");

            var input = new List<IParameterValue>()
            {
                new LongParameterValue("LongOption", 2),
                new FloatParameterValue("FloatOption", 2f),
                new DiscreteParameterValue("StringOption", "2"),
            };

            var parameterSet = new ParameterSet(input);
            var option2 = optionBuilder.BuildOption(parameterSet);

            option2.LongOption.Should().Equals(2);
            option2.FloatOption.Should().Equals(2f);
            option2.StringOption.Should().Equals("2");
        }

        private class TestOption
        {
            public long LongOption = 1;

            public float FloatOption = 1f;

            public string StringOption = string.Empty;
        }

        private class TestOptionBuilderWithParameterAttributeOnly : OptionBuilder<TestOption>
        {
            [Parameter]
            public long LongOption = 100;

            [Parameter(nameof(TestOption.FloatOption))]
            public float Float_Option = 100f;
        }

        private class TestOptionBuilderWithSweepableAttributeOnly : OptionBuilder<TestOption>
        {
            [SweepableParameter]
            public SweepableParameter LongOption = SweepableParameter.CreateLongParameter(0, 100);

            [SweepableParameter(nameof(TestOption.FloatOption))]
            public SweepableParameter Float_Option = SweepableParameter.CreateFloatParameter(0f, 100f);

            [SweepableParameter]
            public SweepableParameter StringOption = SweepableParameter.CreateDiscreteParameter(new object[] { "str1", "str2", "str3", "str4" });
        }
    }
}
