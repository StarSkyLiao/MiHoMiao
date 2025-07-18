using JetBrains.Annotations;
using MiHoMiao.Core.Numerics.GameDesign;

namespace MiHoMiao.xUnit.Core.Numerics.GameDesign;

public class ModuleControllerTests
{
    private class TestModule : IModule
    {
        public bool Initialized { get; private set; }
        public void ModuleInitialize() => Initialized = true;

    }

    private class AnotherTestModule : IModule
    {
        public bool Initialized { [UsedImplicitly] get; private set; }
        public void ModuleInitialize() => Initialized = true;
    }

    [Fact]
    public void AddModule_Generic_SuccessfullyAddsModule()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        TestModule module = new TestModule();

        // Act
        bool result = controller.AddModule(module);

        // Assert
        Assert.True(result);
        Assert.True(module.Initialized);
        Assert.Single(controller);
        Assert.Same(module, controller[typeof(TestModule)]);
    }

    [Fact]
    public void AddModule_Generic_DuplicateType_ReturnsFalse()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        TestModule module1 = new TestModule();
        TestModule module2 = new TestModule();
        controller.AddModule(module1);

        // Act
        bool result = controller.AddModule(module2);

        // Assert
        Assert.False(result);
        Assert.False(module2.Initialized);
        Assert.Single(controller);
        Assert.Same(module1, controller[typeof(TestModule)]);
    }

    [Fact]
    public void AddModule_Type_SuccessfullyAddsModule()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        TestModule module = new TestModule();

        // Act
        bool result = controller.AddModule(typeof(TestModule), module);

        // Assert
        Assert.True(result);
        Assert.True(module.Initialized);
        Assert.Single(controller);
        Assert.Same(module, controller[typeof(TestModule)]);
    }

    [Fact]
    public void AddModules_MultipleModules_ReturnsCorrectCount()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        IModule[] modules = [new TestModule(), new AnotherTestModule()];

        // Act
        int count = controller.AddModules(modules);

        // Assert
        Assert.Equal(2, count);
        Assert.Equal(2, controller.Count);
        Assert.True(modules.All(m => ((dynamic)m).Initialized));
    }

    [Fact]
    public void OverrideModule_ReplacesExistingModule()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        TestModule module1 = new TestModule();
        TestModule module2 = new TestModule();
        controller.AddModule(module1);

        // Act
        controller.OverrideModule(module2);

        // Assert
        Assert.True(module2.Initialized);
        Assert.Single(controller);
        Assert.Same(module2, controller[typeof(TestModule)]);
    }

    [Fact]
    public void GetModule_Generic_ReturnsCorrectModule()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        TestModule module = new TestModule();
        controller.AddModule(module);

        // Act
        TestModule? result = controller.GetModule<TestModule>();

        // Assert
        Assert.Same(module, result);
    }

    [Fact]
    public void GetModule_Generic_NonExistent_ReturnsNull()
    {
        // Arrange
        ModuleController<IModule> controller = [];

        // Act
        TestModule? result = controller.GetModule<TestModule>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void TryGetModule_Type_ReturnsTrueForExisting()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        TestModule module = new TestModule();
        controller.AddModule(module);

        // Act
        bool result = controller.TryGetModule(typeof(TestModule), out IModule? outModule);

        // Assert
        Assert.True(result);
        Assert.Same(module, outModule);
    }

    [Fact]
    public void TryGetModule_Generic_ReturnsTrueForExisting()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        TestModule module = new TestModule();
        controller.AddModule(module);

        // Act
        bool result = controller.TryGetModule(out TestModule? outModule);

        // Assert
        Assert.True(result);
        Assert.Same(module, outModule);
    }

    [Fact]
    public void Collection_Contains_ReturnsCorrectResult()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        TestModule module = new TestModule();
        controller.AddModule(module);

        // Act & Assert
        Assert.Contains(module, controller);
        Assert.DoesNotContain(new AnotherTestModule(), controller);
    }

    [Fact]
    public void Collection_Clear_RemovesAllModules()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        controller.AddModule(new TestModule());
        controller.AddModule(new AnotherTestModule());

        // Act
        controller.Clear();

        // Assert
        Assert.Empty(controller);
    }

    [Fact]
    public void Collection_CopyTo_CopiesCorrectly()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        TestModule module1 = new TestModule();
        AnotherTestModule module2 = new AnotherTestModule();
        controller.AddModule(module1);
        controller.AddModule(module2);
        IModule[] array = new IModule[2];

        // Act
        controller.CopyTo(array, 0);

        // Assert
        Assert.Contains(module1, array);
        Assert.Contains(module2, array);
    }

    [Fact]
    public void Collection_Remove_RemovesModule()
    {
        // Arrange
        ModuleController<IModule> controller = [];
        TestModule module = new TestModule();
        controller.AddModule(module);

        // Act
        bool result = controller.Remove(module);

        // Assert
        Assert.True(result);
        Assert.Empty(controller);
    }

    [Fact]
    public void Collection_Properties_ReturnCorrectValues()
    {
        // Arrange
        ModuleController<IModule> controller = [];

        // Assert
        Assert.False(controller.IsReadOnly);
        Assert.False(controller.IsSynchronized);
        Assert.Same(controller, controller.SyncRoot);
    }
}