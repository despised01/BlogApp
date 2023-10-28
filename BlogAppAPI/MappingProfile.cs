using AutoMapper;
using BlogApp.BLL.RequestModels;
using BlogApp.DLL.Models;

namespace BlogApp
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<UserRequest, User>();
			CreateMap<User, UserRequest>();
			CreateMap<TagRequest, Tag>();
			CreateMap<Tag, TagRequest>();
			CreateMap<CommentRequest, Comment>();
			CreateMap<Comment, Comment>();
			CreateMap<PostRequest, Post>();
			CreateMap<Post, PostRequest>();
			CreateMap<RoleRequest, Role>();
			CreateMap<Role, RoleRequest>();
		}
	}
}
