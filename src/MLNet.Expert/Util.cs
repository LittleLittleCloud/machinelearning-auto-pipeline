﻿// <copyright file="Util.cs" company="BigMiao">
// Copyright (c) BigMiao. All rights reserved.
// </copyright>

using Microsoft.ML;
using Microsoft.ML.Data;
using MLNet.AutoPipeline;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace MLNet.Expert
{
    internal static class Util
    {
        private static Random rng = new Random();

        public static SweepableEstimator<TNewTrain, TOption> CreateSweepableNode<TNewTrain, TOption>(Func<TOption, TNewTrain> estimatorFactory, SweepableOption<TOption> optionBuilder, TransformerScope scope = TransformerScope.Everything, string estimatorName = null)
            where TNewTrain : IEstimator<ITransformer>
            where TOption : class
        {
            return new SweepableEstimator<TNewTrain, TOption>(estimatorFactory, optionBuilder, scope, estimatorName);
        }

        public static SweepableEstimator<TInstance> CreateUnSweepableNode<TInstance>(TInstance instance, TransformerScope scope = TransformerScope.Everything, string estimatorName = null)
            where TInstance : IEstimator<ITransformer>
        {
            return new SweepableEstimator<TInstance>(instance, scope, estimatorName);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IEnumerable<T> PickN<T>(this IEnumerable<T> list, int n)
        {
            Contract.Requires(n >= 0 && n <= list.Count());
            var pickIndex = Enumerable.Range(0, list.Count()).ToList();
            pickIndex.Shuffle();
            return pickIndex.GetRange(0, n).Select(i => list.ToArray()[i]);
        }
    }
}
