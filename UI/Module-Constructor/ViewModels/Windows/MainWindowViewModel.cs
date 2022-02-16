using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using Module_Constructor.Data;
using Module_Constructor.Models;
using Module_Constructor.Services.Interfaces;
using WPR.MVVM.Commands;
using WPR.MVVM.ViewModels;

namespace Module_Constructor.ViewModels.Windows;
public class MainWindowViewModel : WindowViewModel
{
    private readonly I3DVisualizer _Visualizer;

    public MainWindowViewModel(I3DVisualizer Visualizer)
    {
        _Visualizer = Visualizer;
        Title = "Конструктор модулей v0.1";
    }

    #region Properties
    #region ModuleParts : ObservableCollection<ModulePart> - Детали и секции модуля

    /// <summary>Детали и секции модуля</summary>
    private ObservableCollection<ModulePart> _ModuleParts;

    /// <summary>Детали и секции модуля</summary>
    public ObservableCollection<ModulePart> ModuleParts
    {
        get => _ModuleParts;
        set => Set(ref _ModuleParts, value);
    }

    #endregion

    #region Module : Module - Тестовый модуль

    /// <summary>Тестовый модуль</summary>
    private Module _Module;

    /// <summary>Тестовый модуль</summary>
    public Module Module
    {
        get => _Module;
        set => Set(ref _Module, value);
    }

    #endregion

    #region Model : Model3D - Трёхмерная модель модуля

    /// <summary>Трёхмерная модель модуля</summary>
    private Model3D _Model;

    /// <summary>Трёхмерная модель модуля</summary>
    public Model3D Model
    {
        get => _Model;
        set => Set(ref _Model, value);
    }

    #endregion

    #region SelectedPanel : Panel - Выбранная деталь

    /// <summary>Выбранная деталь</summary>
    private Panel _SelectedPanel;

    /// <summary>Выбранная деталь</summary>
    public Panel SelectedPanel
    {
        get => _SelectedPanel;
        set => Set(ref _SelectedPanel, value);
    }

    #endregion
    #endregion

    #region Commands
    #region Command LoadTestDataCommand - Загрузить тестовые данные

    /// <summary>Загрузить тестовые данные</summary>
    private Command _LoadTestDataCommand;

    /// <summary>Загрузить тестовые данные</summary>
    public Command LoadTestDataCommand => _LoadTestDataCommand
        ??= new Command(OnLoadTestDataCommandExecuted, CanLoadTestDataCommandExecute, "Загрузить тестовые данные");

    /// <summary>Проверка возможности выполнения - Загрузить тестовые данные</summary>
    private bool CanLoadTestDataCommandExecute() => true;

    /// <summary>Логика выполнения - Загрузить тестовые данные</summary>
    private void OnLoadTestDataCommandExecuted()
    {
        ModuleParts = new(TestData.GetKitchenCabinetPanels());

        Module = new Module()
        {
            Name = "Кухонная тумба",
            Height = 720,
            Width = 600,
            Depth = 520,
            Parts = ModuleParts
        };
        Model = _Visualizer.CreateModel(Module, SelectedPanel);
    }

    #endregion

    #endregion
}
