using System.Globalization;
using System.Reflection;
using System.Resources;

namespace ExcelShSy.Core.Interfaces.Common;

public interface ILocalizationService
{
    string GetString(string resourceFile, string key);
    
    string GetErrorString(string key);
    string GetMessageString(string key);
}