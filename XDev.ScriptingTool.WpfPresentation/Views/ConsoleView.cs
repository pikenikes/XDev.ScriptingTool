﻿namespace XDev.ScriptingTool.WpfPresentation.Views
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// <see cref="ConsoleView"/>
    /// </summary>
    /// <seealso cref="IDisposable"/>
    [SuppressUnmanagedCodeSecurity]
    internal class ConsoleView : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleView"/> class.
        /// </summary>
        private ConsoleView()
        {
            ConsoleView.AllocConsole();
            ConsoleView.InvalidateOutAndError();
        }

        /// <summary>
        /// Creates a new console window.
        /// </summary>
        /// <returns></returns>
        public static ConsoleView New() => new ConsoleView();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ConsoleView.SetOutAndErrorNull();
            ConsoleView.FreeConsole();
        }

        /// <summary>
        /// Allocs the console.
        /// </summary>
        [DllImport("Kernel32")]
        private static extern void AllocConsole();

        /// <summary>
        /// Frees the console.
        /// </summary>
        [DllImport("Kernel32")]
        private static extern void FreeConsole();

        /// <summary>
        /// Invalidates the out and error.
        /// </summary>
        private static void InvalidateOutAndError()
        {
            Type type = typeof(Console);

            System.Reflection.FieldInfo _out = type.GetField("_out",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.FieldInfo _error = type.GetField("_error",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.MethodInfo _InitializeStdOutError = type.GetMethod("InitializeStdOutError",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Debug.Assert(_out != null);
            Debug.Assert(_error != null);

            Debug.Assert(_InitializeStdOutError != null);

            _out.SetValue(null, null);
            _error.SetValue(null, null);

            _InitializeStdOutError.Invoke(null, new object[] { true });
        }

        /// <summary>
        /// Sets the out and error null.
        /// </summary>
        private static void SetOutAndErrorNull()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }
}