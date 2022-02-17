using System;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Module_Constructor.Models;
using Module_Constructor.Services.Interfaces;

namespace Module_Constructor.Services
{
    public class ModuleManager : IModuleManager
    {
        private readonly ILogger<ModuleManager> _Logger;

        private const string FileExtension = "mtmp";

        public ModuleManager(ILogger<ModuleManager> Logger)
        {
            _Logger = Logger;
        }

        public Module LoadFromFile()
        {
            var ofd = new OpenFileDialog
            {
                Filter = $"Файлы модулей|*.{FileExtension}"
            };

            if (ofd.ShowDialog() != true)
                return null;

            try
            {
                var text = File.ReadAllText(ofd.FileName);
                var module = JsonSerializer.Deserialize<Module>(text);

                _Logger.LogInformation("Загружен модуль из файла {0}", ofd.FileName);
                return module;

            }
            catch (Exception e)
            {
                _Logger.LogInformation(e, "Ошибка загрузки модуля из файла {0}", ofd.FileName);
                return null;
            }
        }

        public bool SaveToFile(Module module)
        {
            var sfd = new SaveFileDialog
            {
                Filter = $"Файлы модулей|*.{FileExtension}"
            };

            if (sfd.ShowDialog() != true)
                return false;

            var serialized = JsonSerializer.Serialize(module);
            try
            {
                File.WriteAllText(sfd.FileName, serialized);
                _Logger.LogInformation("Модуль сохранён в файл {0}", sfd.FileName);
                return true;
            }
            catch (Exception e)
            {
                _Logger.LogInformation(e, "Ошибка сохранения модуля в файл {0}", sfd.FileName);
                return false;
            }
        }
    }
}
