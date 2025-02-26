﻿using AutoMapper;
using Hahn.Jobs.Utils;
using Hahn.Data.Dtos.Recipies;
using Hahn.Data.Interfaces.Repositories;
using Hangfire;
using MediatR;
using Hahn.Jobs.Recipes;

namespace Hahn.Application.Commands.Recipies.Handlers
{
    public class UpsertFoodRecipeCommandHandler : IRequestHandler<UpsertFoodRecipeCommand, FoodRecipeDto>
    {
       public async Task<FoodRecipeDto> Handle(UpsertFoodRecipeCommand request, CancellationToken cancellationToken)
        {
            
            var jobId = JobResultStore.RegisterJob();

            BackgroundJob.Enqueue<RecipeUpsertJob>(
                job => job.RunAsync(
                    request.Dto.Id,
                    request.Dto.Title,
                    request.Dto.Ingredients,
                    request.Dto.Instructions,
                    request.Dto.ImgUrl,
                    jobId));
                       
            var recipe = await JobResultStore.GetJobResultAsync<FoodRecipeDto>(jobId, timeoutSeconds: 30);
            if (recipe == null)
            {
                throw new TimeoutException("The upsert recipe job timed out.");
            }

            return recipe;
        }
    }
}
