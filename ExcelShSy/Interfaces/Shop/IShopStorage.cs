namespace ExcelShSy.Core.Interfaces.Shop
{
    /// <summary>
    /// Manages persistence and retrieval of shop templates used for product synchronization.
    /// </summary>
    public interface IShopStorage
    {
        /// <summary>
        /// Retrieves the shop template with the specified name.
        /// </summary>
        /// <param name="shopName">The name of the shop template to look up.</param>
        /// <returns>The matching shop template, or <c>null</c> if not found.</returns>
        IShopTemplate? GetShopMapping(string shopName);

        /// <summary>
        /// Gets the list of available shop template names.
        /// </summary>
        /// <returns>A collection of shop names.</returns>
        List<string> GetShopList();

        /// <summary>
        /// Saves the provided shop template to persistent storage.
        /// </summary>
        /// <param name="shop">The shop template to persist.</param>
        void SaveShopTemplate(IShopTemplate shop);

        /// <summary>
        /// Updates the existing entry with the data provided by <paramref name="updatedShop"/>.
        /// </summary>
        /// <param name="updatedShop">The shop template containing new values.</param>
        void UpdateShop(IShopTemplate updatedShop);

        /// <summary>
        /// Gets or sets the collection of shop templates maintained by the storage.
        /// </summary>
        List<IShopTemplate> Shops { get; set; }

        /// <summary>
        /// Adds a new shop template entry with the given name.
        /// </summary>
        /// <param name="shopName">The name for the new shop template.</param>
        void AddShop(string shopName);

        /// <summary>
        /// Occurs when the set of shops changes. The argument represents the affected shop name.
        /// </summary>
        event Action<string>? ShopsChanged;

        /// <summary>
        /// Removes the shop template with the provided name.
        /// </summary>
        /// <param name="shopName">The name of the shop template to remove.</param>
        void RemoveShop(string shopName);

        /// <summary>
        /// Renames an existing shop template.
        /// </summary>
        /// <param name="oldName">The current name of the shop template.</param>
        /// <param name="newName">The new name to assign.</param>
        void RenameShop(string oldName, string newName);

        /// <summary>
        /// Checks whether the file at the provided path is missing.
        /// </summary>
        /// <param name="path">The path to inspect.</param>
        /// <returns><c>true</c> when the file does not exist; otherwise, <c>false</c>.</returns>
        bool IsFileExist(string path);
    }
}
