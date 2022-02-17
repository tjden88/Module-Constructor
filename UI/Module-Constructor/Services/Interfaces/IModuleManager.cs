using Module_Constructor.Models;

namespace Module_Constructor.Services.Interfaces
{
    public interface IModuleManager
    {
        Module LoadFromFile();

        bool SaveToFile(Module module);
    }
}
