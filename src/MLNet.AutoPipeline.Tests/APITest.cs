﻿// <copyright file="APITest.cs" company="BigMiao">
// Copyright (c) BigMiao. All rights reserved.
// </copyright>

using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using FluentAssertions;
using Microsoft.ML;
using MLNet.AutoPipeline.API.OptionBuilder;
using MLNet.Sweeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace MLNet.AutoPipeline.Test
{
    public class APITest : TestBase
    {
        public APITest(ITestOutputHelper helper)
            : base(helper)
        {
        }

        [Fact]
        [UseApprovalSubdirectory("ApprovalTests")]
        [UseReporter(typeof(DiffReporter))]
        public void AutoPipeline_should_create_naive_bayes_classifier()
        {
            var context = new MLContext();
            var trainer = context.AutoPipeline().MultiClassification.NaiveBayes("label", "feature");
            Approvals.Verify(trainer.ToCodeGenNodeContract());
        }

        [Fact]
        [UseApprovalSubdirectory("ApprovalTests")]
        [UseReporter(typeof(DiffReporter))]
        public void AutoPipeline_should_create_sdca_maximum_entropy_classifier_with_default_option()
        {
            var context = new MLContext();
            var trainer = context.AutoPipeline().MultiClassification.SdcaMaximumEntropy("label", "feature");
            var parameterValues = SdcaMaximumEntropyOptionBuilder.Default.ValueGenerators.Select(x => x.CreateFromNormalized(0.5));
            var parameterset = new ParameterSet(parameterValues);
            Approvals.Verify(trainer.ToCodeGenNodeContract(parameterset));
        }
    }
}