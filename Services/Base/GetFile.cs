﻿using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;
using ExcelShopSync.Modules;

namespace ExcelShopSync.Services.Base
{
    class GetFile
    {
        public static List<T?> Get<T>(Label label) where T : FileBase
        {
            OpenFileDialog fileDialog = new()
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.xlsm",
                Multiselect = true
            };

            if (fileDialog.ShowDialog() == false || fileDialog.FileNames.Length == 0)
            {
                return new List<T?>();
            }

            label.Content = Path.GetFileName(fileDialog.FileName);
            var result = new List<T?>();

            foreach (var fileName in fileDialog.FileNames)
            {
                var constructor = typeof(T).GetConstructor(new[] { typeof(string) });
                if (constructor != null)
                {
                    var instance = (T?)constructor.Invoke(new object[] { fileName });
                    result.Add(instance);
                }
            }

            return result;
        }
    }
}
