﻿namespace XDev.ScriptingTool.ConsolePresentation
{
    using System;
    using DependencyInjection;

    /// <summary>
    /// <see cref="ConsoleBootrapper"/>
    /// </summary>
    internal class ConsoleBootrapper
    {
        /// <summary>
        /// Gets the dependency injection container.
        /// </summary>
        /// <value>The dependency injection container.</value>
        public IDependencyInjectionContainer DependencyInjectionContainer { get; private set; }

        /// <summary>
        /// Bootstraps the console application.
        /// </summary>
        public void Bootstrap()
        {
            this.DependencyInjectionContainer = new UnityDependencyInjectionContainer();

            this.DependencyInjectionContainer.ConfigureScriptingTool();

            this.DependencyInjectionContainer.ConfigureConsoleOutputs();
        }
    }
}