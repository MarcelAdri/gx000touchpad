using gx000data;
using gx000serverSimComm;
using Moq;

namespace gx000touchpadUnitTests.gx000data;

[TestFixture]
public class ProcessSimDataTests
{
    private TestableProcessSimData _processSimData;
    private Variable _testVariable;
    private string _testVariableName;
    private string _dataType;

    [SetUp]
    public void SetUp()
    {
        _processSimData = new TestableProcessSimData(new GenerateFlightSimContent());
        _testVariableName = VariableDefinitions.FirstMessageName;
        _dataType = DataTypes.StringType;
        _testVariable = new StringVariable(
            _testVariableName,
            "TestText01");
    }

    [Test]
    public void CurrentVariable_WhenSet_ShouldReturnCorrectValue()
    {
        _processSimData.SetVariable(_testVariable);
        
        Assert.That(_processSimData.CurrentVariable, Is.EqualTo(_testVariable));
    }
    
    [Test]
    public void CurrentVariable_WhenSet_ShouldRaisePropertyChangedEvent()
    {
        // Arrange
        bool eventRaised = false;
        
        _processSimData.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ProcessSimData.CurrentVariable))
            {
                eventRaised = true;
            }
        };

        // Act
        _processSimData.SetVariable(_testVariable);

        // Assert
        Assert.That(eventRaised, Is.True, "PropertyChanged event for CurrentVariable was not raised.");
    }
    
    [Test]
    public void VariableName_WhenSet_ShouldReturnCorrectValue()
    {
        _processSimData.SetVariableName(_testVariableName);
        
        Assert.That(_processSimData.VariableName, Is.EqualTo(_testVariableName));
    }
    
    [Test]
    public void VariableName_WhenSet_ShouldRaisePropertyChangedEvent()
    {
        // Arrange
        bool eventRaised = false;
        
        _processSimData.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ProcessSimData.VariableName))
            {
                eventRaised = true;
            }
        };

        // Act
        _processSimData.SetVariableName(_testVariableName);

        // Assert
        Assert.That(eventRaised, Is.True, "PropertyChanged event for VariableName was not raised.");
    }
    
    [Test]
    public void DataType_WhenSet_ShouldReturnCorrectValue()
    {
        _processSimData.SetDataType(_dataType);
        
        Assert.That(_processSimData.DataType, Is.EqualTo(_dataType));
    }
    [Test]
    public void DataType_WhenSet_ShouldRaisePropertyChangedEvent()
    {
        // Arrange
        bool eventRaised = false;
        
        _processSimData.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ProcessSimData.DataType))
            {
                eventRaised = true;
            }
        };

        // Act
        _processSimData.SetDataType(_dataType);

        // Assert
        Assert.That(eventRaised, Is.True, "PropertyChanged event for VariableName was not raised.");
    }
    
    [Test]
    public void Dispose_WhenCalled_DisposesManagedResources()
    {
        // // Arrange
        var mockResource = new Mock<IDisposable>();
        _processSimData.SetSubscription(mockResource.Object);

        // Act
        _processSimData.Dispose();

        // Assert
        mockResource.Verify(m => m.Dispose(), Times.Once);
    }

    [Test]
    public void Dispose_WhenCalled_SetsIsDisposedToTrue()
    {
        // Act
        _processSimData.Dispose();

        // Assert
        Assert.That(_processSimData.IsDisposed);
    }

    [Test]
    public void Dispose_CalledMultipleTimes_DoesNotThrow()
    {
        // Act & Assert
        _processSimData.Dispose();
        _processSimData.Dispose();
    }

    
    [TearDown]
    public void TearDown()
    {
        _processSimData.Dispose();
    }

}