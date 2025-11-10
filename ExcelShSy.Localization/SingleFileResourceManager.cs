using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace ExcelShSy.Localization;

/// <summary>
/// Custom ResourceManager for working with single-file applications.     
/// Loads resources directly from embedded resources, allowing      
/// you to change the language during execution even in single-file mode.           
/// </summary>
public class SingleFileResourceManager(string baseName, Assembly assembly) : ResourceManager(baseName, assembly)
{
    private readonly Assembly _assembly = assembly;
    private readonly string _baseName = baseName;
    private readonly Dictionary<string, ResourceSet> _resourceSets = new();

    public override ResourceSet? GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
    {
        var cultureName = culture.Name;
        
        if (!_resourceSets.TryGetValue(cultureName, out var resourceSet))
        {
            resourceSet = LoadResourceSet(cultureName);
            if (resourceSet != null)
                _resourceSets[cultureName] = resourceSet;
        }

        if (resourceSet == null && tryParents && !string.IsNullOrEmpty(culture.Parent.Name) && culture.Parent.Name != cultureName)
            resourceSet = GetResourceSet(culture.Parent, createIfNotExists, tryParents);

        if (resourceSet == null && cultureName != string.Empty)
        {
            resourceSet = GetResourceSet(CultureInfo.InvariantCulture, createIfNotExists, false);
        }

        resourceSet ??= base.GetResourceSet(culture, createIfNotExists, tryParents);

        return resourceSet;
    }

    private ResourceSet? LoadResourceSet(string cultureName)
    {
        var allResourceNames = _assembly.GetManifestResourceNames();
        
        var possibleNames = new List<string>();
        
        if (string.IsNullOrEmpty(cultureName))
            possibleNames.Add($"{_baseName}.resources");
        else
        {
            possibleNames.Add($"{_baseName}.{cultureName}.resources");
            
            var parts = cultureName.Split('-');
            if (parts.Length == 2)
            {
                var lowerCultureName = $"{parts[0].ToLowerInvariant()}-{parts[1].ToLowerInvariant()}";
                if (lowerCultureName != cultureName)
                {
                    possibleNames.Add($"{_baseName}.{lowerCultureName}.resources");
                }
                
                possibleNames.Add($"{_baseName}.{parts[0].ToLowerInvariant()}.resources");
            }
            else if (parts.Length == 1)
                possibleNames.Add($"{_baseName}.{parts[0].ToLowerInvariant()}.resources");
        }

        foreach (var resourceName in possibleNames)
        {
            var stream = _assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
                return new ResourceSet(stream);
            
            // Если не найдено, ищем по частичному совпадению (case-insensitive)
            var matchingName = allResourceNames.FirstOrDefault(name => 
                name.Equals(resourceName, StringComparison.OrdinalIgnoreCase));
            
            if (matchingName != null)
            {
                stream = _assembly.GetManifestResourceStream(matchingName);
                if (stream != null)
                    return new ResourceSet(stream);
            }
        }

        return null;
    }

    public override void ReleaseAllResources()
    {
        foreach (var resourceSet in _resourceSets.Values)
            resourceSet?.Close();
        _resourceSets.Clear();
        base.ReleaseAllResources();
    }
}

