using LiveCoding.Domain;
using Newtonsoft.Json;

namespace LiveCoding.Persistence;

public class DevRepository : IDevRepository
{
    public IEnumerable<Dev> Get()
    {
        var json = File.ReadAllText("../LiveCoding.Persistence/devs.json");
        var devs = JsonConvert.DeserializeObject<IEnumerable<Dev>>(json);

        return devs.Select(d => new Dev(d.Name, d.OnSite));
    }
}