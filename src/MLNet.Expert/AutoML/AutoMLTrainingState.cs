﻿// <copyright file="AutoMLTrainingState.cs" company="BigMiao">
// Copyright (c) BigMiao. All rights reserved.
// </copyright>

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using MLNet.AutoPipeline;
using MLNet.AutoPipeline.Experiment;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace MLNet.Expert.AutoML
{
    public class AutoMLTrainingState
    {
        public AutoMLTrainingState(EstimatorNodeGroup trainers)
        {
            this.Trainers = trainers;
            this.InputOutputColumnPairs = new List<InputOutputColumnPair>();
            this.Transformers = new Dictionary<DataViewSchema.Column, ISweepablePipelineNode>();
        }

        public AutoMLTrainingState(Dictionary<DataViewSchema.Column, ISweepablePipelineNode> transformers, List<InputOutputColumnPair> inputOutputColumnPairs, EstimatorNodeGroup trainers)
        {
            this.Trainers = trainers;
            this.Transformers = transformers;
            this.InputOutputColumnPairs = inputOutputColumnPairs;
        }

        public Dictionary<DataViewSchema.Column, ISweepablePipelineNode> Transformers { get; private set; }

        public List<DataViewSchema.Column> Columns
        {
            get => this.Transformers.Keys.ToList();
        }

        public List<InputOutputColumnPair> InputOutputColumnPairs { get; private set; }

        public EstimatorNodeGroup Trainers { get; private set; }
    }
}