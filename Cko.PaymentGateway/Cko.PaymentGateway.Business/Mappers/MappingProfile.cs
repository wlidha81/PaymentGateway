using AutoMapper;
using Cko.PaymentGateway.Data.Dtos;
using Cko.PaymentGateway.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cko.PaymentGateway.Business.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentRequestDto, PaymentRequest>().ReverseMap();
            CreateMap<PaymentDetailsDto, PaymentDetails>().ReverseMap();
            CreateMap<PaymentProcessingResultDto, PaymentProcessingResult>().ReverseMap();
            CreateMap<PaymentRequest, PaymentDetails>(MemberList.Source);
        }
    }
}
