using Avalonia.Controls;
using ExcelShSy.Core.Enums;
using ExcelShSy.Core.Interfaces.Common;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace ExcelShSy.Tests
{
    public class MessageTest : IMessages<IMsBox<ButtonResult>>, IMessageCustom<IMsBox<string>, MessageBoxCustomParams>
    {
        public IMsBox<string> GetMessageBoxCustom(MessageBoxCustomParams CustomParams)
        {
            return new TestMsBox();
        }

        public IMsBox<ButtonResult> GetMessageBoxStandard(string title, string message)
        {
            Console.WriteLine($"=> {title}");
            Console.WriteLine(message);
            return new TestMsBox();
        }

        public IMsBox<ButtonResult> GetMessageBoxStandard(string title, string message, MyButtonEnum buttons)
        {
            Console.WriteLine($"=> {title}");
            Console.WriteLine(message);
            return new TestMsBox();
        }

        public IMsBox<ButtonResult> GetMessageBoxStandard(string title, string message, MyButtonEnum buttons, MyIcon icon)
        {
            Console.WriteLine($"=> {title}");
            Console.WriteLine(message);
            return new TestMsBox();
        }
    }
    public class TestMsBox : IMsBox<ButtonResult>, IMsBox<string>
    {
        public string InputValue => throw new NotImplementedException();

        public Task<ButtonResult> ShowAsPopupAsync(ContentControl owner)
        {
            return Task.FromResult(ButtonResult.Ok);
        }

        public Task<ButtonResult> ShowAsPopupAsync(Window owner)
        {
            return Task.FromResult(ButtonResult.Ok);
        }

        public Task<ButtonResult> ShowAsync()
        {
            return Task.FromResult(ButtonResult.Ok);
        }

        public Task<ButtonResult> ShowWindowAsync()
        {
            return Task.FromResult(ButtonResult.Ok);
        }

        public Task<ButtonResult> ShowWindowDialogAsync(Window owner)
        {
            return Task.FromResult(ButtonResult.Ok);
        }

        Task<string> IMsBox<string>.ShowAsPopupAsync(ContentControl owner)
        {
            return Task.FromResult(string.Empty);
        }

        Task<string> IMsBox<string>.ShowAsPopupAsync(Window owner)
        {
            return Task.FromResult(string.Empty);
        }

        Task<string> IMsBox<string>.ShowAsync()
        {
            return Task.FromResult(string.Empty);
        }

        Task<string> IMsBox<string>.ShowWindowAsync()
        {
            return Task.FromResult(string.Empty);
        }

        Task<string> IMsBox<string>.ShowWindowDialogAsync(Window owner)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
