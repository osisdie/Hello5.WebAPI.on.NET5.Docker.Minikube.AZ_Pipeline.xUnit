using System;
using System.Threading;
using CoreFX.Abstractions.Logging;
using Microsoft.Extensions.Logging;

namespace CoreFX.Abstractions.Bases
{
    /// <summary>
    /// Lazy Singleton (abstract base class)
    /// </summary>
    /// <typeparam name="T">Generic instance type</typeparam>
    public abstract class LazySingleton<T>
        where T : LazySingleton<T>, new()
    {
        protected LazySingleton()
        {
            _logger = LogMgr.CreateLogger(GetType());
            _logger.LogTrace($"Initializing...");

            Initialization();
        }

        private void Initialization()
        {
            // Make sure InitialInConstructor() method is called ONLY once
            if (MaxInitializedCount == Interlocked.Add(ref m_CurrentInitializedCount, MaxInitializedCount))
            {
                InitialInConstructor();
            }
        }

        protected abstract void InitialInConstructor();

        public static T Instance => LazyInstance.Value;

        /// <summary>
        /// Interlocked maximum initialized number
        /// </summary>
        private const int MaxInitializedCount = 1;

        /// <summary>
        /// Lazily creates instance
        /// </summary>
        private static readonly Lazy<T> LazyInstance = new Lazy<T>(() => new T());

        /// <summary>
        /// Current initialized count number
        /// </summary>
        private int m_CurrentInitializedCount = 0;

        public readonly Guid _id = Guid.NewGuid();
        public readonly DateTime _ts = DateTime.UtcNow;

        protected ILogger _logger;
    }
}
