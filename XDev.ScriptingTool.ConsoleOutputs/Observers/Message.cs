﻿namespace XDev.ScriptingTool.ConsoleOutputs.Observers
{
    using System;

    /// <summary>
    /// <see cref="Message"/>
    /// </summary>
    internal class Message
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public ConsoleOutputColor Color { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
    }
}