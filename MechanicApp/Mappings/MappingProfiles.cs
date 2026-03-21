using AutoMapper;
using MechanicApp.DTOs;
using MechanicApp.Models;

namespace MechanicApp.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Client mappings
        CreateMap<Client, ClientDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber));
        
        CreateMap<CreateClientDto, Client>();
        CreateMap<UpdateClientDto, Client>();

        // Vehicle mappings
        CreateMap<Vehicle, VehicleDto>();
        CreateMap<CreateVehicleDto, Vehicle>();
        CreateMap<UpdateVehicleDto, Vehicle>();

        // ServiceRequest mappings
        CreateMap<ServiceRequest, ServiceRequestDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.VehicleMake, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Make : null))
            .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Model : null))
            .ForMember(dest => dest.MechanicName, opt => opt.MapFrom(src => src.Mechanic != null && src.Mechanic.User != null 
                ? $"{src.Mechanic.User.FirstName} {src.Mechanic.User.LastName}" 
                : null));
        CreateMap<CreateServiceRequestDto, ServiceRequest>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ServiceStatus.Pending));
        CreateMap<UpdateServiceRequestDto, ServiceRequest>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ServiceStatus>(src.Status)));

        // Mechanic mappings
        CreateMap<Mechanic, MechanicDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop != null ? src.Shop.Name : null));
        CreateMap<CreateMechanicDto, Mechanic>();
        CreateMap<UpdateMechanicDto, Mechanic>();

        // Shop mappings
        CreateMap<Shop, ShopDto>();
        CreateMap<CreateShopDto, Shop>();
        CreateMap<UpdateShopDto, Shop>();

        // Conversation mappings
        CreateMap<Conversation, ConversationDto>()
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client != null && src.Client.User != null 
                ? $"{src.Client.User.FirstName} {src.Client.User.LastName}" 
                : null))
            .ForMember(dest => dest.MechanicName, opt => opt.MapFrom(src => src.Mechanic != null && src.Mechanic.User != null 
                ? $"{src.Mechanic.User.FirstName} {src.Mechanic.User.LastName}" 
                : null));
        CreateMap<CreateConversationDto, Conversation>();
        CreateMap<UpdateConversationDto, Conversation>();

        // Message mappings
        CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender != null 
                ? $"{src.Sender.FirstName} {src.Sender.LastName}" 
                : null));
        CreateMap<CreateMessageDto, Message>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MessageStatus.Sent));
        CreateMap<UpdateMessageDto, Message>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<MessageStatus>(src.Status)));
    }
}
