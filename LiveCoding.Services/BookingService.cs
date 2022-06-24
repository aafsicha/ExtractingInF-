using LiveCoding.Domain;

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

            var numberOfAvailableDevsByDate = NumberOfAvailableDevsByDate(devs);

            var maxNumberOfDevs = numberOfAvailableDevsByDate.Values.Max();

            if (maxNumberOfDevs <= devs.Count() * 0.6)
            {
                return false;
            }

            var bestDate = numberOfAvailableDevsByDate.First(kv => kv.Value == maxNumberOfDevs).Key;

            return Book(bars, maxNumberOfDevs, bestDate);
        }

        private bool Book(List<Bar> bars, int maxNumberOfDevs, DateTime bestDate)
        {
            foreach (var bar in bars)
            {
                if (bar.Capacity.Value >= maxNumberOfDevs && bar.OpenedDays.Contains(bestDate.DayOfWeek))
                {
                    BarFunctions.book(bar, bestDate);
                    _bookingRepository.Save(new Booking(bar, bestDate));
                    return true;
                }
            }

            return false;
        }

        private static Dictionary<DateTime, int> NumberOfAvailableDevsByDate(List<Dev> devs)
        {
            var numberOfAvailableDevsByDate = new Dictionary<DateTime, int>();
            foreach (var dev in devs)
            {
                foreach (var date in dev.OnSite)
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

            return numberOfAvailableDevsByDate;
        }
    }
}