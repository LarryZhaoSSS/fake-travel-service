using System;
using System.Collections.Generic;
using travel_service.Database;
using travel_service.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace travel_service.Services
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;
        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TouristRoutePicture> GetPictureAsync(int pictureId)
        {
            return await _context.touristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetPicturesByTouristRouteIdAsync(Guid touristRouteId)
        {
            return await _context.touristRoutePictures.Where(p => p.TouristRouteId == touristRouteId).ToListAsync();
        }

        public async Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.Include(t => t.TouristPictures).FirstOrDefaultAsync(n => n.Id == touristRouteId);
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue)
        {
            IQueryable<TouristRoute> result = _context.TouristRoutes.Include(t => t.TouristPictures);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(t => t.Title.Contains(keyword));
            }
            if (ratingValue >= 0)
            {
                result = ratingOperator switch
                {
                    "largerThan" => result.Where(t => t.Rating >= ratingValue),
                    "lessThan" => result.Where(t => t.Rating <= ratingValue),
                    _ => result.Where(t => t.Rating == ratingValue),
                };
            }
            return await result.ToListAsync();
        }

        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }
        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
        }
        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }
            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.touristRoutePictures.Add(touristRoutePicture);
        }
        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture picture)
        {
            _context.touristRoutePictures.Remove(picture);
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesByIdListAsync(IEnumerable<Guid> ids)
        {
            return await _context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();

        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes)
        {
            _context.TouristRoutes.RemoveRange(touristRoutes);
        }
        public async Task<ShoppingCart> GetShoppingCartByUserId(string userId)
        {
            return await _context.ShoppingCarts
                .Include(s => s.User).
                Include(s => s.ShoppingCartItems).ThenInclude(li => li.TouristRoute)
                .Where(s => s.UserId == userId).FirstOrDefaultAsync();
        }
        public async Task CreateShoppingCart(ShoppingCart shoppingCart)
        {
            await _context.ShoppingCarts.AddAsync(shoppingCart);
        }
        public async Task AddShoppingCartItem(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }
        public async Task<LineItem> GetShoppingCartItemByItemId(int lineItemId)
        {
            return await _context.LineItems.Where(li => li.Id == lineItemId).FirstOrDefaultAsync();
        }
        public void DeleteShoppingCartItem(LineItem lineItem)
        {
            _context.LineItems.Remove(lineItem);
        }
        public async Task<IEnumerable<LineItem>> GetShoppingCartItemsByIdListAsync(IEnumerable<int> ids)
        {
            return await _context.LineItems
               .Where(li => ids.Contains(li.Id))
               .ToListAsync();
        }
        public void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems)
        {
            _context.RemoveRange(lineItems);
        }
    }
}
