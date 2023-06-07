using AutoMapper;
using GymEats.Common.Model;
using GymEats.Data.Entity;
using GymEats.Services.Repository.Common;
using GymEats.Services.Suggestic.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.Service
{
    public class UserShoppingService : IUserShoppingService
    {
        private readonly IGenericRepository<ShoppingList> _shoppingListRepository;
        private readonly IMapper _mapper;

        public UserShoppingService(IGenericRepository<ShoppingList> shoppingListRepository, IMapper mapper)
        {
            _shoppingListRepository = shoppingListRepository;
            _mapper = mapper;
        }

        public async Task<List<ShoppingList>> GetAllListtItems(string userId)
        {
            var data = await _shoppingListRepository.GetAsync(x => x.IsDeleted == false && x.UserId == userId);

            return data.ToList();
        }

        public async Task<int> AddItemToShoppingList(UserCartRequestModel model)
        {
            var data = await _shoppingListRepository.GetAsync(x => x.ProductId == model.ProductId && x.UserId == model.UserId);
            if (data.FirstOrDefault() == null)
            {
                ShoppingList userCart = new ShoppingList();
                userCart.Id = new Guid();
                userCart.ProductId = model.ProductId;
                userCart.UserId = model.UserId;
                userCart.ProductName = model.ProductName;
                userCart.Quantity = model.Quantity;
                userCart.Price = model.Price;   
                userCart.RecipeId = string.IsNullOrEmpty(model.RecipeId) ? "":model.RecipeId;
                userCart.ServingUnit = string.IsNullOrEmpty(model.ServingUnit) ? "" : model.ServingUnit;
                userCart.RecipeId = string.IsNullOrEmpty(model.RecipeId) ? "" : model.RecipeId;
                userCart.CreatedOn = DateTime.UtcNow;
                userCart.IsChecked = false;
                userCart.IsDeleted = false;
                userCart.IsActive = true;
                await _shoppingListRepository.InsertAsync(userCart);
            }
            else
            {
                var res = data.FirstOrDefault();
                res.Quantity = res.Quantity + model.Quantity > 0 ? model.Quantity : 1;
                await _shoppingListRepository.UpdateAsync(res);
            }

            return (int)await _shoppingListRepository.SaveAsync();
        }

        public async Task<int> UpdateItemToShoppingList(int quantity, string productId, string userId)
        {
            ShoppingList cart;
            var data = await _shoppingListRepository.GetAsync(x => x.ProductId == productId && x.UserId == userId) ;
            if(data != null && data.Any())
            {
                cart = data.FirstOrDefault();
                cart.Quantity = quantity;
                await _shoppingListRepository.UpdateAsync(cart);
            }
            return (int) await _shoppingListRepository.SaveAsync();
        }

        public async Task<double> GetTotalPriceOfItem(string userId)
        {
            var cartItemList = await _shoppingListRepository.GetAsync(x => x.UserId == userId);
            double totalPrice = 0;
            if(cartItemList != null && cartItemList.Any())
            {
                foreach(var item in cartItemList)
                {
                    totalPrice += (double)(item.Price * item.Quantity);
                }
            }
            return totalPrice;
        }
    }
}
