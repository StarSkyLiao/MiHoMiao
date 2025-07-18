using MiHoMiao.Core.Numerics.GameDesign;

namespace MiHoMiao.xUnit.Core.Numerics.GameDesign;

public class DataBoxTests
{
    [Fact]
    public void Get_NonExistingKey_ReturnsZeroAndCreatesEntry()
    {
        // Arrange
        DataBox<string> dataBox = new DataBox<string>();
        const string Key = "testKey";

        // Act
        double result = dataBox[Key];

        // Assert
        Assert.Equal(0.0, result);
        Assert.True(dataBox.Contains(Key));
    }

    [Fact]
    public void Set_ExistingKey_UpdatesValue()
    {
        // Arrange
        DataBox<string> dataBox = new DataBox<string>();
        const string Key = "testKey";
        dataBox[Key] = 10.0;

        // Act
        dataBox[Key] = 20.0;
        double result = dataBox[Key];

        // Assert
        Assert.Equal(20.0, result);
    }

    [Fact]
    public void TryInsert_NewKey_ReturnsTrueAndAddsValue()
    {
        // Arrange
        DataBox<string> dataBox = new DataBox<string>();
        const string Key = "testKey";
        const double Value = 42.0;

        // Act
        bool result = dataBox.TryInsert(Key, Value);

        // Assert
        Assert.True(result);
        Assert.Equal(Value, dataBox[Key]);
    }

    [Fact]
    public void TryInsert_ExistingKey_ReturnsFalseAndDoesNotUpdate()
    {
        // Arrange
        DataBox<string> dataBox = new DataBox<string>();
        const string Key = "testKey";
        dataBox[Key] = 10.0;

        // Act
        bool result = dataBox.TryInsert(Key, 20.0);

        // Assert
        Assert.False(result);
        Assert.Equal(10.0, dataBox[Key]);
    }

    [Fact]
    public void Contains_NonExistingKey_ReturnsFalse()
    {
        // Arrange
        DataBox<string> dataBox = new DataBox<string>();
        const string Key = "testKey";

        // Act
        bool result = dataBox.Contains(Key);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_ExistingKey_ReturnsTrue()
    {
        // Arrange
        DataBox<string> dataBox = new DataBox<string>();
        const string Key = "testKey";
        dataBox[Key] = 10.0;

        // Act
        bool result = dataBox.Contains(Key);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void RegisterSetter_TriggerActionOnSet()
    {
        // Arrange
        DataBox<string> dataBox = new DataBox<string>();
        const string Key = "testKey";
        double capturedValue = 0.0;

        // Act
        dataBox.RegisterSetter(Key, CapturedValueHandle);
        dataBox[Key] = 15.0;

        // Assert
        Assert.Equal(15.0, capturedValue);
        Assert.Equal(15.0, dataBox[Key]);
        return;

        void CapturedValueHandle(double value) => capturedValue = value;
    }

    [Fact]
    public void RegisterSetter_MultipleActions_TriggerAllOnSet()
    {
        // Arrange
        DataBox<string> dataBox = new DataBox<string>();
        const string Key = "testKey";
        int callCount = 0;

        // Act
        dataBox.RegisterSetter(Key, CallCounter1);
        dataBox.RegisterSetter(Key, CallCounter2);
        dataBox[Key] = 10.0;

        // Assert
        Assert.Equal(2, callCount);
        return;

        void CallCounter1(double _) => callCount++;
        void CallCounter2(double _) => callCount++;
    }

    [Fact]
    public void GenericType_IntKey_WorksCorrectly()
    {
        // Arrange
        DataBox<int> dataBox = new DataBox<int>();
        const int Key = 42;

        // Act
        dataBox[Key] = 100.0;
        double result = dataBox[Key];

        // Assert
        Assert.Equal(100.0, result);
        Assert.True(dataBox.Contains(Key));
    }
}