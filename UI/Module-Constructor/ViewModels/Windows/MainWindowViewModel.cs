using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    private readonly IModuleManager _ModuleManager;
    private readonly IModuleBuilder _ModuleBuilder;

    public MainWindowViewModel(I3DVisualizer Visualizer, IModuleManager ModuleManager, IModuleBuilder ModuleBuilder)
    {
        _Visualizer = Visualizer;
        _ModuleManager = ModuleManager;
        _ModuleBuilder = ModuleBuilder;
        Title = "Конструктор модулей v0.1";
    }

    #region Properties

    #region ModuleParts : ObservableCollection<ModulePart> - Детали и секции модуля

    /// <summary>Детали и секции модуля</summary>
    private ObservableCollection<Panel> _Panels;

    /// <summary>Детали и секции модуля</summary>
    public ObservableCollection<Panel> Panels
    {
        get => _Panels;
        set => Set(ref _Panels, value);
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
        set => IfSet(ref _SelectedPanel, value).Then(() => UpdateVisualizationCommand.Execute());
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
        Panels = new(TestData.GetKitchenCabinetPanels());

        Module = new Module()
        {
            Name = "Кухонная тумба",
            Height = 720,
            Width = 600,
            Depth = 520,
            Panels = Panels
        };
        Model = _Visualizer.CreateModel(Module, SelectedPanel);
    }

    #endregion

    #region Command LoadDeskTestDataCommand - Загрузить тестовые данные (тумба с цоколем)

    /// <summary>Загрузить тестовые данные (тумба с цоколем)</summary>
    private Command _LoadDeskTestDataCommand;

    /// <summary>Загрузить тестовые данные (тумба с цоколем)</summary>
    public Command LoadDeskTestDataCommand => _LoadDeskTestDataCommand
        ??= new Command(OnLoadDeskTestDataCommandExecuted, CanLoadDeskTestDataCommandExecute, "Загрузить тестовые данные (тумба с цоколем)");

    /// <summary>Проверка возможности выполнения - Загрузить тестовые данные (тумба с цоколем)</summary>
    private bool CanLoadDeskTestDataCommandExecute() => true;

    /// <summary>Логика выполнения - Загрузить тестовые данные (тумба с цоколем)</summary>
    private void OnLoadDeskTestDataCommandExecuted()
    {
        Panels = new(TestData.GetDeskPanels());

        Module = new Module()
        {
            Name = "Тумба с цоколем",
            Height = 900,
            Width = 500,
            Depth = 450,
            Panels = Panels
        };
        Model = _Visualizer.CreateModel(Module, SelectedPanel);

    }

    #endregion

    #region Command UpdateVisualizationCommand - Обновить визуализацию модели

    /// <summary>Обновить визуализацию модели</summary>
    private Command _UpdateVisualizationCommand;

    /// <summary>Обновить визуализацию модели</summary>
    public Command UpdateVisualizationCommand => _UpdateVisualizationCommand
        ??= new Command(OnUpdateVisualizationCommandExecuted, CanUpdateVisualizationCommandExecute, "Обновить визуализацию модели");

    /// <summary>Проверка возможности выполнения - Обновить визуализацию модели</summary>
    private bool CanUpdateVisualizationCommandExecute() => true;

    /// <summary>Логика выполнения - Обновить визуализацию модели</summary>
    private void OnUpdateVisualizationCommandExecuted()
    {
        Model = _Visualizer.CreateModel(Module, SelectedPanel);
    }

    #endregion

    #region Command SaveModuleCommand - Сохранить в файл

    /// <summary>Сохранить в файл</summary>
    private Command _SaveModuleCommand;

    /// <summary>Сохранить в файл</summary>
    public Command SaveModuleCommand => _SaveModuleCommand
        ??= new Command(OnSaveModuleCommandExecuted, CanSaveModuleCommandExecute, "Сохранить в файл");

    /// <summary>Проверка возможности выполнения - Сохранить в файл</summary>
    private bool CanSaveModuleCommandExecute() => true;

    /// <summary>Логика выполнения - Сохранить в файл</summary>
    private void OnSaveModuleCommandExecuted()
    {
        _ModuleManager.SaveToFile(Module);
    }

    #endregion

    #region Command LoadModuleCommand - Загрузить из файла

    /// <summary>Загрузить из файла</summary>
    private Command _LoadModuleCommand;

    /// <summary>Загрузить из файла</summary>
    public Command LoadModuleCommand => _LoadModuleCommand
        ??= new Command(OnLoadModuleCommandExecuted, CanLoadModuleCommandExecute, "Загрузить из файла");

    /// <summary>Проверка возможности выполнения - Загрузить из файла</summary>
    private bool CanLoadModuleCommandExecute() => true;

    /// <summary>Логика выполнения - Загрузить из файла</summary>
    private void OnLoadModuleCommandExecuted()
    {
       Module = _ModuleManager.LoadFromFile();
       Panels = new(Module?.Panels ?? Enumerable.Empty<Panel>());

       UpdateVisualizationCommand.Execute();
    }

    #endregion

    #region Command AddPanelCommand : PanelOrientation - Добавить деталь

    /// <summary>Добавить деталь</summary>
    private Command _AddPanelCommand;

    /// <summary>Добавить деталь</summary>
    public Command AddPanelCommand => _AddPanelCommand
        ??= new Command(OnAddPanelCommandExecuted, CanAddPanelCommandExecute, "Добавить деталь");

    /// <summary>Проверка возможности выполнения - Добавить деталь</summary>
    private bool CanAddPanelCommandExecute(object p) => p is PanelOrientation;

    /// <summary>Проверка возможности выполнения - Добавить деталь</summary>
    private void OnAddPanelCommandExecuted(object p)
    {
        var orientation = (PanelOrientation) p;

        var ldsp = new Models.Material()
        {
            Name = "ЛДСП 16",
            Thickness = 16
        };

        var panel = new Panel()
        {
            Orientation = orientation,
            Material = ldsp,
            Name = "Новая деталь"
        };

        switch (orientation)
        {
            case PanelOrientation.Horizontal:
                panel.LeftMargin = 0;
                panel.RightMargin = 0;
                panel.FrontMargin = 0;
                panel.BackMargin = 0;
                break;
            case PanelOrientation.Vertical:
                panel.TopMargin = 0;
                panel.BottomMargin = 0;
                panel.FrontMargin = 0;
                panel.BackMargin = 0;
                break;
            case PanelOrientation.Frontal:
                panel.LeftMargin = 0;
                panel.RightMargin = 0;
                panel.TopMargin = 0;
                panel.BottomMargin = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(p), p, null);
        }

        _ModuleBuilder.AddPanel(Module,panel);
    }

    #endregion

    #region Command DeletePanelCommand - Удалить деталь

    /// <summary>Удалить деталь</summary>
    private Command _DeletePanelCommand;

    /// <summary>Удалить деталь</summary>
    public Command DeletePanelCommand => _DeletePanelCommand
        ??= new Command(OnDeletePanelCommandExecuted, CanDeletePanelCommandExecute, "Удалить деталь");

    /// <summary>Проверка возможности выполнения - Удалить деталь</summary>
    private bool CanDeletePanelCommandExecute() => SelectedPanel is {};

    /// <summary>Логика выполнения - Удалить деталь</summary>
    private void OnDeletePanelCommandExecuted()
    {
        _ModuleBuilder.RemovePanel(Module, SelectedPanel);
    }

    #endregion

    #region Command SetPanelUpCommand - Переместить деталь выше

    /// <summary>Переместить деталь выше</summary>
    private Command _SetPanelUpCommand;

    /// <summary>Переместить деталь выше</summary>
    public Command SetPanelUpCommand => _SetPanelUpCommand
        ??= new Command(OnSetPanelUpCommandExecuted, CanSetPanelUpCommandExecute, "Переместить деталь выше");

    /// <summary>Проверка возможности выполнения - Переместить деталь выше</summary>
    private bool CanSetPanelUpCommandExecute() => SelectedPanel is {Order: > 1};

    /// <summary>Логика выполнения - Переместить деталь выше</summary>
    private void OnSetPanelUpCommandExecuted()
    {
        _ModuleBuilder.SetPanelOrder(Module, SelectedPanel, SelectedPanel.Order - 1);
        Panels = new(Module.Panels.OrderBy(p => p.Order));
    }

    #endregion

    #region Command SetPanelDownCommand - Переместить деталь ниже

    /// <summary>Переместить деталь ниже</summary>
    private Command _SetPanelDownCommand;

    /// <summary>Переместить деталь ниже</summary>
    public Command SetPanelDownCommand => _SetPanelDownCommand
        ??= new Command(OnSetPanelDownCommandExecuted, CanSetPanelDownCommandExecute, "Переместить деталь ниже");

    /// <summary>Проверка возможности выполнения - Переместить деталь ниже</summary>
    private bool CanSetPanelDownCommandExecute()
    {
        var allPanelsCount = Module.Panels.Count;
        return SelectedPanel is {} p && p.Order < allPanelsCount;
    }

    /// <summary>Логика выполнения - Переместить деталь ниже</summary>
    private void OnSetPanelDownCommandExecuted()
    {
        _ModuleBuilder.SetPanelOrder(Module, SelectedPanel, SelectedPanel.Order + 1);
        Panels = new(Module.Panels.OrderBy(p => p.Order));
    }

    #endregion

    #endregion
}
