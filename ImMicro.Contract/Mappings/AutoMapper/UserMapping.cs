using AutoMapper;
using HelpersToolbox.Extensions;
using ImMicro.Contract.App.User;
using ImMicro.Contract.Service.User;
using ImMicro.Model.User;

namespace ImMicro.Contract.Mappings.AutoMapper
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<CreateUserRequest, CreateUserRequestServiceRequest>();
            CreateMap<UpdateUserRequest, UpdateUserRequestServiceRequest>();
            CreateMap<RefreshTokenContract, RefreshTokenContractServiceRequest>();
            CreateMap<GetTokenContract, GetTokenContractServiceRequest>();
            CreateMap<User, UserView>().ForMember(p => p.Type, b => b.MapFrom(p => p.Type.GetDisplayName()));
        }
    }
}