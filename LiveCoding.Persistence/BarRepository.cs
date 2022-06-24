using LiveCoding.Domain;
using Newtonsoft.Json;

namespace LiveCoding.Persistence;

public class BarRepository : IBarRepository
{
    public IEnumerable<Bar> Get()
    {
        var json = File.ReadAllText("../LiveCoding.Persistence/bars.json");
        var bars = JsonConvert.DeserializeObject<IEnumerable<BarData>>(json);

        return bars.Select(c => new Bar(c.Name, new Capacity(c.Capacity), c.Open));
    }
}
public record BarData(string Name, int Capacity, DayOfWeek[] Open);