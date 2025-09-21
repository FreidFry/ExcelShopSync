using System.Windows.Input;

namespace ExcelShSy.LocalDataBaseModule;

public class RelayCommand<T> : ICommand
{
    private readonly Action<T> _execute;
    private readonly Predicate<T>? _canExecute;

    public RelayCommand(Action<T> execute, Predicate<T>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) =>
        parameter is T t && (_canExecute?.Invoke(t) ?? true);

    public void Execute(object? parameter) =>
        _execute((T)parameter!);

    public event EventHandler? CanExecuteChanged;

}