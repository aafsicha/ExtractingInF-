using System.Collections.Generic;
using System.Linq;
using LiveCoding.Domain;
using LiveCoding.Persistence;

namespace LiveCoding.Tests;

public class FakeBarRepository : IBarRepository
{
    private readonly IEnumerable<Bar> _bars;

    public FakeBarRepository(Bar[] barDatas)
    {
        _bars = barDatas;
    }

    public IEnumerable<Bar> Get()
    {
        return _bars;
    }
}