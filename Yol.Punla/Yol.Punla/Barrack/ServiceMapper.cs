/* Chito.notes
 * Automapper headaches bug
 * 1. Automapper works well in debug mode and release mode "Not Link" assemblies.
 * 2. No 1 if what the F? So in release mode link "SDK assemblies", automapper will work in
 *    a. Do not put MapperConfiguration in the constructor
 *    b. Do not put .ToString() in the mapping code like this .ForMember(dst => dst.Price, src => src.MapFrom(source => source.Price.ToString()))
 * */

using Yol.Punla.AttributeBase;
using ViewModel = Yol.Punla.ViewModels;
using System.Diagnostics;
using AutoMapper;

namespace Yol.Punla.Barrack
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(IServiceMapper))]
    [DefaultModuleInterfaced(ParentInterface = typeof(IServiceMapper))]
    public class ServiceMapper : IServiceMapper
    {
        private IMapper _instance { get; set; }

        public IMapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.WriteLine("HOPEPH AutoMapper in Action.");
                    var config = new MapperConfiguration(cfg => {

                        #region MentalHealthFacility
                        cfg.CreateMap<Entity.MentalHealthFacility, Contract.MentalHealthFacilityK>()
                           .ForMember(dst => dst.ID, src => src.MapFrom(source => source.ID))
                           .ForMember(dst => dst.GroupName, src => src.MapFrom(source => source.GroupName))
                           .ForMember(dst => dst.FirstName, src => src.MapFrom(source => source.FirstName))
                           .ForMember(dst => dst.LastName, src => src.MapFrom(source => source.LastName))
                           .ForMember(dst => dst.MobilePhone, src => src.MapFrom(source => source.PhoneNumber))
                           .ForMember(dst => dst.PhotoUrl, src => src.MapFrom(source => source.PhotoUrl))
                           .ForMember(dst => dst.Description, src => src.MapFrom(source => source.Description))
                           .ForMember(dst => dst.Latitude, src => src.MapFrom(source => source.Latitude))
                           .ForMember(dst => dst.Longitude, src => src.MapFrom(source => source.Longitude))
                           .ForMember(dst => dst.Address, src => src.MapFrom(source => source.Location));

                        cfg.CreateMap<Contract.MentalHealthFacilityK, Entity.MentalHealthFacility>()
                          .ForMember(dst => dst.ID, src => src.MapFrom(source => source.ID))
                          .ForMember(dst => dst.GroupName, src => src.MapFrom(source => source.GroupName))
                          .ForMember(dst => dst.FirstName, src => src.MapFrom(source => source.FirstName))
                          .ForMember(dst => dst.LastName, src => src.MapFrom(source => source.LastName))
                          .ForMember(dst => dst.PhoneNumber, src => src.MapFrom(source => source.MobilePhone))
                          .ForMember(dst => dst.PhotoUrl, src => src.MapFrom(source => source.PhotoUrl))
                          .ForMember(dst => dst.Description, src => src.MapFrom(source => source.Description))
                          .ForMember(dst => dst.Latitude, src => src.MapFrom(source => source.Latitude))
                          .ForMember(dst => dst.Longitude, src => src.MapFrom(source => source.Longitude))
                          .ForMember(dst => dst.Location, src => src.MapFrom(source => source.Address));
                        #endregion

                        #region MyRegion
                        cfg.CreateMap<Entity.Wiki, Contract.WikiK>()
                           .ForMember(dst => dst.Title, src => src.MapFrom(source => source.Title))
                           .ForMember(dst => dst.Content, src => src.MapFrom(source => source.Content))
                           .ForMember(dst => dst.IconPath, src => src.MapFrom(source => source.IconPath));

                        cfg.CreateMap<Contract.WikiK, Entity.Wiki>()
                          .ForMember(dst => dst.Title, src => src.MapFrom(source => source.Title))
                          .ForMember(dst => dst.Content, src => src.MapFrom(source => source.Content))
                          .ForMember(dst => dst.IconPath, src => src.MapFrom(source => source.IconPath))
                          .ForMember(dst => dst.ForceToVersionNo, src => src.MapFrom(source => source.ForceToVersionNo))
                          .ForMember(dst => dst.ForceToVersionNoIOS, src => src.MapFrom(source => source.ForceToVersionNoIOS));
                        #endregion

                        #region Contact

                        cfg.CreateMap<Contract.ContactK, Entity.Contact>()
                          .ForMember(dst => dst.RemoteId, src => src.MapFrom(source => source.Id))
                          .ForMember(dst => dst.FirstName, src => src.MapFrom(source => source.FirstName))
                          .ForMember(dst => dst.LastName, src => src.MapFrom(source => source.LastName))
                          .ForMember(dst => dst.EmailAdd, src => src.MapFrom(source => source.EmailAddress))
                          .ForMember(dst => dst.FBId, src => src.MapFrom(source => source.FBId))
                          .ForMember(dst => dst.Birthdate, src => src.MapFrom(source => source.BirthDate))
                          .ForMember(dst => dst.GenderCode, src => src.MapFrom(source => source.GenderCode))
                          .ForMember(dst => dst.MobilePhone, src => src.MapFrom(source => source.MobilePhone))
                          .ForMember(dst => dst.Password, src => src.MapFrom(source => source.Password))
                          .ForMember(dst => dst.FBLink, src => src.MapFrom(source => source.FBLink))
                          .ForMember(dst => dst.PhotoURL, src => src.MapFrom(source => source.PhotoURL));

                        cfg.CreateMap<Entity.Contact, Contract.ContactK>()
                         .ForMember(dst => dst.Id, src => src.MapFrom(source => source.RemoteId))
                         .ForMember(dst => dst.FirstName, src => src.MapFrom(source => source.FirstName))
                         .ForMember(dst => dst.LastName, src => src.MapFrom(source => source.LastName))
                         .ForMember(dst => dst.EmailAddress, src => src.MapFrom(source => source.EmailAdd))
                         .ForMember(dst => dst.FBId, src => src.MapFrom(source => source.FBId))
                         .ForMember(dst => dst.BirthDate, src => src.MapFrom(source => source.Birthdate))
                         .ForMember(dst => dst.GenderCode, src => src.MapFrom(source => source.GenderCode))
                         .ForMember(dst => dst.MobilePhone, src => src.MapFrom(source => source.MobilePhone))
                         .ForMember(dst => dst.Password, src => src.MapFrom(source => source.Password))
                         .ForMember(dst => dst.CompanyName, src => src.MapFrom(source => source.CompanyName))
                         .ForMember(dst => dst.FBLink, src => src.MapFrom(source => source.FBLink))
                         .ForMember(dst => dst.PhotoURL, src => src.MapFrom(source => source.PhotoURL));

                        #endregion

                        #region PostFeed

                        cfg.CreateMap<Contract.PostFeedK, Entity.PostFeed>()
                         .ForMember(dst => dst.PostFeedID, src => src.MapFrom(source => source.PostFeedID))
                         .ForMember(dst => dst.Title, src => src.MapFrom(source => source.Title))
                         .ForMember(dst => dst.ContentText, src => src.MapFrom(source => source.ContentText))
                         .ForMember(dst => dst.ContentURL, src => src.MapFrom(source => source.ContentURL))
                         .ForMember(dst => dst.PosterFirstName, src => src.MapFrom(source => source.FirstName))
                         .ForMember(dst => dst.PosterLastName, src => src.MapFrom(source => source.LastName))
                         .ForMember(dst => dst.DatePosted, src => src.MapFrom(source => source.DateCreated))
                         .ForMember(dst => dst.DateModified, src => src.MapFrom(source => source.DateModified))
                         .ForMember(dst => dst.NoOfComments, src => src.MapFrom(source => source.CommentQty))
                         .ForMember(dst => dst.NoOfSupports, src => src.MapFrom(source => source.LikeQty))
                         .ForMember(dst => dst.PosterProfilePhotoFB, src => src.MapFrom(source => source.FBPhotoURL))
                         .ForMember(dst => dst.PosterProfilePhoto, src => src.MapFrom(source => source.ContactPhotoURL))
                         .ForMember(dst => dst.PostFeedLevel, src => src.MapFrom(source => source.PostFeedLevel))
                         .ForMember(dst => dst.PostFeedParentId, src => src.MapFrom(source => source.PostFeedParentId))
                         .ForMember(dst => dst.IsDelete, src => src.MapFrom(source => source.IsDelete))
                         .ForMember(dst => dst.IsSelfSupported, src => src.MapFrom(source => source.IsSelfLike))
                         .ForMember(dst => dst.AliasName, src => src.MapFrom(source => source.AliasName))
                         .ForMember(dst => dst.ContactsWhoLiked, src => src.MapFrom(source => source.ContactsWhoLiked))
                         .ForMember(dst => dst.PosterId, src => src.MapFrom(source => source.ContactId))
                         .ForMember(dst => dst.ContactGender, src => src.MapFrom(source => source.ContactGender))
                         .ForMember(dst => dst.HasUnReadComments, src => src.MapFrom(source => source.HasUnReadComments));

                        cfg.CreateMap<Entity.PostFeed, Contract.PostFeedK>()
                         .ForMember(dst => dst.PostFeedID, src => src.MapFrom(source => source.PostFeedID))
                         .ForMember(dst => dst.Title, src => src.MapFrom(source => source.Title))
                         .ForMember(dst => dst.ContentText, src => src.MapFrom(source => source.ContentText))
                         .ForMember(dst => dst.ContentURL, src => src.MapFrom(source => source.ContentURL))
                         .ForMember(dst => dst.FirstName, src => src.MapFrom(source => source.PosterFirstName))
                         .ForMember(dst => dst.LastName, src => src.MapFrom(source => source.PosterLastName))
                         .ForMember(dst => dst.DateCreated, src => src.MapFrom(source => source.DatePosted))
                         .ForMember(dst => dst.DateModified, src => src.MapFrom(source => source.DateModified))
                         .ForMember(dst => dst.CommentQty, src => src.MapFrom(source => source.NoOfComments))
                         .ForMember(dst => dst.LikeQty, src => src.MapFrom(source => source.NoOfSupports))
                         .ForMember(dst => dst.FBPhotoURL, src => src.MapFrom(source => source.PosterProfilePhotoFB))
                         .ForMember(dst => dst.ContactPhotoURL, src => src.MapFrom(source => source.PosterProfilePhoto))
                         .ForMember(dst => dst.PostFeedLevel, src => src.MapFrom(source => source.PostFeedLevel))
                         .ForMember(dst => dst.PostFeedParentId, src => src.MapFrom(source => source.PostFeedParentId))
                         .ForMember(dst => dst.IsDelete, src => src.MapFrom(source => source.IsDelete))
                         .ForMember(dst => dst.IsSelfLike, src => src.MapFrom(source => source.IsSelfSupported))
                         .ForMember(dst => dst.AliasName, src => src.MapFrom(source => source.AliasName))
                         .ForMember(dst => dst.ContactsWhoLiked, src => src.MapFrom(source => source.ContactsWhoLiked))
                         .ForMember(dst => dst.ContactId, src => src.MapFrom(source => source.PosterId));

                        #endregion

                        #region Others

                        cfg.CreateMap<Entity.PostFeedLike, Contract.PostFeedLikeK>().ReverseMap();

                        #endregion

                    });

                    _instance = config.CreateMapper();
                }

                return _instance;
            }
        }
    }
}
