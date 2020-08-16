﻿// <copyright file="EstimatorNodeChain.cs" company="BigMiao">
// Copyright (c) BigMiao. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;
using MLNet.Sweeper;

namespace MLNet.AutoPipeline
{
    public class EstimatorNodeChain : IEstimatorNode
    {
        private readonly IList<IEstimatorNode> _nodes;

        private ISweeper _sweeper;

        public EstimatorNodeType NodeType => EstimatorNodeType.NodeChain;

        public EstimatorNodeChain(IEstimator<ITransformer>[] estimators, TransformerScope[] scopes)
            : this()
        {
            for (int i = 0; i != estimators.Length; ++i)
            {
                var estimatorWrapper = new UnsweepableNode<IEstimator<ITransformer>>(estimators[i], scopes[i]);
                this.Append(estimatorWrapper);
            }
        }

        public EstimatorNodeChain(IList<IEstimatorNode> nodes)
        {
            this._nodes = nodes;
        }

        public EstimatorNodeChain()
        {
            this._nodes = new List<IEstimatorNode>();
        }

        public EstimatorNodeChain Append<TNewTrans>(TNewTrans estimator, TransformerScope scope = TransformerScope.Everything)
            where TNewTrans : IEstimator<ITransformer>
        {
            var estimatorWrapper = new UnsweepableNode<TNewTrans>(estimator, scope);
            return this.Append(estimatorWrapper);
        }

        public EstimatorNodeChain Append<TNewTrains, TOption>(Func<TOption, TNewTrains> estimatorBuilder, SweepableOption<TOption> optionBuilder, TransformerScope scope = TransformerScope.Everything)
            where TNewTrains : IEstimator<ITransformer>
            where TOption : class
        {
            var autoEstimator = new SweepableNode<TNewTrains, TOption>(estimatorBuilder, optionBuilder, scope);

            return this.Append(autoEstimator);
        }

        public EstimatorNodeChain Append<TNewTrans, TOption>(SweepableNode<TNewTrans, TOption> estimatorBuilder)
            where TNewTrans : IEstimator<ITransformer>
            where TOption : class
        {
            return this.Append(Util.CreateEstimatorSingleNode(estimatorBuilder));
        }

        public EstimatorNodeChain Append<TNewTrans>(UnsweepableNode<TNewTrans> unsweepableNode)
            where TNewTrans : IEstimator<ITransformer>
        {
            return this.Append(Util.CreateEstimatorSingleNode(unsweepableNode));
        }

        public EstimatorNodeChain Append(IEstimatorNode node)
        {
            this._nodes.Add(node);

            return this;
        }

        public EstimatorNodeChain Append(IEnumerable<IEstimatorNode> nodes)
        {
            foreach ( var node in nodes)
            {
                this.Append(node);
            }

            return this;
        }

        public void UseSweeper(ISweeper sweeper)
        {
            this._sweeper = sweeper;
        }

        public IEnumerable<SweepablePipeline> BuildSweepablePipelines()
        {
            if (this._nodes.Count == 0)
            {
                return new List<SweepablePipeline>();
            }

            // TODO: use stack and yield to save memory.
            var paths = new List<SweepablePipeline>();
            foreach (var node in this._nodes)
            {
                var newPath = new List<SweepablePipeline>();

                if (paths.Count == 0)
                {
                    foreach (var _path in node.BuildSweepablePipelines())
                    {
                        paths.Add(_path);
                    }
                }
                else
                {
                    foreach (var _path in node.BuildSweepablePipelines())
                    {
                        foreach (var _existPath in paths)
                        {
                        }
                    }

                    paths = newPath;
                }
            }

            return paths;
        }

        public string Summary()
        {
            return $"NodeChain({string.Join("=>", this._nodes.Select(node => node.Summary()))})";
        }
    }
}
