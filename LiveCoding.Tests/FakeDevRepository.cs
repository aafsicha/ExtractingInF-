using System.Collections.Generic;
using System.Linq;
using LiveCoding.Domain;
using LiveCoding.Persistence;

namespace LiveCoding.Tests;

public class FakeDevRepository : IDevRepository
{
    private readonly IEnumerable<Dev> _devs;

    public FakeDevRepository(Dev[] devDatas)
    {
        _devs = devDatas;
    }

    public IEnumerable<Dev> Get()
    {
        return _devs;
    }
}