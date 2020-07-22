using System;
using System.Collections.Generic;
using travel_service.Models;
using System.Threading.Tasks;
namespace travel_service.Services
{
    public interface ITouristRouteRepository
    {
        Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue);
        Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId);
        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);
        Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId);
        Task<TouristRoutePicture> GetPictureAsync(int pictureId);
        void AddTouristRoute(TouristRoute touristRoute);
        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);
        void DeleteTouristRoute(TouristRoute touristRoute);
        void DeleteTouristRoutePicture(TouristRoutePicture picture);
        Task<IEnumerable<TouristRoute>> GetTouristRoutesByIdListAsync(IEnumerable<Guid> ids);
        void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes);
        Task<bool> SaveAsync();
    }
}
