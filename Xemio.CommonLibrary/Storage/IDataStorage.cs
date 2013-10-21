namespace Xemio.CommonLibrary.Storage
{
    public interface IDataStorage
    {
        /// <summary>
        /// Stores the given <paramref name="instance"/> using a default key.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance.</param>
        void Store<T>(T instance);
        /// <summary>
        /// Stores the given <paramref name="instance"/> using the given <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        void Store<T>(T instance, string key);
        /// <summary>
        /// Retrieves the instance using a default key.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        T Retrieve<T>();
        /// <summary>
        /// Retrieves the instance using the given <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="key">The key.</param>
        T Retrieve<T>(string key);
    }
}