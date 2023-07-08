using System.IO;
using System.Linq;
using NUnit.Framework;

namespace YPermitin.FIASToolSet.DistributionReader.Tests;

public class Tests
{
    private readonly string _workingDirectory;

    public Tests()
    {
        _workingDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SampleData"
        );
    }
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetVersionAsStringTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        string version = reader.GetVersionAsString();

        Assert.IsNotNull(version);
    }
    
    [Test]
    public void GetVersionTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        int version = reader.GetVersion();

        Assert.True(version > 0);
    }
    
    [Test]
    public void GetNormativeDocKindsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var itemsReader = reader.GetNormativeDocKinds();
        var allItems = itemsReader.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(4, allItems.Count);
        
        Assert.AreEqual(0, allItems[0].Id);
        Assert.AreEqual("Не определено", allItems[0].Name);
    }
}