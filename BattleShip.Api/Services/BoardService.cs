using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShip.Api.Exceptions;
using BattleShip.Api.Models;
using BattleShip.Api.Models.Enums;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.Extensions.Caching.Memory;

namespace BattleShip.Api.Services
{
    public interface IBoardService
    {
        Task<bool> CreateBoardAsync();

        Task<bool> AddShipAsync(CreateShipRequest request);

        void ClearBoardFromCache();

        Task<Tile[,]> GetBoardAsync();
    }
    public class BoardService : IBoardService
    {
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "BattleShipBoard";

        public BoardService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        public Task<bool> CreateBoardAsync()
        {
            // Check if a board already exists in the cache
            var cachedBoardExists = _memoryCache.TryGetValue<Tile[,]>(CacheKey, out var cachedBoard);

            if (cachedBoardExists)
                throw new BadRequestException("Board already exists, If you want to create a new board you will have to hit delete endpoint to clear existing board.");

            // Create board
            var boardSize = 10;
            var board = new Tile[boardSize, boardSize];

            for (var i = 0; i < boardSize; i++)
            {
                for (var j = 0; j < boardSize; j++)
                {
                    board[i, j] = new Tile();
                }
            }

            // Add it to cache
            var cacheExpirationOptions =
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(60),
                    Priority = CacheItemPriority.Normal
                };
            _memoryCache.Set<Tile[,]>(CacheKey, board, cacheExpirationOptions);
            _memoryCache.GetOrCreate(CacheKey, cacheEntry => board);


            return Task.FromResult(true);
        }

        public async Task<bool> AddShipAsync(CreateShipRequest request)
        {
            // Add ship to board based on it orientation and size
            var board = await  GetBoardAsync();
            for (var i = 0; i < request.Size; i++)
            {
                try
                {
                    if (board[request.XCoordinate, request.YCoordinate].IsHit || board[request.XCoordinate, request.YCoordinate].Name != "-")
                        throw new BadRequestException("Cannot place ship, invalid position!");

                    if (request.XCoordinate < board.GetLength(0) && request.YCoordinate < board.GetLength(1))
                    {
                        board[request.XCoordinate, request.YCoordinate] = new Tile
                        {
                            IsHit = false,
                            Name = request.ShipName
                        };
                        if (request.Orientation == Orientation.Horizontal)
                            request.YCoordinate += 1;
                        else
                            request.XCoordinate += 1;

                    }
                }
                catch (Exception)
                {
                    throw new BadRequestException("Cannot place ship, invalid position!");
                }
            }

            // Update cache
            var cacheExpirationOptions =
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(60),
                    Priority = CacheItemPriority.Normal
                };
            _memoryCache.Set<Tile[,]>(CacheKey, board, cacheExpirationOptions); 
            return true;
        }

        public Task<Tile[,]> GetBoardAsync()
        {
            var cachedBoardExists = _memoryCache.TryGetValue<Tile[,]>(CacheKey, out var cachedBoard);

            if (!cachedBoardExists)
                throw  new BadRequestException("No board exists, please create a new board!");

            return Task.FromResult(cachedBoard);
        }

        public void ClearBoardFromCache()
        {
            _memoryCache.Remove(CacheKey);
        }
    }
}
