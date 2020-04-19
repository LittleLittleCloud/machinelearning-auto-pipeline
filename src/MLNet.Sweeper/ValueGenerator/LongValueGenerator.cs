﻿// <copyright file="LongValueGenerator.cs" company="BigMiao">
// Copyright (c) BigMiao. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace MLNet.Sweeper
{
    /// <summary>
    /// The integer type parameter sweep.
    /// </summary>
    public class LongValueGenerator : INumericValueGenerator
    {
        private readonly Option _options;

        public string Name => this._options.Name;

        public LongValueGenerator(Option options)
        {
            this._options = options;
        }

        // REVIEW: Is float accurate enough?
        public IParameterValue CreateFromNormalized(double normalizedValue)
        {
            var val = Utils.AXPlusB(this._options.Min, this._options.Max, normalizedValue, this._options.LogBase);

            return new LongParameterValue(this._options.Name, Convert.ToInt64(val));
        }

        public IParameterValue this[int i]
        {
            get
            {
                return this.CreateFromNormalized(i * 1.0 / this._options.Steps);
            }
        }

        public int Count
        {
            get
            {
                return this._options.Steps + 1;
            }
        }

        public float NormalizeValue(IParameterValue value)
        {
            var valueTyped = value as LongParameterValue;

            if (this._options.LogBase)
            {
                return (float)((Math.Log(valueTyped.Value) - Math.Log(this._options.Min)) / (Math.Log(this._options.Max) - Math.Log(this._options.Min)));
            }
            else
            {
                return (float)(valueTyped.Value - this._options.Min) / (this._options.Max - this._options.Min);
            }
        }

        public bool InRange(IParameterValue value)
        {
            var valueTyped = value as LongParameterValue;
            return this._options.Min <= valueTyped.Value && valueTyped.Value <= this._options.Max;
        }

        public class Option : NumericValueGeneratorOptionBase
        {
            public long Min;

            public long Max;
        }
    }
}
