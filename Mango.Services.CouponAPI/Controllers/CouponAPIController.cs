using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {

        private readonly ApplicationDBContext _db;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;

        public CouponAPIController(ApplicationDBContext db,IMapper mapper)
        { 
         _db = db;
        _responseDto = new ResponseDto();
         _mapper = mapper;   
        }
        [HttpGet]
        public async Task<ResponseDto> GetAllCoupons()
        {
            try
            {
             
                IEnumerable<Coupon> coupons = await _db.Coupons.ToListAsync();
                _responseDto.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);

            }
            catch(Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

            }
            return _responseDto;

        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResponseDto> GetCoupon(int id)
        {
            try
            {
                Coupon coupon =  await _db.Coupons.FirstAsync(k => k.CouponId == id);
                _responseDto.Result = _mapper.Map<CouponDto>(coupon);


            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;  
            }
            return _responseDto;
            

        }
        [HttpGet]
        [Route("GetByCode/{code}")]
        public async Task<ResponseDto> GetByCode(string code)
        {
            try
            {
                Coupon? coupon = await _db.Coupons.FirstOrDefaultAsync(k => k.CouponCode.ToLower() == code.ToLower());
                if(coupon is null)
                {
                    _responseDto.IsSuccess = false;
                }
                _responseDto.Result = _mapper.Map<CouponDto>(coupon);


            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;

        }
        [HttpPost]
        public async Task<ResponseDto> Post([FromBody] CouponDto _couponDto)
        {
            try
            {
               
                await _db.Coupons.AddAsync(_mapper.Map<Coupon>(_couponDto));
                await _db.SaveChangesAsync();
             
                _responseDto.Result = _mapper.Map<CouponDto>(_couponDto);


            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;

        }
        [HttpPut]
        public async Task<ResponseDto> Put([FromBody] CouponDto _couponDto)
        {
            try
            {

                 _db.Coupons.Update(_mapper.Map<Coupon>(_couponDto));
                await _db.SaveChangesAsync();

                _responseDto.Result = _mapper.Map<CouponDto>(_couponDto);


            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;

        }
        [HttpDelete]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                Coupon coupon = await _db.Coupons.FirstAsync(u=>u.CouponId == id); 
                _db.Coupons.Remove(coupon);
                await _db.SaveChangesAsync();



            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;

        }


    }
}
