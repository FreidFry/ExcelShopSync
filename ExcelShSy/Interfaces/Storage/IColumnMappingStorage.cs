using ExcelShSy.Core.Interfaces.Shop;

namespace ExcelShSy.Core.Interfaces.Storage
{
    /// <summary>
    /// Describes a storage mechanism for maintaining column mappings between source data and shop templates.
    /// </summary>
    public interface IColumnMappingStorage
    {
        /// <summary>
        /// Gets the collection of column mappings where the key represents a logical field and the list contains possible column names.
        /// </summary>
        public Dictionary<string, List<string>> Columns { get; }

        /// <summary>
        /// Adds a set of column aliases for the specified logical field.
        /// </summary>
        /// <param name="key">The logical field name.</param>
        /// <param name="values">The list of column aliases that map to the field.</param>
        void AddColumn(string key, List<string> values);

        /// <summary>
        /// Adds a single column alias for the specified logical field.
        /// </summary>
        /// <param name="key">The logical field name.</param>
        /// <param name="value">The column alias to associate.</param>
        void AddColumn(string key, string value);

        /// <summary>
        /// Adds column mappings defined by the provided shop template.
        /// </summary>
        /// <param name="shop">The shop template that contains predefined column mappings.</param>
        void AddColumn(IShopTemplate shop);
    }
}