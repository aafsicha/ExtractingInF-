using LiveCoding.Domain;
using LiveCoding.Persistence;

namespace LiveCoding.Services
{
    public class BookingService
    {
        private readonly IBarRepository _barRepo;
        private readonly IDevRepository _devRepo;
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBarRepository barRepo,
            IDevRepository devRepo,
            IBookingRepository bookingRepository)
        {
            _barRepo = barRepo;
            _devRepo = devRepo;
            _bookingRepository = bookingRepository;
        }

        public bool ReserveBar()
        {
            var bars = _barRepo.Get().ToList();
            var devs = _devRepo.Get().ToList();

            var numberOfAvailableDevsByDate = new Dictionary<DateTime, int>();
            foreach (var devData in devs)
            {
                foreach (var date in devData.OnSite)
                {
                    if (numberOfAvailableDevsByDate.ContainsKey(date))
                    {
                        numberOfAvailableDevsByDate[date]++;
                    }
                    else
                    {
                        numberOfAvailableDevsByDate.Add(date, 1);
                    }
                }
            }

            var maxNumberOfDevs = numberOfAvailableDevsByDate.Values.Max();

            if (maxNumberOfDevs <= devs.Count() * 0.6)
            {
                return false;
            }

            var bestDate = numberOfAvailableDevsByDate.First(kv => kv.Value == maxNumberOfDevs).Key;

            foreach (var bar in bars)
            {
                if (bar.Capacity.Value >= maxNumberOfDevs && bar.OpenedDays.Contains(bestDate.DayOfWeek))
                {
                    BookBar(bar.Name, bestDate);
                    _bookingRepository.Save(new Booking(bar, bestDate));
                    return true;
                }
            }

            return false;
        }

        private void BookBar(string name, DateTime dateTime)
        {
            Console.WriteLine("Bar booked: " + name + " at " + dateTime);
        }
    }
}