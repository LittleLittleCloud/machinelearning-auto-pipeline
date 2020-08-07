﻿// <copyright file="LightGbmOptionBuilder.cs" company="BigMiao">
// Copyright (c) BigMiao. All rights reserved.
// </copyright>

using Microsoft.ML.Trainers.LightGbm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MLNet.AutoPipeline
{
    /// <summary>
    /// Sweepable option for <see cref="LightGbmMulticlassTrainer"/>.
    /// </summary>
    public class LightGbmOptionBuilder : OptionBuilder<LightGbmMulticlassTrainer.Options>
    {
        /// <summary>
        /// Learning rate.
        /// <para>Default sweeping configuration.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>type</term>
        /// <description><c>Float</c></description>
        /// </item>
        /// <item>
        /// <term>minimum value</term>
        /// <description><c>1E-3F</c></description>
        /// </item>
        /// <item>
        /// <term>maximum value</term>
        /// <description><c>0.1F</c></description>
        /// </item>
        /// <item>
        /// <term>log base</term>
        /// <description><c>true</c></description>
        /// </item>
        /// <item>
        /// <term>step</term>
        /// <description><c>20</c></description>
        /// </item>
        /// </list>
        /// </summary>
        [SweepableParameter]
        public SweepableParameter LearningRate = SweepableParameter.CreateDoubleParameter(0.001, 0.1, true, 20);

        /// <summary>
        /// The maximum number of leaves in one tree.
        /// <para>Default sweeping configuration.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>type</term>
        /// <description><c>Int</c></description>
        /// </item>
        /// <item>
        /// <term>minimum value</term>
        /// <description><c>10</c></description>
        /// </item>
        /// <item>
        /// <term>maximum value</term>
        /// <description><c>1000</c></description>
        /// </item>
        /// <item>
        /// <term>log base</term>
        /// <description><c>true</c></description>
        /// </item>
        /// <item>
        /// <term>step</term>
        /// <description><c>20</c></description>
        /// </item>
        /// </list>
        /// </summary>
        [SweepableParameter]
        public SweepableParameter NumberOfLeaves = SweepableParameter.CreateInt32Parameter(10, 1000, true, 20);

        /// <summary>
        /// The number of boosting iterations. A new tree is created in each iteration, so this is equivalent to the number of trees.
        /// <para>Default sweeping configuration.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>type</term>
        /// <description><c>Int</c></description>
        /// </item>
        /// <item>
        /// <term>minimum value</term>
        /// <description><c>10</c></description>
        /// </item>
        /// <item>
        /// <term>maximum value</term>
        /// <description><c>1000</c></description>
        /// </item>
        /// <item>
        /// <term>log base</term>
        /// <description><c>true</c></description>
        /// </item>
        /// <item>
        /// <term>step</term>
        /// <description><c>20</c></description>
        /// </item>
        /// </list>
        /// </summary>
        [SweepableParameter]
        public SweepableParameter NumberOfIterations = SweepableParameter.CreateInt32Parameter(10, 1000, true, 20);

        /// <summary>
        /// The minimal number of data points required to form a new tree leaf.
        /// <para>Default sweeping configuration.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>type</term>
        /// <description><c>Int</c></description>
        /// </item>
        /// <item>
        /// <term>minimum value</term>
        /// <description><c>10</c></description>
        /// </item>
        /// <item>
        /// <term>maximum value</term>
        /// <description><c>1000</c></description>
        /// </item>
        /// <item>
        /// <term>log base</term>
        /// <description><c>true</c></description>
        /// </item>
        /// <item>
        /// <term>step</term>
        /// <description><c>20</c></description>
        /// </item>
        /// </list>
        /// </summary>
        [SweepableParameter]
        public SweepableParameter MinimumExampleCountPerLeaf = SweepableParameter.CreateInt32Parameter(10, 1000, true, 20);

        /// <summary>
        /// The name of the example weight column.
        /// </summary>
        [Parameter]
        public string ExampleWeightColumnName = default;

        internal static LightGbmOptionBuilder Default = new LightGbmOptionBuilder();
    }
}
