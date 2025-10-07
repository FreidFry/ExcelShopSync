using System.ComponentModel;

namespace ExcelShSy.LocalDataBaseModule.Persistance.Models;

public class DynamicRow : INotifyPropertyChanged
{
    public int Id { get; init; }

    public readonly List<string?> Values = [];
    
    private readonly Dictionary<string, string?> _values = new();

    public string? this[string column]
    {
        get => _values.TryGetValue(column, out var v) ? v : null;
        set
        {
            if (_values.TryGetValue(column, out var old) && old == value)
                return;

            _values[column] = value;
            Values.Add(value);
            if (old != null)
                Values.Remove(old);
            OnPropertyChanged($"Item[{column}]");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
