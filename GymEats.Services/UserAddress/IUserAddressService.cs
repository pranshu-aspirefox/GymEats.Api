using GymEats.Common.Model;

namespace GymEats.Services.UserAddress
{
    public interface IUserAddressService
    {
        Task<UserAddressRequest> AddNewAddress(UserAddressRequest userAddress);
        Task<Data.Entity.UserAddress> GetAddressById(int addressId);
        Task<bool> RemoveAddress(int addressId);
        Task<Data.Entity.UserAddress> UpdateAddress(UserAddressRequest model, int addressId);
        Task<IEnumerable<GymEats.Data.Entity.UserAddress>> GetAllAddressByUserId(string userId);
    }
}