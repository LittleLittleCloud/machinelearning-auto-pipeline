﻿// <copyright file="MLContextExtension.cs" company="BigMiao">
// Copyright (c) BigMiao. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Runtime;

namespace MLNet.AutoPipeline
{
    /// <summary>
    /// Class containing AutoPipeline extension methods to <see cref="MLContext"/>.
    /// </summary>
    public static class MLContextExtension
    {
        /// <summary>
        /// Extension method for creating <see cref="AutoPipelineCatalog"/>.
        /// </summary>
        /// <param name="context">ML Context.</param>
        /// <returns><see cref="AutoPipelineCatalog"/>.</returns>
        public static AutoPipelineCatalog AutoML(this MLContext context)
        {
            Logger.Instance.Channel = (context as IChannelProvider).Start("AutoPipeline");
            return new AutoPipelineCatalog(context);
        }
    }
}
