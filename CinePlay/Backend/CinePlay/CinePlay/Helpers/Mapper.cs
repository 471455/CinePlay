using AutoMapper;
using MyMDb.Models;
using MyMDb.DTOs;

namespace MyMDb.Helpers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Media, MediaDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<MediaDto, Media>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Movie, MovieDto>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<MovieDto, Movie>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Series, SeriesDto>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<SeriesDto, Series>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Episode, EpisodeDto>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<EpisodeDto, Episode>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); ;

            CreateMap<Review, ReviewDto>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ReviewDto, Review>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<AppUser, UserDto>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UserDto, AppUser>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<UserProfile, ProfileDto>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ProfileDto, UserProfile>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
