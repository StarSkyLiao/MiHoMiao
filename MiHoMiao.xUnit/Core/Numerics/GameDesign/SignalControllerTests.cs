using MiHoMiao.Core.Numerics.GameDesign;

namespace MiHoMiao.xUnit.Core.Numerics.GameDesign;

public class SignalControllerTests
{
    private readonly SignalController m_Controller = new SignalController();
    
    // Arrange
    delegate void TestDelegate();
    
    [Fact]
    public void RegisterSignal_WhenSignalNotRegistered_AddsSignalToDictionary()
    {
        // Act
        m_Controller.RegisterSignal<TestDelegate>();

        // Assert
        TestDelegate? signal = m_Controller.GetSignal<TestDelegate>();
        Assert.Null(signal); // Signal is registered with null delegate initially
    }

    [Fact]
    public void AddToSignal_WhenSignalExists_AddsDelegate()
    {
        m_Controller.RegisterSignal<TestDelegate>();
        TestDelegate testDelegate = () => { };

        // Act
        m_Controller.AddToSignal(testDelegate);

        // Assert
        TestDelegate? signal = m_Controller.GetSignal<TestDelegate>();
        Assert.NotNull(signal);
        Assert.Contains(testDelegate, signal.GetInvocationList());
    }

    [Fact]
    public void AddToSignal_WithType_WhenSignalExists_AddsDelegate()
    {
        m_Controller.RegisterSignal<TestDelegate>();
        Delegate testDelegate = (TestDelegate)(() => { });

        // Act
        m_Controller.AddToSignal(typeof(TestDelegate), testDelegate);

        // Assert
        TestDelegate? signal = m_Controller.GetSignal<TestDelegate>();
        Assert.NotNull(signal);
        Assert.Contains(testDelegate, signal.GetInvocationList());
    }

    [Fact]
    public void RemoveSignal_WhenSignalExists_RemovesDelegate()
    {
        m_Controller.RegisterSignal<TestDelegate>();
        TestDelegate testDelegate = () => { };
        m_Controller.AddToSignal(testDelegate);

        // Act
        m_Controller.RemoveSignal(testDelegate);

        // Assert
        TestDelegate? signal = m_Controller.GetSignal<TestDelegate>();
        Assert.Null(signal); // Delegate is removed, signal is null
    }

    [Fact]
    public void RemoveSignal_WithType_WhenSignalExists_RemovesDelegate()
    {
        m_Controller.RegisterSignal<TestDelegate>();
        Delegate testDelegate = (Action)(() => { });
        m_Controller.AddToSignal(typeof(TestDelegate), testDelegate);

        // Act
        m_Controller.RemoveSignal(typeof(TestDelegate), testDelegate);

        // Assert
        TestDelegate? signal = m_Controller.GetSignal<TestDelegate>();
        Assert.Null(signal); // Delegate is removed, signal is null
    }

    [Fact]
    public void GetSignal_WhenSignalExists_ReturnsSignal()
    {
        m_Controller.RegisterSignal<TestDelegate>();
        TestDelegate testDelegate = () => { };
        m_Controller.AddToSignal(testDelegate);

        // Act
        TestDelegate? signal = m_Controller.GetSignal<TestDelegate>();

        // Assert
        Assert.NotNull(signal);
        Assert.Contains(testDelegate, signal.GetInvocationList());
    }

    [Fact]
    public void GetSignal_WhenSignalDoesNotExist_ReturnsNull()
    {
        // Act
        TestDelegate? signal = m_Controller.GetSignal<TestDelegate>();

        // Assert
        Assert.Null(signal);
    }
    
}