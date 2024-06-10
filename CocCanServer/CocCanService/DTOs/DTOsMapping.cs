using AutoMapper;
using CocCanService.DTOs.Product;
using CocCanService.DTOs.Category;
using CocCanService.DTOs.Location;
using CocCanService.DTOs.Menu;
using CocCanService.DTOs.Staff;
using CocCanService.DTOs.Store;
using System;
using CocCanService.DTOs.Order;
using CocCanService.DTOs.OrderDetail;
using CocCanService.DTOs.MenuDetail;
using CocCanService.DTOs.TimeSlot;
using CocCanService.DTOs.Session;
using CocCanService.DTOs.PickUpSpot;
using CocCanService.DTOs.Customer;
using CocCanService.DTOs.Enum;

namespace CocCanService.DTOs
{
    public class DTOsMapping : Profile
    {
        public DTOsMapping()
        {

            //Category
            CreateMap<Repository.Entities.Category, CategoryDTO>().ReverseMap();
            CreateMap<CreateCategoryDTO, Repository.Entities.Category>()
                .ForMember(
                    des => des.Id,
                    act => Guid.NewGuid())
                .ForMember(
                    des => des.Status,
                    act => act.UseValue(1));
            CreateMap<UpdateCategoryDTO, Repository.Entities.Category>()
                .ForMember(
                    des => des.Name,
                    act =>
                        {
                            act.UseDestinationValue();
                            act.PreCondition(src => src.Name != "");
                            act.MapFrom(src => src.Name);
                        })
                .ForMember(
                    des => des.Image,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Image != "");
                        act.MapFrom(src => src.Image);
                    })
                .ForMember(
                    des => des.Id,
                    act =>
                        {
                            act.UseDestinationValue();
                            act.Ignore();
                        })
                .ForMember(
                    des => des.Status,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    });

            //Customer
            CreateMap<Repository.Entities.Customer, CustomerDTO>().ReverseMap();
            CreateMap<CreateCustomerDTO, Repository.Entities.Customer>()
                .ForMember(
                    des => des.Id,
                    act => Guid.NewGuid())
                .ForMember(
                    des => des.Status,
                    act => act.UseValue(1));
            CreateMap<UpdateCustomerDTO, Repository.Entities.Customer>()
                .ForMember(
                    des => des.Fullname,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Fullname != "");
                        act.MapFrom(src => src.Fullname);
                    })
                .ForMember(
                    des => des.Image,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Image != "");
                        act.MapFrom(src => src.Image);
                    })
                .ForMember(
                    des => des.Email,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Email != "");
                        act.MapFrom(src => src.Email);
                    })
                .ForMember(
                    des => des.Phone,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Phone != "");
                        act.MapFrom(src => src.Phone);
                    })
                .ForMember(
                    des => des.Id,
                    act =>  
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    })
                .ForMember(
                    des => des.Status,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    });

            //Location
            CreateMap<Repository.Entities.Location, LocationDTO>().ReverseMap();
            CreateMap<CreateLocationDTO, Repository.Entities.Location>()
                .ForMember(
                    des => des.Id,
                    act => Guid.NewGuid())
                .ForMember(
                    des => des.Status,
                    act => act.UseValue(1));
            CreateMap<UpdateLocationDTO, Repository.Entities.Location>()
                .ForMember(
                    des => des.Name,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Name != "");
                        act.MapFrom(src => src.Name);
                    })
                .ForMember(
                    des => des.Address,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Address != "");
                        act.MapFrom(src => src.Address);
                    })
                .ForMember(
                    des => des.Id,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    })
                .ForMember(
                    des => des.Status,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    });

            //Menu
            CreateMap<Repository.Entities.Menu, MenuDTO>().ReverseMap();
            CreateMap<CreateMenuDTO, Repository.Entities.Menu>()
                .ForMember(
                    des => des.Id,
                    act => Guid.NewGuid())
                .ForMember(
                    des => des.Status,
                    act => act.UseValue(1));
            CreateMap<UpdateMenuDTO, Repository.Entities.Menu>()
                .ForMember(
                    des => des.Name,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Name != "");
                        act.MapFrom(src => src.Name);
                    })
                .ForMember(
                    des => des.Id,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    })
                .ForMember(
                    des => des.Status,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    });
            //Order
            CreateMap<Repository.Entities.Order, OrderDTO>().ReverseMap();
            //OrderDetail
            CreateMap<Repository.Entities.OrderDetail, OrderDetailDTO>().ReverseMap();
            CreateMap<CreateOrderDetailDTO, Repository.Entities.OrderDetail>()
                .ForMember(
                    des => des.Id,
                    act => Guid.NewGuid())
                .ForMember(
                    des => des.Status,
                    act => act.UseValue(1));
            CreateMap<UpdateOrderDetailDTO, Repository.Entities.OrderDetail>()
                .ForMember(
                    des => des.Quantity,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Quantity != 0);
                        act.MapFrom(src => src.Quantity);
                    })
                .ForMember(
                    des => des.MenuDetailId,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.MenuDetailId.ToString() != "");
                        act.MapFrom(src => src.MenuDetailId);
                    })
                .ForMember(
                    des => des.OrderId,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.OrderId.ToString() != "");
                        act.MapFrom(src => src.OrderId);
                    })
                .ForMember(
                    des => des.Id,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    })
                .ForMember(
                    des => des.Status,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    });

            //MenuDetail
            CreateMap<Repository.Entities.MenuDetail, MenuDetailDTO>().ReverseMap();
            CreateMap<CreateMenuDetailDTO, Repository.Entities.MenuDetail>()
                .ForMember(
                    des => des.Id,
                    act => Guid.NewGuid())
                .ForMember(
                    des => des.Status,
                    act => act.UseValue(1));
            CreateMap<UpdateMenuDetailDTO, Repository.Entities.MenuDetail>()
                .ForMember(
                    des => des.Price,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Price != 0);
                        act.MapFrom(src => src.Price);
                    })
                .ForMember(
                    des => des.MenuId,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.MenuId.ToString() != "");
                        act.MapFrom(src => src.MenuId);
                    })
                .ForMember(
                    des => des.ProductId,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.ProductId.ToString() != "");
                        act.MapFrom(src => src.ProductId);
                    })
                .ForMember(
                    des => des.Id,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    })
                .ForMember(
                    des => des.Status,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    });

            //Staff
            CreateMap<Repository.Entities.Staff, StaffDTO>()
                .ForMember(
                    des => des.Role,
                    act => act.MapFrom(src => ((Enum.RoleType)src.Role).ToString()));
            CreateMap<CreateStaffDTO, Repository.Entities.Staff>()
                .ForMember(
                    des => des.Id,
                    act => Guid.NewGuid())
                .ForMember(
                    des => des.Role,
                    act => act.MapFrom(src => (int) (RoleType) System.Enum.Parse(typeof(RoleType), src.Role)))
                .ForMember(
                    des => des.Status,
                    act => act.UseValue(1));
            //CreateMap<Repository.Entities.Staff, StaffDTO>().ReverseMap();

            //Product
            CreateMap<Repository.Entities.Product, ProductDTO>().ReverseMap()
                .ForMember(
                    des => des.Category,
                    act => act.MapFrom(src => src.Category));
            CreateMap<CreateProductDTO, Repository.Entities.Product>()
                .ForMember(
                    des => des.Id,
                    act => Guid.NewGuid())
                .ForMember(
                    des => des.Status,
                    act => act.UseValue(1));
            //Store
            CreateMap<Repository.Entities.Store, StoreDTO>().ReverseMap()
                .ForMember(
                    des => des.Products,
                    act => act.MapFrom(src => src.Products));
            CreateMap<CreateStoreDTO, Repository.Entities.Store>()
                .ForMember(
                    des => des.Id,
                    act => Guid.NewGuid())
                .ForMember(
                    des => des.Status,
                    act => act.UseValue(1));
            CreateMap<UpdateStoreDTO, Repository.Entities.Store>()
                .ForMember(
                    des => des.Name,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Name != "");
                        act.MapFrom(src => src.Name);
                    })
                .ForMember(
                    des => des.Image,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.PreCondition(src => src.Image != "");
                        act.MapFrom(src => src.Image);
                    })
                .ForMember(
                    des => des.Id,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    })
                .ForMember(
                    des => des.Status,
                    act =>
                    {
                        act.UseDestinationValue();
                        act.Ignore();
                    });
            //TimeSlot
            CreateMap<Repository.Entities.TimeSlot, TimeSlotDTO>().ReverseMap()
                .ForMember(
                    des => des.StartTime,
                    act => act.MapFrom(src => TimeSpan.Parse(src.StartTime))
                )
                .ForMember(
                    des => des.EndTime,
                    act => act.MapFrom(src => TimeSpan.Parse(src.EndTime))
                );
            CreateMap<CreateTimeSlotDTO, Repository.Entities.TimeSlot>()
                .ForMember(
                    des => des.Id,
                    act => Guid.NewGuid())
                .ForMember(
                    des => des.StartTime,
                    act => act.MapFrom(src => TimeSpan.Parse(src.StartTime))
                )
                .ForMember(
                    des => des.EndTime,
                    act => act.MapFrom(src => TimeSpan.Parse(src.EndTime))
                )
                .ForMember(
                    des => des.Status,
                    act => act.UseValue(1));
            //Session
            CreateMap<Repository.Entities.Session, SessionDTO>().ReverseMap();
            CreateMap<CreateSessionDTO, Repository.Entities.Session>()
                .ForMember(
                    des => des.Id,
                    act => Guid.NewGuid())
                .ForMember(
                    des => des.Status,
                    act => act.UseValue(1));



            //Location
            //CreateMap<Repository.Entities.Location, LocationDTO>().ReverseMap();
            //Category
            CreateMap<Repository.Entities.Category, CategoryDTO>().ReverseMap();
            //CreateMap<CreateProductDTO, Repository.Entities.Product>()
            //    .ForMember(
            //        des => des.Id,
            //        act => Guid.NewGuid())
            //    .ForMember(
            //        des => des.Status,
            //        act => act.UseValue(1));
            //Menu
            //CreateMap<Repository.Entities.Menu, MenuDTO>().ReverseMap();

            //PickUpSpot
            CreateMap<Repository.Entities.PickUpSpot, PickUpSpotDTO>().ReverseMap();

        }
    }
}
