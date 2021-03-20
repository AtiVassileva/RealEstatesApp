using AutoMapper;
using RealEstates.Services.Profiler;

namespace RealEstates.Services
{
    public abstract class BaseService
    {
        protected BaseService()
        {
            InitializeAutoMapper();
        }

        protected IMapper Mapper { get; private set; }

        private void InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RealEstatesProfiler>();
            });
            this.Mapper = config.CreateMapper();
        }
    }
}