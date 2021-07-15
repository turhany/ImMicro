using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hangfire;
using HelpersToolbox.Extensions;
using ImMicro.Business.Services.Abstract;
using ImMicro.Business.Validators;
using ImMicro.Common.Resources;
using ImMicro.Contract.Dtos;
using ImMicro.Data;
using ImMicro.Data.Repositories.Abstract;

namespace ImMicro.Business.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task ImportDataAsync(MemoryStream memoryStream, CancellationToken cancellationToken)
        {
            List<UserImportDto> deserializedUserImportDtoItems;
            try
            { 
                deserializedUserImportDtoItems = Utf8Json.JsonSerializer.Deserialize<List<UserImportDto>>(memoryStream); 
            }
            catch
            {
                throw new ValidationException(Literals.InvalidJsonFile);
            }
            
            var userItemsForDbInsert =  ValidateAndNormalizeData(deserializedUserImportDtoItems, cancellationToken);
            
            await InsertUserDataAsync(userItemsForDbInsert);
        }
        
        private List<User> ValidateAndNormalizeData(List<UserImportDto> deserializedUserImportDtoItems, CancellationToken cancellationToken)
        {
            var userItemsForDbInsert = new ConcurrentBag<User>();
            
            var exceptions = new ConcurrentQueue<Exception>();
            var validationExceptions = new ConcurrentQueue<ValidationException>();
            var validator = new UserImportDtoValidator();

            Parallel.ForEach(deserializedUserImportDtoItems, (userImportDto, state) =>
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        state.Break();
                    }
                    
                    validator.ValidateAndThrow(userImportDto);

                    var entity = ConvertToUser(userImportDto);

                    userItemsForDbInsert.Add(entity);
                }
                catch (ValidationException e)
                {
                    validationExceptions.Enqueue(e);
                    state.Break();
                }
                catch (Exception e)
                {
                    exceptions.Enqueue(e);
                    state.Break();
                }
            });

            if (validationExceptions.Count > 0)
                throw new ValidationException(Literals.InvalidJsonFile, validationExceptions.SelectMany(p => p.Errors).ToList());

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);

            return userItemsForDbInsert.ToList();
        }
        
        private User ConvertToUser(UserImportDto userImportDto)
        {
            return new User
            {
                Name = userImportDto.Name,
                Surname = userImportDto.Surname,
                Email = userImportDto.Email
            };
        }
        
        private async Task InsertUserDataAsync(List<User> userItemsForDbInsert)
        {
            var userItemsForDbInsertChunks = userItemsForDbInsert.Chunk(500);
            foreach (var userItemForDbChunksItem in userItemsForDbInsertChunks)
            {
                await _userRepository.BulkInsertAsync(userItemForDbChunksItem);
            }
        }

        [DisableConcurrentExecution(timeoutInSeconds: 21600)]//6 hour
        public void SendQueueForInit(int itemLimit)
        {
            throw new NotImplementedException();
        }
    }
}