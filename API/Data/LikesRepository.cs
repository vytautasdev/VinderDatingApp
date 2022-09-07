﻿using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository : ILikesRepository


{
    private readonly DataContext _dataContext;

    public LikesRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
    {
        return await _dataContext.Likes.FindAsync(sourceUserId, likedUserId);
    }

    public async Task<AppUser> GetUserWithLikes(int userId)
    {
        return await _dataContext.Users
            .Include(x => x.LikedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
    {
        var users = _dataContext.Users.OrderBy(u => u.UserName).AsQueryable();
        var likes = _dataContext.Likes.AsQueryable();

        if (likesParams.Predicate == "liked")
        {
            likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
            users = likes.Select(like => like.LikedUser);
        }

        if (likesParams.Predicate == "likedBy")
        {
            likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
            users = likes.Select(like => like.SourceUser);
        }

        var likedUsers = users.Select(user => new LikeDto
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Age = user.Dob.CalculateUserAge(),
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).ImageUrl,
            City = user.City,
            Id = user.Id
        });

        return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
    }
}