﻿// <copyright file="SweepablePipelineExtension.cs" company="BigMiao">
// Copyright (c) BigMiao. All rights reserved.
// </copyright>

using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLNet.AutoPipeline
{
    public static class EstimatorChainExtension
    {
        public static SweepablePipeline
            Append<TLastTrain>(
                this EstimatorChain<TLastTrain> estimatorChain,
                INode transformer)
            where TLastTrain : class, ITransformer
        {
            var estimators = estimatorChain.GetType().GetField("_estimators", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(estimatorChain) as IEstimator<ITransformer>[];
            var scopes = estimatorChain.GetType().GetField("_scopes", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(estimatorChain) as TransformerScope[];

            var singleNodeChain = new SweepablePipeline();
            for (int i = 0; i != estimators.Length; ++i)
            {
                var estimator = Util.CreateUnSweepableNode(estimators[i], scopes[i]);
                singleNodeChain.Append(estimator);
            }

            return singleNodeChain.Append(transformer);
        }

        public static SweepablePipeline
            Append<TLastTran>(
                this TLastTran estimator,
                INode transformer)
            where TLastTran : IEstimator<ITransformer>
        {
            return new SweepablePipeline()
                        .Append(new UnsweepableNode<TLastTran>(estimator))
                        .Append(transformer);
        }
    }
}