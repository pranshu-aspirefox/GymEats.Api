using AutoMapper;
using GymEats.Common.Model;
using GymEats.Services.Repository.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GymEats.Services.UserAddress
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IGenericRepository<Data.Entity.UserAddress> _userAddressRepository;
        private readonly IMapper _mapper;

        public UserAddressService(IGenericRepository<GymEats.Data.Entity.UserAddress> userAddressRepository, IMapper mapper)
        {
            _userAddressRepository = userAddressRepository;
            _mapper = mapper;
        }

        public async Task<UserAddressRequest> AddNewAddress(UserAddressRequest userAddress)
        {
            try
            {
                GymEats.Data.Entity.UserAddress data = _mapper.Map<UserAddressRequest, GymEats.Data.Entity.UserAddress>(userAddress);
                await _userAddressRepository.InsertAsync(data);
                var result = await _userAddressRepository.SaveAsync();
                if ((int)result > 0)
                    return userAddress;
                return new UserAddressRequest();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GymEats.Data.Entity.UserAddress> UpdateAddress(UserAddressRequest model, int addressId)
        {
            try
            {
                var data = (await _userAddressRepository.GetAsync(x => x.Id == addressId)).FirstOrDefault();
                if (data != null)
                {
                    if(model.Latitude != 0)
                        data.Latitude = model.Latitude;
                    if (model.Longitude != 0)
                        data.Longitude = model.Longitude;
                    if(!string.IsNullOrEmpty(model.Country))
                        data.Country = model.Country;
                    if (!string.IsNullOrEmpty(model.State))
                        data.State = model.State;
                    if (!string.IsNullOrEmpty(model.City))
                        data.City = model.City;
                    if (!string.IsNullOrEmpty(model.Zipcode))
                        data.Zipcode = model.Zipcode;
                    if (!string.IsNullOrEmpty(model.Street_Num))
                        data.Street_Num = model.Street_Num;
                    if (!string.IsNullOrEmpty(model.Street_Name))
                        data.Street_Name = model.Street_Name;
                    data.IsPrimary = model.IsPrimary;
                    await _userAddressRepository.UpdateAsync(data);
                    var result = await _userAddressRepository.SaveAsync();
                    if ((int)result > 0)
                        return data;
                }
                return new Data.Entity.UserAddress();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> RemoveAddress(int addressId)
        {
            try
            {
                var data = await _userAddressRepository.GetByIdAsync(addressId);
                if (data != null)
                {
                    await _userAddressRepository.DaleteAsync(data.Id);
                    var res = await _userAddressRepository.SaveAsync();
                    return (int)res > 0;
                }
                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<GymEats.Data.Entity.UserAddress> GetAddressById(int addressId)
        {
            try
            {
                var result = await _userAddressRepository.GetByIdAsync(addressId);
                if (result != null)
                    return result;
                return new Data.Entity.UserAddress();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<IEnumerable<GymEats.Data.Entity.UserAddress>> GetAllAddressByUserId(string userId)
        {
            try
            {
                var result = await _userAddressRepository.GetAsync(x => x.UserId == userId);
                if (result != null)
                    return result;
                return new List<Data.Entity.UserAddress>();
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
